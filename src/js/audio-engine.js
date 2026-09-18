/**
 * CHARM BOT NOTIFIER - Audio Engine
 * Captura Contínua do Áudio do Sistema, Detecção Automática de Sons (Auto-Slicer),
 * Fingerprinting Espectral e Comparação em Tempo Real.
 */

class SentinelAudioEngine {
  constructor() {
    this.audioCtx = null;
    this.mediaStream = null;
    this.sourceNode = null;
    this.analyserNode = null;
    this.scriptProcessor = null;
    
    this.isConnected = false;
    this.isListening = false;
    
    // Configurações de análise
    this.fftSize = 512;
    this.bandCount = 24;
    this.frameIntervalMs = 25;
    
    // Histórico de espectro deslizante (últimos 6 segundos = 240 frames)
    this.maxHistoryFrames = 240;
    this.liveHistory = [];
    
    // Auto-Slicer (Detecção e fatiamento automático de novos sons do jogo)
    this.autoCaptureEnabled = true;
    this.silenceThreshold = 0.025; // Sensibilidade para detectar barulho
    this.isSlicing = false;
    this.currentSliceSamples = [];
    this.sliceSilenceCount = 0;
    this.sliceSampleRate = 44100;
    this.preRollBuffer = []; // Guarda últimos 150ms para não cortar o ataque inicial do som
    this.maxPreRollSamples = 6615; // ~150ms a 44.1kHz
    this.lastSoundEndTime = 0;
    
    // Callbacks
    this.onMatchUpdate = null;   // (triggerId, confidence)
    this.onDetection = null;     // (trigger, confidence)
    this.onStatusChange = null;  // (status, message)
    this.onSoundCaptured = null; // (capturedSoundObject)
    
    // Loop de análise
    this.analysisInterval = null;
    
    // Triggers carregados em memória
    this.triggers = [];
  }

  /**
   * Conecta ao áudio do sistema operacional via getDisplayMedia
   */
  async connectSystemAudio() {
    try {
      if (this.mediaStream) {
        this.disconnect();
      }

      this.mediaStream = await navigator.mediaDevices.getDisplayMedia({
        video: {
          displaySurface: 'monitor',
          width: { max: 1 },
          height: { max: 1 },
          frameRate: { max: 1 }
        },
        audio: {
          echoCancellation: false,
          noiseSuppression: false,
          autoGainControl: false,
          channelCount: 2
        }
      });

      const audioTracks = this.mediaStream.getAudioTracks();
      if (!audioTracks || audioTracks.length === 0) {
        this.mediaStream.getTracks().forEach(t => t.stop());
        throw new Error('Nenhum áudio do sistema foi selecionado! Marque "Compartilhar áudio do sistema" ao escolher a tela.');
      }

      // Desliga o vídeo para uso mínimo de CPU
      const videoTracks = this.mediaStream.getVideoTracks();
      videoTracks.forEach(track => track.stop());

      audioTracks[0].onended = () => {
        this.disconnect();
        if (this.onStatusChange) {
          this.onStatusChange('disconnected', 'Leitura de áudio encerrada.');
        }
      };

      const AudioContextClass = window.AudioContext || window.webkitAudioContext;
      this.audioCtx = new AudioContextClass();
      if (this.audioCtx.state === 'suspended') {
        await this.audioCtx.resume();
      }

      this.sliceSampleRate = this.audioCtx.sampleRate;
      this.maxPreRollSamples = Math.floor(this.sliceSampleRate * 0.15); // 150ms pre-roll

      this.sourceNode = this.audioCtx.createMediaStreamSource(this.mediaStream);
      this.analyserNode = this.audioCtx.createAnalyser();
      this.analyserNode.fftSize = this.fftSize;
      this.analyserNode.smoothingTimeConstant = 0.15;

      this.sourceNode.connect(this.analyserNode);

      // Criar ScriptProcessor para monitoramento de PCM e Auto-Slicer de sons
      this.setupAutoSlicer();

      this.isConnected = true;
      this.isListening = true;
      this.startAnalysisLoop();

      if (this.onStatusChange) {
        this.onStatusChange('connected', 'Leitura de áudio ATIVA! Capturando sons do jogo em tempo real.');
      }

      return true;
    } catch (err) {
      console.error('Erro ao conectar áudio:', err);
      if (this.onStatusChange) {
        this.onStatusChange('error', err.message || 'Falha ao capturar áudio.');
      }
      throw err;
    }
  }

