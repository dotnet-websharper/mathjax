namespace WebSharper.MathJax.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.MathJax

[<JavaScript>]
module Client =

    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let mathjaxConfig() =
        MathJax.Config <- MathJaxConfig(
            Loader = LoaderConfig(
                Load = [| "input/tex"; "input/asciimath"; "input/mml"; "output/chtml" |]
            ),
            Tex = TexConfig(
                InlineMath = [| ("$", "$"); ("\\(", "\\)") |]
            ),
            Asciimath = AsciiMathConfig(
                Delimiters = [| ("`", "`") |]
            )
        )

    let renderTex2CHTML(id: string) =
        let html = MathJax.Tex2chtml("\\int_0^1 x^2 dx") |> As<Dom.Node>
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderTex2MML(id: string) =
        let mml = MathJax.Tex2mml("\\int_0^1 x^2 dx") |> As<string>
        JS.Document.GetElementById(id).TextContent <- mml

    let renderAscii2CHTML(id: string) =
        let html = MathJax.Asciimath2chtml("x = (-b +- sqrt(b^2 - 4ac)) / (2a)") |> As<Dom.Node>
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderAscii2MML(id: string) =
        let mml = MathJax.Asciimath2mml("x = (-b +- sqrt(b^2 - 4ac)) / (2a)") |> As<string>
        JS.Document.GetElementById(id).TextContent <- mml

    let renderMathML2CHTML(id: string) =
        let mathML = """
        <math xmlns="http://www.w3.org/1998/Math/MathML">
            <msubsup>
                <mo>∫</mo>
                <mn>0</mn>
                <mn>1</mn>
            </msubsup>
            <msup><mi>x</mi><mn>2</mn></msup><mi>d</mi><mi>x</mi>
        </math>
        """
        let html = MathJax.Mathml2chtml(mathML) |> As<Dom.Node>
        JS.Document.GetElementById(id).AppendChild(html) |> ignore

    let renderMathML2MML(id: string) =
        let mathML = """
        <math xmlns="http://www.w3.org/1998/Math/MathML">
            <msubsup>
                <mo>∫</mo>
                <mn>0</mn>
                <mn>1</mn>
            </msubsup>
            <msup><mi>x</mi><mn>2</mn></msup><mi>d</mi><mi>x</mi>
        </math>
        """
        let mml = MathJax.Mathml2mml(mathML) |> As<string>
        JS.Document.GetElementById(id).TextContent <- mml

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate
            .Main()
            .PageInit(fun () ->
                // async {
                //     mathjaxConfig()
                //     MathJax.Startup.Promise.Then(fun _ ->
                //         MathJax.Typeset()

                //         MathJax.TypesetPromise().Then(fun _ ->
                //             Console.Log("✅ typesetPromise finished")
                //         ) |> ignore

                //         MathJax.GetMetricsFor(JS.Document.GetElementById("container"), true) |> ignore

                //         renderTex2CHTML("resultTex2CHTML")
                //         renderTex2MML("resultTex2MML")
                //         renderAscii2CHTML("resultAscii2CHTML")
                //         renderAscii2MML("resultAscii2MML")
                //         renderMathML2CHTML("resultMathML2CHTML")
                //         renderMathML2MML("resultMathML2MML")
                //     ) |> ignore
                // }
                // |> Async.StartImmediate
                mathjaxConfig()
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

                // Test: ascii2chtml
                renderAscii2CHTML("resultAscii2CHTML")

                // Test: ascii2mml
                renderAscii2MML("resultAscii2MML")

                // Test: mathml2chtml
                renderMathML2CHTML("resultMathML2CHTML")

                // Test: mathml2mml
                renderMathML2MML("resultMathML2MML")
            )
            .Doc()
        |> Doc.RunById "main"
