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
namespace WebSharper.MathJax.Extension

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module MathJaxV3 =
    let MathJaxClass = Class "MathJax"
    let HandlerListClass = Class "MathJax.HandlerList"
    let MathDocumentClass = Class "MathJax.MathDocument"
    let MathListClass = Class "MathJax.MathList"
    let LinkedListClass = Class "MathJax.LinkedList"
    let ListItemClass = Class "MathJax.ListItem"
    let BitFieldClass = Class "MathJax.BitField"
    let PrioritizedListClass = Class "MathJax.PrioritizedList"
    let RenderListClass = Class "MathJax.RenderList"
    let MathItemClass = Class "MathJax.MathItem"
    let DOMAdaptorClass = Class "MathJax.DOMAdaptor"
    let InputJaxClass = Class "MathJax.InputJax"
    let FunctionListClass = Class "MathJax.FunctionList"
    let OutputJaxClass = Class "MathJax.OutputJax"
    let MathJaxStartupClass = Class "MathJax.Startup"
    let HandlerClass = Class "MathJax.Handler"
    let MathJaxLoaderClass = Class "MathJax.Loader"

    let ContainerList = T<string> + T<obj> + !|T<obj> + !|(!|T<obj>)
    let OptionList = T<obj>
    let SortFn = T<obj> * T<obj> ^-> T<bool>
    let MmlNode = T<obj>
    let MmlFactoryClass = T<obj>

    let ResetList =
        Pattern.Config "ResetList" { 
            Required = []
            Optional = [ 
                "all", T<bool>
                "processed", T<bool>
                "inputJax", !|T<obj>
                "outputJax", !|T<obj> 
            ]
        }

    let Metrics =
        Pattern.Config "Metrics" { 
            Required = []
            Optional = [ 
                "em", T<float>
                "ex", T<float>
                "containerWidth", T<float>
                "lineWidth", T<float>
                "scale", T<float> 
            ] 
        }

    let Location =
        Pattern.Config "Location" { 
            Required = []
            Optional = [ 
                "i", T<float>
                "n", T<float>
                "delim", T<string>
                "node", T<obj> 
            ] 
        }

    let RenderData =
        Pattern.Config "RenderData" { 
            Required = []
            Optional = [ 
                "id", T<string>
                "renderDoc", MathDocumentClass ^-> T<bool>
                "renderMath", MathItemClass * MathDocumentClass?documen ^-> T<bool>
                "convert", T<bool> 
            ] 
        }

    let AttributeData =
        Pattern.Config "AttributeData" { 
            Required = []
            Optional = [ 
                "name", T<string>
                "value", T<string> 
            ] 
        }

    let PageBBox =
        Pattern.Config "PageBBox" { 
            Required = []
            Optional = [ 
                "left", T<float>
                "right", T<float>
                "top", T<float>
                "bottom", T<float> 
            ] 
        }

    let ProtoItem =
        Pattern.Config "ProtoItem" { 
            Required = []
            Optional = [ 
                "math", T<string>
                "start", Location.Type
                "end", Location.Type
                "open", T<string>
                "close", T<string>
                "n", T<int>
                "display", T<bool> 
            ] 
        }

    let PackageError =
        Class "PackageError" 
            |=> Inherits T<Error>
            |+> Static [ 
                Constructor(T<string> * T<string>) 
            ]
            |+> Instance [ 
                "package" =@ T<string> 
            ]

    let LoaderConfig =
        Pattern.Config "LoaderConfig" { 
            Required = []
            Optional = [ 
                "paths", T<obj>
                "source", T<obj>
                "dependencies", T<obj>
                "provides", T<obj>
                "load", !|T<string>
                "ready", T<string> ^-> T<unit> + T<string>
                "failed", PackageError ^-> T<unit>
                "require", T<string> ^-> T<obj>
                "pathFilters", !|T<obj>
                "versionWarnings", T<bool> 
            ] 
        }

    let TexConfig =
        Pattern.Config "TexConfig" { 
            Required = []
            Optional = [ 
                "inlineMath", !|T<string * string>
                "displayMath", !|T<string * string>
                "packages", !|T<string> 
            ] 
        }

    let AsciiMathConfig =
        Pattern.Config "AsciiMathConfig" { 
            Required = []
            Optional = [ 
                "delimiters", !|T<string * string> 
            ] 
        }

    let ChtmlConfig =
        Pattern.Config "ChtmlConfig" { 
            Required = []
            Optional = [ 
                "fontURL", T<string>
                "font", T<string> 
            ] 
        }

    let StartupConfig =
        Pattern.Config "StartupConfig" { 
            Required = []
            Optional = [ 
                "input", !|T<string>
                "output", T<string>
                "handler", T<string>
                "document", T<obj>
                "adaptor", T<string>
                "typeset", T<bool>
                "ready", T<unit> ^-> T<unit>
                "pageReady", T<unit> ^-> T<unit> 
            ] 
        }

    let MathJaxConfig =
        Pattern.Config "MathJaxConfig" { 
            Required = []
            Optional = [ 
                "tex", TexConfig + T<obj>
                "asciimath", AsciiMathConfig.Type
                "chtml", ChtmlConfig.Type
                "startup", StartupConfig + T<obj>
                "loader", LoaderConfig + T<obj>
            ] 
        }

    FunctionListClass 
        |=> Inherits PrioritizedListClass
        |+> Instance [ 
            "execute" => !+(!|T<obj>) ^-> T<bool>

            "asyncExecute" => !+(!|T<obj>) ^-> T<Promise<unit>> 
        ]
        |> ignore

    DOMAdaptorClass 
        |=> Nested [ AttributeData; PageBBox ]
        |+> Instance
            [
              // property
              "document" =? T<obj>

              // methods
              "parse" => T<string>?text * !?T<string>?format ^-> T<obj>

              "node" => T<string>?kind * !?OptionList?def * !?(!|T<obj>)?children * !?T<string>?ns ^-> T<obj>

              "text" => T<string>?text ^-> T<obj>

              "head" => T<obj>?doc ^-> T<obj>

              "body" => T<obj>?doc ^-> T<obj>

              "root" => T<obj>?doc ^-> T<obj>

              "doctype" => T<obj>?doc ^-> T<string>

              "tags" => T<obj>?node * T<string> * !?T<string>?ns ^-> !|T<obj>

              "getElements" => ContainerList?nodes * T<obj>?doc ^-> !|T<obj>

              "contains" => T<obj>?container * T<obj>?node ^-> T<bool>

              "parent" => T<obj>?node ^-> T<obj>

              "append" => T<obj>?node * T<obj>?child ^-> T<obj>

              "insert" => T<obj>?nchild * T<obj>?ochild ^-> T<unit>

              "remove" => T<obj>?child ^-> T<obj>

              "replace" => T<obj>?nnode * T<obj>?onode ^-> T<obj>

              "clone" => T<obj>?node ^-> T<obj>

              "split" => T<obj>?node * T<int>?n ^-> T<obj>

              "next" => T<obj>?node ^-> T<obj>

              "previous" => T<obj>?node ^-> T<obj>

              "firstChild" => T<obj>?node ^-> T<obj>

              "lastChild" => T<obj>?node ^-> T<obj>

              "childNodes" => T<obj>?node ^-> (!|T<obj>)

              "childNode" => T<obj>?node * T<int>?i ^-> T<obj>

              "kind" => T<obj>?node ^-> T<string>

              "value" => T<obj>?node ^-> T<string>

              "textContent" => T<obj>?node ^-> T<string>

              "innerHTML" => T<obj>?node ^-> T<string>

              "outerHTML" => T<obj>?node ^-> T<string>

              "serializeXML" => T<obj>?node ^-> T<string>

              "setAttribute" => T<obj>?node * T<string> * (T<string> + T<int>)?value * !?T<string>?ns ^-> T<unit>

              "setAttributes" => T<obj>?node * OptionList?def ^-> T<unit>

              "getAttribute" => T<obj>?node * T<string> ^-> T<string>

              "removeAttribute" => T<obj>?node * T<string> ^-> T<unit>

              "hasAttribute" => T<obj>?node * T<string> ^-> T<bool>

              "allAttributes" => T<obj>?node ^-> !|AttributeData

              "addClass" => T<obj>?node * T<string> ^-> T<unit>

              "removeClass" => T<obj>?node * T<string> ^-> T<unit>

              "hasClass" => T<obj>?node * T<string> ^-> T<bool>

              "allClasses" => T<obj>?node ^-> !|T<string>

              "setStyle" => T<obj>?node * T<string> * T<string>?value ^-> T<unit>

              "getStyle" => T<obj>?node * T<string> ^-> T<string>

              "allStyles" => T<obj>?node ^-> T<string>

              "insertRules" => T<obj>?node * (!|T<string>)?rules ^-> T<unit>

              "fontSize" => T<obj>?node ^-> T<int>

              "fontFamily" => T<obj>?node ^-> T<string>

              "nodeSize" => T<obj>?node * !?T<int>?em * !?T<bool>?local ^-> !|T<float>

              "nodeBBox" => T<obj>?node ^-> PageBBox 
            ]
        |> ignore

    InputJaxClass 
        |=> Nested [ ProtoItem ]
        |+> Instance
            [
              // properties
              "name" =? T<string>

              "processStrings" =? T<bool>

              "options" =? OptionList

              "preFilters" =? FunctionListClass

              "postFilters" =? FunctionListClass

              "adaptor" =? DOMAdaptorClass

              "mmlFactory" =? MmlFactoryClass

              // methods
              "setAdaptor" => DOMAdaptorClass ^-> T<unit>

              "setMmlFactory" => MmlFactoryClass ^-> T<unit>

              "initialize" => T<unit> ^-> T<unit>

              "reset" => !+(!|T<obj>) ^-> T<unit>

              "findMath" => (T<obj> + !|T<string>)?which * !?OptionList?options ^-> !|ProtoItem

              "compile" => MathItemClass * MathDocumentClass ^-> MmlNode 
            ]
        |> ignore

    OutputJaxClass
        |+> Static [ Constructor(!?OptionList) ]
        |+> Instance
            [
              // properties
              "name" =? T<string>
              "options" =? OptionList
              "postFilters" =? FunctionListClass
              "adaptor" =? DOMAdaptorClass

              // methods
              "setAdaptor" => DOMAdaptorClass ^-> T<unit>
              "initialize" => T<unit> ^-> T<unit>
              "reset" => !+T<obj> ^-> T<unit>
              "typeset" => MathItemClass * !?MathDocumentClass ^-> T<obj>
              "escaped" => MathItemClass * !?MathDocumentClass ^-> T<obj>
              "getMetrics" => MathDocumentClass ^-> T<unit>
              "styleSheet" => MathDocumentClass ^-> T<obj>
              "pageElements" => MathDocumentClass ^-> T<obj> 
            ]
        |> ignore

    PrioritizedListClass
        |+> Static [ 
            Constructor T<unit>; "DEFAULTPRIORITY" =? T<int> 
        ]
        |+> Instance
            [ 
                "add" => T<obj>?item * !?T<int>?priority ^-> T<obj>

                "remove" => T<obj>?item ^-> T<unit> 
            ]
        |> ignore

    HandlerListClass 
        |=> Inherits PrioritizedListClass
        |+> Instance
            [ 
                "register" => T<obj>?handler ^-> T<obj>

                "unregister" => T<obj>?handler ^-> T<unit>

                "handlesDocument" => MathDocumentClass ^-> T<obj>

                "document" => MathDocumentClass * !?T<obj>?options ^-> MathDocumentClass 
            ]
        |> ignore

    MathItemClass
        |+> Static [ 
            Constructor(T<string> * InputJaxClass * !?T<bool> * !?Location * !?Location) 
        ]
        |+> Instance
            [
                // properties
                "math" =? T<string>

                "inputJax" =? InputJaxClass

                "display" =? T<bool>

                "isEscaped" =? T<bool>

                "start" =? Location

                "end" =? Location

                "root" =? MmlNode

                "typesetRoot" =? T<obj>

                "metrics" =? Metrics

                "inputData" =? OptionList

                "outputData" =? OptionList

                "_state" =? T<int>

                // methods
                "render" => MathDocumentClass ^-> T<unit>

                "rerender" => MathDocumentClass * !?T<int> ^-> T<unit>

                "convert" => MathDocumentClass * !?T<int> ^-> T<unit>

                "compile" => MathDocumentClass ^-> T<unit>

                "typeset" => MathDocumentClass ^-> T<unit>

                "updateDocument" => MathDocumentClass ^-> T<unit>

                "removeFromDocument" => T<bool> ^-> T<unit>

                "setMetrics" => T<float> * T<float> * T<float> * T<float> * T<float> ^-> T<unit>

                "state" => !?T<int> * !?T<bool> ^-> T<int>

                "reset" => !?T<bool> ^-> T<unit> 
            ]
        |> ignore

    MathListClass
        |+> Instance 
            [ 
                "isBefore" => MathItemClass * MathItemClass ^-> T<bool> 
            ]
        |> ignore

    ListItemClass
        |+> Static [ 
            Constructor(!?T<obj>?data) 
        ]
        |+> Instance 
            [ 
                "data" =? T<obj>
                "next" =? ListItemClass
                "prev" =? ListItemClass 
            ]
        |> ignore

    LinkedListClass
        |+> Static [ 
            Constructor(!+(!|T<obj>)) 
        ]
        |+> Instance
            [
                // methods
                "isBefore" => T<obj> * T<obj> ^-> T<bool>
                "push" => !+T<obj> ^-> LinkedListClass
                "pop" => T<unit> ^-> T<obj>
                "unshift" => !+T<obj> ^-> LinkedListClass
                "shift" => T<unit> ^-> T<obj>
                "remove" => !+T<obj> ^-> T<unit>
                "clear" => T<unit> ^-> LinkedListClass
                "insert" => T<obj> * !?SortFn ^-> LinkedListClass
                "sort" => !?SortFn ^-> LinkedListClass
                "merge" => LinkedListClass * !?SortFn ^-> LinkedListClass

              ]
        |> ignore

    BitFieldClass
        |+> Static
            [

                "MAXBIT" =? T<int>

                "next" =? T<int>

                "names" =? T<obj>

                "allocate" => !+(!|T<string>) ^-> T<unit>

                "has" => T<string> ^-> T<bool> 
            ]
        |+> Instance
            [ 
                "bits" =? T<int>

                "set" => T<string> ^-> T<unit>

                "clear" => T<string> ^-> T<unit>

                "isSet" => T<string> ^-> T<bool>

                "reset" => T<unit> ^-> T<unit>

                "getBit" => T<string> ^-> T<int> 
            ]
        |> ignore

    RenderListClass 
        |=> Nested [ RenderData ] 
        |=> Inherits PrioritizedListClass
        |+> Static
            [ 
                "create" => (!|T<obj>) ^-> RenderListClass

                "action" => T<string> * T<obj> ^-> !|RenderData

                "methodActions" => T<string> * !?T<string> ^-> !|(T<obj> * T<obj> ^-> T<bool>) 
            ]
        |+> Instance
            [ 
                "renderDoc" => MathDocumentClass * !?T<int> ^-> T<unit>

                "renderMath" => MathItemClass * MathDocumentClass * !?T<int> ^-> T<unit>

                "renderConvert" => MathItemClass * MathDocumentClass * !?T<int> ^-> T<unit>

                "findID" => T<string> ^-> RenderData 
            ]
        |> ignore

    MathDocumentClass 
        |=> Nested [ ResetList ]
        |+> Static
            [ 
                Constructor(!+T<obj[]>)
                Constructor !?(T<obj> * DOMAdaptorClass * OptionList) 
            ]
        |+> Instance
            [
                // properties
                "KIND" =? T<string>

                "OPTIONS" =? OptionList

                "ProcessBits" =? BitFieldClass

                "document" =? T<obj>

                "kind" =? T<string>

                "options" =? OptionList

                "math" =? MathListClass

                "renderActions" =? RenderListClass

                "processed" =? BitFieldClass

                "inputJax" =? !|InputJaxClass

                "outputJax" =? OutputJaxClass

                "adaptor" =? DOMAdaptorClass

                "mmlFactory" =? MmlFactoryClass

                // methods

                "addRenderAction" => T<string> *+ (!|T<obj>) ^-> T<unit>

                "removeRenderAction" => T<string> ^-> T<unit>

                "render" => T<unit> ^-> MathDocumentClass

                "rerender" => !?T<int> ^-> MathDocumentClass

                "convert" => T<string> * !?OptionList?options ^-> T<obj> + MmlNode

                "findMath" => !?OptionList?options ^-> MathDocumentClass

                "compile" => T<unit> ^-> MathDocumentClass

                "compileMath" => MathItemClass ^-> T<unit>

                "compileError" => MathItemClass ^-> T<unit>

                "getMetrics" => T<unit> ^-> MathDocumentClass

                "typeset" => T<unit> ^-> MathDocumentClass

                "typesetError" => MathItemClass * T<Error> ^-> MathDocumentClass

                "updateDocument" => T<unit> ^-> MathDocumentClass

                "removeFromDocument" => !?T<bool> ^-> MathDocumentClass

                "state" => T<int> * !?T<bool> ^-> MathDocumentClass

                "reset" => !?ResetList?options ^-> MathDocumentClass

                "clear" => T<unit> ^-> MathDocumentClass

                "concat" => MathListClass?list ^-> MathDocumentClass

                "clearMathItemsWithin" => ContainerList?containers ^-> !|MathItemClass

                "getMathItemsWithin" => ContainerList?containers ^-> !|MathItemClass 
            ]
        |> ignore

    HandlerClass
        |+> Static [ 
            Constructor(DOMAdaptorClass * !?T<int>) 
        ]
        |+> Instance
            [ 
                "name" =@ T<string>
                "adaptor" =@ DOMAdaptorClass
                "priority" =@ T<int>
                "documentClass" =@ MathDocumentClass

                // methods
                "handlesDocument" => T<obj> ^-> T<bool>
                "create" =? T<obj> * OptionList ^-> MathDocumentClass 
            ]
        |> ignore

    MathJaxLoaderClass
        |+> Static
            [
                // properties
                "versions" =? T<obj>
                "pathFilters" =? FunctionListClass

                // methods
                "ready" => !+T<string> ^-> T<Promise<_>>[!|T<string>]
                "load" => !+T<string> ^-> T<Promise<_>>[T<unit> + !|T<string>]
                "preLoad" => !+T<string> ^-> T<unit>
                "defaultReady" => T<unit> ^-> T<unit>
                "getRoot" => T<unit> ^-> T<string>
                "checkVersion" => T<string>?name * T<string>?version * !?T<string>?_type ^-> T<bool> 
            ]
        |> ignore

    MathJaxStartupClass
        |+> Static
            [
                // properties
                "constructors" =? T<obj>
                "input" =@ !|InputJaxClass
                "output" =@ OutputJaxClass
                "handler" =? HandlerClass
                "adaptor" =? DOMAdaptorClass
                "elements" =@ T<obj>
                "document" =? MathDocumentClass

                "promiseResolve" =@ (T<unit> ^-> T<unit>)
                "promiseReject" =@ (T<obj> ^-> T<unit>)
                "promise" =? T<Promise<unit>>
                "pagePromise" =? T<Promise<unit>>

                // methods
                "toMML" => MmlNode?node ^-> T<string>
                "registerConstructor" => T<string>?name * T<obj>?constructor ^-> T<unit>

                "useHandler" => T<string>?name * !?T<bool>?force ^-> T<unit>
                "useAdaptor" => T<string>?name * !?T<bool>?force ^-> T<unit>
                "useInput" => T<string>?name * !?T<bool>?force ^-> T<unit>
                "useOutput" => T<string>?name * !?T<bool>?force ^-> T<unit>

                "extendHandler" => (HandlerClass?handler ^-> HandlerClass)?extend * !?T<int>?priority ^-> T<unit>

                "defaultReady" => T<unit> ^-> T<unit>
                "defaultPageReady" => T<unit> ^-> T<Promise<unit>>

                "getComponents" => T<unit> ^-> T<unit>
                "makeMethods" => T<unit> ^-> T<unit>
                "makeTypesetMethods" => T<unit> ^-> T<unit>

                "makeOutputMethods" => T<string>?iname * T<string>?oname * InputJaxClass?input ^-> T<unit>
                "makeMmlMethods" => T<string>?name * InputJaxClass?input ^-> T<unit>
                "makeResetMethod" => T<string>?name * InputJaxClass?input ^-> T<unit>

                "getInputJax" => T<unit> ^-> !|InputJaxClass
                "getOutputJax" => T<unit> ^-> OutputJaxClass
                "getAdaptor" => T<unit> ^-> DOMAdaptorClass
                "getHandler" => T<unit> ^-> HandlerClass
                "getDocument" => !?T<obj>?root ^-> MathDocumentClass 
            ]
        |> ignore

    MathJaxClass
        |+> Static
            [
                // properties
                "version" =? T<string>

                "startup" =@ MathJaxStartupClass

                "options" =@ T<obj>

                "debug" =@ T<bool>

                "config" =@ MathJaxConfig

                "loader" =@ MathJaxLoaderClass

                // typeset
                "typeset" => !?(!|T<obj>) ^-> T<unit>

                "typesetPromise" => !?(!|(T<obj> + T<obj>)) ^-> T<Promise<unit>>

                "typesetClear" => !?T<obj> ^-> T<unit>

                // TeX
                "tex2chtml" => T<string> * !?T<obj> ^-> T<obj>

                "tex2chtmlPromise" => T<string> * !?T<obj> ^-> T<Promise<obj>>

                "tex2mml" => T<string> * !?T<obj> ^-> T<obj>

                "tex2mmlPromise" => T<string> * !?T<obj> ^-> T<Promise<obj>>

                "texReset" => T<unit> ^-> T<unit>                

                // CHTML
                "chtmlStylesheet" => T<unit> ^-> T<obj>

                "getMetricsFor" => T<obj> * T<bool> ^-> Metrics

                // AsciiMath
                "asciimath2mml" => T<string> * !?T<obj> ^-> T<obj>

                "asciimath2mmlPromise" => T<string> * !?T<obj> ^-> T<Promise<obj>>

                "asciimathReset" => T<unit> ^-> T<unit>

                "asciimath2chtml" => T<string> * !?T<obj> ^-> T<obj>

                "asciimath2chtmlPromise" => T<string> * !?T<obj> ^-> T<Promise<obj>>

                // MathML
                "mathml2mml" => T<string> * !?T<obj> ^-> T<obj>

                "mathml2mmlPromise" => T<string> * !?T<obj> ^-> T<Promise<obj>>

                "mathmlReset" => T<unit> ^-> T<unit>

                "mathml2chtml" => T<string> * !?T<obj> ^-> T<obj>

                "mathml2chtmlPromise" => T<string> * !?T<obj> ^-> T<Promise<obj>>
            ]
        |> ignore

    let Assembly =
        Assembly [ 
            Namespace "WebSharper.MathJax.Resources" [
                Resource "Js" "https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js"
                |> AssemblyWide
            ]
            Namespace "WebSharper.MathJax" [ 
                MathJaxClass
                MathJaxConfig
                HandlerListClass
                MathDocumentClass
                MathListClass
                LinkedListClass
                ListItemClass
                BitFieldClass
                PrioritizedListClass
                RenderListClass
                MathItemClass
                DOMAdaptorClass
                InputJaxClass
                FunctionListClass
                OutputJaxClass
                MathJaxStartupClass
                HandlerClass
                MathJaxLoaderClass
                Metrics
                Location
                PackageError
                LoaderConfig
                TexConfig
                AsciiMathConfig
                ChtmlConfig
                StartupConfig 
            ] 
        ]
