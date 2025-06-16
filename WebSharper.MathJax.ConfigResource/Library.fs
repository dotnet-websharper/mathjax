namespace WebSharper.MathJax.Extension

open WebSharper.Core.Resources

type ConfigResource() =
    interface IResource with
        member this.Render ctx =
            fun writer ->
                let html = writer Scripts

                let configJson =
                    ctx.GetSetting("MathjaxConfig")
                    |> Option.defaultValue ""

                html.RenderBeginTag "script"
                
                html.WriteLine()
                // write the configuration code
                html.WriteLine(configJson.Trim())

                html.RenderEndTag()