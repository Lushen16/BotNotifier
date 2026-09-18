const express = require('express');
const cors = require('cors');
const path = require('path');

const app = express();
const PORT = process.env.PORT || 3333;

app.use(cors());
app.use(express.json());
app.use(express.static(path.join(__dirname, 'public')));
app.use('/src', express.static(path.join(__dirname, 'src')));

// Endpoint para relay de Webhook do Discord (evita bloqueios de CORS e adblockers)
app.post('/api/discord-notify', async (req, res) => {
  const { webhookUrl, payload } = req.body;

  if (!webhookUrl || !webhookUrl.startsWith('https://discord.com/api/webhooks/')) {
    return res.status(400).json({
      success: false,
      error: 'URL de Webhook inválida. Deve começar com https://discord.com/api/webhooks/'
    });
  }

  try {
    const response = await fetch(webhookUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload)
    });

    if (response.ok || response.status === 204) {
      return res.json({ success: true, status: response.status });
    } else {
      const errorText = await response.text();
      return res.status(response.status).json({
        success: false,
        error: `Discord retornou status ${response.status}: ${errorText}`
      });
    }
  } catch (err) {
    console.error('Erro ao enviar mensagem para o Discord:', err);
    return res.status(500).json({
      success: false,
      error: `Falha na conexão: ${err.message}`
    });
  }
});

// Servir o index.html na raiz
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, 'index.html'));
});

app.listen(PORT, () => {
  console.log(`=======================================================`);
  console.log(`🚀 POKÉBOT SENTINEL - SERVIDOR RODANDO COM SUCESSO!`);
  console.log(`🔗 Acesse: http://localhost:${PORT}`);
  console.log(`=======================================================`);
});
