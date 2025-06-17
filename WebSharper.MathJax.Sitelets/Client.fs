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

    let renderTex2CHTML(id: string) =
        let html = MathJax.Tex2chtml("$$x = {-b \\pm \\sqrt{b^2 - 4ac} \\over 2a}$$") |> As<Dom.Node>
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderTex2MML(id: string) =
        let mml = MathJax.Tex2mml("$$x = {-b \\pm \\sqrt{b^2 - 4ac} \\over 2a}$$") |> As<string>
        JS.Document.GetElementById(id).TextContent <- mml

    let renderMathML2CHTML(id: string) =
        let mathML = """
        <math xmlns="http://www.w3.org/1998/Math/MathML">
            <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow> <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow> <mi>x</mi> <mo>=</mo> <mrow data-mjx-texclass="ORD"> <mfrac> <mrow> <mo>&#x2212;</mo> <mi>b</mi> <mo>&#xB1;</mo> <msqrt> <msup> <mi>b</mi> <mn>2</mn> </msup> <mo>&#x2212;</mo> <mn>4</mn> <mi>a</mi> <mi>c</mi> </msqrt> </mrow> <mrow> <mn>2</mn> <mi>a</mi> </mrow> </mfrac> </mrow> <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow> <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow>
        </math>
        """
        let html = MathJax.Mathml2chtml(mathML) |> As<Dom.Node>
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderMathML2MML(id: string) =
        let mathML = """
        <math xmlns="http://www.w3.org/1998/Math/MathML">
            <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow> <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow> <mi>x</mi> <mo>=</mo> <mrow data-mjx-texclass="ORD"> <mfrac> <mrow> <mo>&#x2212;</mo> <mi>b</mi> <mo>&#xB1;</mo> <msqrt> <msup> <mi>b</mi> <mn>2</mn> </msup> <mo>&#x2212;</mo> <mn>4</mn> <mi>a</mi> <mi>c</mi> </msqrt> </mrow> <mrow> <mn>2</mn> <mi>a</mi> </mrow> </mfrac> </mrow> <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow> <mrow data-mjx-texclass="ORD"> <mo>$</mo> </mrow>
        </math>
        """
        let mml = MathJax.Mathml2mml(mathML) |> As<string>
        JS.Document.GetElementById(id).TextContent <- mml

    let Main () =
        Templates.MainTemplate.Main()
            .PageInit(fun () ->
                // Test: typeset()
                MathJax.Typeset()

                // Test: typesetPromise()
                MathJax.TypesetPromise().Then(fun _ ->
                    Console.Log("✅ typesetPromise finished")
                ) |> ignore

                // Test: tex2chtml
                renderTex2CHTML("resultTex2CHTML")

                // Test: tex2mml
                renderTex2MML("resultTex2MML")

                //Test: mathml2chtml
                renderMathML2CHTML("resultMathML2CHTML")

                // Test: mathml2mml
                renderMathML2MML("resultMathML2MML")
            )
            .Doc()
