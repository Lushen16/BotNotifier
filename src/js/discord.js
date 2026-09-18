/**
 * PokéBot Sentinel - Módulo de Notificações Discord
 * Formatação de Rich Embeds e envio seguro via Relay ou Direct Fetch
 */

class SentinelDiscord {
  constructor() {
    this.lastSentTimes = {}; // Anti-flood local por trigger
  }

  /**
   * Converte cor hexadecimal para inteiro (exigido pelo Discord Embed)
   */
  hexToInt(hex) {
    if (!hex) return 0x5865f2;
    const cleanHex = hex.replace('#', '');
    return parseInt(cleanHex, 16) || 0x5865f2;
  }

  /**
   * Monta o texto de menção baseado na configuração
   */
  formatMention(mentionType, mentionId) {
    if (mentionType === 'everyone') return '@everyone';
    if (mentionType === 'here') return '@here';
    if (mentionType === 'role' && mentionId) return `<@&${mentionId.trim()}>`;
    if (mentionType === 'user' && mentionId) return `<@${mentionId.trim()}>`;
    return '';
  }

  /**
   * Dispara um alerta para o Discord
   */
  async sendAlert(trigger, confidence, settings) {
    const webhookUrl = settings.webhookUrl;
    if (!webhookUrl || !webhookUrl.startsWith('https://discord.com/api/webhooks/')) {
      throw new Error('URL do Webhook do Discord não configurada ou inválida!');
    }

    const mention = this.formatMention(settings.mentionType, settings.mentionId);
    const nowIso = new Date().toISOString();
    const formattedTime = new Date().toLocaleTimeString('pt-BR');

    // Mapeamento de cor e ícone por gatilho
    let colorHex = trigger.color || '#ff0844';
    let alertEmoji = trigger.icon || '🚨';
    let alertTitle = `${alertEmoji} ALERTA: ${trigger.name.toUpperCase()} DETECTADO!`;

    const embed = {
      title: alertTitle,
      description: `O som correspondente a **${trigger.name}** foi reconhecido com sucesso no áudio do jogo!`,
      color: this.hexToInt(colorHex),
      fields: [
        {
          name: '🎯 Gatilho',
          value: `\`${trigger.name}\``,
          inline: true
        },
        {
          name: '📊 Confiança',
          value: `**${confidence}%** (Limiar: ${trigger.threshold}%)`,
          inline: true
        },
        {
          name: '⏰ Horário',
          value: `\`${formattedTime}\``,
          inline: true
        },
        {
          name: '🛡️ Sistema',
          value: 'PokéBot Sentinel v1.0',
          inline: true
        }
      ],
      footer: {
        text: 'PokéBot Sentinel • Monitoramento Acústico de Jogo'
      },
      timestamp: nowIso
    };

    if (trigger.description) {
      embed.fields.push({
        name: '📝 Observação',
        value: trigger.description,
        inline: false
      });
    }

    const payload = {
      username: 'PokéBot Sentinel',
      avatar_url: 'https://i.imgur.com/8Qp49X0.png',
      content: mention ? `${mention} **Atenção! Som de evento detectado!**` : undefined,
      embeds: [embed]
    };

    return await this.dispatchWebhook(webhookUrl, payload);
  }

  /**
   * Envia uma notificação de teste para verificar se o Webhook está funcionando
   */
  async sendTestNotification(webhookUrl, mentionType, mentionId) {
    if (!webhookUrl || !webhookUrl.startsWith('https://discord.com/api/webhooks/')) {
      throw new Error('Insira uma URL de Webhook válida do Discord.');
    }

    const mention = this.formatMention(mentionType, mentionId);
    const formattedTime = new Date().toLocaleTimeString('pt-BR');

    const payload = {
      username: 'PokéBot Sentinel',
      avatar_url: 'https://i.imgur.com/8Qp49X0.png',
      content: mention ? `${mention} **Teste de Conexão do PokéBot Sentinel**` : undefined,
      embeds: [
        {
          title: '✅ Webhook Configurado com Sucesso!',
          description: 'Esta é uma mensagem de teste enviada pelo **PokéBot Sentinel**. O sistema está pronto para enviar avisos de **Seta GM**, **Player Desconhecido** e **Pokémon Fora da Hunt**!',
          color: 0x00ff87,
          fields: [
            {
              name: '🔌 Status',
              value: '🟢 Operacional',
              inline: true
            },
            {
              name: '⏰ Horário do Teste',
              value: `\`${formattedTime}\``,
              inline: true
            },
            {
              name: '⚙️ Menção Configurada',
              value: mention || 'Nenhuma',
              inline: true
            }
          ],
          footer: {
            text: 'PokéBot Sentinel • Pronto para caçar!'
          },
          timestamp: new Date().toISOString()
        }
      ]
    };

    return await this.dispatchWebhook(webhookUrl, payload);
  }

  /**
   * Despacha a mensagem usando o relay do servidor local ou fallback para direct fetch
   */
  async dispatchWebhook(webhookUrl, payload) {
    try {
      // 1. Tentar via servidor local Express (Relay sem problemas de CORS ou bloqueadores)
      const res = await fetch('/api/discord-notify', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ webhookUrl, payload })
      });

      if (res.ok) {
        return { success: true, method: 'relay' };
      } else {
        const errorData = await res.json().catch(() => ({}));
        console.warn('Relay local retornou erro, tentando fallback direto:', errorData);
        // Fallback para fetch direto se o servidor local falhar
        return await this.directFetch(webhookUrl, payload);
      }
    } catch (err) {
      console.warn('Falha no relay local, tentando requisição direta ao Discord:', err);
      return await this.directFetch(webhookUrl, payload);
    }
  }

  async directFetch(webhookUrl, payload) {
    const res = await fetch(webhookUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    });

    if (res.ok || res.status === 204) {
      return { success: true, method: 'direct' };
    }

    const text = await res.text();
    throw new Error(`Discord retornou erro ${res.status}: ${text}`);
  }
}

window.sentinelDiscord = new SentinelDiscord();
