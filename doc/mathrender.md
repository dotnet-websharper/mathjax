# Rendering math with WebSharper

Rendering math in WebSharper uses the [MathJax](https://www.mathjax.org/)
binding, `WebSharper.MathJax`. The detailed usage notes are in
[doc.md](doc.md) and the [README](../README.md).

MathJax 4 (bound here) supports three input formats and two output formats:

| Input     | Component         |
|-----------|-------------------|
| TeX       | `input/tex`       |
| MathML    | `input/mml`       |
| AsciiMath | `input/asciimath` |

| Output | Component      |
|--------|----------------|
| CHTML  | `output/chtml` |
| SVG    | `output/svg`   |

## Rendering static expressions

MathJax typesets the math it finds in the page when it loads. Configure it before
the component script, and put math in the page with your chosen delimiters (see
[doc.md](doc.md#configuring-mathjax)).

## Rendering dynamic expressions

When content changes, call `MathJax.TypesetClear` before replacing the DOM, then
`MathJax.TypesetPromise` for the new content. Use `MathJax.WhenReady` to run code
after all pending typesetting/conversion work.

```fsharp
let render (container: Dom.Element) (tex: string) =
    MathJax.TypesetClear([| box container |])
    container.InnerHTML <- tex
    MathJax.TypesetPromise([| box container |])
```

### TeX

Convert TeX with `MathJax.Tex2chtml`, `MathJax.Tex2svg` or
`MathJax.Tex2mml` (and the promise variants), or put TeX in the page and call
`TypesetPromise`.

### MathML

Convert serialized MathML with `MathJax.Mathml2chtml`,
`MathJax.Mathml2svg` or `MathJax.Mathml2mml`, or put `<math>` elements in the
page and typeset them.

### AsciiMath

Convert AsciiMath with `MathJax.Asciimath2chtml`,
`MathJax.Asciimath2svg` or `MathJax.Asciimath2mml`. AsciiMath is not part of the
combined components; add `input/asciimath` to `loader.load` in the
configuration.

The WebSharper binding for MathJax 2.x is no longer maintained; see the older
`websharper50`/`websharper60` version branches if you need it.