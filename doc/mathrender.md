# Rendering math with WebSharper

Rendering math expressions in WebSharper works with [MathJax](https://www.mathjax.org/) JavaScript library. The documentation for MathJax extension can be found [here](#docs/extensions/mathjax).

MathJax, so WebSharper too allows you to use a variety common math formatting systems but not only that, it supports more than one output formatting.

Supported input formats:
- TeX
- MathML
- AsciiMath

Supported output formats:
- CommonHtml
- HTML-CSS
- NativeMML
- SVG
- PreviewHTML
- PlainSource

## Rendering expresions

To render static expressions (it's part of the static html file (or generated with WebSharper to the html file at the beginning)) we don't have to do anything else but configuring the MathJax ([see an example here]()).

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

If we have an expression calculated via [MathJS](#docs/extensions/mathjs) we can convert it into a TeX string with the `.ToTex()` method.

In the following example we use `Math.Derivative()` to get a `Node` from the expression which can be converted to TeX with its `.ToTex()` method.

<div style="width:100%;min-height:400px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000Cy"></iframe><div>
