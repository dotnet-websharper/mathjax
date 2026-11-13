import { build } from 'esbuild'
import { readFileSync, writeFileSync, copyFileSync, mkdirSync } from 'fs'

const name = 'WebSharper.MathJax.Sample'

mkdirSync('../dist/Scripts', { recursive: true })

// Bundle the WebSharper output. The MathJax component itself is loaded by the
// AssemblyWide resource that WebSharper puts in the head (MathJax must be
// configured before it loads), so the bundle only contains our code.
await build({
  entryPoints: [`./build/${name}.js`],
  bundle: true,
  minify: true,
  format: 'iife',
  outfile: `../dist/Scripts/${name}.min.js`,
  globalName: 'wsbundle'
})

copyFileSync('./site.css', '../dist/site.css')

// Fold the WebSharper head (the MathJax config + component `<script>` tags)
// into the page at the ws-head marker.
const head = readFileSync(`./build/${name}.head.html`, 'utf8')
const html = readFileSync('./index.html', 'utf8').replace('<!-- ws-head -->', head)
writeFileSync('../dist/index.html', html)

console.log('Bundled:', name)