# WebSharper.MathJax

WebSharper Extensions for MathJax 3.2.2

[MathJax](https://www.mathjax.org/) is a JavaScript tool to render math expression from TeX, MathML and AsciiMath formated text. It can be configured to render into one of many output formats. It can be used not to just to render static text into a formated math formula, but can be used to render dynamically changing text too.

> Click [here](https://dotnet-websharper.github.io/mathjax/) to see a demo

## Version 3 Overview

MathJax v3 introduces a modern, modular API that replaces the older `Hub.Queue` system. Configuration must now happen **before** loading the library. WebSharper supports this setup for both **SPA** and **Sitelets** projects.


## Configuring MathJax

### SPA Projects

For SPA projects:

- Set the global MathJax configuration in a `<script>` tag **before** importing the MathJax library which is done inside `Scripts/WebSharper.MathJax.Sample.head.js`. It is important to add before that script:

```html
<script>
    MathJax = {
        loader: {
            load: ['input/tex', 'input/asciimath', 'input/mml', 'output/chtml']
        },
        tex: {
            inlineMath: [['$', '$'], ['\\(', '\\)']]
        }
    };
</script>
<script src="Scripts/WebSharper.MathJax.Sample.head.js"></script>
```

### Sitelets Projects

For Sitelets projects:

- Place the configuration inside `appsettings.json`:

```json
{
  "websharper": {
    "mathjaxConfig": "MathJax = { \"loader\": { \"load\": [\"input/tex\",\"input/asciimath\",\"input/mml\",\"output/chtml\"] }, \"tex\": { \"inlineMath\": [[\"$\",\"$\"], [\"\\\\(\",\"\\\\)\"]] } }"
  }
}
```

> This will inject the configuration **before** the MathJax script tag automatically.

with these basic configuration we're ready to render TeX strings on the screen. We simply have to have the formula in the `html` file, for example:

```html
<p>This is inline math: $a \ne 0$ or \(a \ne 0\)</p>
<p>This is display math: $$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$</p>
```

## Supported input formats

| Format    | Loader Module       |
|-----------|---------------------|
| TeX       | `"input/tex"`       |
| MathML    | `"input/mml"`       |
| AsciiMath | `"input/asciimath"` |


## Supported output formats

| Format      | Output Module          |
|-------------|------------------------| 
| CHTML       | `"output/chtml"`       |
| SVG         | `"output/svg"`         |

## Using the MathJax API (WebSharper Bindings)

WebSharper provides convenient bindings for dynamic rendering using MathJax v3 API:

`MathJax.Typeset()`

Rerun typesetting on the whole page.

`MathJax.TypesetPromise()`

Like `Typeset()`, but returns a JavaScript `Promise`.

`MathJax.Tex2chtml(tex: string)`

Convert TeX string into HTML (CHTML DOM node).

`MathJax.Tex2mml(tex: string)`

Convert TeX string into MathML (as text).

`MathJax.Mathml2chtml(mml: string)`

Convert MathML string into HTML (CHTML DOM node).

`MathJax.Mathml2mml(mml: string)`

Convert MathML string into cleaned-up MathML (as text).

## Dynamic math content

Most of the time, you'll want to render a math expression that changes while the application is running. In MathJax v3, instead of using `Hub.Queue`, you simply:

- You update the DOM content manually

- Call `MathJax.Typeset()` to re-render the expression

This pattern works well when binding expressions to `Var` and updating them with `View.Sink`. You can typeset either specific elements or the entire page with `MathJax.Typeset()` or `MathJax.TypesetPromise()`.

```fsharp
let asciiMathPage() = 
    let rvExpression = Var.Create @"sum_(i=1)^n i^3=((n(n+1))/2)^2"

    // Re-render on input change
    rvExpression.View
    |> View.Sink (fun ascii ->
        let el = JS.Document.GetElementById("tex")
        el.InnerHTML <- "`" + ascii + "`"
        MathJax.Typeset()
    )

    IndexTemplate.AsciiMath()
        .Expression(rvExpression)
        .Doc()
```