  /**
   * Monitor contínuo de PCM para fatiar sons automaticamente quando ocorrem
   */
  setupAutoSlicer() {
    const bufferSize = 2048;
    this.scriptProcessor = this.audioCtx.createScriptProcessor(bufferSize, 1, 1);

    this.scriptProcessor.onaudioprocess = (e) => {
      if (!this.isConnected || !this.autoCaptureEnabled) return;

      const inputData = e.inputBuffer.getChannelData(0);
      const len = inputData.length;

      // Calcular RMS (Root Mean Square) do bloco
      let sumSq = 0;
      for (let i = 0; i < len; i++) {
        sumSq += inputData[i] * inputData[i];
      }
      const rms = Math.sqrt(sumSq / len);

      // Manter buffer de pre-roll circular para não cortar o início do som
      for (let i = 0; i < len; i++) {
        this.preRollBuffer.push(inputData[i]);
      }
      if (this.preRollBuffer.length > this.maxPreRollSamples) {
        this.preRollBuffer.splice(0, this.preRollBuffer.length - this.maxPreRollSamples);
      }

      const now = Date.now();
      // Não recapturar repetidamente no mesmo milésimo de segundo
      const cooldownSinceLastSound = now - this.lastSoundEndTime;

      // Verificar se iniciou um som (volume acima do limiar)
      if (!this.isSlicing) {
        if (rms > this.silenceThreshold && cooldownSinceLastSound > 400) {
          this.isSlicing = true;
          this.sliceSilenceCount = 0;
          // Iniciar novo som com o pre-roll anterior
          this.currentSliceSamples = [...this.preRollBuffer];
          for (let i = 0; i < len; i++) {
            this.currentSliceSamples.push(inputData[i]);
          }
        }
      } else {
        // Gravando o som em andamento
        for (let i = 0; i < len; i++) {
          this.currentSliceSamples.push(inputData[i]);
        }

        // Se o volume caiu de volta para silêncio
        if (rms < this.silenceThreshold * 0.8) {
          this.sliceSilenceCount++;
        } else {
          this.sliceSilenceCount = 0;
        }

        // Finalizar som se houve ~350ms de silêncio ou atingiu o tamanho máximo (3.5s)
        const silenceBlocksNeeded = Math.ceil((this.sliceSampleRate * 0.35) / bufferSize);
        const maxSamples = Math.floor(this.sliceSampleRate * 3.5);

        if (this.sliceSilenceCount >= silenceBlocksNeeded || this.currentSliceSamples.length >= maxSamples) {
          this.finalizeSoundSlice();
        }
      }
    };

    // Conecta o nó para receber os eventos
    this.sourceNode.connect(this.scriptProcessor);
    // Conecta ao destino com volume mudo para manter o pipeline ativo sem eco local
    const muteGain = this.audioCtx.createGain();
    muteGain.gain.value = 0;
    this.scriptProcessor.connect(muteGain);
    muteGain.connect(this.audioCtx.destination);
  }

