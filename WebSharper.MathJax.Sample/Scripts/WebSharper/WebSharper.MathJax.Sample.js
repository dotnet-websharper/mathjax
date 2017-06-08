(function()
{
 "use strict";
 var Global,WebSharper,MathJax,Sample,Client;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 MathJax=WebSharper.MathJax=WebSharper.MathJax||{};
 Sample=MathJax.Sample=MathJax.Sample||{};
 Client=Sample.Client=Sample.Client||{};
 Client.Main=function()
 {
  var mathJaxConfig;
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
 };
}());
