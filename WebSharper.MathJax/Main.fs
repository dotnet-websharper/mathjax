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
open WebSharper.InterfaceGenerator

module Definition =
    let MathJaxClass = Class "MathJax"
    let MathJaxHubClass = Class "MathJax.Hub"
    let MathJaxAjaxClass = Class "MathJax.Ajax"
    let MathJaxMessageClass = Class "MathJax.Message"
    let MathJaxHTMLClass = Class "MathJax.HTML"
    let MathJaxCallbackClass = Class "MathJax.Callback"
    let MathJaxCallbackQueueClass = Class "MathJax.Callback.Queue"
    let MathJaxCallbackSignalClass = Class "MathJax.Callback.Signal"
    let MathJaxLocalizationClass = Class "MathJax.Localization"
    let MathJaxInputJaxClass = Class "MathJax.InputJax"
    let MathJaxOutputJaxClass = Class "MathJax.OuputJax"
    let MathJaxElementJaxClass = Class "MathJax.ElementJax"
    let MathJaxObjectClass = Class "MathJax.Object"

    let Browser =
        Pattern.Config "Browser" {
            Required = []
            Optional = 
                [ 
                    "version", T<string>
                    "isMac", T<bool>
                    "isPC", T<bool>
                    "isMobile", T<bool>
                    "isFirefox", T<bool>
                    "isSafari", T<bool>
                    "isChrome", T<bool>
                    "isOpera", T<bool>
                    "sMSIE", T<bool>
                    "isKonqueror", T<bool>
                    "versionAtLast", T<string> ^-> T<bool>
                    "Select", Type.ArrayOf (T<string> * T<bool>)
                ]
        }

    let MenuSetting =
        Pattern.Config "MenuSetting" {
            Required = []
            Optional = 
                [ 
                    "zoom", T<string>
                    "CTRL", T<bool>
                    "ALT", T<bool>
                    "CMD", T<bool>
                    "Shift", T<bool>
                    "zscale", T<string>
                    "content", T<string>
                    "texHints", T<bool>
                    "inTabOrder", T<bool>
                    "semantics", T<bool>
                ]
        }

    let ErrorSetting =
        Pattern.Config "ErrorSetting" {
            Required = []
            Optional = 
                [ 
                    "message", T<string>
                    "style", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                ]
        }

    let Tex2Jax =
        Pattern.Config "tex2jax" {
            Required = []
            Optional = 
                [
                    "inlineMath", Type.ArrayOf T<string * string>
                    "displayMath", Type.ArrayOf T<string * string>
                    "balanceBraces", T<bool>
                    "processEscapes", T<bool>
                    "processRefs", T<bool>
                    "processEnvironments", T<bool>
                    "preview", T<string>
                    "skipTags", Type.ArrayOf T<string>
                    "ignoreClass", T<string>
                    "processClass", T<string>
                ]
        }

    let Mml2Jax =
        Pattern.Config "mml2jax" {
            Required = []
            Optional =
                [
                    "preview", Type.ArrayOf T<string>
                ]
        }

    let AsciiMath2Jax =
        Pattern.Config "asciimath2jax" {
            Required = []
            Optional =
                [
                    "delimiters", Type.ArrayOf T<string * string>
                    "preview", Type.ArrayOf T<string>
                    "skipTags", Type.ArrayOf T<string>
                    "ingnoreClass", T<string>
                    "processClass", T<string>

                ]
        }

    let JsMath2Jax =
        Pattern.Config "jsMath2jax" {
            Required = []
            Optional =
                [
                    "preview", Type.ArrayOf T<string>
                ]
        }

    let Equation =
        Pattern.Config "equation" {
            Required = []
            Optional = 
                [
                    "autoNumber", T<string>
                    "formatNumber", T<string> ^-> T<string>
                    "formatTag", T<string> ^-> T<string>
                    "formatID", T<unit> ^-> T<string>
                    "formatURL", T<string> ^-> T<string>
                    "useLabelIds", T<bool>
                ]
        }

    let TeX =
        Pattern.Config "TeX" {
            Required = []
            Optional = 
                [
                    "TagSide", T<string>
                    "TagIndent", T<string>
                    "MultLineWidth", T<string>
                    "equationNumbers", Equation.Type
                    "Macros", T<string> * T<string> + T<string> * (T<string> * T<int>)
                    "MAXMACROS", T<int>
                    "MAXBUFFER", T<int>
                ]
        }

    let MathML =
        Pattern.Config "MathML" {
            Required = []
            Optional =
                [
                    "userMathMLspacing", T<bool>
                ]
        }

    let AsciiMath =
        Pattern.Config "AsciiMath" {
            Required = []
            Optional =
                [
                    "fixphi", T<bool>
                    "useMathMLspacing", T<bool>
                    "displaystyle", T<bool>
                    "decimalsign", T<string>
                ]
        }

    let LineBreak = 
        Pattern.Config "linebreak" {
            Required = []
            Optional =
                [
                    "automatic", T<bool>
                    "width", T<bool>
                ]
        }

    let CommonHTML =
        Pattern.Config "CommonHTML" {
            Required = []
            Optional = 
                [
                    "scale", T<int>
                    "minScaleAdjust", T<int>
                    "mtextFontInherit", T<bool>
                    "linebreaks", LineBreak.Type
                ]
        }

    let ToolTip =
        Pattern.Config "tooltip" {
            Required = []
            Optional =
                [
                    "delayPost", T<int>
                    "delayClear", T<int>
                    "offsetX", T<int>
                    "offsetY", T<int>
                ]
        }

    let HTMLCSS =
        Pattern.Config "HTML-CSS" {
            Required = []
            Optional = 
                [
                    "scale", T<int>
                    "minScaleAdjust", T<int>
                    "availableFonts", Type.ArrayOf T<string>
                    "preferredFont", T<string>
                    "webFont", T<string>
                    "imageDont", T<string>
                    "undefinedFamily", T<string>
                    "mtextFontInherit", T<bool>
                    "EqnChunk", T<int>
                    "EqnChunkFactor", T<float>
                    "EqnChunkDelay", T<int>
                    "matchFontHeight", T<bool>
                    "linebreaks", LineBreak.Type
                    "styles", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                    "showMathMenu", T<bool>
                    "tooltip", ToolTip.Type
                    "noReflows", T<bool>
                ]
        }

    let NativeMML =
        Pattern.Config "NativeMML" {
            Required = []
            Optional =
                [
                    "scale", T<int>
                    "minScaleAdjust", T<int>
                    "matchFontHeight", T<bool>
                    "showMathMath", T<bool>
                    "showMathMenuMSIE", T<bool>
                    "styles", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                ]
        }

    let SVG =
        Pattern.Config "SVG" {
            Required = []
            Optional =
                [
                    "scale", T<int>
                    "minScaleAdjust", T<int>
                    "font", T<string>
                    "blacker", T<int>
                    "undefinedFamily", T<string>
                    "mtextFontInherit", T<bool>
                    "addMMLclasses", T<bool>
                    "useFontCache", T<bool>
                    "useGlobalCache", T<bool>
                    "EqnChunk", T<int>
                    "EqnChunkFactor", T<float>
                    "EqnChunkDelay", T<int>
                    "matchFontHeight", T<bool>
                    "linebreaks", LineBreak.Type
                    "styles", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                    "tooltip", ToolTip.Type                    
                ]
        }

    let PreviewHTML =
        Pattern.Config "PreviewHTML" {
            Required = []
            Optional = 
                [
                    "scale", T<int>
                    "minScaleAdjust", T<int>
                    "mtextFontInherit", T<bool>
                    "linebreaks", LineBreak.Type
                ]
        }

    let PlainSource =
        Pattern.Config "PlainSource" {
            Required = []
            Optional = 
                [
                    "styles", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                ]
        }

    let FastPreview =
        Pattern.Config "fast-preview" {
            Required = []
            Optional =
                [
                    "EqnChunk", T<int>
                    "EqnChunkFactor", T<float>
                    "EqnChunkDelay", T<int>
                    "color", T<string>
                    "updateTime", T<int>
                    "updateDelay", T<int>
                    "messageStyle", T<int>
                    "disabled", T<bool>
                ]
        }

    let ContentMathML =
        Pattern.Config "content-mathml" {
            Required = []
            Optional =
                [
                    "collapePlusMinus", T<bool>
                    "cistyles", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                    "symbols", Type.ArrayOf T<string * string>
                ]
        }

    let Config =
        Pattern.Config "Config" {
            Required = []
            Optional = 
                [ 
                    "jax", Type.ArrayOf T<string>
                    "extensions", Type.ArrayOf T<string>
                    "config", Type.ArrayOf T<string>
                    "styleSheets", Type.ArrayOf T<string>
                    "styles", Type.ArrayOf (T<string> * Type.ArrayOf T<string * string>)
                    "preJax", T<string>
                    "postJax", T<string>
                    "preRemoveClass", T<string>
                    "showProcessingMessages", T<bool>
                    "messageStyle", T<string>
                    "displayAlign", T<string>
                    "displayIndent", T<string>
                    "delayStartupUntil", T<string>
                    "skipStartupTypeset", T<bool>
                    "elements", Type.ArrayOf T<string>
                    "positionToHash", T<bool>
                    "showMathMenu", T<bool>
                    "showMathMenuMSIE", T<bool>
                    "menuSettings", MenuSetting.Type
                    "errorSettings", ErrorSetting.Type
                    "ignoreMMLattributes", Type.ArrayOf (T<string> * T<bool>)
                    "v1.0-compatible", T<bool>
                    "tex2jax", Tex2Jax.Type
                    "mml2jax", Mml2Jax.Type
                    "asciimath2jax", AsciiMath2Jax.Type
                    "jsMath2jax", JsMath2Jax.Type
                    "TeX", TeX.Type
                    "MathML", MathML.Type
                    "AsciiMath", AsciiMath.Type
                    "CommonHTML", CommonHTML.Type
                    "HTML-CSS", HTMLCSS.Type
                    "NativeMML", NativeMML.Type
                    "SVG", SVG.Type
                    "PreviewHTML", PreviewHTML.Type
                    "PlainSource", PlainSource.Type
                    "fast-preview", FastPreview.Type
                    "content-mathml", ContentMathML.Type
                ]
        }

    let Callback
        = T<JavaScript.Function>
        + Type.ArrayOf T<JavaScript.Function>
        + T<JavaScript.Function> * Type.ArrayOf T<obj>
        + T<obj> * T<JavaScript.Function>
        + T<obj> * T<JavaScript.Function> * Type.ArrayOf T<obj>
        + T<string> * T<obj>
        + T<string> * T<obj> * Type.ArrayOf T<obj>
        + T<string>

    let Hooks =
        Pattern.Config "Hooks" {
            Required = []
            Optional = 
                [ 
                    "Add", Callback * T<int> ^-> Callback
                    "Remove", T<string> ^-> T<unit>
                    "Execute", T<unit> ^-> T<unit>
                ]
        }

    let Domain =
        Pattern.Config "Domain" {
            Required = []
            Optional = 
                [ 
                    "version", T<string>
                    "file", T<string>
                    "isLoaded", T<bool>
                    "strings", Type.ArrayOf T<string>
                ]
        }

    let TranslationData =
        Pattern.Config "TranslationData" {
            Required = []
            Optional = 
                [ 
                    "menuTitle", T<string>
                    "version", T<string>
                    "directory", T<string>
                    "file", T<string>
                    "isLoaded", T<bool>
                    "fontFamily", T<string>
                    "fontDirection", T<string>
                    "plural", T<int> ^-> T<int>
                    "number", T<int> ^-> T<string>
                    "domains", Domain.Type //fix
                ]
        }

    MathJaxHubClass
        |+> Static [
            // Methods
            "Config" => Config.Type ^-> T<unit>
            |> WithComment "Sets the configuration options (stored in MathJax.Hub.config)."

            "Configured" => T<unit> ^-> T<unit>
            |> WithComment "MathJax�s startup sequence is delayed until this routine is called."

            "Register.PreProcessor" => Callback ^-> T<unit>
            |> WithComment "Used by preprocessors to register themselves with MathJax so that they will be called during the MathJax.Hub.PreProcess() action."
            
            "Register.MessageHook" => T<string> * Callback ^-> T<unit>
            |> WithComment "Registers a listener for a particular message being sent to the hub processing signal."

            "Register.StartupHook" => T<string> * Callback ^-> T<unit>
            |> WithComment "Registers a listener for a particular message being sent to the startup signal."

            "Register.LoadHook" => T<string> * Callback ^-> Callback
            |> WithComment "Registers a callback to be called when a particular file is completely loaded and processed."

            "Queue" => Type.ArrayOf Callback ^-> Callback
            |> WithComment "Pushes the given callbacks onto the main MathJax command queue."

            "TypeSet" => !? T<JavaScript.Dom.Node> * !? Callback ^-> Callback
            |> WithComment "Calls the preprocessors on the given element (or elements if it is an array of elements), and then typesets any math elements within the element."
            
            "PreProcess" => !? T<JavaScript.Dom.Node> * !? Callback ^-> Callback
            |> WithComment "Calls the loaded preprocessors on the entire document, or on the given DOM element (or elements, if it is an array of elements)."

            "Process" => !? T<JavaScript.Dom.Node> * !? Callback ^-> Callback
            |> WithComment "Scans either the entire document or a given DOM element (or array of elements) for MathJax <script> tags and processes the math those tags contain."
             
            "Update" => !? T<JavaScript.Dom.Node> * !? Callback ^-> Callback
            |> WithComment "Scans either the entire document or a given DOM element (or elements if it is an array of elements) for mathematics that has changed since the last time it was processed, or is new, and typesets the mathematics they contain."

            "Reprocess" => !? T<JavaScript.Dom.Node> * !? Callback ^-> Callback
            |> WithComment "Removes any typeset mathematics from the document or DOM element (or elements if it is an array of elements), and then processes the mathematics again, re-typesetting everything."

            "Rerender" => !? T<JavaScript.Dom.Node> * !? Callback ^-> Callback
            |> WithComment "Removes any typeset mathematics from the document or DOM element (or elements if it is an array of elements), and then renders the mathematics again, re-typesetting everything from the current internal version (without calling the input jax again)."

            "getAllJax" => !? T<JavaScript.Dom.Node> ^-> Type.ArrayOf MathJaxElementJaxClass
            |> WithComment "Returns a list of all the element jax in the document or a specific DOM element."

            "getJaxByType" => T<string> * !? T<JavaScript.Dom.Node> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Returns a list of all the element jax of a given MIME-type in the document or a specific DOM element."

            "getJaxByInputType" => T<string> * !? T<JavaScript.Dom.Node> ^-> Type.ArrayOf MathJaxElementJaxClass
            |> WithComment "Returns a list of all the element jax associated with input <script> tags with the given MIME-type within the given DOM element or the whole document."

            "getJaxFor" => T<JavaScript.Dom.Node> ^-> MathJaxElementJaxClass
            |> WithComment "Returns the element jax associated with a given DOM element. If the element does not have an associated element jax, null is returned."

            "isJax" => T<JavaScript.Dom.Node> ^-> T<int>
            |> WithComment "Returns 0 if the element is not a <script> that can be processed by MathJax or the result of an output jax, returns -1 if the element is an unprocessed <script> tag that could be handled by MathJax, and returns 1 if the element is a processed <script> tag or an element that is the result of an output jax."

            "setRenderer" => T<string> * !? T<string> ^-> T<unit>
            |> WithComment "Sets the output jax for the given element jax type (or jax/mml if none is specified) to be the one given by renderer, which must be the name of a renderer, such as NativeMML or HTML-CSS."

            "Insert" => T<JavaScript.Dom.Node> * T<JavaScript.Dom.Node> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Inserts data from the src JavaScript.Dom.Nodeect into the dst JavaScript.Dom.Nodeect. The key:value pairs in src are (recursively) copied into dst, so that if value is itself an JavaScript.Dom.Nodeect, its content is copied into the corresponding JavaScript.Dom.Nodeect in dst."

            "formatError" => T<string> ^-> T<JavaScript.Dom.Node>
            |> WithComment "This is called when an internal error occurs during the processing of a math element (i.e., an error in the MathJax code itself)."
            

            // Properties
            "config" =? Config.Type
            |> WithGetterInline "$this.config"

            "processSectionDelay" =? T<int>
            |> WithGetterInline "$this.processSectionDelay"

            "processUpdateTime" =? T<int>
            |> WithGetterInline "$this.processUpdateTime"

            "processUpdateDelay" =? T<int>
            |> WithGetterInline "$this.processUpdateDelay"

            "signal" =? T<string>
            |> WithGetterInline "$this.signal"

            "queue" =? T<string>
            |> WithGetterInline "$this.queue"

            "Browser" =? Browser.Type
            |> WithGetterInline "$this.Browser"

            "inputJax" =? T<string>
            |> WithGetterInline "$this.inputJax"

            "outputJax" =? T<string>
            |> WithGetterInline "$this.outputJax"
        ]
        |> ignore

    MathJaxAjaxClass
        |+> Static [
            "Require" => T<string> * !? Callback ^-> Callback
            |> WithComment "Loads the given file if it hasn�t been already. The file must be a JavaScript file or a CSS stylesheet."

            "Load" => T<string> * !? Callback ^-> Callback
            |> WithComment "Used internally to load a given file without checking if it already has been loaded, or where it is to be found."

            "loadComplete" => T<string> ^-> T<unit>
            |> WithComment "Called from within the loaded files to inform MathJax that the file has been completely loaded and initialized."

            "loadTimeout" => T<string> ^-> T<unit>
            |> WithComment "Called when the timeout period is over and the file hasn�t loaded. This indicates an error condition, and the MathJax.Ajax.loadError() method will be executed, then the file�s callback will be run with MathJax.Ajax.STATUS.ERROR as its parameter."

            "loadError" => T<string> ^-> T<unit>
            |> WithComment "The default error handler called when a file fails to load. It puts a warning message into the MathJax message box on screen."

            "LoadHook" => T<string> * Callback ^-> T<unit>
            |> WithComment "Registers a callback to be executed when the given file is loaded. The file load operation needs to be started when this method is called, so it can be used to register a hook for a file that may be loaded in the future."

            "Preloading" => Type.ArrayOf T<string> ^-> T<unit>
            |> WithComment "Used with combined configuration files to indicate what files are in the configuration file. "

            "Styles" => T<string> * !? Callback ^-> Callback
            |> WithComment "Creates a stylesheet from the given style data. styles can either be a string containing a stylesheet definition, or an object containing a CSS Style Object."

            "Styles" => T<obj> * !? Callback ^-> Callback
            |> WithComment "Creates a stylesheet from the given style data. styles can either be a string containing a stylesheet definition, or an object containing a CSS Style Object."

            "fileURL" => T<string> ^-> T<string>
            |> WithComment "Returns a complete URL to a file (replacing [MathJax] with the actual root URL location)."
            
            //properties
            "timout" =? T<int>

            "STATUS.OK" =? T<bool>

            "STATUS.ERROR" =? T<bool>

            "loaded" =? T<string>

            "loading" =? T<obj>

            "loadHooks" =? T<obj>
        ]
        |> ignore

    MathJaxMessageClass
        |+> Static [
            "Set" => T<string> * !? T<int> * !? T<int> ^-> T<int>
            |> WithComment "This sets the message being displayed to the given message string. If n is not null, it represents a message id number and the text is set for that message id, otherwise a new id number is created for this message."

            "Clear" => T<int> * !? T<int> ^-> T<unit>
            |> WithComment "This causes the message with id n to be removed after the given delay, in milliseconds."

            "Remove" => T<unit> ^-> T<unit>
            |> WithComment "This removes the message frame from the window (it will reappear when future messages are set, however)."

            "File" => T<string> ^-> T<int>
            |> WithComment "This sets the message area to a �Loading file� message, where file is the name of the file (with [MathJax] representing the root directory)."

            "filterText" => T<string> * T<int> ^-> T<string>
            |> WithComment "This method is called on each message before it is displayed. It can be used to modify (e.g., shorten) the various messages before they are displayed."
        
            "Log" => T<unit> ^-> T<unit>
            |> WithComment "Returns a string of all the messages issued so far, separated by newlines. This is used in debugging MathJax operations."
        ]
        |> ignore

    MathJaxHTMLClass
        |+> Static [
            "Element" => T<string> * !? T<obj> * !? T<obj> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Creates a DOM element of the given type. If attributes is non-null, it is an object that contains key:value pairs of attributes to set for the newly created element."

            "addElement" => T<JavaScript.Dom.Node>?parent * T<string>?ty * !? T<obj>?attr * !? T<obj>?cont ^-> T<JavaScript.Dom.Node> 
            |> WithInline "$parent.appendChild(MathJax.HTML.Element($ty,$attr,$cont))"
            |> WithComment "Creates a DOM element and appends it to the parent node provided. It is equivalent to."

            "TextNode" => T<string> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Creates a DOM text node with the given text as its content."

            "addText" => T<JavaScript.Dom.Node> * T<string> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Creates a DOM text node with the given text and appends it to the parent node."

            "setScript" => T<string> * T<string> ^-> T<string>
            |> WithComment "Sets the contents of the script element to be the given text, properly taking into account the browser limitations and bugs."

            "getScript" => T<string> ^-> T<string>
            |> WithComment "Gets the contents of the script element, properly taking into account the browser limitations and bugs."

            "Cookie.Set" => T<string> * T<obj> ^-> T<unit>
            |> WithComment "Creates a MathJax cookie using the MathJax.HTML.Cookie.prefix and the name as the cookie name, and the key:value pairs in the data object as the data for the cookie."

            "Cookie.Get" => T<string> * !? T<obj> ^-> T<obj>

            //properties
            "Cookie.prefix" =? T<string>

            "Cookie.expires" =? T<int>
        ]
        |> ignore

    MathJaxCallbackQueueClass
        |+> Instance [
            "Push" => Type.ArrayOf Callback ^-> Callback
            |> WithComment "Adds commands to the queue and runs them (if the queue is not pending or running another command)."

            "Process" => T<unit> ^-> T<unit>
            |> WithComment "Process the commands in the queue, provided the queue is not waiting for another command to complete."

            "Suspend" => T<unit> ^-> T<unit>
            |> WithComment "Increments the running property, indicating that any commands that are added to the queue should not be executed immediately, but should be queued for later execution (when its Resume() is called)."

            "Resume" => T<unit> ^-> T<unit>
            |> WithComment "Decrements the running property, if it is positive. When it is zero, commands can be processed, but that is not done automatically � you would need to call Process() to make that happen."

            "wait" => Callback ^-> Callback
            |> WithComment "Used internally when an entry in the queue is a Callback object rather than a callback specification."

            "call" => T<unit> ^-> T<unit>
            |> WithComment "An internal function used to restart processing of the queue after it has been waiting for a command to complete."
            
            //properties
            "pending" =? T<int>

            "running" =? T<int>

            "queue" =? Type.ArrayOf Callback
        ]
        |> ignore

    MathJaxCallbackSignalClass
        |+> Instance [
            "Post" => T<string> * !? Callback ^-> Callback
            |> WithComment "Posts a message to all the listeners for the signal."

            "Clear" => !? Callback ^-> Callback
            |> WithComment "This causes the history of past messages to be cleared so new listeners will not receive them."

            "Interest" => Callback * !? T<bool> ^-> Callback
            |> WithComment "This method registers a new listener on the signal. It creates a Callback object from the callback specification, attaches it to the signal, and returns that Callback object."

            "NoInterest" => Callback ^-> T<unit>
            |> WithComment "This removes a listener from the signal so that no new messages will be sent to it."

            "MessageHook" => T<string> * Callback ^-> Callback
            |> WithComment "This creates a callback that is called whenever the signal posts the given message."

            "ExecuteHook" => T<string> ^-> T<unit>
            |> WithComment "Used internally to call the listeners when a particular message is posted to the signal."


            //properties
            "name" =? T<string>

            "posted" =? Type.ArrayOf T<string>

            "listeners" =? Type.ArrayOf Callback
        ]
        |> ignore

    MathJaxCallbackClass
        |=> Nested [
            MathJaxCallbackQueueClass
            MathJaxCallbackSignalClass
        ]
        |+> Static [
            "Delay" => T<int> * !? Callback ^-> Callback
            |> WithComment "Waits for the specified time (given in milliseconds) and then performs the callback."

            "Queue" => Type.ArrayOf Callback ^-> MathJaxCallbackQueueClass
            |> WithComment "Creates a MathJax.CallBack.Queue object and pushes the given callbacks into the queue."

            "Signal" => T<string> ^-> MathJaxCallbackQueueClass
            |> WithComment "Looks for a named signal, creates it if it doesn�t already exist, and returns the signal object."

            "ExecuteHooks" => Type.ArrayOf T<obj> * !? (Type.ArrayOf T<obj>) * !? (Type.ArrayOf T<bool>) ^-> Type.ArrayOf Callback
            |> WithComment "Calls each callback in the hooks array (or the single hook if it is not an array), passing it the arguments stored in the data array."

            "Hooks" => T<bool> ^-> Hooks.Type
            |> WithComment "Creates a prioritized list of hooks that are called in order based on their priority (low priority numbers are handled first)."
        ]
        |+> Instance [
            "reset" => T<unit> ^-> T<unit>
            |> WithComment "Clears the callback�s called property."

            "hook" =? T<obj>

            "data" =? Type.ArrayOf T<string>

            "object" =? T<obj>

            "called" =? T<bool>

            "autoReset" =? T<bool>

            "isCallback" =? T<bool>
        ]
        |> ignore

    MathJaxLocalizationClass
        |+> Static [
            "_" => T<int> * T<string> * !? T<string> ^-> T<string>
            |> WithComment "The function (described in detail above) that returns the translated string for a given id, substituting the given arguments as needed."

            "_" => Type.ArrayOf T<string * int> * !? T<string> ^-> T<string>
            |> WithComment "The function (described in detail above) that returns the translated string for a given id, substituting the given arguments as needed."

            "setLocale" => T<string> ^-> T<unit>
            |> WithComment "Sets the selected locale to the given one."

            "addTranslation" => T<string> * T<string> * TranslationData.Type ^-> T<unit>
            |> WithComment "Defines (or adds to) the translation data for the given locale and domain."

            "setCSS" => T<JavaScript.Dom.Node> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Sets the CSS for the given div to reflect the needs of the locale."

            "fontFamily" => T<unit> ^-> T<string>
            |> WithComment "Get the font-family needed to display text in the selected language."

            "fontDirection" => T<unit> ^-> T<string>
            |> WithComment "Get the direction needed to display text in the selected language."

            "plural" => T<int> ^-> T<int>
            |> WithComment "The method that returns the index into the list of plural texts for the value n."

            "number" => T<int> ^-> T<string>
            |> WithComment "The method that returns the localized version of the number n."

            "loadDomain" => T<string> * !? Callback
            |> WithComment "This causes MathJax to load the data file for the given domain in the current language, and calls the callback when that is complete."

            "Try" => Callback ^-> T<unit>
            |> WithComment "This method runs the function fn with error trapping and if an asynchronous file load is performed (for loading localizaton data), reruns the function again after the file loads."

            //properties
            "locale" =? T<string>

            "directory" =? T<string>

            "strings" =? Type.ArrayOf T<string>
        ]
        |> ignore

    MathJaxElementJaxClass
        |+> Static [
            //properties
            "id" =? T<string>
            "version" =? T<string>
            "directory" =? T<string>
        ]
        |+> Instance [
            "Text" => T<string> * Callback
            |> WithComment "Sets the input text for this element to the given text and reprocesses the mathematics."

            "Renderer" => !? Callback ^-> Callback
            |> WithComment "Removes the output and produces it again (for example, if CSS has changed that would alter the spacing of the mathematics)."

            "Reprocess" => !? Callback ^-> Callback
            |> WithComment "Removes the output and then retranslates the input into the internal form and reredners the output again."

            "Remove" => T<unit> ^-> T<unit>
            |> WithComment "Removes the output for this element from the web page (but does not remove the original <script>)."

            "SourceElement" => T<unit> ^-> T<JavaScript.Dom.Node>
            |> WithComment "Returns a reference to the original <script> DOM element associated to this element jax."

            "needsUpdate" => T<unit> ^-> T<bool>
            |> WithComment "Indicates whether the mathematics has changed so that its output needs to be updated."

            //properties
            "inputJax" =? T<string>

            "outputJax" =? T<string>

            "inputID" =? T<string>

            "originalText" =? T<string>

            "mimeType" =? T<string>
        ]
        |> ignore

    MathJaxInputJaxClass
        |+> Instance [
            "Process" => T<JavaScript.Dom.Node> * T<obj> ^-> MathJaxElementJaxClass
            |> WithComment "This is the method that the MathJax.Hub calls when it needs the input jax to process the given math <script>."

            "Translate" => T<JavaScript.Dom.Node> * T<obj> ^-> MathJaxElementJaxClass
            |> WithComment "This is the main routine called by MathJax when a <script> of the appropriate type is found."

            "Register" => T<string> ^-> T<unit>
            |> WithComment "This registers the MIME-type associated with this input jax so that MathJax knows to call this input jax when it sees a <script> of that type."

            "needsUpdate" => MathJaxElementJaxClass ^-> T<bool>
            |> WithComment "This implements the element jax�s needsUpdate() method, and returns true if the jax needs to be rerendered (i.e., the text has changed), and false otherwise."
                
            //properties
            "id" =? T<string>

            "version" =? T<string>

            "directory" =? T<string>

            "elementJax" =? T<string>
        ]
        |> ignore

    MathJaxOutputJaxClass
        |+> Instance [
            "preProcess" => T<obj> ^-> T<unit>
            |> WithComment "This is called by MathJax.Hub to ask the output processor to prepare to process math scripts."

            "preTranslate" => T<obj> ^-> T<unit>
            |> WithComment "This routine replaces preProcess() above when the jax�s jax.js file is loaded."

            "Translate" => T<JavaScript.Dom.Node> * T<obj> ^-> MathJaxElementJaxClass
            |> WithComment "This is the main routine called by MathJax when an element jax is to be converted to output."

            "postTranslate" => T<obj> ^-> T<unit>
            |> WithComment "This routines is called by MathJax.Hub when the translation of math elements is complete, and can be used by the output processor to finalize any actions that it needs to complete."

            "Register" => T<string> ^-> T<unit>
            |> WithComment "This registers the MIME-type for the element jax associated with this output jax so that MathJax knows to call this jax when it wants to display an element jax of that type."

            "Remove" => MathJaxElementJaxClass ^-> T<unit>
            |> WithComment "Removes the output associated with the given element jax."

            "getJaxFromMath" => T<JavaScript.Dom.Node> ^-> MathJaxElementJaxClass
            |> WithComment "This is called by the event-handling code (MathEvents) to get the element jax associated with the DOM element that caused an event to occur."

            "Zoom" => MathJaxElementJaxClass * T<JavaScript.Dom.Node> * T<JavaScript.Dom.Node> * T<int> * T<int>
            |> WithComment "This routine is called by the zoom-handling code (MathZoom) when an expression has received its zoom trigger event (e.g., a double-click)."

            //properties
            "id" =? T<string>

            "version" =? T<string>

            "directory" =? T<string>

            "fontDir" =? T<string>

            "imageDir" =? T<string>
        ]
        |> ignore

    MathJaxObjectClass
        |+> Static [
            "Subclass" => T<obj> * !? T<obj> ^-> T<obj>
            |> WithComment "Creates a subclass of the given class using the contents of the def object to define new methods and properties of the object class, and the contents of the optional static object to define new static methods and properties."

            "Augment" => T<obj> * !? T<obj> ^-> T<obj>
            |> WithComment "Adds new properties and methods to the class prototype. All instances of the object already in existence will receive the new properties and methods automatically."

            //properties
            "SUPER" =? T<obj>
        ]
        |+> Instance [
            "Init" => T<obj> ^-> T<obj>
            |> WithComment "An optional function that is called when an instance of the class is created."

            "isa" => T<obj> ^-> T<bool>
            |> WithComment "Returns true if the object is an instance of the given class, or of a subclass of the given class, and false otherwise."

            "can" => T<string> ^-> T<bool>
            |> WithComment "Checks if the object has the given method and returns true if so, otherwise returns false."

            "has" => T<string> ^-> T<bool>
            |> WithComment "Checks if the object has the given property and returns true if so, otherwise returns false."


            //properties
            "contructor" =? T<obj>
        ]
        |> ignore

    let MJC =
        MathJaxClass
            |=> Nested [
                MathJaxLocalizationClass
            ]
            |+> Static [

                "Hub" =? MathJaxHubClass

                "Ajax" =? MathJaxAjaxClass

                "Message" =? MathJaxMessageClass

                "HTML" =? MathJaxHTMLClass

                "CallBack" =? MathJaxCallbackClass

                "Object" =? MathJaxObjectClass

                "InputJax" =? MathJaxInputJaxClass

                "OutputJax" =? MathJaxOutputJaxClass

                "ElementJax" =? MathJaxElementJaxClass

                "version" =? T<string>

                "fileversion" =? T<string>

                "isReady" =? T<bool>
            ]
        //|> ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.MathJax.Resources" [
                Resource "Js" "https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.1/MathJax.js"
                |> AssemblyWide
            ]

            Namespace "WebSharper.MathJax" [
                MJC
                MathJaxInputJaxClass
                MathJaxOutputJaxClass
                MathJaxElementJaxClass
                MathJaxObjectClass
                MathJaxHubClass
                MathJaxAjaxClass
                MathJaxMessageClass
                MathJaxHTMLClass
                MathJaxCallbackClass
                Browser
                MenuSetting
                ErrorSetting
                Config
                Hooks
                Domain
                TranslationData
                Tex2Jax
                Mml2Jax
                AsciiMath2Jax
                JsMath2Jax
                TeX
                MathML
                AsciiMath
                CommonHTML
                HTMLCSS
                NativeMML
                SVG
                PreviewHTML
                PlainSource
                FastPreview
                ContentMathML
                LineBreak
                ToolTip
                Equation
            ]
        ]


[<Sealed>]
type Extension() =
    interface IExtension with
        member x.Assembly = MathJaxV3.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()