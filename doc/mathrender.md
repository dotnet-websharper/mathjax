# Rendering math with WebSharper

Rendering math expressions in WebSharper works with [MathJax](https://www.mathjax.org/) JavaScript library. The documentation for MathJax extension can be found [here](./#docs/extensions/mathjax).

MathJax, so WebSharper too allows you to use a variety of common math formatting systems but not only that, it supports more than one output formatting too.

## Supported input formats

| Format    | Extension           | Jax include         |
|-----------|---------------------|---------------------|
| TeX       | `"tex2jax.js"`      | `"input/TeX"`       |
| MathML    | `"mml2jax.js"`      | `"input/MathML"`    |
| AsciiMath | `"asciimath2jax.js"`| `"input/AsciiMath"` |


Supported output formats:

| Format      | Jax include            |
|-------------|------------------------| 
| CommonHtml  | `"output/CommonHTML"`  |
| HTML-CSS    | `"output/HTML-CSS"`    |
| NativeMML   | `"output/NativeMML"`   |
| SVG         | `"output/SVG"`         |
| PreviewHTML | `"output/PreviewHTML"` |
| PlainSource | `"output/PlainSource"` |

## Rendering expresions

To render static expressions (it's part of the static html file or generated with WebSharper to the html file at the beginning) we don't have to do anything else but configuring MathJax ([see an example here](./#docs/extensions/mathjax)).

In these examples we render dynamicaly changing formulas. In order to do that we have to use and call the `MathJax.Hub.Queue` on every update.

### TeX

To render TeX expressions we have to include `"input/TeX"` in the Jax config, and `"tex2jax.js"` in the extensions.
([TeX documentation](https://en.wikibooks.org/wiki/TeX/def))

<div style="width:100%;min-height:300px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000DN"></iframe><div>

### MathML

To render MathML expressions we have to include `"input/MathML"` in the Jax config, and `"mml2jax.js"` in the extensions.
([MathML documentation](https://www.w3.org/TR/MathML/))

<div style="width:100%;min-height:450px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000Dy"></iframe><div>

### Ascii Math

To render AsciiMath expressions we have to include `"input/AsciiMath"` in the Jax config, and `"asciimath2jax.js"` in the extensions.
([AsciiMath documentation](http://asciimath.org/))

<div style="width:100%;min-height:300px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000E1"></iframe><div>

## An example for expressions

There are many functions in [MathJS](./#docs/extensions/mathjs) that calculates an expression, solves a problem. In this example we'll use the `Math.Derivative` function to get a `Node` with the result in it. A `Node` then can be converted to a `String`, but with the [MathJax extension](./#docs/extensions/mathjax) we can render the result. To do that we have to set up `MathJax` to parse and render `TeX` formulas then by using the `Node`'s `ToTex()` function we convert the result into a `String` with the formula in `TeX` formatting.

(Most of the functions don't result a `Node`, but they can be converted to `Node` by `Math.Parse()` or by other means. ([MathJax documentation](mathjax.org))

<div style="width:100%;min-height:400px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000Cy"></iframe><div>
