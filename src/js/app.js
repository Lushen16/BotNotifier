/**
 * CHARM BOT NOTIFIER - Controlador Principal da Aplicação
 * Gerenciamento de Sons Capturados, Drag & Drop, Classificação e Alertas Discord
 */

class CharmNotifierApp {
  constructor() {
    this.triggers = [];
    this.capturedSounds = []; // Bandeja de sons capturados recentemente
    this.currentDraggedSoundId = null;

    this.settings = {
      webhookUrl: '',
      mentionType: 'none',
      mentionId: '',
      playLocalSound: true,
      screenFlash: true,
      autoCapture: true
    };

    this.visualizer = null;
    this.audioEngine = window.sentinelAudioEngine;
    this.storage = window.sentinelStorage;
    this.discord = window.sentinelDiscord;
  }

  async init() {
    console.log('Inicializando CHARM BOT NOTIFIER...');

    // Inicializar visualizador canvas
    this.visualizer = new window.SentinelVisualizer('audioVisualizer');
    this.visualizer.drawIdle();

    // Carregar configurações salvas
    await this.loadSettings();

    // Carregar ou inicializar categorias padrão
    await this.loadTriggers();

    // Configurar listeners do motor de áudio
    this.setupAudioEngineCallbacks();

    // Configurar listeners de interface
    this.setupEventListeners();

    // Renderizar categorias e bandeja de sons
    this.renderTriggers();
    this.renderCapturedSounds();

    this.logActivity('CHARM BOT NOTIFIER inicializado com sucesso.', 'info');
  }

  setupAudioEngineCallbacks() {
    // Atualização de semelhança ao vivo
    this.audioEngine.onMatchUpdate = (triggerId, confidence) => {
      this.updateTriggerMeter(triggerId, confidence);
    };

    // Detecção de som correspondente
    this.audioEngine.onDetection = async (trigger, confidence) => {
      await this.handleSoundDetected(trigger, confidence);
    };

    // Mudança de status da leitura de áudio
    this.audioEngine.onStatusChange = (status, message) => {
      this.updateConnectionStatus(status, message);
    };

    // Auto-Slicer: Som do sistema detectado e capturado automaticamente!
    this.audioEngine.onSoundCaptured = (capturedItem) => {
      this.addCapturedSound(capturedItem);
    };
  }

  /**
   * Adiciona um novo som capturado na bandeja para arrastar
   */
  addCapturedSound(sound) {
    this.capturedSounds.unshift(sound);
    if (this.capturedSounds.length > 20) {
      this.capturedSounds.pop();
    }
    this.renderCapturedSounds();
    this.showToast(`🎵 Novo som capturado do jogo (${sound.duration.toFixed(1)}s)! Arraste para a categoria desejada.`, 'info');
    this.logActivity(`Som capturado do sistema: <strong>${sound.name}</strong> (${sound.duration.toFixed(1)}s)`, 'info');
  }

  /**
   * Simula um som capturado automaticamente para testes imediatos de arrastar e soltar
   */
  async simulateCapturedSound() {
    const tones = ['gm', 'msg_player', 'teleport', 'pokemon', 'seta'];
    const randomTone = tones[Math.floor(Math.random() * tones.length)];
    const names = {
      gm: 'Mensagem GM (Amostra)',
      msg_player: 'Mensagem Jogador (Amostra)',
      teleport: 'Teleport (Amostra)',
      pokemon: 'Pokemon Fora Hunt (Amostra)',
      seta: 'Som de Seta (Amostra)'
    };

    const preset = await this.audioEngine.generatePresetTone(randomTone);
    const sampleItem = {
      id: `sound_${Date.now()}_${Math.random().toString(36).substr(2, 4)}`,
      name: names[randomTone] || `Som #${Date.now().toString().slice(-4)}`,
      timestamp: new Date().toLocaleTimeString('pt-BR'),
      duration: preset.duration,
      audioDataUrl: preset.audioDataUrl,
      signature: preset.signature
    };

    this.addCapturedSound(sampleItem);
  }

