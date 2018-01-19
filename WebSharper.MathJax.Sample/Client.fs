namespace WebSharper.MathJax.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.UI.Templating
open WebSharper.MathJax

[<JavaScript>]
module Client =

    [<Inline "$o()">]
    let call (o: obj) = X<unit>

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

        let f = 
            div [] [
                text "When $a \\ne 0$, there are two solutions to \(ax^2 + bx + c = 0\) and they are $$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$"
            ]

        JQuery.Of("#btn").On("click", (fun _ _ ->
            JQuery.Of("#tex").Append(f.Dom).Ignore
            MathJax.Hub.Queue([| "Typeset", MathJax.Hub :> obj, [| f.Dom :> obj |] |]) |> ignore
        )).Ignore
        ()
