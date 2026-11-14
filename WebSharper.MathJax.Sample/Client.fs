namespace WebSharper.MathJax.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.MathJax

[<JavaScript>]
module Client =

    // The template is the whole document, loaded from the DOM, so editing
    // index.html and refreshing the browser is enough.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

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
    // The storage key is shared with the bootstrap script in index.html.
    let themeStorageKey = "websharper-mathjax-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    /// A converted MathJax node is a DOM element; wrap it as a `Doc` so the
    /// template can insert it with `ws-hole`.
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

    // The two containers that are driven imperatively by MathJax.
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
        // Synchronous; only safe while no font data needs loading.
        MathJax.Typeset()
        typesetInfo.Value <- sprintf "typeset() — %d tracked expressions" (trackedItems ())

    let typesetPromise () =
        MathJax.TypesetPromise().Then(fun () ->
            typesetInfo.Value <-
                sprintf "typesetPromise() — %d tracked expressions" (trackedItems ())
        ) |> ignore

    let typesetClear () =
        // Tell MathJax to forget the math it has typeset (it stays on the page).
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
        // TeX -> CHTML / MathML (synchronous)
        tex2Chtml.Value <- asDoc (MathJax.Tex2chtml texSource)
        tex2Mml.Value <- MathJax.Tex2mml texSource

        // AsciiMath -> CHTML / MathML (synchronous)
        asciimath2Chtml.Value <- asDoc (MathJax.Asciimath2chtml asciimathSource)
        asciimath2Mml.Value <- MathJax.Asciimath2mml asciimathSource

        // MathML -> CHTML / MathML (synchronous)
        mathml2Chtml.Value <- asDoc (MathJax.Mathml2chtml mathmlSource)
        mathml2Mml.Value <- MathJax.Mathml2mml mathmlSource

        // The same conversions using the promise-based variants. These wait
        // for any pending typeset/conversion work, and for font data.
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

        // whenReady() queues an action after all pending MathJax work.
        MathJax.WhenReady(fun () ->
            status.Value <- "Conversions finished (whenReady)."
            null) |> ignore

    // ---------------------------------------------------------------
    // 4. Diagnostics — startup, loader, metrics, document
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
                let text =
                    MathJax.Startup.Adaptor.CssText(As<Dom.Element> css)
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

        // loader.load() resolves once the component is available (it is
        // already loaded by the combined component, so this is a no-op).
        MathJax.Loader.Load("input/asciimath").Then(fun _ ->
            status.Value <- "loader.load('input/asciimath') resolved.") |> ignore

    // ---------------------------------------------------------------
    // Entry point
    // ---------------------------------------------------------------

    [<SPAEntryPoint>]
    let Main () =
        // Restore a previously chosen theme (light is the default).
        let savedTheme =
            try JS.Window.LocalStorage.GetItem(themeStorageKey) with _ -> null
        if savedTheme = "dark" || savedTheme = "light" then
            theme.Value <- savedTheme
        applyTheme ()

        // `.Bind()` renders the whole document body; the containers are
        // available in `staticInit` / `previewInit` afterwards.
        IndexTemplate()
            .status(status.V)
            .typesetInfo(typesetInfo.V)
            .diagnostics(diagnostics.V)
            .source(source)
            .themeLabel(themeLabel.V)
            .tex2chtml(Doc.EmbedView tex2Chtml.View)
            .tex2chtmlPromise(Doc.EmbedView tex2ChtmlPromise.View)
            .tex2mml(tex2Mml.V)
            .tex2mmlPromise(tex2MmlPromise.V)
            .asciimath2chtml(Doc.EmbedView asciimath2Chtml.View)
            .asciimath2chtmlPromise(Doc.EmbedView asciimath2ChtmlPromise.View)
            .asciimath2mml(asciimath2Mml.V)
            .asciimath2mmlPromise(asciimath2MmlPromise.V)
            .mathml2chtml(Doc.EmbedView mathml2Chtml.View)
            .mathml2chtmlPromise(Doc.EmbedView mathml2ChtmlPromise.View)
            .mathml2mml(mathml2Mml.V)
            .mathml2mmlPromise(mathml2MmlPromise.V)
            .toggleTheme(fun _ -> toggleTheme ())
            .typeset(fun _ -> typeset ())
            .typesetPromise(fun _ -> typesetPromise ())
            .typesetClear(fun _ -> typesetClear ())
            .renderPreview(fun _ -> renderPreview ())
            .runConversions(fun _ -> runConversions ())
            .runDiagnostics(fun _ -> runDiagnostics ())
            .Bind()

        // The containers are in the DOM now. MathJax's conversion helpers are
        // created when startup completes, so wait for that promise before
        // using them (in v4 startup may need to fetch font data).
        staticMath <- JS.Document.GetElementById "static-math"
        preview <- JS.Document.GetElementById "preview"

        MathJax.Startup.Promise.Then(fun () ->
            renderPreview ()
            runConversions ()
            runDiagnostics ()) |> ignore