  /**
   * Renderiza a bandeja de sons capturados
   */
  renderCapturedSounds() {
    const container = document.getElementById('capturedSoundsTray');
    const emptyNotice = document.getElementById('trayEmptyNotice');
    if (!container) return;

    if (this.capturedSounds.length === 0) {
      if (emptyNotice) emptyNotice.style.display = 'block';
      container.innerHTML = '';
      return;
    }

    if (emptyNotice) emptyNotice.style.display = 'none';
    container.innerHTML = '';

    this.capturedSounds.forEach((sound, index) => {
      const card = document.createElement('div');
      card.className = 'captured-sound-card';
      card.id = `captured_${sound.id}`;
      card.setAttribute('draggable', 'true');

      card.addEventListener('dragstart', (e) => this.handleDragStart(e, sound.id));
      card.addEventListener('dragend', () => this.handleDragEnd(card));

      card.innerHTML = `
        <div class="drag-handle" title="Segure e arraste este som para uma categoria abaixo">
          <i class="fas fa-grip-vertical"></i>
        </div>
        <div class="captured-sound-info">
          <div class="captured-sound-title">
            <strong>${sound.name}</strong>
            <span class="captured-sound-time">${sound.timestamp}</span>
          </div>
          <div class="captured-sound-meta">
            <span class="badge badge-info"><i class="fas fa-clock"></i> ${sound.duration.toFixed(2)}s</span>
          </div>
        </div>
        <div class="captured-sound-actions">
          <button class="btn btn-sm btn-outline" onclick="app.previewCapturedSound('${sound.id}')" title="Ouvir som capturado">
            <i class="fas fa-play"></i>
          </button>
          <div class="quick-assign-dropdown">
            <button class="btn btn-sm btn-primary" onclick="app.toggleQuickAssignMenu('${sound.id}')" title="Classificar para uma categoria">
              <i class="fas fa-tag"></i> Classificar
            </button>
            <div class="quick-menu" id="quickMenu_${sound.id}">
              ${this.triggers.map(t => `
                <div class="quick-menu-item" onclick="app.assignSoundToTrigger('${sound.id}', '${t.id}')">
                  ${t.icon} ${t.name}
                </div>
              `).join('')}
            </div>
          </div>
          <button class="btn-text-danger" onclick="app.removeCapturedSound('${sound.id}')" title="Remover da lista">
            <i class="fas fa-trash-alt"></i>
          </button>
        </div>
      `;

      container.appendChild(card);
    });
  }

  toggleQuickAssignMenu(soundId) {
    const menu = document.getElementById(`quickMenu_${soundId}`);
    if (!menu) return;
    const isOpen = menu.classList.contains('is-open');
    document.querySelectorAll('.quick-menu').forEach(m => m.classList.remove('is-open'));
    if (!isOpen) menu.classList.add('is-open');
  }

  previewCapturedSound(soundId) {
    const sound = this.capturedSounds.find(s => s.id === soundId);
    if (sound && sound.audioDataUrl) {
      this.audioEngine.playAudioUrl(sound.audioDataUrl);
    }
  }

  removeCapturedSound(soundId) {
    this.capturedSounds = this.capturedSounds.filter(s => s.id !== soundId);
    this.renderCapturedSounds();
  }

  clearAllCapturedSounds() {
    this.capturedSounds = [];
    this.renderCapturedSounds();
    this.showToast('Bandeja de sons limpa.', 'info');
  }

  /* ================= DRAG AND DROP ================= */

  handleDragStart(e, soundId) {
    this.currentDraggedSoundId = soundId;
    e.dataTransfer.setData('text/plain', soundId);
    e.dataTransfer.effectAllowed = 'copyMove';

    const card = document.getElementById(`captured_${soundId}`);
    if (card) card.classList.add('is-dragging');

    // Destaca as zonas de drop
    document.querySelectorAll('.drop-target-area').forEach(el => {
      el.classList.add('can-drop-highlight');
    });
  }

  handleDragEnd(card) {
    this.currentDraggedSoundId = null;
    if (card) card.classList.remove('is-dragging');

    document.querySelectorAll('.drop-target-area').forEach(el => {
      el.classList.remove('can-drop-highlight', 'is-drag-over');
    });
  }

  handleDragOver(e, triggerId) {
    e.preventDefault();
    e.dataTransfer.dropEffect = 'copy';
    const dropArea = document.getElementById(`dropArea_${triggerId}`);
    if (dropArea) dropArea.classList.add('is-drag-over');
  }

