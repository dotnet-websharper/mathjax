// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2026 IntelliFactory
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

module Definition =
    let MathJaxClass = Class "MathJax"
    let MathJaxStartupClass = Class "MathJaxStartup"
    let MathJaxLoaderClass = Class "MathJaxLoader"
    let InputJaxListClass = Class "InputJaxList"
    let MenuClass = Class "MathJax.Menu"
    let PackageClass = Class "MathJax.Package"
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
    let HandlerClass = Class "MathJax.Handler"

    let ContainerList = T<string> + T<obj> + !|T<obj> + !|(!|T<obj>)
    let OptionList = T<obj>
    let SortFn = T<obj> * T<obj> ^-> T<bool>
    let MmlNode = T<obj>
    let MmlFactoryClass = T<obj>

    // Options accepted by the conversion helpers (`tex2chtml`, `asciimath2svg`, ...).
    let ConversionOptions =
        Pattern.Config "ConversionOptions" {
            Required = []
            Optional = [
                "display", T<bool>
                "em", T<float>
                "ex", T<float>
                "containerWidth", T<float>
                "scale", T<float>
                "family", T<string>
            ]
        }

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
                "scale", T<float>
                "family", T<string>
                "display", T<bool>
            ] 
        }

    let Location =
        Pattern.Config "Location" { 
            Required = []
            Optional = [ 
                "i", T<int>
                "n", T<int>
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

    // `(jax: InputJax, error: Error) => void` - `tex.formatError`.
    let FormatError = InputJaxClass * T<Error> ^-> T<unit>

    let TeXInputConfig =
        Pattern.Config "TeXInputConfig" {
            Required = []
            Optional = [
                "inlineMath", T<obj>
                "displayMath", T<obj>
                "packages", T<obj>
                "macros", T<obj>
                "processEscapes", T<bool>
                "processEnvironments", T<bool>
                "processRefs", T<bool>
                "numberPattern", T<string>
                "tagSide", T<string>
                "tagIndent", T<string>
                "useLabelIds", T<bool>
                "maxBuffer", T<int>
                "maxMacros", T<int>
                "maxTemplateSubtitutions", T<int>
                "mathStyle", T<string>
                "formatError", FormatError
            ]
        }

    let MathMLInputConfig =
        Pattern.Config "MathMLInputConfig" {
            Required = []
            Optional = [
                "parseAs", T<string>
                "forceReparse", T<bool>
                "mmlFilters", !|T<obj>
                "verify", T<obj>
            ]
        }

    let AsciiMathInputConfig =
        Pattern.Config "AsciiMathInputConfig" {
            Required = []
            Optional = [
                "fixphi", T<bool>
                "decipher", T<bool>
                "decimalsign", T<string>
                "mathvariant", T<string>
                "delimiters", T<obj>
            ]
        }

    let LinebreakConfig =
        Pattern.Config "LinebreakConfig" {
            Required = []
            Optional = [
                "inline", T<bool>
                "width", T<string>
                "lineleading", T<float>
                "LinebreakVisitor", T<obj>
            ]
        }

    let CommonOutputConfig =
        [
            "scale", T<float>
            "minScale", T<float>
            "mtextInheritFont", T<bool>
            "merrorInheritFont", T<bool>
            "mtextFont", T<string>
            "merrorFont", T<string>
            "mathmlSpacing", T<bool>
            "skipAttributes", T<obj>
            "exFactor", T<float>
            "displayAlign", T<string>
            "displayIndent", T<string>
            "displayOverflow", T<string>
            "linebreaks", LinebreakConfig.Type
            "font", T<string>
            "fontExtensions", !|T<obj>
            "htmlHDW", T<string>
            "wrapperFactory", T<obj>
            "fontData", T<obj>
            "fontPath", T<string>
            "styleJson", T<obj>
        ]

    let ChtmlConfig =
        Pattern.Config "ChtmlConfig" { 
            Required = []
            Optional = [
                "adaptiveCSS", T<bool>
                "matchFontHeight", T<bool>
                "fontURL", T<string>
            ] @ CommonOutputConfig
        }

    let SvgConfig =
        Pattern.Config "SvgConfig" {
            Required = []
            Optional = [
                "blacker", T<int>
                "fontCache", T<string>
                "localID", T<string>
                "useXlink", T<bool>
                "internalSpeechTitles", T<bool>
                "titleID", T<int>
            ] @ CommonOutputConfig
        }

    let MenuSettings =
        Pattern.Config "MenuSettings" {
            Required = []
            Optional = [
                "showSRE", T<bool>
                "showLatex", T<bool>
                "texHints", T<bool>
                "semantics", T<bool>
                "zoom", T<string>
                "zscale", T<string>
                "renderer", T<string>
                "alt", T<bool>
                "cmd", T<bool>
                "ctrl", T<bool>
                "shift", T<bool>
                "scale", T<string>
                "overflow", T<string>
                "breakInline", T<bool>
                "autocollapse", T<bool>
                "collapsible", T<bool>
                "enrich", T<bool>
                "assistiveMml", T<bool>
                "backgroundColor", T<string>
                "backgroundOpacity", T<string>
                "braille", T<bool>
                "brailleCode", T<string>
                "brailleSpeech", T<bool>
                "brailleCombine", T<bool>
                "foregroundColor", T<string>
                "foregroundOpacity", T<string>
                "highlight", T<string>
                "infoPrefix", T<bool>
                "infoRole", T<bool>
                "infoType", T<bool>
                "inTabOrder", T<bool>
                "locale", T<string>
                "magnification", T<string>
                "magnify", T<string>
                "speech", T<bool>
                "speechRules", T<string>
                "subtitles", T<bool>
                "treeColoring", T<bool>
                "viewBraille", T<bool>
                "voicing", T<bool>
                "help", T<bool>
                "roleDescription", T<string>
                "tabSelects", T<string>
            ]
        }

    let MenuOptions =
        Pattern.Config "MenuOptions" {
            Required = []
            Optional = [
                "settings", MenuSettings.Type
                "annotation", T<string>
            ]
        }

    // The document options (`MathJax.config.options`).
    let DocumentOptions =
        Pattern.Config "DocumentOptions" {
            Required = []
            Optional = [
                "skipHtmlTags", !|T<string>
                "ignoreHtmlClass", T<string>
                "processHtmlClass", T<string>
                "enableMenu", T<bool>
                "menuOptions", MenuOptions.Type
            ]
        }

    FunctionListClass 
        |=> Inherits PrioritizedListClass
        |+> Instance [ 
            "execute" => !+(!|T<obj>) ^-> T<bool>

            "asyncExecute" => !+(!|T<obj>) ^-> T<Promise<unit>> 

            "addList" => !|(T<obj> + (T<obj> * T<int>)) ^-> T<unit>
        ]
        |> ignore

    DOMAdaptorClass 
        |=> Nested [ AttributeData; PageBBox ]
        |+> Instance
            [
              // properties
              "document" =? T<obj>

              "canMeasureNodes" =? T<bool>

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

              "getElement" => T<string>?selector * !?T<obj>?node ^-> T<obj>

              "contains" => T<obj>?container * T<obj>?node ^-> T<bool>

              "parent" => T<obj>?node ^-> T<obj>

              "append" => T<obj>?node * T<obj>?child ^-> T<obj>

              "insert" => T<obj>?nchild * T<obj>?ochild ^-> T<unit>

              "remove" => T<obj>?child ^-> T<obj>

              "replace" => T<obj>?nnode * T<obj>?onode ^-> T<obj>

              "clone" => T<obj>?node * !?T<bool>?deep ^-> T<obj>

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

              "setProperty" => T<obj>?node * T<string> * T<obj>?value ^-> T<unit>

              "getProperty" => T<obj>?node * T<string> ^-> T<obj>

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

              "cssText" => T<obj>?node ^-> T<string>

              "fontSize" => T<obj>?node ^-> T<int>

              "fontFamily" => T<obj>?node ^-> T<string>

              "nodeSize" => T<obj>?node * !?T<int>?em * !?T<bool>?local ^-> !|T<float>

              "nodeBBox" => T<obj>?node ^-> PageBBox 

              "createWorker" => (T<obj>?event ^-> T<unit>) * OptionList?options ^-> T<Promise<obj>>
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
              "font" =? T<obj>

              // methods
              "setAdaptor" => DOMAdaptorClass ^-> T<unit>
              "initialize" => T<unit> ^-> T<unit>
              "reset" => !+T<obj> ^-> T<unit>
              "typeset" => MathItemClass * !?MathDocumentClass ^-> T<obj>
              "escaped" => MathItemClass * !?MathDocumentClass ^-> T<obj>
              "getMetrics" => MathDocumentClass ^-> T<unit>
              "getMetricsFor" => T<Dom.Element> * T<bool> ^-> Metrics
              "styleSheet" => MathDocumentClass ^-> T<obj>
              "pageElements" => MathDocumentClass ^-> T<obj>
              "addExtension" => T<obj> * !?T<string> ^-> !|T<string>
              "updateFontStyles" => T<unit> ^-> T<unit>
              "clearCache" => T<unit> ^-> T<unit>
              "clearFontCache" => T<unit> ^-> T<unit>
              "measureText" => T<string> * T<string> * !?T<obj> ^-> T<obj> 
            ]
        |> ignore

    PrioritizedListClass
        |+> Static [ 
            Constructor T<unit>; "DEFAULTPRIORITY" =? T<int> 
        ]
        |+> Instance
            [ 
                "add" => T<obj>?item * !?T<int>?priority ^-> T<obj>

                "remove" => T<obj>?item ^-> PrioritizedListClass 
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

                "setMetrics" => T<float> * T<float> * T<float> * T<float> ^-> T<unit>

                "state" => !?T<int> * !?T<bool> ^-> T<int>

                "reset" => !?T<bool> ^-> T<unit> 

                "clear" => T<unit> ^-> T<unit>
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
                "remove" => !+T<obj> ^-> LinkedListClass
                "clear" => T<unit> ^-> LinkedListClass
                "insert" => T<obj> * !?SortFn ^-> LinkedListClass
                "sort" => !?SortFn ^-> LinkedListClass
                "merge" => LinkedListClass * !?SortFn ^-> LinkedListClass
                "reversed" => T<unit> ^-> T<obj>

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

                "inputJax" =? InputJaxListClass

                "outputJax" =? OutputJaxClass

                "adaptor" =? DOMAdaptorClass

                "mmlFactory" =? MmlFactoryClass

                "menu" =? MenuClass

                // methods

                "addRenderAction" => T<string> *+ (!|T<obj>) ^-> T<unit>

                "removeRenderAction" => T<string> ^-> T<unit>

                "render" => T<unit> ^-> MathDocumentClass

                "renderPromise" => T<unit> ^-> T<Promise<obj>>

                "rerender" => !?T<int> ^-> MathDocumentClass

                "rerenderPromise" => !?T<int> ^-> T<Promise<obj>>

                "convert" => T<string> * !?OptionList?options ^-> T<obj> + MmlNode

                "convertPromise" => T<string> * !?OptionList?options ^-> T<Promise<obj>>

                "whenReady" => (T<unit> ^-> T<obj>) ^-> T<Promise<obj>>

                "actionPromises" => T<unit> ^-> T<Promise<obj>>

                "clearPromises" => T<unit> ^-> T<unit>

                "savePromise" => T<Promise<obj>> ^-> T<unit>

                "done" => T<unit> ^-> T<Promise<unit>>

                "addMenu" => T<unit> ^-> MathDocumentClass

                "checkLoading" => T<unit> ^-> T<bool>

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

    let PackageConfig =
        Pattern.Config "PackageConfig" {
            Required = []
            Optional = [
                "ready", T<string> ^-> T<obj>
                "failed", PackageError ^-> T<unit>
                "checkReady", T<unit> ^-> T<Promise<unit>>
                "extraLoads", !|T<string>
                "rendererExtensions", !|T<string>
            ]
        }

    PackageClass
        |+> Static [
            "packages" =? T<obj>
            "loadPromise" => T<string> ^-> T<Promise<unit>>
            "resolvePath" => T<string> * !?T<bool> ^-> T<string>
            "loadAll" => T<unit> ^-> T<unit>
        ]
        |+> Instance [
            "name" =? T<string>
            "isLoaded" =? T<bool>
            "promise" =? T<Promise<string>>
            "result" =? T<obj>
            "hasFailed" =? T<bool>
            "canLoad" =? T<bool>
            "load" => T<unit> ^-> T<unit>
        ]
        |> ignore

    // The `MathJax.loader` runtime object.
    MathJaxLoaderClass
        |+> Instance [
            "ready" => !+T<string> ^-> T<Promise<obj>>
            "load" => !+T<string> ^-> T<Promise<obj>>
            "preLoaded" => !+T<string> ^-> T<unit>
            "defaultReady" => T<unit> ^-> T<unit>
            "getRoot" => T<unit> ^-> T<string>
            "checkVersion" => T<string> * T<string> * !?T<string> ^-> T<bool>
            "saveVersion" => T<string> ^-> T<unit>
            "addPackageData" => T<string> * PackageConfig ^-> T<unit>
            "pathFilters" =? FunctionListClass
            "versions" =? T<obj>
            "nestedLoads" =? !|T<Promise<obj>>
        ]
        |> ignore

    // The `loader` section of the MathJax configuration.
    let MathJaxLoaderConfig = 
        Pattern.Config "MathJaxLoaderConfig" {
            Required = []
            Optional = [
                "paths", T<obj>
                "source", T<obj>
                "dependencies", T<obj>
                "provides", T<obj>
                "load", !|T<string>
                "ready", T<string> ^-> T<obj>
                "failed", PackageError ^-> T<unit>
                "require", T<string> ^-> T<obj>
                "pathFilters", FunctionListClass.Type
                "versionWarnings", T<bool>
            ]
        }

    // The `startup` section of the MathJax configuration.
    let MathJaxStartupConfig = 
        Pattern.Config "MathJaxStartupConfig" {
            Required = []
            Optional = [
                "input", !|T<string>
                "output", T<string>
                "handler", T<string>
                "adaptor", T<string>
                "document", T<string> + T<obj>
                "elements", T<obj>
                "typeset", T<bool>
                "ready", T<unit> ^-> T<unit>
                "pageReady", T<unit> ^-> T<Promise<unit>>
                "invalidOption", T<string>
                "optionError", T<string> * T<string> ^-> T<unit>
                "polyfillHasOwn", T<bool>
                "loadAllFontFiles", T<bool>
            ]
        }

    // The `MathJax` global configuration object.
    let MathJaxConfig =
        Pattern.Config "MathJaxConfig" { 
            Required = []
            Optional = [ 
                "tex", TeXInputConfig.Type
                "mml", MathMLInputConfig.Type
                "mathml", MathMLInputConfig.Type
                "asciimath", AsciiMathInputConfig.Type
                "chtml", ChtmlConfig.Type
                "svg", SvgConfig.Type
                "startup", MathJaxStartupConfig.Type
                "loader", MathJaxLoaderConfig.Type
                "options", DocumentOptions.Type
                "a11y", T<obj>
                "sre", T<obj>
            ] 
        }

    // The `MathJax.startup` runtime object.
    MathJaxStartupClass
        |+> Instance [
            "constructors" =? T<obj>
            "input" =? InputJaxListClass
            "output" =? OutputJaxClass
            "handler" =? HandlerClass
            "adaptor" =? DOMAdaptorClass
            "elements" =? T<obj>
            "document" =? MathDocumentClass
            "extensions" =? PrioritizedListClass
            "hasTypeset" =? T<bool>
            "mathjax" =? T<obj>
            "visitor" =? T<obj>

            "promise" =? T<Promise<unit>>
            "pagePromise" =? T<Promise<unit>>
            "promiseResolve" => !?T<obj> ^-> T<unit>
            "promiseReject" => T<obj> ^-> T<unit>

            "toMML" => MmlNode?node ^-> T<string>
            "registerConstructor" => T<string> * T<obj> ^-> T<unit>

            "useHandler" => T<string> * !?T<bool> ^-> T<unit>
            "useAdaptor" => T<string> * !?T<bool> ^-> T<unit>
            "useInput" => T<string> * !?T<bool> ^-> T<unit>
            "useOutput" => T<string> * !?T<bool> ^-> T<unit>

            "extendHandler" => (HandlerClass ^-> HandlerClass) * !?T<int> ^-> T<unit>

            "defaultReady" => T<unit> ^-> T<unit>
            "defaultPageReady" => T<unit> ^-> T<Promise<unit>>
            "defaultOptionError" => T<string> * T<string> ^-> T<unit>

            "getComponents" => T<unit> ^-> T<unit>

            "makeMethods" => T<unit> ^-> T<unit>
            "makeTypesetMethods" => T<unit> ^-> T<unit>
            "makeOutputMethods" => T<string> * T<string> * InputJaxClass ^-> T<unit>
            "makeMmlMethods" => T<string> * InputJaxClass ^-> T<unit>
            "makeResetMethod" => T<string> * InputJaxClass ^-> T<unit>

            "typesetPromise" => !?T<obj> ^-> T<Promise<unit>>

            "getInputJax" => T<unit> ^-> InputJaxListClass
            "getOutputJax" => T<unit> ^-> OutputJaxClass
            "getAdaptor" => T<unit> ^-> DOMAdaptorClass
            "getHandler" => T<unit> ^-> HandlerClass
            "getDocument" => !?T<obj> ^-> MathDocumentClass
        ]
        |> ignore

    // The `MathJax.startup.document.inputJax` array, which also exposes the
    // loaded input jax by name (matching `config.startup.input`).
    InputJaxListClass
        |+> Instance [
            "" =@ InputJaxClass |> Indexed T<int>
            "length" =? T<int>
            "tex" =? InputJaxClass
            "mml" =? InputJaxClass
            "mathml" =? InputJaxClass
            "asciimath" =? InputJaxClass
        ]
        |> ignore

    // The `MathJax.startup.document.menu` context-menu object.
    MenuClass
        |+> Static [
            "MENU_STORAGE" =? T<string>
        ]
        |+> Instance [
            "loadingPromise" =? T<Promise<unit>>
            "settings" =@ MenuSettings

            "addMenu" => T<unit> ^-> MenuClass
            "checkLoading" => T<unit> ^-> T<bool>
            "clear" => T<unit> ^-> T<unit>
            "resetDefaults" => T<unit> ^-> T<unit>
            "setScale" => T<float> ^-> T<unit>
            "setRenderer" => T<string> ^-> T<unit>
            "setOutputJax" => T<string> * !?T<bool> ^-> T<Promise<unit>>
            "setTabOrder" => T<bool> ^-> T<unit>
            "setAssistiveMml" => T<bool> ^-> T<unit>
            "applySettings" => T<unit> ^-> T<unit>
            "saveUserSettings" => T<unit> ^-> T<unit>
            "toMML" => MathItemClass ^-> T<string>
            "toSVG" => MathItemClass ^-> T<Promise<string>>
            "zoom" => T<obj> * T<obj> ^-> T<unit>
        ]
        |> ignore

    MathJaxClass
        |+> Static
            [
                // properties
                "version" =? T<string>

                "_" =? T<obj>

                "startup" =@ MathJaxStartupClass

                "options" =@ DocumentOptions

                "config" =@ MathJaxConfig

                "loader" =@ MathJaxLoaderClass

                // typeset
                "typeset" => !?T<obj>?elements ^-> T<unit>

                "typesetPromise" => !?T<obj>?elements ^-> T<Promise<unit>>

                "typesetClear" => !?T<obj>?elements ^-> T<unit>

                // asynchronous coordination (v4)
                "whenReady" => (T<unit> ^-> T<obj>) ^-> T<Promise<obj>>

                "done" => T<unit> ^-> T<Promise<unit>>

                // TeX -> CHTML / SVG / MathML
                "tex2chtml" => T<string> * !?ConversionOptions?options ^-> T<Dom.Node>

                "tex2chtmlPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<Dom.Node>>

                "tex2svg" => T<string> * !?ConversionOptions?options ^-> T<Dom.Node>

                "tex2svgPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<Dom.Node>>

                "tex2mml" => T<string> * !?ConversionOptions?options ^-> T<string>

                "tex2mmlPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<string>>

                "texReset" => T<unit> ^-> T<unit>        

                // CHTML / SVG
                "chtmlStylesheet" => T<unit> ^-> T<Dom.Node>

                "svgStylesheet" => T<unit> ^-> T<Dom.Node>

                "getMetricsFor" => T<Dom.Element> * T<bool> ^-> Metrics

                // AsciiMath -> CHTML / SVG / MathML
                "asciimath2chtml" => T<string> * !?ConversionOptions?options ^-> T<Dom.Node>

                "asciimath2chtmlPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<Dom.Node>>

                "asciimath2svg" => T<string> * !?ConversionOptions?options ^-> T<Dom.Node>

                "asciimath2svgPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<Dom.Node>>

                "asciimath2mml" => T<string> * !?ConversionOptions?options ^-> T<string>

                "asciimath2mmlPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<string>>

                "asciimathReset" => T<unit> ^-> T<unit>

                // MathML -> CHTML / SVG / MathML
                "mathml2chtml" => T<string> * !?ConversionOptions?options ^-> T<Dom.Node>

                "mathml2chtmlPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<Dom.Node>>

                "mathml2svg" => T<string> * !?ConversionOptions?options ^-> T<Dom.Node>

                "mathml2svgPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<Dom.Node>>

                "mathml2mml" => T<string> * !?ConversionOptions?options ^-> T<string>

                "mathml2mmlPromise" => T<string> * !?ConversionOptions?options ^-> T<Promise<string>>

                "mathmlReset" => T<unit> ^-> T<unit>
            ]
        |> ignore

    let Assembly =
        Assembly [ 
            Namespace "WebSharper.MathJax.Resources" [
                Resource "Js" "https://cdn.jsdelivr.net/npm/mathjax@4.1.3/tex-mml-chtml.js"  
                |> RequiresExternal [T<ConfigResource>]              
                |> AssemblyWide
            ]            

            Namespace "WebSharper.MathJax" [ 
                MathJaxClass
                MathJaxConfig
                MathJaxStartupClass
                MathJaxStartupConfig
                MathJaxLoaderClass
                MathJaxLoaderConfig
                PackageClass
                PackageConfig
                MenuClass
                MenuSettings
                MenuOptions
                DocumentOptions
                TeXInputConfig
                MathMLInputConfig
                AsciiMathInputConfig
                LinebreakConfig
                ConversionOptions
                HandlerListClass
                MathDocumentClass
                MathListClass
                LinkedListClass
                ListItemClass
                BitFieldClass
                PrioritizedListClass
                RenderListClass
                MathItemClass
                InputJaxListClass
                DOMAdaptorClass
                InputJaxClass
                FunctionListClass
                OutputJaxClass
                HandlerClass
                Metrics
                Location
                PackageError
                ChtmlConfig
                SvgConfig
            ] 
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()