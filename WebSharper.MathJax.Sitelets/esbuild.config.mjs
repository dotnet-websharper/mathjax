import { existsSync, cpSync, readdirSync, mkdirSync, copyFileSync, writeFileSync } from 'fs'
import { build } from 'esbuild'

const outDir = './wwwroot/Scripts/WebSharper/'
mkdirSync(outDir, { recursive: true })

if (existsSync('./build/Content/WebSharper/')) {
  cpSync('./build/Content/WebSharper/', './wwwroot/Content/WebSharper/', { recursive: true });
}

const scriptDir = './build/Scripts/WebSharper/WebSharper.MathJax.Sitelets/';

for (const file of readdirSync(scriptDir)) {
  if (file.endsWith('.js')) {
    const options = {
      entryPoints: [scriptDir + file],
      bundle: true,
      minify: true,
      format: 'iife',
      outfile: outDir + file,
      globalName: 'wsbundle'
    };

    console.log('Bundling:', file);
    build(options);
  } else if (file.endsWith('.css')) {
    copyFileSync(scriptDir + file, outDir + file);
  }
}

// The Sitelet bundle references /Scripts/WebSharper/home.css. The sample styles
// the page inline (with the light/dark palette), so ship an empty stylesheet
// when WebSharper does not emit one for the bundle.
const bundleCss = outDir + 'home.css';
if (!existsSync(bundleCss)) {
  writeFileSync(bundleCss, '/* MathJax Sitelets sample: styles are inline in Main.html */\n');
}