  handleDragLeave(e, triggerId) {
    const dropArea = document.getElementById(`dropArea_${triggerId}`);
    if (dropArea) dropArea.classList.remove('is-drag-over');
  }

  async handleDrop(e, triggerId) {
    e.preventDefault();
    const soundId = e.dataTransfer.getData('text/plain') || this.currentDraggedSoundId;
    const dropArea = document.getElementById(`dropArea_${triggerId}`);
    if (dropArea) dropArea.classList.remove('is-drag-over');

    if (!soundId) return;
    await this.assignSoundToTrigger(soundId, triggerId);
  }

  /**
   * Associa um som capturado a um gatilho/categoria de alerta
   */
  async assignSoundToTrigger(soundId, triggerId) {
    const sound = this.capturedSounds.find(s => s.id === soundId);
    const trigger = this.triggers.find(t => t.id === triggerId);

    if (!sound || !trigger) return;

    // Atualizar dados do gatilho
    trigger.signature = sound.signature;
    trigger.audioDataUrl = sound.audioDataUrl;
    trigger.duration = sound.duration;
    trigger.enabled = true; // Ativa automaticamente o gatilho ao definir o som!

    await this.storage.saveTrigger(trigger);
    this.audioEngine.setTriggers(this.triggers);

    this.showToast(`🎯 Som classificado com sucesso como "${trigger.name}"! Monitoramento ativo.`, 'success');
    this.logActivity(`Som classificado: <strong>${sound.name}</strong> atribuído a <strong>${trigger.name}</strong>`, 'success');

    this.renderTriggers();
  }

  /* ================= GATILHOS E CATEGORIAS ================= */

  async loadTriggers() {
    let saved = await this.storage.getAllTriggers();

    const expectedIds = ['trigger_gm', 'trigger_msg_player', 'trigger_teleport', 'trigger_pokemon', 'trigger_seta'];
    const hasAll5 = saved && expectedIds.every(id => saved.some(t => t.id === id));

    if (!saved || saved.length === 0 || !hasAll5) {
      console.log('Criando as 5 categorias padrão com sons iniciais...');

      const gmPreset = await this.audioEngine.generatePresetTone('gm');
      const msgPlayerPreset = await this.audioEngine.generatePresetTone('msg_player');
      const teleportPreset = await this.audioEngine.generatePresetTone('teleport');
      const pokePreset = await this.audioEngine.generatePresetTone('pokemon');
      const setaPreset = await this.audioEngine.generatePresetTone('seta');

      const defaultTriggers = [
        {
          id: 'trigger_gm',
          name: 'MENSAGEM GM',
          description: 'Aviso imediato quando staff ou GM mandar mensagem ou falar na tela',
          icon: '🚨',
          color: '#ff0844',
          threshold: 82,
          cooldown: 6,
          enabled: true,
          signature: gmPreset.signature,
          audioDataUrl: gmPreset.audioDataUrl,
          duration: gmPreset.duration,
          isPreset: true
        },
        {
          id: 'trigger_msg_player',
          name: 'MENSAGEM DE JOGADOR',
          description: 'Aviso quando outro jogador enviar mensagem privada (PM) ou falar por perto',
          icon: '💬',
          color: '#ffb703',
          threshold: 78,
          cooldown: 6,
          enabled: true,
          signature: msgPlayerPreset.signature,
          audioDataUrl: msgPlayerPreset.audioDataUrl,
          duration: msgPlayerPreset.duration,
          isPreset: true
        },
        {
          id: 'trigger_teleport',
          name: 'TELEPORT',
          description: 'Detecta o som característico de teleporte (GM ou player teleportando)',
          icon: '⚡',
          color: '#a855f7',
          threshold: 80,
          cooldown: 5,
          enabled: true,
          signature: teleportPreset.signature,
          audioDataUrl: teleportPreset.audioDataUrl,
          duration: teleportPreset.duration,
          isPreset: true
        },
        {
          id: 'trigger_pokemon',
          name: 'POKEMON FORA DA HUNT',
          description: 'Avisa quando spawna um Pokémon raro, shiny ou som fora do padrão da hunt',
          icon: '🐉',
          color: '#00f2fe',
          threshold: 80,
          cooldown: 10,
          enabled: true,
          signature: pokePreset.signature,
          audioDataUrl: pokePreset.audioDataUrl,
          duration: pokePreset.duration,
          isPreset: true
        },
        {
          id: 'trigger_seta',
          name: 'SOM DE SETA',
          description: 'Alerta instantâneo quando uma seta de alvo, target ou ataque for disparada',
          icon: '🎯',
          color: '#ff4d00',
          threshold: 80,
          cooldown: 5,
          enabled: true,
          signature: setaPreset.signature,
          audioDataUrl: setaPreset.audioDataUrl,
          duration: setaPreset.duration,
          isPreset: true
        }
      ];

      for (const t of defaultTriggers) {
        await this.storage.saveTrigger(t);
      }

      saved = defaultTriggers;
    }

    this.triggers = saved;
    this.audioEngine.setTriggers(this.triggers);
  }

