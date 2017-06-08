#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.MathJax")
        .VersionFrom("WebSharper")

let main =
    bt.WebSharper.Extension("WebSharper.MathJax")
        .SourcesFromProject()
        .Embed([])
        .References(fun r -> [])

let sample =
    bt.WebSharper.SiteletWebsite("WebSharper.MathJax.Sample")
        .SourcesFromProject()
        .Embed([])
        .References(fun r ->
            [
                r.Project(main)
                r.NuGet("WebSharper.UI.Next").Latest(true).Reference()
            ])

//let tests =
//    bt.WebSharper.SiteletWebsite("WebSharper.MathJax.Tests")
//        .SourcesFromProject()
//        .Embed([])
//        .References(fun r ->
//            [
//                r.Project(main)
//                r.NuGet("WebSharper.Testing").Reference()
//                r.NuGet("WebSharper.UI.Next").Reference()
//            ])

bt.Solution [
    main
//  tests
    sample

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.MathJax-2.7.1"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/https://github.com/intellifactory/websharper.mathjax"
                Description = "WebSharper Extensions for MathJax 2.7.1"
                RequiresLicenseAcceptance = true })
        .Add(main)
]
|> bt.Dispatch
