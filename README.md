# 🚨 CHARM BOT NOTIFIER

Aplicação web moderna para monitoramento acústico em tempo real de sons do sistema operacional / jogos, com fatiamento automático de amostras sonoras, interface de **arrastar e soltar (Drag & Drop)** para classificação e envio instantâneo de alertas no **Discord via Webhooks**.

---

## 🎯 5 Categorias Fixas de Monitoramento

1. 🚨 **MENSAGEM GM**: Alerta prioritário quando staff ou Game Master mandar mensagem, PM ou falar na tela.
2. 💬 **MENSAGEM DE JOGADOR**: Detecta quando outro player enviar mensagem privada ou falar próximo a você.
3. ⚡ **TELEPORT**: Reconhece o som de teleporte (GM ou player teleportando na área).
4. 🐉 **POKEMON FORA DA HUNT**: Notifica spawns raros, shinies ou pokémon fora do padrão da hunt.
5. 🎯 **SOM DE SETA**: Identifica sons de mira, alvo travado ou disparo de setas/ataques.

---

## 🚀 Como Executar

### 1. Clonar e Instalar Dependências
```bash
git clone https://github.com/Lushen16/BotNotifier.git
cd BotNotifier
npm install
```

### 2. Iniciar a Aplicação
```bash
npm start
```
Acesse no seu navegador: **[http://localhost:3333](http://localhost:3333)**

---

## 🎮 Como Usar

1. **Ligue a Leitura de Áudio**:
   - Clique em **"Ligar Leitura de Áudio"**.
   - Na janela de compartilhamento, selecione a tela/jogo e **marque a caixa "Compartilhar áudio do sistema"**.
   - *(O vídeo é desligado na hora para consumir zero CPU, mantendo apenas a leitura sonora ativa)*.

2. **Captura Automática (Auto-Slicer)**:
   - Conforme os sons tocam no jogo, o sistema detecta os picos acústicos e gera blocos de sons fatiados na **"Bandeja de Sons Capturados"**.
   - Clique em **▶️** para escutar e validar a amostra capturada.

3. **Arrastar e Soltar (Drag & Drop)**:
   - **Arraste o card do som** diretamente para uma das 5 categorias desejadas (Mensagem GM, Jogador, Teleport, etc.).
   - Pronto! O som fica memorizado no navegador via `IndexedDB`.

4. **Alertas no Discord**:
   - Insira a **URL do Webhook do Discord** no painel lateral.
   - Configure menções opcionais (`@everyone`, `@here` ou ID de cargo).
   - Quando aquele som tocar novamente no jogo, o BOT NOTIFIER identifica o padrão e envia a notificação no canal do Discord!

---

## 🛠️ Tecnologias Utilizadas

- **Web Audio API**: Análise espectral contínua e FFT de 24 bandas perceptuais logarítmicas.
- **Auto Sound Slicer**: Detector de transientes e corte automático de silêncios em tempo real.
- **HTML5 Drag & Drop**: Interface intuitiva para classificar amostras sonoras.
- **IndexedDB & LocalStorage**: Persistência de assinaturas de áudio e configurações offline.
- **Node.js & Express**: Servidor e relay seguro de Webhook do Discord.
- **Cyberpunk Dark Theme**: Interface responsiva e animada de alto desempenho.
