(function()
{
 "use strict";
 var Global,WebSharper,MathJax,Sample,Client,UI,Next,Doc;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 MathJax=WebSharper.MathJax=WebSharper.MathJax||{};
 Sample=MathJax.Sample=MathJax.Sample||{};
 Client=Sample.Client=Sample.Client||{};
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 Client.Main=function()
 {
  var mathJaxConfig,f,a,_this;
  mathJaxConfig={};
  mathJaxConfig.extensions=["tex2jax.js","mml2jax.js","asciimath2jax.js"];
  mathJaxConfig.jax=["input/TeX","output/CommonHTML","input/MathML","input/AsciiMath"];
  mathJaxConfig.tex2jax={};
  mathJaxConfig.tex2jax.inlineMath=[["$","$"],["\\(","\\)"]];
  mathJaxConfig.mml2jax={};
  mathJaxConfig.mml2jax.preview=["mathml"];
  mathJaxConfig.asciimath2jax={};
  mathJaxConfig.asciimath2jax.delimiters=[["`","`"]];
  mathJaxConfig.asciimath2jax.preview=["AsciiMath"];
  mathJaxConfig.menuSettings={};
  mathJaxConfig.menuSettings.zoom="Click";
  mathJaxConfig.displayAlign="left";
  Global.MathJax.Hub.Config(mathJaxConfig);
  f=(a=[Doc.TextNode("When $a \\ne 0$, there are two solutions to \\(ax^2 + bx + c = 0\\) and they are $$x = {-b \\pm \\sqrt{b^2-4ac} \\over 2a}.$$")],Doc.Element("div",[],a));
  _this=Global.jQuery("#btn");
  _this.on("click",function()
  {
   var _this$1,v,a$1;
   _this$1=Global.jQuery("#tex");
   _this$1.append.apply(_this$1,[f.elt].concat([]));
   return v=(a$1=[["Typeset",Global.MathJax.Hub,[f.elt]]],Global.MathJax.Hub.Queue(a$1));
  });
 };
}());
