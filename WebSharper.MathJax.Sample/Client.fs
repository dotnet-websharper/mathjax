// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
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
            Elt.div [] [
                text "When $a \\ne 0$, there are two solutions to \(ax^2 + bx + c = 0\) and they are $$x = {-b \pm \sqrt{b^2-4ac} \over 2a}.$$"
            ]

        JQuery.Of("#btn").On("click", (fun _ _ ->
            JQuery.Of("#tex").Append(f.Dom).Ignore
            MathJax.Hub.Queue([| "Typeset", MathJax.Hub :> obj, [| f.Dom :> obj |] |]) |> ignore
        )).Ignore
        ()
