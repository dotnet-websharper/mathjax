namespace WebSharper.MathJax.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Templating
open WebSharper.MathJax

[<JavaScript>]
module Client =
    [<SPAEntryPoint>]
    let Main () =
        let mathJaxConfig = new MathJax.Config()
        mathJaxConfig.Extensions <- [| "tex2jax.js"; "mml2jax.js"; "asciimath2jax.js" |]
        mathJaxConfig.Jax <- [| "input/TeX"; "output/CommonHTML"; "input/MathML"; "input/AsciiMath" |]
        
        mathJaxConfig.Tex2jax <- Tex2jax()
        mathJaxConfig.Tex2jax.InlineMath <- [| ("$", "$"); ("\\(", "\\)") |]
        
        mathJaxConfig.Mml2jax <- Mml2jax()
        mathJaxConfig.Mml2jax.Preview <- [| "mathml" |]

        mathJaxConfig.Asciimath2jax <- Asciimath2jax()
        mathJaxConfig.Asciimath2jax.Delimiters <- [| ("`", "`") |]
        mathJaxConfig.Asciimath2jax.Preview <- [| "AsciiMath" |]

        mathJaxConfig.MenuSettings <- MenuSetting()
        mathJaxConfig.MenuSettings.Zoom <- "Click"
        mathJaxConfig.DisplayAlign <- "left"
        
        MathJax.Hub.Config(mathJaxConfig)