  renderTriggers() {
    const container = document.getElementById('triggersGrid');
    if (!container) return;

    container.innerHTML = '';

    this.triggers.forEach(trigger => {
      const hasAudio = trigger.signature && trigger.signature.length > 0;
      const card = document.createElement('div');
      card.className = `trigger-card ${trigger.enabled ? 'is-enabled' : 'is-disabled'}`;
      card.id = `card_${trigger.id}`;
      card.style.setProperty('--trigger-color', trigger.color || '#ff0844');

      card.innerHTML = `
        <div class="card-header">
          <div class="trigger-title-group">
            <span class="trigger-icon">${trigger.icon || '🔔'}</span>
            <div>
              <h3 class="trigger-name">${trigger.name}</h3>
              <p class="trigger-desc">${trigger.description || 'Categoria de alerta ativa'}</p>
            </div>
          </div>
          <div class="trigger-toggle-wrap">
            <label class="switch" title="${trigger.enabled ? 'Alerta Ativado' : 'Alerta Desativado'}">
              <input type="checkbox" ${trigger.enabled ? 'checked' : ''} onchange="app.toggleTrigger('${trigger.id}', this.checked)">
              <span class="slider round"></span>
            </label>
          </div>
        </div>

        <!-- Área de Drop Alvo para Arrastar o Som -->
        <div class="drop-target-area ${hasAudio ? 'has-sound' : 'needs-sound'}" 
             id="dropArea_${trigger.id}"
             ondragover="app.handleDragOver(event, '${trigger.id}')"
             ondragleave="app.handleDragLeave(event, '${trigger.id}')"
             ondrop="app.handleDrop(event, '${trigger.id}')">
          <div class="drop-target-content">
            <i class="fas ${hasAudio ? 'fa-check-circle' : 'fa-hand-point-down'} drop-icon"></i>
            <div class="drop-target-text">
              <strong>${hasAudio ? `Som Definido (${(trigger.duration || 0).toFixed(1)}s)` : 'Arraste um Som Aqui'}</strong>
              <small>${hasAudio ? 'Solte outro som para substituir' : `Solte aqui para classificar como ${trigger.name}`}</small>
            </div>
            ${hasAudio ? `
              <button class="btn btn-sm btn-outline btn-round" onclick="event.stopPropagation(); app.previewSound('${trigger.id}')" title="Ouvir som classificado">
                <i class="fas fa-play"></i>
              </button>
            ` : ''}
          </div>
        </div>

        <!-- Indicador de Semelhança ao Vivo -->
        <div class="meter-section">
          <div class="meter-header">
            <span class="meter-label"><i class="fas fa-radar"></i> Semelhança no Áudio</span>
            <span class="meter-value" id="val_${trigger.id}">0%</span>
          </div>
          <div class="meter-bar-track">
            <div class="meter-bar-fill" id="fill_${trigger.id}"></div>
            <div class="meter-threshold-line" style="left: ${trigger.threshold}%;" title="Limiar de Ativação: ${trigger.threshold}%"></div>
          </div>
        </div>

        <!-- Sliders de Sensibilidade e Cooldown -->
        <div class="trigger-controls">
          <div class="control-row">
            <label>
              <span>Sensibilidade (${trigger.threshold}%)</span>
              <small>Mínimo de semelhança</small>
            </label>
            <input type="range" min="50" max="98" value="${trigger.threshold}" 
                   oninput="app.updateTriggerThreshold('${trigger.id}', this.value)">
          </div>

          <div class="control-row">
            <label>
              <span>Cooldown (${trigger.cooldown}s)</span>
              <small>Pausa entre avisos</small>
            </label>
            <input type="range" min="2" max="60" value="${trigger.cooldown}" 
                   oninput="app.updateTriggerCooldown('${trigger.id}', this.value)">
          </div>
        </div>

        <!-- Ações Auxiliares -->
        <div class="card-extra-actions">
          <label class="btn btn-sm btn-outline btn-upload" title="Fazer upload de arquivo direto para este alerta">
            <i class="fas fa-upload"></i> Upload
            <input type="file" accept="audio/*" onchange="app.handleAudioUpload('${trigger.id}', this.files[0])" style="display:none;">
          </label>
          <button class="btn btn-sm btn-warning" onclick="app.simulateDetection('${trigger.id}')" title="Testar disparo deste alerta no Discord">
            <i class="fas fa-bolt"></i> Testar Alerta
          </button>
          ${!trigger.isPreset ? `
            <button class="btn-text-danger" onclick="app.removeTrigger('${trigger.id}')" title="Excluir alerta">
              <i class="fas fa-trash-alt"></i>
            </button>
          ` : ''}
        </div>
      `;

      container.appendChild(card);
    });
  }

