# MathJax

[MathJax](mathjax.org) is a JavaScript tool to render math expression from TeX, MathML and AsciiMath formated text. It can be configured to render into one of many output formats. It can be used not to just to render static text into a formated math formula, but can be used to render dynamically changing text too.

## Configuring MathJax

Like in `JavaScript`, we have to configure `MathJax` before using it.

To configure it we need the `MathJax.Config()` object. The options are the same as the original config, but the usage of it is slightly different. 

A basic example:

```javascript
MathJax.Hub.Config({
    extensions: ["tex2jax.js"],
    jax: ["input/TeX", "output/HTML-CSS"],
    tex2jax: {
        inlineMath: [ ["$", "$"], ["\\(", "\\)"] ]
    }
})
```

```fsharp
MathJax.Hub.Config(
    MathJax.Config(
        Extensions = [| "tex2jax.js" |],
        Jax = [| "input/TeX"; "output/HTML-CSS"; |],
        Tex2jax = MathJax.Tex2jax(
            InlineMath = [| ("$", "$"); ("\\(", "\\)") |]
        )
    )
)
```

with these basic options we're ready to render TeX strings on the screen. We simply have to have the formula in the `html` file, for example:

```html
<p>This is inline math: $a \ne 0$ or \(a \ne 0\)</p>
<p>This is display math: $$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$</p>
```

## Supported input formats

| Format    | Extension           | Jax include         |
|-----------|---------------------|---------------------|
| TeX       | `"tex2jax.js"`      | `"input/TeX"`       |
| MathML    | `"mml2jax.js"`      | `"input/MathML"`    |
| AsciiMath | `"asciimath2jax.js"`| `"input/AsciiMath"` |


## Supported output formats

| Format      | Jax include            |
|-------------|------------------------| 
| CommonHtml  | `"output/CommonHTML"`  |
| HTML-CSS    | `"output/HTML-CSS"`    |
| NativeMML   | `"output/NativeMML"`   |
| SVG         | `"output/SVG"`         |
| PreviewHTML | `"output/PreviewHTML"` |
| PlainSource | `"output/PlainSource"` |

### Other config types

Every config option from `MathJax` got its unique config type in WebSharper. They all have the same fields with the same usage. So when we'd write in `JavaScript`:

```javascript
menuSettings: {
    zoom: "None",
    CTRL: false,
    ...
    errorSettings: {
        message: ["[Math Processing Error]"]
    }
    ...
}
```

We have to write this is F#:

```fsharp
let menuSettings = MathJax.MenuSetting(
    Zoom = "None",
    CTRL = false,
    ...
    ErrorSettings = MathJax.ErrorSetting(
        Message = [| "[Math Processing Error]" |]
    )
    ...
)
```

## Using MathJax

After configuring `MathJax` all we have to do is to write our math expressions on the screen. After the build `MathJax` will pick up the expressions from the `html` file and will render them accordingly.

## Dynamic math content

Most of the time we'll want to render an expression we've calculated while running. To do that we'll have to use MathJax's Queue.

For example we store the expression string in a View, then we'll have to add an `on.viewUpdate` attribute to the container element with the `MathJax.Hub.Queue` method.

This can be called on every individual DOM element, or can be called on the whole page. To update the whole page, all we have to do is this function:

```fsharp
MathJax.Hub.Queue([| "Typeset"; MathJax.Hub :> obj |])
```

To call on a specific DOM element, see the example below:

<div style="width:100%;min-height:300px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000DN"></iframe><div>