  /**
   * Finaliza um pedaço de som detectado e emite para a bandeja de arrastar
   */
  async finalizeSoundSlice() {
    this.isSlicing = false;
    this.lastSoundEndTime = Date.now();
    const rawSamples = new Float32Array(this.currentSliceSamples);
    this.currentSliceSamples = [];

    // Ignora estalos minúsculos com menos de 0.25 segundos
    const minSamples = Math.floor(this.sliceSampleRate * 0.25);
    if (rawSamples.length < minSamples) {
      return;
    }

    try {
      // Criar AudioBuffer
      const tempBuffer = this.audioCtx.createBuffer(1, rawSamples.length, this.sliceSampleRate);
      tempBuffer.getChannelData(0).set(rawSamples);

      // Cortar silêncios nas bordas
      const trimmedBuffer = this.trimSilence(tempBuffer, this.audioCtx);

      if (trimmedBuffer.duration < 0.2) return;

      // Gerar assinatura espectral
      const signature = await this.generateSignatureFromAudioBuffer(trimmedBuffer);
      const audioDataUrl = this.audioBufferToWavDataUrl(trimmedBuffer);

      const capturedItem = {
        id: `sound_${Date.now()}_${Math.random().toString(36).substr(2, 4)}`,
        name: `Som #${Date.now().toString().slice(-4)}`,
        timestamp: new Date().toLocaleTimeString('pt-BR'),
        duration: trimmedBuffer.duration,
        audioDataUrl: audioDataUrl,
        signature: signature,
        audioBuffer: trimmedBuffer
      };

      console.log('⚡ [CHARM NOTIFIER] Som capturado do sistema!', capturedItem);

      if (this.onSoundCaptured) {
        this.onSoundCaptured(capturedItem);
      }
    } catch (e) {
      console.warn('Erro ao fatiar som:', e);
    }
  }

  disconnect() {
    if (this.analysisInterval) {
      clearInterval(this.analysisInterval);
      this.analysisInterval = null;
    }

    if (this.scriptProcessor) {
      this.scriptProcessor.disconnect();
      this.scriptProcessor = null;
    }

    if (this.mediaStream) {
      this.mediaStream.getTracks().forEach(t => t.stop());
      this.mediaStream = null;
    }

    if (this.audioCtx && this.audioCtx.state !== 'closed') {
      this.audioCtx.close().catch(() => {});
      this.audioCtx = null;
    }

    this.isConnected = false;
    this.isListening = false;
    this.isSlicing = false;
    this.currentSliceSamples = [];
    this.liveHistory = [];
  }

  /**
   * Loop principal de extração espectral e comparação com os triggers
   */
  startAnalysisLoop() {
    if (this.analysisInterval) clearInterval(this.analysisInterval);

    const freqData = new Uint8Array(this.analyserNode.frequencyBinCount);

    this.analysisInterval = setInterval(() => {
      if (!this.analyserNode || !this.isListening) return;

      this.analyserNode.getByteFrequencyData(freqData);

      const bands = this.extractBands(freqData);
      
      this.liveHistory.push(bands);
      if (this.liveHistory.length > this.maxHistoryFrames) {
        this.liveHistory.shift();
      }

      this.matchActiveTriggers();
    }, this.frameIntervalMs);
  }

  extractBands(freqData) {
    const bands = new Float32Array(this.bandCount);
    const binCount = freqData.length;
    let totalEnergy = 0;

    for (let b = 0; b < this.bandCount; b++) {
      const startBin = Math.floor(Math.pow(b / this.bandCount, 1.8) * (binCount - 1));
      const endBin = Math.max(startBin + 1, Math.floor(Math.pow((b + 1) / this.bandCount, 1.8) * (binCount - 1)));
      
      let sum = 0;
      let count = 0;
      for (let i = startBin; i <= endBin && i < binCount; i++) {
        sum += freqData[i];
        count++;
      }
      const val = count > 0 ? sum / (count * 255.0) : 0;
      bands[b] = val;
      totalEnergy += val * val;
    }

    const norm = Math.sqrt(totalEnergy);
    if (norm > 0.001) {
      for (let b = 0; b < this.bandCount; b++) {
        bands[b] = bands[b] / norm;
      }
    }

    return {
      vector: bands,
      energy: norm
    };
  }