  updateTriggerMeter(triggerId, confidence) {
    const fillEl = document.getElementById(`fill_${triggerId}`);
    const valEl = document.getElementById(`val_${triggerId}`);
    if (!fillEl || !valEl) return;

    fillEl.style.width = `${confidence}%`;
    valEl.textContent = `${confidence}%`;

    const trigger = this.triggers.find(t => t.id === triggerId);
    if (trigger && confidence >= trigger.threshold) {
      fillEl.classList.add('is-firing');
    } else {
      fillEl.classList.remove('is-firing');
    }
  }

  async handleSoundDetected(trigger, confidence) {
    console.log(`🚨 DISPARO DE ALERTA: ${trigger.name} (${confidence}%)`);

    const card = document.getElementById(`card_${trigger.id}`);
    if (card) {
      card.classList.add('is-alerting');
      setTimeout(() => card.classList.remove('is-alerting'), 2500);
    }

    if (this.settings.screenFlash) {
      document.body.classList.add('screen-alert-flash');
      setTimeout(() => document.body.classList.remove('screen-alert-flash'), 1200);
    }

    if (this.settings.playLocalSound) {
      this.audioEngine.playLocalAlertSiren();
    }

    const logId = this.logActivity(`Alerta Identificado: <strong>${trigger.name}</strong> (${confidence}%)`, 'alert');

    if (this.settings.webhookUrl) {
      try {
        await this.discord.sendAlert(trigger, confidence, this.settings);
        this.updateLogStatus(logId, '✅ Discord Notificado');
        this.showToast(`🚨 Alerta de ${trigger.name} enviado ao Discord!`, 'success');
      } catch (err) {
        console.error('Erro ao notificar Discord:', err);
        this.updateLogStatus(logId, '❌ Falha no Discord');
        this.showToast(`Erro ao enviar ao Discord: ${err.message}`, 'error');
      }
    } else {
      this.updateLogStatus(logId, '⚠️ Sem Webhook');
      this.showToast(`Alerta ${trigger.name} detectado! Configure o Webhook do Discord na lateral.`, 'warning');
    }
  }

  async toggleSystemAudio() {
    const btn = document.getElementById('connectAudioBtn');

    if (this.audioEngine.isConnected) {
      this.audioEngine.disconnect();
      this.visualizer.stop();
      this.updateConnectionStatus('disconnected', 'Leitura de áudio desativada.');
    } else {
      try {
        btn.disabled = true;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Conectando...';

        await this.audioEngine.connectSystemAudio();
        this.visualizer.start(this.audioEngine.analyserNode);
        this.showToast('Leitura de áudio ATIVA! O sistema capturará os sons automaticamente.', 'success');
      } catch (err) {
        this.showHelpModal();
        this.showToast(err.message || 'Falha ao ativar leitura de áudio.', 'error');
      } finally {
        btn.disabled = false;
      }
    }
  }

