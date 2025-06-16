namespace WebSharper.MathJax.Extension

open WebSharper.Core.Resources

type ConfigResource() =
    interface IResource with
        member this.Render ctx =
            fun writer ->
                let html = writer Scripts

                let defaultValue = """
                    MathJax =
                        {
                          loader: {
                            load: [
                              "input/tex", "input/mml",
                              "[tex]/newcommand", "[tex]/action",
                              "output/chtml"
                            ]
                          },
                          tex: {
                            inlineMath: [["$", "$"], ["\\(", "\\)"]],
                            packages: ["base", "newcommand", "action"]
                          }
                        }
                    """

                let configJson =
                    ctx.GetSetting("MathjaxConfig")
                    |> Option.defaultValue ""

                html.RenderBeginTag "script"
                
                html.WriteLine()
                // write the configuration code
                html.WriteLine(configJson.Trim())

                html.RenderEndTag()