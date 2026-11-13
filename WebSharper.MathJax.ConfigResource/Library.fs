namespace WebSharper.MathJax.Extension

open System
open WebSharper.Core.Resources

type ConfigResource() =
    interface IResource with
        member this.Render ctx =
            fun writer ->
                match ctx.GetSetting "MathjaxConfig" with
                | Some configJson when not (String.IsNullOrWhiteSpace configJson) ->
                    let html = writer Scripts
                    html.RenderBeginTag "script"
                    html.WriteLine()
                    // write the configuration code
                    html.WriteLine(configJson.Trim())
                    html.RenderEndTag()
                | _ -> ()