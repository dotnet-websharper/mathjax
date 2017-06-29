# MathJax

[MathJax](mathjax.org) is a JavaScript tool to render math expression from TeX, MathML and AsciiMath text.

## Configuring MathJax

Like in JavaScript, we have to configure MathJax before using it.

To configure it we need the `MathJax.Config()` object. The options are the same as the original config, but the usage of it is slightly different. 

A basic example:

```js
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

Every config option from MathJax got its unique config type in WebSharper. They all have the same fields with the same usage.

## Using MathJax

After configuring MathJax all we have to do is to write our math expressions on the screen. After the build MathJax will pick up the expressions from the html file and will render them accordingly.

## Dynamic math content

Most of the time we'll want to render an expression we calculated while running. To do that we'll have to use MathJax's Queue.

For example we store the expression string in a View, then we'll have to add an `on.viewUpdate` attribute to the container element with the `MathJax.Hub.Queue` method.

<div style="width:100%;min-height:300px;position:relative"><iframe style="position:absolute;border:none;width:100%;height:100%" src="http://test2.try.websharper.com/embed/setr/0000DN"></iframe><div>