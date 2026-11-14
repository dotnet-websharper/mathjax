namespace WebSharper.MathJax.Sitelets

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.JavaScript
open WebSharper.MathJax

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =

    // ---------------------------------------------------------------
    // Inputs
    // ---------------------------------------------------------------

    let texSource = @"x = {-b \pm \sqrt{b^2-4ac} \over 2a}"
    let asciimathSource = @"sum_(i=1)^n i^3=((n(n+1))/2)^2"
    let mathmlSource =
        """<math xmlns="http://www.w3.org/1998/Math/MathML">
             <msup><mi>x</mi><mn>2</mn></msup><mo>+</mo>
             <mi>x</mi><mo>+</mo><mn>1</mn><mo>=</mo><mn>0</mn>
           </math>"""

    let defaultPreview =
        @"$$\displaystyle \int_0^1 x^2\,dx = \frac{1}{3}$$"

    // ---------------------------------------------------------------
    // State bound into the template
    // ---------------------------------------------------------------

    let status = Var.Create "Ready."
    let typesetInfo = Var.Create ""
    let diagnostics = Var.Create ""
    let source = Var.Create defaultPreview

    let tex2Chtml = Var.Create Doc.Empty
    let tex2ChtmlPromise = Var.Create Doc.Empty
    let tex2Mml = Var.Create ""
    let tex2MmlPromise = Var.Create ""

    let asciimath2Chtml = Var.Create Doc.Empty
    let asciimath2ChtmlPromise = Var.Create Doc.Empty
    let asciimath2Mml = Var.Create ""
    let asciimath2MmlPromise = Var.Create ""

    let mathml2Chtml = Var.Create Doc.Empty
    let mathml2ChtmlPromise = Var.Create Doc.Empty
    let mathml2Mml = Var.Create ""
    let mathml2MmlPromise = Var.Create ""

    // Theme: light is the default; the choice is persisted in localStorage.
    // The storage key is shared with the bootstrap script in Main.html.
    let themeStorageKey = "websharper-mathjax-sitelets-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    // A converted MathJax node is a DOM element; wrap it as a `Doc` so the
    // template can insert it with `ws-hole`.
    let asDoc (node: Dom.Node) : Doc =
        Doc.Static (As<Dom.Element> node)

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <-
            if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    // Containers driven imperatively by MathJax, captured after the template
    // is rendered.
    let mutable staticMath : Dom.Element = As<Dom.Element> null
    let mutable preview : Dom.Element = As<Dom.Element> null

    let trackedItems () =
        if isNull staticMath then 0
        else
            MathJax.Startup.Document
                .GetMathItemsWithin([| box staticMath |])
                .Length

    // ---------------------------------------------------------------
    // 1. Page typesetting
    // ---------------------------------------------------------------

    let typeset () =
        MathJax.Typeset()
        typesetInfo.Value <- sprintf "typeset() — %d tracked expressions" (trackedItems ())

    let typesetPromise () =
        MathJax.TypesetPromise().Then(fun () ->
            typesetInfo.Value <-
                sprintf "typesetPromise() — %d tracked expressions" (trackedItems ())
        ) |> ignore

    let typesetClear () =
        MathJax.TypesetClear()
        typesetInfo.Value <-
            sprintf "typesetClear() — %d tracked expressions" (trackedItems ())

    // ---------------------------------------------------------------
    // 2. Dynamic preview (typesetClear + typesetPromise on new content)
    // ---------------------------------------------------------------

    let renderPreview () =
        if isNull preview then
            status.Value <- "The preview container is not in the DOM yet."
        else
            // Forget the old math *before* replacing the DOM content.
            MathJax.TypesetClear([| box preview |])
            MathJax.TexReset()
            preview.InnerHTML <- source.Value
            MathJax.TypesetPromise([| box preview |]).Then(fun () ->
                status.Value <- "Preview typeset."
            ) |> ignore

    // ---------------------------------------------------------------
    // 3. Conversions — sync and promise-based, all input formats
    // ---------------------------------------------------------------

    let runConversions () =
        // TeX -> CHTML / MathML
        tex2Chtml.Value <- asDoc (MathJax.Tex2chtml texSource)
        tex2Mml.Value <- MathJax.Tex2mml texSource

        // AsciiMath -> CHTML / MathML
        asciimath2Chtml.Value <- asDoc (MathJax.Asciimath2chtml asciimathSource)
        asciimath2Mml.Value <- MathJax.Asciimath2mml asciimathSource

        // MathML -> CHTML / MathML
        mathml2Chtml.Value <- asDoc (MathJax.Mathml2chtml mathmlSource)
        mathml2Mml.Value <- MathJax.Mathml2mml mathmlSource

        // The same conversions with the promise-based helpers.
        MathJax.Tex2chtmlPromise(texSource).Then(fun node ->
            tex2ChtmlPromise.Value <- asDoc node) |> ignore

        MathJax.Tex2mmlPromise(texSource).Then(fun mml ->
            tex2MmlPromise.Value <- mml) |> ignore

        MathJax.Asciimath2chtmlPromise(asciimathSource).Then(fun node ->
            asciimath2ChtmlPromise.Value <- asDoc node) |> ignore

        MathJax.Asciimath2mmlPromise(asciimathSource).Then(fun mml ->
            asciimath2MmlPromise.Value <- mml) |> ignore

        MathJax.Mathml2chtmlPromise(mathmlSource).Then(fun node ->
            mathml2ChtmlPromise.Value <- asDoc node) |> ignore

        MathJax.Mathml2mmlPromise(mathmlSource).Then(fun mml ->
            mathml2MmlPromise.Value <- mml) |> ignore

        MathJax.WhenReady(fun () ->
            status.Value <- "Conversions finished (whenReady)."
            null) |> ignore

    // ---------------------------------------------------------------
    // 4. Diagnostics
    // ---------------------------------------------------------------

    let runDiagnostics () =
        let inputNames =
            [| 0 .. MathJax.Startup.Input.Length - 1 |]
            |> Array.map (fun i -> MathJax.Startup.Input.[i].Name)
            |> String.concat ", "

        let metrics =
            if isNull staticMath then "n/a"
            else
                let m = MathJax.GetMetricsFor(staticMath, true)
                sprintf "em %.1f / ex %.1f / width %.1f / scale %.2f / family '%s'"
                    m.Em m.Ex m.ContainerWidth m.Scale m.Family

        let stylesheet =
            try
                let css = MathJax.ChtmlStylesheet()
                let text = MathJax.Startup.Adaptor.CssText(As<Dom.Element> css)
                sprintf "chtmlStylesheet(): %d chars" text.Length
            with _ -> "chtmlStylesheet(): n/a"

        diagnostics.Value <-
            String.concat "\n" [
                "version: " + MathJax.Version
                "input jax (array): " + inputNames
                "input jax (named): tex = " + MathJax.Startup.Input.Tex.Name
                "loader root: " + MathJax.Loader.GetRoot()
                "metrics: " + metrics
                stylesheet
                "document menu: " + (if isNull MathJax.Startup.Document.Menu then "absent" else "present")
                "tracked expressions: " + string (trackedItems ())
            ]

        MathJax.Loader.Load("input/asciimath").Then(fun _ ->
            status.Value <- "loader.load('input/asciimath') resolved.") |> ignore

    // ---------------------------------------------------------------
    // Entry point
    // ---------------------------------------------------------------

    let Main () =
        Templates.MainTemplate.Main()
            .status(status.V)
            .typesetInfo(typesetInfo.V)
            .diagnostics(diagnostics.V)
            .Source(source)
            .themeLabel(themeLabel.V)
            .Tex2Chtml(Doc.EmbedView tex2Chtml.View)
            .Tex2ChtmlPromise(Doc.EmbedView tex2ChtmlPromise.View)
            .Tex2Mml(tex2Mml.V)
            .Tex2MmlPromise(tex2MmlPromise.V)
            .Asciimath2Chtml(Doc.EmbedView asciimath2Chtml.View)
            .Asciimath2ChtmlPromise(Doc.EmbedView asciimath2ChtmlPromise.View)
            .Asciimath2Mml(asciimath2Mml.V)
            .Asciimath2MmlPromise(asciimath2MmlPromise.V)
            .Mathml2Chtml(Doc.EmbedView mathml2Chtml.View)
            .Mathml2ChtmlPromise(Doc.EmbedView mathml2ChtmlPromise.View)
            .Mathml2Mml(mathml2Mml.V)
            .Mathml2MmlPromise(mathml2MmlPromise.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .Typeset(fun _ -> typeset ())
            .TypesetPromise(fun _ -> typesetPromise ())
            .TypesetClear(fun _ -> typesetClear ())
            .RenderPreview(fun _ -> renderPreview ())
            .RunConversions(fun _ -> runConversions ())
            .RunDiagnostics(fun _ -> runDiagnostics ())
            .PageInit(fun () ->
                // Restore a previously chosen theme (light is the default).
                let savedTheme =
                    try JS.Window.LocalStorage.GetItem(themeStorageKey) with _ -> null
                if savedTheme = "dark" || savedTheme = "light" then
                    theme.Value <- savedTheme
                applyTheme ()

                // The containers are in the DOM now. MathJax's conversion
                // helpers are created when startup completes, so wait for that
                // promise before using them.
                staticMath <- JS.Document.GetElementById "static-math"
                preview <- JS.Document.GetElementById "preview"

                MathJax.Startup.Promise.Then(fun () ->
                    renderPreview ()
                    runConversions ()
                    runDiagnostics ()) |> ignore
            )
            .Doc()