  updateConnectionStatus(status, message) {
    const statusBadge = document.getElementById('audioStatusBadge');
    const connectBtn = document.getElementById('connectAudioBtn');

    if (status === 'connected') {
      statusBadge.className = 'status-badge status-connected';
      statusBadge.innerHTML = '<span class="status-dot"></span> Leitura de Áudio: LIGADA';
      connectBtn.className = 'btn btn-danger';
      connectBtn.innerHTML = '<i class="fas fa-stop"></i> Desligar Leitura de Áudio';
    } else {
      statusBadge.className = 'status-badge status-disconnected';
      statusBadge.innerHTML = '<span class="status-dot"></span> Leitura de Áudio: DESLIGADA';
      connectBtn.className = 'btn btn-primary btn-pulse';
      connectBtn.innerHTML = '<i class="fas fa-volume-up"></i> Ligar Leitura de Áudio';
    }

    if (message) {
      this.logActivity(message, status === 'connected' ? 'success' : 'info');
    }
  }

  async toggleTrigger(id, enabled) {
    const trigger = this.triggers.find(t => t.id === id);
    if (!trigger) return;

    trigger.enabled = enabled;
    await this.storage.saveTrigger(trigger);
    this.audioEngine.setTriggers(this.triggers);

    const card = document.getElementById(`card_${id}`);
    if (card) {
      card.classList.toggle('is-enabled', enabled);
      card.classList.toggle('is-disabled', !enabled);
    }
  }

  async updateTriggerThreshold(id, value) {
    const trigger = this.triggers.find(t => t.id === id);
    if (!trigger) return;

    trigger.threshold = parseInt(value, 10);
    await this.storage.saveTrigger(trigger);
    this.audioEngine.setTriggers(this.triggers);

    const card = document.getElementById(`card_${id}`);
    if (card) {
      const line = card.querySelector('.meter-threshold-line');
      if (line) line.style.left = `${trigger.threshold}%`;
      const label = card.querySelector('.control-row label span');
      if (label) label.textContent = `Sensibilidade (${trigger.threshold}%)`;
    }
  }

  async updateTriggerCooldown(id, value) {
    const trigger = this.triggers.find(t => t.id === id);
    if (!trigger) return;

    trigger.cooldown = parseInt(value, 10);
    await this.storage.saveTrigger(trigger);
    this.audioEngine.setTriggers(this.triggers);

    const card = document.getElementById(`card_${id}`);
    if (card) {
      const rows = card.querySelectorAll('.control-row label span');
      if (rows[1]) rows[1].textContent = `Cooldown (${trigger.cooldown}s)`;
    }
  }

  previewSound(id) {
    const trigger = this.triggers.find(t => t.id === id);
    if (!trigger || !trigger.audioDataUrl) {
      this.showToast('Nenhum som gravado para este alerta.', 'warning');
      return;
    }
    this.audioEngine.playAudioUrl(trigger.audioDataUrl);
  }

  async simulateDetection(id) {
    const trigger = this.triggers.find(t => t.id === id);
    if (!trigger) return;
    this.showToast(`Simulando alerta de "${trigger.name}"...`, 'info');
    await this.handleSoundDetected(trigger, 95);
  }

  async handleAudioUpload(triggerId, file) {
    if (!file) return;
    try {
      this.showToast(`Processando áudio "${file.name}"...`, 'info');
      const result = await this.audioEngine.processAudioFile(file);

      const trigger = this.triggers.find(t => t.id === triggerId);
      if (!trigger) return;

      trigger.signature = result.signature;
      trigger.audioDataUrl = result.audioDataUrl;
      trigger.duration = result.duration;
      trigger.enabled = true;

      await this.storage.saveTrigger(trigger);
      this.audioEngine.setTriggers(this.triggers);

      this.showToast(`Áudio atribuído a "${trigger.name}"!`, 'success');
      this.renderTriggers();
    } catch (err) {
      this.showToast(`Erro ao carregar áudio: ${err.message}`, 'error');
    }
  }

  openAddTriggerModal() {
    document.getElementById('newTriggerName').value = '';
    document.getElementById('newTriggerDesc').value = '';
    document.getElementById('addTriggerModal').classList.add('is-open');
  }

