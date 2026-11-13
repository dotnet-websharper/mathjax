# MathJax

[MathJax](https://www.mathjax.org/) is a JavaScript tool that renders math
expressions written in TeX, MathML or AsciiMath. It supports several output
formats (CHTML and SVG in MathJax 4), and it can render both static page content
and dynamically generated content.

The WebSharper binding lives in `WebSharper.MathJax` and targets MathJax 4.x.
See the [README](../README.md) for the full API and the sample.

## Configuring MathJax

MathJax 4 must be configured **before** the component script loads. In a
WebSharper SPA, set the global in the page head:

```html
<script>
    MathJax = {
        loader: { load: ['input/asciimath'] },
        tex: { inlineMath: { '[+]': [['$', '$']] } }
    };
</script>
<script src="https://cdn.jsdelivr.net/npm/mathjax@4.1.3/tex-mml-chtml.js"></script>
```

In a Sitelets application, put the same code (as a string) in
`appsettings.json` under `websharper.mathjaxConfig`; the binding's
`ConfigResource` injects it before the component.

In v4, configuration values that can be extended (`inlineMath`, `packages`, …)
use the `{ '[+]': … }` form, which appends to the component's defaults. A plain
array replaces the defaults.

With this configuration, static math in the page is typeset automatically:

```html
<p>This is inline math: $a \ne 0$ or \(a \ne 0\).</p>
<p>This is display math: $$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$</p>
```

## Supported input formats

| Format    | Component        | Helpers |
|-----------|------------------|---------|
| TeX       | `input/tex`      | `tex2chtml`, `tex2svg`, `tex2mml` |
| MathML    | `input/mml`      | `mathml2chtml`, `mathml2svg`, `mathml2mml` |
| AsciiMath | `input/asciimath`| `asciimath2chtml`, `asciimath2svg`, `asciimath2mml` |

## Supported output formats

| Format | Component      | Stylesheet |
|--------|----------------|------------|
| CHTML  | `output/chtml` | `MathJax.ChtmlStylesheet()` |
| SVG    | `output/svg`   | `MathJax.SvgStylesheet()` |

The conversion helpers that exist are created by MathJax at startup from the
configured input and output components; the `…2svg` helpers require an SVG
output component.

## Using the API

```fsharp
open WebSharper.MathJax

// Re-typeset the page. Prefer the promise-based form in v4, where font data
// may need to be loaded asynchronously.
MathJax.TypesetPromise().Then(fun () -> ()) |> ignore

// Convert a single expression.
let node = MathJax.Tex2chtml(@"\sqrt{x^2+1}")
let mml : string = MathJax.Tex2mml(@"\sqrt{x^2+1}")
```

## Dynamic math content

When the page content changes, forget the old math before replacing the DOM and
typeset the new content:

```fsharp
let container = JS.Document.GetElementById "preview"

MathJax.TypesetClear([| box container |])
MathJax.TexReset()
container.InnerHTML <- "$$\\frac{1}{2}$$"
MathJax.TypesetPromise([| box container |]).Then(fun () -> ()) |> ignore
```

`MathJax.WhenReady` queues an action after any pending typeset/conversion work,
which is the v4 replacement for manually chaining `MathJax.startup.promise`.