// Gera apresentacao-reformulacao.pdf a partir de slides.html (16:9, 1920x1080).
// Uso, na raiz do repositório:  node docs/apresentacao/gerar-pdf.js
// Requer Playwright (npm i -D playwright && npx playwright install chromium).
const path = require('path');
const { chromium } = require('playwright');
(async () => {
  const dir = __dirname;
  const browser = await chromium.launch();
  const page = await browser.newPage({ viewport: { width: 1920, height: 1080 } });
  await page.goto('file://' + path.join(dir, 'slides.html'), { waitUntil: 'networkidle' });
  await page.evaluate(() => document.fonts.ready);
  await page.pdf({ path: path.join(dir, 'apresentacao-reformulacao.pdf'), width: '1920px', height: '1080px', printBackground: true, preferCSSPageSize: true });
  await browser.close();
  console.log('PDF gerado em docs/apresentacao/apresentacao-reformulacao.pdf');
})();