  closeAddTriggerModal() {
    document.getElementById('addTriggerModal').classList.remove('is-open');
  }

  async createCustomTrigger() {
    const name = document.getElementById('newTriggerName').value.trim();
    const desc = document.getElementById('newTriggerDesc').value.trim();
    const icon = document.getElementById('newTriggerIcon').value || '🔔';
    const color = document.getElementById('newTriggerColor').value || '#a855f7';

    if (!name) {
      this.showToast('Informe um nome para a categoria!', 'warning');
      return;
    }

    const newTrigger = {
      id: `trigger_${Date.now()}`,
      name: name.toUpperCase(),
      description: desc || 'Alerta customizado',
      icon: icon,
      color: color,
      threshold: 80,
      cooldown: 8,
      enabled: true,
      signature: [],
      audioDataUrl: null,
      duration: 0,
      isPreset: false
    };

    this.triggers.push(newTrigger);
    await this.storage.saveTrigger(newTrigger);
    this.audioEngine.setTriggers(this.triggers);

    this.closeAddTriggerModal();
    this.renderTriggers();
    this.showToast(`Categoria "${name}" criada! Arraste um som capturado para ativá-la.`, 'success');
  }

  async removeTrigger(id) {
    const trigger = this.triggers.find(t => t.id === id);
    if (!trigger) return;

    if (!confirm(`Deseja excluir a categoria "${trigger.name}"?`)) return;

    await this.storage.deleteTrigger(id);
    this.triggers = this.triggers.filter(t => t.id !== id);
    this.audioEngine.setTriggers(this.triggers);

    this.renderTriggers();
    this.showToast(`Categoria "${trigger.name}" excluída.`, 'info');
  }

  async resetDefaultPresets() {
    if (!confirm('Deseja restaurar as 5 categorias fixas padrão (MENSAGEM GM, MENSAGEM DE JOGADOR, TELEPORT, POKEMON FORA DA HUNT, SOM DE SETA)?')) return;

    for (const t of this.triggers) {
      await this.storage.deleteTrigger(t.id);
    }
    this.triggers = [];
    await this.loadTriggers();
    this.renderTriggers();
    this.showToast('As 5 categorias fixas foram restauradas com sucesso!', 'success');
  }

  async loadSettings() {
    this.settings.webhookUrl = await this.storage.getSetting('webhookUrl', '');
    this.settings.mentionType = await this.storage.getSetting('mentionType', 'none');
    this.settings.mentionId = await this.storage.getSetting('mentionId', '');
    this.settings.playLocalSound = await this.storage.getSetting('playLocalSound', true);
    this.settings.screenFlash = await this.storage.getSetting('screenFlash', true);

    document.getElementById('webhookUrlInput').value = this.settings.webhookUrl;
    document.getElementById('mentionTypeSelect').value = this.settings.mentionType;
    document.getElementById('mentionIdInput').value = this.settings.mentionId;
    document.getElementById('localSoundToggle').checked = this.settings.playLocalSound;
    document.getElementById('screenFlashToggle').checked = this.settings.screenFlash;

    this.toggleMentionIdVisibility();
  }

  async saveSettings() {
    this.settings.webhookUrl = document.getElementById('webhookUrlInput').value.trim();
    this.settings.mentionType = document.getElementById('mentionTypeSelect').value;
    this.settings.mentionId = document.getElementById('mentionIdInput').value.trim();
    this.settings.playLocalSound = document.getElementById('localSoundToggle').checked;
    this.settings.screenFlash = document.getElementById('screenFlashToggle').checked;

    await this.storage.saveSetting('webhookUrl', this.settings.webhookUrl);
    await this.storage.saveSetting('mentionType', this.settings.mentionType);
    await this.storage.saveSetting('mentionId', this.settings.mentionId);
    await this.storage.saveSetting('playLocalSound', this.settings.playLocalSound);
    await this.storage.saveSetting('screenFlash', this.settings.screenFlash);

    this.showToast('Configurações salvas!', 'success');
  }