  matchActiveTriggers() {
    if (!this.triggers || this.triggers.length === 0) return;
    const now = Date.now();

    for (const trigger of this.triggers) {
      if (!trigger.enabled || !trigger.signature || trigger.signature.length === 0) {
        if (this.onMatchUpdate) this.onMatchUpdate(trigger.id, 0);
        continue;
      }

      const sigLength = trigger.signature.length;
      if (this.liveHistory.length < sigLength) {
        continue;
      }

      let bestConfidence = 0;
      const searchOffsets = [0, -1, -2, -3];

      for (const offset of searchOffsets) {
        const endIndex = this.liveHistory.length + offset;
        const startIndex = endIndex - sigLength;
        if (startIndex < 0) continue;

        const windowSlice = this.liveHistory.slice(startIndex, endIndex);
        const confidence = this.computeSpectrogramSimilarity(windowSlice, trigger.signature);
        if (confidence > bestConfidence) {
          bestConfidence = confidence;
        }
      }

      const confidencePercent = Math.min(100, Math.round(bestConfidence * 100));
      if (this.onMatchUpdate) {
        this.onMatchUpdate(trigger.id, confidencePercent);
      }

      const threshold = trigger.threshold || 80;
      const cooldownMs = (trigger.cooldown || 5) * 1000;
      const lastTriggered = trigger.lastTriggeredTime || 0;

      if (confidencePercent >= threshold && (now - lastTriggered) >= cooldownMs) {
        trigger.lastTriggeredTime = now;
        console.log(`🎯 [CHARM NOTIFIER] Alerta Disparado: ${trigger.name} (${confidencePercent}%)`);
        if (this.onDetection) {
          this.onDetection(trigger, confidencePercent);
        }
      }
    }
  }

  computeSpectrogramSimilarity(liveFrames, targetSignature) {
    const len = targetSignature.length;
    let dotProduct = 0;
    let liveNormSq = 0;
    let targetNormSq = 0;
    let totalLiveEnergy = 0;

    for (let t = 0; t < len; t++) {
      const liveVec = liveFrames[t].vector;
      const targetVec = targetSignature[t].vector;
      totalLiveEnergy += liveFrames[t].energy;

      for (let b = 0; b < this.bandCount; b++) {
        const lv = liveVec[b];
        const tv = targetVec[b];
        dotProduct += lv * tv;
        liveNormSq += lv * lv;
        targetNormSq += tv * tv;
      }
    }

    const avgEnergy = totalLiveEnergy / len;
    if (avgEnergy < 0.04) {
      return 0;
    }

    const denominator = Math.sqrt(liveNormSq) * Math.sqrt(targetNormSq);
    if (denominator <= 0.0001) return 0;

    let similarity = dotProduct / denominator;
    return Math.max(0, similarity);
  }

  async generateSignatureFromAudioBuffer(audioBuffer) {
    const channelData = audioBuffer.getChannelData(0);
    const sampleRate = audioBuffer.sampleRate;
    const frameSize = Math.floor(sampleRate * (this.frameIntervalMs / 1000));
    const totalFrames = Math.floor(channelData.length / frameSize);

    const maxFrames = Math.min(totalFrames, 200);
    const signature = [];

    for (let f = 0; f < maxFrames; f++) {
      const slice = channelData.subarray(f * frameSize, (f + 1) * frameSize);
      const bands = this.extractBandsFromPcm(slice, sampleRate);
      signature.push(bands);
    }

    return signature;
  }

  extractBandsFromPcm(samples, sampleRate) {
    const bands = new Float32Array(this.bandCount);
    const N = samples.length;
    let totalEnergy = 0;

    const minFreq = 100;
    const maxFreq = Math.min(8000, sampleRate / 2);

    for (let b = 0; b < this.bandCount; b++) {
      const centerFreq = minFreq * Math.pow(maxFreq / minFreq, b / (this.bandCount - 1));
      const omega = (2 * Math.PI * centerFreq) / sampleRate;
      const coeff = 2 * Math.cos(omega);

      let q1 = 0, q2 = 0;
      for (let i = 0; i < N; i++) {
        const hann = 0.5 * (1 - Math.cos((2 * Math.PI * i) / N));
        const s = samples[i] * hann;
        const q0 = coeff * q1 - q2 + s;
        q2 = q1;
        q1 = q0;
      }

      const power = q1 * q1 + q2 * q2 - q1 * q2 * coeff;
      const amplitude = Math.sqrt(Math.max(0, power)) / N;

      bands[b] = amplitude;
      totalEnergy += amplitude * amplitude;
    }

    const norm = Math.sqrt(totalEnergy);
    if (norm > 0.0001) {
      for (let b = 0; b < this.bandCount; b++) {
        bands[b] = bands[b] / norm;
      }
    }

    return {
      vector: bands,
      energy: norm
    };
  }

