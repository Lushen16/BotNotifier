/**
 * PokéBot Sentinel - Audio Visualizer Canvas
 * Renderização de Espectrograma e Onda Sonora estilo Cyberpunk/Gamer
 */

class SentinelVisualizer {
  constructor(canvasId) {
    this.canvas = document.getElementById(canvasId);
    if (!this.canvas) return;
    this.ctx = this.canvas.getContext('2d');
    this.animationId = null;
    this.peakLevels = [];

    // Redimensionar responsivamente
    this.resize();
    window.addEventListener('resize', () => this.resize());
  }

  resize() {
    if (!this.canvas) return;
    const rect = this.canvas.getBoundingClientRect();
    this.canvas.width = rect.width * (window.devicePixelRatio || 1);
    this.canvas.height = rect.height * (window.devicePixelRatio || 1);
    this.ctx.scale(window.devicePixelRatio || 1, window.devicePixelRatio || 1);
  }

  start(analyserNode) {
    if (!this.canvas || !analyserNode) return;
    this.analyser = analyserNode;
    this.bufferLength = analyserNode.frequencyBinCount;
    this.freqData = new Uint8Array(this.bufferLength);
    this.timeData = new Uint8Array(this.bufferLength);
    this.peakLevels = new Array(64).fill(0);

    const render = () => {
      this.animationId = requestAnimationFrame(render);
      this.draw();
    };

    if (this.animationId) cancelAnimationFrame(this.animationId);
    render();
  }

  stop() {
    if (this.animationId) {
      cancelAnimationFrame(this.animationId);
      this.animationId = null;
    }
    this.drawIdle();
  }

  draw() {
    const width = this.canvas.clientWidth;
    const height = this.canvas.clientHeight;
    const ctx = this.ctx;

    this.analyser.getByteFrequencyData(this.freqData);
    this.analyser.getByteTimeDomainData(this.timeData);

    // Fundo limpo com leve rastro (efeito persistência de fósforo)
    ctx.fillStyle = 'rgba(10, 14, 26, 0.35)';
    ctx.fillRect(0, 0, width, height);

    // Linha central guia
    ctx.strokeStyle = 'rgba(0, 242, 254, 0.08)';
    ctx.lineWidth = 1;
    ctx.beginPath();
    ctx.moveTo(0, height / 2);
    ctx.lineTo(width, height / 2);
    ctx.stroke();

    // 1. Barras de Espectro de Frequência (64 barras estilizadas)
    const barCount = 64;
    const barSpacing = 4;
    const totalSpacing = barSpacing * (barCount - 1);
    const barWidth = Math.max(2, (width - totalSpacing) / barCount);

    for (let i = 0; i < barCount; i++) {
      // Distribuição logarítmica para valorizar graves e médios
      const binIndex = Math.floor(Math.pow(i / barCount, 1.6) * (this.bufferLength - 1));
      const value = this.freqData[binIndex] || 0;
      const percent = value / 255;
      const barHeight = Math.max(3, percent * (height * 0.75));
      const x = i * (barWidth + barSpacing);
      const y = height - barHeight;

      // Atualiza picos
      if (barHeight > (this.peakLevels[i] || 0)) {
        this.peakLevels[i] = barHeight;
      } else {
        this.peakLevels[i] = Math.max(0, (this.peakLevels[i] || 0) - 1.5);
      }

      // Gradiente de cor por frequência (Ciano -> Roxo -> Vermelho)
      const grad = ctx.createLinearGradient(0, height, 0, y);
      grad.addColorStop(0, 'rgba(0, 242, 254, 0.2)');
      grad.addColorStop(0.5, 'rgba(79, 172, 254, 0.8)');
      grad.addColorStop(1, 'rgba(255, 8, 68, 0.95)');

      ctx.fillStyle = grad;
      ctx.shadowColor = 'rgba(0, 242, 254, 0.5)';
      ctx.shadowBlur = 6;
      ctx.fillRect(x, y, barWidth, barHeight);

      // Ponto de pico no topo da barra
      ctx.fillStyle = '#ffffff';
      ctx.shadowColor = '#00f2fe';
      ctx.shadowBlur = 4;
      const peakY = Math.max(0, height - this.peakLevels[i] - 2);
      ctx.fillRect(x, peakY, barWidth, 2);
    }

    // 2. Onda Sonora (Osciloscópio Neon sutil no topo)
    ctx.shadowBlur = 8;
    ctx.shadowColor = '#00ffcc';
    ctx.lineWidth = 2;
    ctx.strokeStyle = 'rgba(0, 255, 204, 0.7)';
    ctx.beginPath();

    const sliceWidth = width / this.bufferLength;
    let wx = 0;

    for (let i = 0; i < this.bufferLength; i++) {
      const v = this.timeData[i] / 128.0;
      const wy = (v * height) / 2;

      if (i === 0) {
        ctx.moveTo(wx, wy);
      } else {
        ctx.lineTo(wx, wy);
      }

      wx += sliceWidth;
    }

    ctx.stroke();
    ctx.shadowBlur = 0;
  }

  drawIdle() {
    if (!this.canvas) return;
    const width = this.canvas.clientWidth;
    const height = this.canvas.clientHeight;
    const ctx = this.ctx;

    ctx.fillStyle = '#0a0e1a';
    ctx.fillRect(0, 0, width, height);

    // Linha pulsante de espera
    ctx.strokeStyle = 'rgba(0, 242, 254, 0.2)';
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.moveTo(0, height / 2);
    ctx.lineTo(width, height / 2);
    ctx.stroke();

    ctx.fillStyle = 'rgba(255, 255, 255, 0.35)';
    ctx.font = '13px monospace';
    ctx.textAlign = 'center';
    ctx.fillText('NENHUM ÁUDIO CONECTADO — CLIQUE EM "CONECTAR ÁUDIO DO SISTEMA"', width / 2, height / 2 + 30);
  }
}

window.SentinelVisualizer = SentinelVisualizer;