  async testDiscordWebhook() {
    const webhookUrl = document.getElementById('webhookUrlInput').value.trim();
    const mentionType = document.getElementById('mentionTypeSelect').value;
    const mentionId = document.getElementById('mentionIdInput').value.trim();
    const btn = document.getElementById('testWebhookBtn');

    if (!webhookUrl) {
      this.showToast('Informe a URL do Webhook do Discord!', 'warning');
      return;
    }

    try {
      btn.disabled = true;
      btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Enviando...';

      await this.discord.sendTestNotification(webhookUrl, mentionType, mentionId);
      this.showToast('✅ Mensagem de teste entregue no Discord com sucesso!', 'success');
      this.logActivity('Teste de Webhook Discord enviado.', 'success');
    } catch (err) {
      this.showToast(`Falha no Discord: ${err.message}`, 'error');
      this.logActivity(`Erro no Discord: ${err.message}`, 'error');
    } finally {
      btn.disabled = false;
      btn.innerHTML = '<i class="fas fa-paper-plane"></i> Testar Webhook';
    }
  }

  showHelpModal() {
    document.getElementById('helpModal').classList.add('is-open');
  }

  closeHelpModal() {
    document.getElementById('helpModal').classList.remove('is-open');
  }

  toggleMentionIdVisibility() {
    const val = document.getElementById('mentionTypeSelect').value;
    const group = document.getElementById('mentionIdGroup');
    const label = document.getElementById('mentionIdLabel');

    if (val === 'role') {
      group.style.display = 'block';
      label.textContent = 'ID do Cargo no Discord:';
    } else if (val === 'user') {
      group.style.display = 'block';
      label.textContent = 'ID do Usuário no Discord:';
    } else {
      group.style.display = 'none';
    }
  }

  logActivity(message, type = 'info') {
    const feed = document.getElementById('activityFeed');
    if (!feed) return null;

    const id = `log_${Date.now()}_${Math.random().toString(36).substr(2, 4)}`;
    const timeStr = new Date().toLocaleTimeString('pt-BR');

    const item = document.createElement('div');
    item.className = `activity-item activity-${type}`;
    item.id = id;

    let icon = 'info-circle';
    if (type === 'alert') icon = 'bell';
    if (type === 'success') icon = 'check-circle';
    if (type === 'error') icon = 'exclamation-circle';

    item.innerHTML = `
      <span class="activity-time">${timeStr}</span>
      <span class="activity-icon"><i class="fas fa-${icon}"></i></span>
      <span class="activity-content">${message}</span>
      <span class="activity-badge" id="badge_${id}"></span>
    `;

    feed.insertBefore(item, feed.firstChild);

    if (feed.children.length > 40) {
      feed.removeChild(feed.lastChild);
    }

    return id;
  }

  updateLogStatus(logId, text) {
    const badge = document.getElementById(`badge_${logId}`);
    if (badge) {
      badge.textContent = text;
      badge.style.display = 'inline-block';
    }
  }

  showToast(message, type = 'info') {
    const toast = document.getElementById('toast');
    if (!toast) return;

    toast.textContent = message;
    toast.className = `toast toast-${type} is-visible`;

    clearTimeout(this.toastTimer);
    this.toastTimer = setTimeout(() => {
      toast.classList.remove('is-visible');
    }, 4000);
  }

  setupEventListeners() {
    document.getElementById('connectAudioBtn').addEventListener('click', () => this.toggleSystemAudio());
    document.getElementById('helpAudioBtn').addEventListener('click', () => this.showHelpModal());
    document.getElementById('saveSettingsBtn').addEventListener('click', () => this.saveSettings());
    document.getElementById('testWebhookBtn').addEventListener('click', () => this.testDiscordWebhook());
    document.getElementById('mentionTypeSelect').addEventListener('change', () => this.toggleMentionIdVisibility());
    document.getElementById('addNewTriggerBtn').addEventListener('click', () => this.openAddTriggerModal());
    document.getElementById('resetPresetsBtn').addEventListener('click', () => this.resetDefaultPresets());
    document.getElementById('clearTrayBtn').addEventListener('click', () => this.clearAllCapturedSounds());

    // Fechar menus ao clicar fora
    document.addEventListener('click', (e) => {
      if (!e.target.closest('.quick-assign-dropdown')) {
        document.querySelectorAll('.quick-menu').forEach(m => m.classList.remove('is-open'));
      }
    });
  }
}

window.addEventListener('DOMContentLoaded', () => {
  window.app = new CharmNotifierApp();
  window.app.init();
});