  trimSilence(buffer, ctx) {
    const channelData = buffer.getChannelData(0);
    const threshold = 0.02;
    let start = 0;
    let end = channelData.length - 1;

    while (start < channelData.length && Math.abs(channelData[start]) < threshold) {
      start++;
    }

    while (end > start && Math.abs(channelData[end]) < threshold) {
      end--;
    }

    const margin = Math.floor(buffer.sampleRate * 0.05);
    start = Math.max(0, start - margin);
    end = Math.min(channelData.length - 1, end + margin);

    const length = Math.max(buffer.sampleRate * 0.2, end - start);
    const trimmedBuffer = ctx.createBuffer(buffer.numberOfChannels, length, buffer.sampleRate);

    for (let c = 0; c < buffer.numberOfChannels; c++) {
      const srcData = buffer.getChannelData(c);
      const dstData = trimmedBuffer.getChannelData(c);
      for (let i = 0; i < length; i++) {
        dstData[i] = srcData[start + i] || 0;
      }
    }

    return trimmedBuffer;
  }

  audioBufferToWavDataUrl(buffer) {
    const numOfChan = buffer.numberOfChannels;
    const length = buffer.length * numOfChan * 2 + 44;
    const out = new DataView(new ArrayBuffer(length));
    const channels = [];
    let sample = 0;
    let offset = 0;
    let pos = 0;

    function setUint16(data) {
      out.setUint16(pos, data, true);
      pos += 2;
    }
    function setUint32(data) {
      out.setUint32(pos, data, true);
      pos += 4;
    }

    setUint32(0x46464952); // "RIFF"
    setUint32(length - 8);
    setUint32(0x45564157); // "WAVE"

    setUint32(0x20746d66); // "fmt "
    setUint32(16);
    setUint16(1);
    setUint16(numOfChan);
    setUint32(buffer.sampleRate);
    setUint32(buffer.sampleRate * 2 * numOfChan);
    setUint16(numOfChan * 2);
    setUint16(16);

    setUint32(0x61746164); // "data"
    setUint32(length - pos - 4);

    for (let i = 0; i < buffer.numberOfChannels; i++) {
      channels.push(buffer.getChannelData(i));
    }

    while (offset < buffer.length) {
      for (let i = 0; i < numOfChan; i++) {
        sample = Math.max(-1, Math.min(1, channels[i][offset]));
        sample = (0.5 + sample < 0 ? sample * 32768 : sample * 32767) | 0;
        out.setInt16(pos, sample, true);
        pos += 2;
      }
      offset++;
    }

    const blob = new Blob([out.buffer], { type: 'audio/wav' });
    return URL.createObjectURL(blob);
  }

  async processAudioFile(file) {
    const arrayBuffer = await file.arrayBuffer();
    const tempCtx = new (window.AudioContext || window.webkitAudioContext)();
    const decodedBuffer = await tempCtx.decodeAudioData(arrayBuffer);
    const trimmedBuffer = this.trimSilence(decodedBuffer, tempCtx);
    const signature = await this.generateSignatureFromAudioBuffer(trimmedBuffer);
    const wavDataUrl = this.audioBufferToWavDataUrl(trimmedBuffer);

    tempCtx.close().catch(() => {});
    return {
      audioBuffer: trimmedBuffer,
      signature: signature,
      audioDataUrl: wavDataUrl,
      duration: trimmedBuffer.duration
    };
  }

