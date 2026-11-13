namespace WebSharper.MathJax.Sitelets

open WebSharper
open WebSharper.UI
open WebSharper.UI.Templating
open WebSharper.JavaScript
open WebSharper.MathJax

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =

    let texSource = @"x = {-b \pm \sqrt{b^2-4ac} \over 2a}"

    let mathmlSource =
        """<math xmlns="http://www.w3.org/1998/Math/MathML">
             <msup><mi>x</mi><mn>2</mn></msup><mo>+</mo>
             <mi>x</mi><mo>+</mo><mn>1</mn><mo>=</mo><mn>0</mn>
           </math>"""

    let renderTex2CHTML(id: string) =
        let html = MathJax.Tex2chtml(texSource)
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderTex2MML(id: string) =
        JS.Document.GetElementById(id).TextContent <- MathJax.Tex2mml(texSource)

    let renderMathML2CHTML(id: string) =
        let html = MathJax.Mathml2chtml(mathmlSource)
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderMathML2MML(id: string) =
        JS.Document.GetElementById(id).TextContent <- MathJax.Mathml2mml(mathmlSource)

    let renderAsciiMath2CHTML(id: string) =
        let html = MathJax.Asciimath2chtml(@"sum_(i=1)^n i^3=((n(n+1))/2)^2")
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderAsciiMath2MML(id: string) =
        JS.Document.GetElementById(id).TextContent <-
            MathJax.Asciimath2mml(@"sum_(i=1)^n i^3=((n(n+1))/2)^2")

    let Main () =
        Templates.MainTemplate.Main()
            .PageInit(fun () ->
                // typesetPromise is the recommended entry point in v4: it
                // waits for font data and any pending typesetting.
                MathJax.TypesetPromise().Then(fun () ->
                    MathJax.WhenReady(fun () ->
                        renderTex2CHTML("resultTex2CHTML")
                        renderTex2MML("resultTex2MML")
                        renderMathML2CHTML("resultMathML2CHTML")
                        renderMathML2MML("resultMathML2MML")
                        renderAsciiMath2CHTML("resultAsciiMath2CHTML")
                        renderAsciiMath2MML("resultAsciiMath2MML")
                        null
                    ) |> ignore
                ) |> ignore
            )
            .Doc()