  async generatePresetTone(type) {
    const tempCtx = new (window.AudioContext || window.webkitAudioContext)();
    let duration = 0.8;
    let sampleRate = 44100;
    let numSamples = Math.floor(duration * sampleRate);
    let buffer = tempCtx.createBuffer(1, numSamples, sampleRate);
    let data = buffer.getChannelData(0);

    if (type === 'gm') {
      for (let i = 0; i < numSamples; i++) {
        let t = i / sampleRate;
        let env = Math.exp(-t * 4);
        let f = t < 0.4 ? 1046 : 1318;
        data[i] = Math.sin(2 * Math.PI * f * t) * env * 0.8;
      }
    } else if (type === 'msg_player' || type === 'player') {
      // Som suave de mensagem privada / chime de jogador
      duration = 0.6;
      numSamples = Math.floor(duration * sampleRate);
      buffer = tempCtx.createBuffer(1, numSamples, sampleRate);
      data = buffer.getChannelData(0);
      for (let i = 0; i < numSamples; i++) {
        let t = i / sampleRate;
        let env = Math.exp(-t * 4);
        let f = t < 0.28 ? 587.33 : 880.0; // D5 para A5
        data[i] = Math.sin(2 * Math.PI * f * t) * env * 0.75;
      }
    } else if (type === 'teleport') {
      // Som de teleporte rápido com pitch ascendente e efeito warp
      duration = 0.5;
      numSamples = Math.floor(duration * sampleRate);
      buffer = tempCtx.createBuffer(1, numSamples, sampleRate);
      data = buffer.getChannelData(0);
      for (let i = 0; i < numSamples; i++) {
        let t = i / sampleRate;
        let env = Math.exp(-t * 4.5);
        let f = 300 + (t * 2200) + Math.sin(t * 60) * 150;
        data[i] = Math.sin(2 * Math.PI * f * t) * env * 0.8;
      }
    } else if (type === 'seta') {
      // Som característico de seta / hit rápido
      duration = 0.45;
      numSamples = Math.floor(duration * sampleRate);
      buffer = tempCtx.createBuffer(1, numSamples, sampleRate);
      data = buffer.getChannelData(0);
      for (let i = 0; i < numSamples; i++) {
        let t = i / sampleRate;
        let env = Math.exp(-t * 8);
        let f = 1600 - t * 1200;
        data[i] = Math.sin(2 * Math.PI * f * t) * env * 0.85;
      }
    } else if (type === 'pokemon') {
      for (let i = 0; i < numSamples; i++) {
        let t = i / sampleRate;
        let env = Math.exp(-t * 2.5);
        let f = 880 + Math.sin(t * 30) * 200 + t * 400;
        data[i] = Math.sin(2 * Math.PI * f * t) * env * 0.75;
      }
    }

    const trimmedBuffer = this.trimSilence(buffer, tempCtx);
    const signature = await this.generateSignatureFromAudioBuffer(trimmedBuffer);
    const wavDataUrl = this.audioBufferToWavDataUrl(trimmedBuffer);

    tempCtx.close().catch(() => {});
    return {
      signature: signature,
      audioDataUrl: wavDataUrl,
      duration: trimmedBuffer.duration
    };
  }

  playAudioUrl(url) {
    if (!url) return;
    const audio = new Audio(url);
    audio.play().catch(e => console.warn('Falha na reprodução do áudio:', e));
  }

  playLocalAlertSiren() {
    try {
      const ctx = new (window.AudioContext || window.webkitAudioContext)();
      const osc = ctx.createOscillator();
      const gain = ctx.createGain();

      osc.type = 'sawtooth';
      osc.frequency.setValueAtTime(880, ctx.currentTime);
      osc.frequency.exponentialRampToValueAtTime(440, ctx.currentTime + 0.3);
      osc.frequency.exponentialRampToValueAtTime(880, ctx.currentTime + 0.6);

      gain.gain.setValueAtTime(0.3, ctx.currentTime);
      gain.gain.exponentialRampToValueAtTime(0.01, ctx.currentTime + 0.7);

      osc.connect(gain);
      gain.connect(ctx.destination);

      osc.start();
      osc.stop(ctx.currentTime + 0.7);
    } catch (e) {
      console.warn('Erro ao tocar sirene local:', e);
    }
  }

  setTriggers(triggersList) {
    this.triggers = triggersList;
  }
}

window.sentinelAudioEngine = new SentinelAudioEngine();
