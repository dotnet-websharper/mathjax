// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2016 IntelliFactory
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

IntelliFactory = {
    Runtime: {
        Ctor: function (ctor, typeFunction) {
            ctor.prototype = typeFunction.prototype;
            return ctor;
        },

        Cctor: function (cctor) {
            var init = true;
            return function () {
                if (init) {
                    init = false;
                    cctor();
                }
            };
        },

        Class: function (members, base, statics) {
            var proto = base ? new base() : {};
            var typeFunction = function (copyFrom) {
                if (copyFrom) {
                    for (var f in copyFrom) { this[f] = copyFrom[f] }
                }
            }
            for (var m in members) { proto[m] = members[m] }
            typeFunction.prototype = proto;
            if (statics) {
                for (var f in statics) { typeFunction[f] = statics[f] }
            }
            return typeFunction;
        },

        Clone: function (obj) {
            var res = {};
            for (var p in obj) { res[p] = obj[p] }
            return res;
        },

        NewObject:
            function (kv) {
                var o = {};
                for (var i = 0; i < kv.length; i++) {
                    o[kv[i][0]] = kv[i][1];
                }
                return o;
            },

        DeleteEmptyFields:
            function (obj, fields) {
                for (var i = 0; i < fields.length; i++) {
                    var f = fields[i];
                    if (obj[f] === void (0)) { delete obj[f]; }
                }
                return obj;
            },

        GetOptional:
            function (value) {
                return (value === void (0)) ? null : { $: 1, $0: value };
            },

        SetOptional:
            function (obj, field, value) {
                if (value) {
                    obj[field] = value.$0;
                } else {
                    delete obj[field];
                }
            },

        SetOrDelete:
            function (obj, field, value) {
                if (value === void (0)) {
                    delete obj[field];
                } else {
                    obj[field] = value;
                }
            },

        Bind: function (f, obj) {
            return function () { return f.apply(this, arguments) };
        },

        CreateFuncWithArgs: function (f) {
            return function () { return f(Array.prototype.slice.call(arguments)) };
        },

        CreateFuncWithOnlyThis: function (f) {
            return function () { return f(this) };
        },

        CreateFuncWithThis: function (f) {
            return function () { return f(this).apply(null, arguments) };
        },

        CreateFuncWithThisArgs: function (f) {
            return function () { return f(this)(Array.prototype.slice.call(arguments)) };
        },

        CreateFuncWithRest: function (length, f) {
            return function () { return f(Array.prototype.slice.call(arguments, 0, length).concat([Array.prototype.slice.call(arguments, length)])) };
        },

        CreateFuncWithArgsRest: function (length, f) {
            return function () { return f([Array.prototype.slice.call(arguments, 0, length), Array.prototype.slice.call(arguments, length)]) };
        },

        BindDelegate: function (func, obj) {
            var res = func.bind(obj);
            res.$Func = func;
            res.$Target = obj;
            return res;
        },

        CreateDelegate: function (invokes) {
            if (invokes.length == 0) return null;
            if (invokes.length == 1) return invokes[0];
            var del = function () {
                var res;
                for (var i = 0; i < invokes.length; i++) {
                    res = invokes[i].apply(null, arguments);
                }
                return res;
            };
            del.$Invokes = invokes;
            return del;
        },

        CombineDelegates: function (dels) {
            var invokes = [];
            for (var i = 0; i < dels.length; i++) {
                var del = dels[i];
                if (del) {
                    if ("$Invokes" in del)
                        invokes = invokes.concat(del.$Invokes);
                    else
                        invokes.push(del);
                }
            }
            return IntelliFactory.Runtime.CreateDelegate(invokes);
        },

        DelegateEqual: function (d1, d2) {
            if (d1 === d2) return true;
            if (d1 == null || d2 == null) return false;
            var i1 = d1.$Invokes || [d1];
            var i2 = d2.$Invokes || [d2];
            if (i1.length != i2.length) return false;
            for (var i = 0; i < i1.length; i++) {
                var e1 = i1[i];
                var e2 = i2[i];
                if (!(e1 === e2 || ("$Func" in e1 && "$Func" in e2 && e1.$Func === e2.$Func && e1.$Target == e2.$Target)))
                    return false;
            }
            return true;
        },

        ThisFunc: function (d) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                args.unshift(this);
                return d.apply(null, args);
            };
        },

        ThisFuncOut: function (f) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return f.apply(args.shift(), args);
            };
        },

        ParamsFunc: function (length, d) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return d.apply(null, args.slice(0, length).concat([args.slice(length)]));
            };
        },

        ParamsFuncOut: function (length, f) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return f.apply(null, args.slice(0, length).concat(args[length]));
            };
        },

        ThisParamsFunc: function (length, d) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                args.unshift(this);
                return d.apply(null, args.slice(0, length + 1).concat([args.slice(length + 1)]));
            };
        },

        ThisParamsFuncOut: function (length, f) {
            return function () {
                var args = Array.prototype.slice.call(arguments);
                return f.apply(args.shift(), args.slice(0, length).concat(args[length]));
            };
        },

        Curried: function (f, n, args) {
            args = args || [];
            return function (a) {
                var allArgs = args.concat([a === void (0) ? null : a]);
                if (n == 1)
                    return f.apply(null, allArgs);
                if (n == 2)
                    return function (a) { return f.apply(null, allArgs.concat([a === void (0) ? null : a])); }
                return IntelliFactory.Runtime.Curried(f, n - 1, allArgs);
            }
        },

        Curried2: function (f) {
            return function (a) { return function (b) { return f(a, b); } }
        },

        Curried3: function (f) {
            return function (a) { return function (b) { return function (c) { return f(a, b, c); } } }
        },

        UnionByType: function (types, value, optional) {
            var vt = typeof value;
            for (var i = 0; i < types.length; i++) {
                var t = types[i];
                if (typeof t == "number") {
                    if (Array.isArray(value) && (t == 0 || value.length == t)) {
                        return { $: i, $0: value };
                    }
                } else {
                    if (t == vt) {
                        return { $: i, $0: value };
                    }
                }
            }
            if (!optional) {
                throw new Error("Type not expected for creating Choice value.");
            }
        },

        OnLoad:
            function (f) {
                if (!("load" in this)) {
                    this.load = [];
                }
                this.load.push(f);
            },

        Start:
            function () {
                function run(c) {
                    for (var i = 0; i < c.length; i++) {
                        c[i]();
                    }
                }
                if ("init" in this) {
                    run(this.init);
                    this.init = [];
                }
                if ("load" in this) {
                    run(this.load);
                    this.load = [];
                }
            },
    }
}

IntelliFactory.Runtime.OnLoad(function () {
    if (window.WebSharper && WebSharper.Activator && WebSharper.Activator.Activate)
        WebSharper.Activator.Activate()
});

// Polyfill

if (!Date.now) {
    Date.now = function now() {
        return new Date().getTime();
    };
}

if (!Math.trunc) {
    Math.trunc = function (x) {
        return x < 0 ? Math.ceil(x) : Math.floor(x);
    }
}

function ignore() { };
function id(x) { return x };
function fst(x) { return x[0] };
function snd(x) { return x[1] };
function trd(x) { return x[2] };

if (!console) {
    console = {
        count: ignore,
        dir: ignore,
        error: ignore,
        group: ignore,
        groupEnd: ignore,
        info: ignore,
        log: ignore,
        profile: ignore,
        profileEnd: ignore,
        time: ignore,
        timeEnd: ignore,
        trace: ignore,
        warn: ignore
    }
};
(function()
{
 "use strict";
 var Global,WebSharper,MathJax,Sample,Client,Operators,UI,Next,Doc,AttrProxy,DomUtility,Client$1,DocNode,Array,Elt,View,DocElemNode,Arrays,List,T,Unchecked,Attrs,SC$1,Docs,Var,Snap,Attrs$1,Dyn,Enumerator,SC$2,Abbrev,Fresh,T$1,Seq,SC$3,IntelliFactory,Runtime;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 MathJax=WebSharper.MathJax=WebSharper.MathJax||{};
 Sample=MathJax.Sample=MathJax.Sample||{};
 Client=Sample.Client=Sample.Client||{};
 Operators=WebSharper.Operators=WebSharper.Operators||{};
 UI=WebSharper.UI=WebSharper.UI||{};
 Next=UI.Next=UI.Next||{};
 Doc=Next.Doc=Next.Doc||{};
 AttrProxy=Next.AttrProxy=Next.AttrProxy||{};
 DomUtility=Next.DomUtility=Next.DomUtility||{};
 Client$1=Next.Client=Next.Client||{};
 DocNode=Client$1.DocNode=Client$1.DocNode||{};
 Array=Next.Array=Next.Array||{};
 Elt=Next.Elt=Next.Elt||{};
 View=Next.View=Next.View||{};
 DocElemNode=Next.DocElemNode=Next.DocElemNode||{};
 Arrays=WebSharper.Arrays=WebSharper.Arrays||{};
 List=WebSharper.List=WebSharper.List||{};
 T=List.T=List.T||{};
 Unchecked=WebSharper.Unchecked=WebSharper.Unchecked||{};
 Attrs=Next.Attrs=Next.Attrs||{};
 SC$1=Global.StartupCode$WebSharper_UI_Next$DomUtility=Global.StartupCode$WebSharper_UI_Next$DomUtility||{};
 Docs=Next.Docs=Next.Docs||{};
 Var=Next.Var=Next.Var||{};
 Snap=Next.Snap=Next.Snap||{};
 Attrs$1=Client$1.Attrs=Client$1.Attrs||{};
 Dyn=Attrs$1.Dyn=Attrs$1.Dyn||{};
 Enumerator=WebSharper.Enumerator=WebSharper.Enumerator||{};
 SC$2=Global.StartupCode$WebSharper_UI_Next$Attr_Client=Global.StartupCode$WebSharper_UI_Next$Attr_Client||{};
 Abbrev=Next.Abbrev=Next.Abbrev||{};
 Fresh=Abbrev.Fresh=Abbrev.Fresh||{};
 T$1=Enumerator.T=Enumerator.T||{};
 Seq=WebSharper.Seq=WebSharper.Seq||{};
 SC$3=Global.StartupCode$WebSharper_UI_Next$Abbrev=Global.StartupCode$WebSharper_UI_Next$Abbrev||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
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
 Operators.FailWith=function(msg)
 {
  throw Global.Error(msg);
 };
 Doc=Next.Doc=Runtime.Class({},null,Doc);
 Doc.Element=function(name,attr,children)
 {
  var attr$1,children$1,a;
  attr$1=AttrProxy.Concat(attr);
  children$1=Doc.Concat(children);
  a=DomUtility.CreateElement(name);
  return Elt.New(a,attr$1,children$1);
 };
 Doc.Concat=function(xs)
 {
  var x,d;
  x=Array.ofSeqNonCopying(xs);
  d=Doc.Empty();
  return Array.TreeReduce(d,Doc.Append,x);
 };
 Doc.TextNode=function(v)
 {
  var a,a$1;
  a={
   $:5,
   $0:DomUtility.CreateText(v)
  };
  a$1=View.Const();
  return Doc.Mk(a,a$1);
 };
 Doc.Empty=function()
 {
  var a,a$1;
  a=DocNode.EmptyDoc;
  a$1=View.Const();
  return Doc.Mk(a,a$1);
 };
 Doc.Append=function(a,b)
 {
  var x,x$1,y,a$1;
  x=(x$1=a.updates,(y=b.updates,View.Map2Unit(x$1,y)));
  a$1={
   $:0,
   $0:a.docNode,
   $1:b.docNode
  };
  return Doc.Mk(a$1,x);
 };
 Doc.Mk=function(node,updates)
 {
  return new Doc.New(node,updates);
 };
 Doc.New=Runtime.Ctor(function(docNode,updates)
 {
  this.docNode=docNode;
  this.updates=updates;
 },Doc);
 AttrProxy=Next.AttrProxy=Runtime.Class({},null,AttrProxy);
 AttrProxy.Concat=function(xs)
 {
  var x,d;
  x=Array.ofSeqNonCopying(xs);
  d=Attrs.EmptyAttr();
  return Array.TreeReduce(d,AttrProxy.Append,x);
 };
 AttrProxy.Append=function(a,b)
 {
  return Attrs.AppendTree(a,b);
 };
 DomUtility.CreateElement=function(name)
 {
  return DomUtility.Doc().createElement(name);
 };
 DomUtility.Doc=function()
 {
  SC$1.$cctor();
  return SC$1.Doc;
 };
 DomUtility.CreateText=function(s)
 {
  return DomUtility.Doc().createTextNode(s);
 };
 DomUtility.InsertAt=function(parent,pos,node)
 {
  var m,v;
  !(node.parentNode===parent?pos===(m=node.nextSibling,Unchecked.Equals(m,null)?null:m):false)?v=parent.insertBefore(node,pos):void 0;
 };
 DocNode.EmptyDoc={
  $:3
 };
 Array.ofSeqNonCopying=function(xs)
 {
  var q,o,v;
  if(xs instanceof Global.Array)
   return xs;
  else
   if(xs instanceof T)
    return Arrays.ofList(xs);
   else
    if(xs===null)
     return[];
    else
     {
      q=[];
      o=Enumerator.Get(xs);
      try
      {
       while(o.MoveNext())
        {
         v=q.push(o.Current());
        }
       return q;
      }
      finally
      {
       if("Dispose"in o)
        o.Dispose();
      }
     }
 };
 Array.TreeReduce=function(defaultValue,reduction,array)
 {
  var l;
  function loop(off,len)
  {
   var $1,l2;
   return len<=0?defaultValue:(len===1?(off>=0?off<l:false)?true:false:false)?Arrays.get(array,off):(l2=len/2>>0,reduction(loop(off,l2),loop(off+l2,len-l2)));
  }
  l=Arrays.length(array);
  return loop(0,l);
 };
 Array.MapTreeReduce=function(mapping,defaultValue,reduction,array)
 {
  var l;
  function loop(off,len)
  {
   var $1,l2;
   return len<=0?defaultValue:(len===1?(off>=0?off<l:false)?true:false:false)?mapping(Arrays.get(array,off)):(l2=len/2>>0,reduction(loop(off,l2),loop(off+l2,len-l2)));
  }
  l=Arrays.length(array);
  return loop(0,l);
 };
 Elt=Next.Elt=Runtime.Class({},Doc,Elt);
 Elt.New=function(el,attr,children)
 {
  var node,rvUpdates,attrUpdates,updates,a;
  node=Docs.CreateElemNode(el,attr,children.docNode);
  rvUpdates=Var.Create$1(children.updates);
  attrUpdates=Attrs.Updates(node.Attr);
  updates=(a=rvUpdates.v,View.Bind(function(a$1)
  {
   return View.Map2Unit(attrUpdates,a$1);
  },a));
  return new Elt.New$1({
   $:1,
   $0:node
  },updates,el,rvUpdates);
 };
 Elt.New$1=Runtime.Ctor(function(docNode,updates,elt,rvUpdates)
 {
  Doc.New.call(this,docNode,updates);
  this.docNode$1=docNode;
  this.updates$1=updates;
  this.elt=elt;
  this.rvUpdates=rvUpdates;
 },Elt);
 View.Const=function(x)
 {
  var o;
  o=Snap.CreateForever(x);
  return function()
  {
   return o;
  };
 };
 View.Map2Unit=function(a,a$1)
 {
  return View.CreateLazy(function()
  {
   var s1,s2;
   s1=a();
   s2=a$1();
   return Snap.Map2Unit(s1,s2);
  });
 };
 View.Bind=function(fn,view)
 {
  return View.Join(View.Map(fn,view));
 };
 View.CreateLazy=function(observe)
 {
  var lv;
  lv={
   c:null,
   o:observe
  };
  return function()
  {
   var c;
   c=lv.c;
   return c===null?(c=lv.o(),lv.c=c,Snap.IsForever(c)?lv.o=null:Snap.WhenObsolete(c,function()
   {
    lv.c=null;
   }),c):c;
  };
 };
 View.Join=function(a)
 {
  return View.CreateLazy(function()
  {
   return Snap.Join(a());
  });
 };
 View.Map=function(fn,a)
 {
  return View.CreateLazy(function()
  {
   var a$1;
   a$1=a();
   return Snap.Map(fn,a$1);
  });
 };
 DocElemNode=Next.DocElemNode=Runtime.Class({
  Equals:function(o)
  {
   return this.ElKey===o.ElKey;
  }
 },null,DocElemNode);
 DocElemNode.New=function(Attr,Children,Delimiters,El,ElKey,Render)
 {
  var $1;
  return new DocElemNode(($1={
   Attr:Attr,
   Children:Children,
   El:El,
   ElKey:ElKey
  },(Runtime.SetOptional($1,"Delimiters",Delimiters),Runtime.SetOptional($1,"Render",Render),$1)));
 };
 Arrays.get=function(arr,n)
 {
  Arrays.checkBounds(arr,n);
  return arr[n];
 };
 Arrays.length=function(arr)
 {
  return arr.dims===2?arr.length*arr.length:arr.length;
 };
 Arrays.checkBounds=function(arr,n)
 {
  if(n<0?true:n>=arr.length)
   Operators.FailWith("Index was outside the bounds of the array.");
 };
 T=List.T=Runtime.Class({
  GetEnumerator:function()
  {
   return new T$1.New(this,null,function(e)
   {
    var m,xs;
    m=e.s;
    return m.$==0?false:(xs=m.$1,(e.c=m.$0,e.s=xs,true));
   },void 0);
  }
 },null,T);
 Arrays.ofList=function(xs)
 {
  var l,q;
  q=[];
  l=xs;
  while(!(l.$==0))
   {
    q.push(List.head(l));
    l=List.tail(l);
   }
  return q;
 };
 Arrays.foldBack=function(f,arr,zero)
 {
  var acc,$1,len,i,$2;
  acc=zero;
  len=arr.length;
  for(i=1,$2=len;i<=$2;i++)acc=f(arr[len-i],acc);
  return acc;
 };
 Unchecked.Equals=function(a,b)
 {
  var m,eqR,k,k$1;
  if(a===b)
   return true;
  else
   {
    m=typeof a;
    if(m=="object")
    {
     if(((a===null?true:a===void 0)?true:b===null)?true:b===void 0)
      return false;
     else
      if("Equals"in a)
       return a.Equals(b);
      else
       if(a instanceof Global.Array?b instanceof Global.Array:false)
        return Unchecked.arrayEquals(a,b);
       else
        if(a instanceof Global.Date?b instanceof Global.Date:false)
         return Unchecked.dateEquals(a,b);
        else
         {
          eqR=[true];
          for(var k$2 in a)if(function(k$3)
          {
           eqR[0]=!a.hasOwnProperty(k$3)?true:b.hasOwnProperty(k$3)?Unchecked.Equals(a[k$3],b[k$3]):false;
           return!eqR[0];
          }(k$2))
           break;
          if(eqR[0])
           {
            for(var k$3 in b)if(function(k$4)
            {
             eqR[0]=!b.hasOwnProperty(k$4)?true:a.hasOwnProperty(k$4);
             return!eqR[0];
            }(k$3))
             break;
           }
          return eqR[0];
         }
    }
    else
     return m=="function"?"$Func"in a?a.$Func===b.$Func?a.$Target===b.$Target:false:("$Invokes"in a?"$Invokes"in b:false)?Unchecked.arrayEquals(a.$Invokes,b.$Invokes):false:false;
   }
 };
 Unchecked.arrayEquals=function(a,b)
 {
  var eq,i;
  if(Arrays.length(a)===Arrays.length(b))
   {
    eq=true;
    i=0;
    while(eq?i<Arrays.length(a):false)
     {
      !Unchecked.Equals(Arrays.get(a,i),Arrays.get(b,i))?eq=false:void 0;
      i=i+1;
     }
    return eq;
   }
  else
   return false;
 };
 Unchecked.dateEquals=function(a,b)
 {
  return a.getTime()===b.getTime();
 };
 Attrs.EmptyAttr=function()
 {
  SC$2.$cctor();
  return SC$2.EmptyAttr;
 };
 Attrs.AppendTree=function(a,b)
 {
  return a===null?b:b===null?a:new AttrProxy({
   $:2,
   $0:a,
   $1:b
  });
 };
 Attrs.Updates=function(dyn)
 {
  var x,d;
  x=dyn.DynNodes;
  d=View.Const();
  return Array.MapTreeReduce(function(x$1)
  {
   return x$1.NChanged();
  },d,View.Map2Unit,x);
 };
 Attrs.Insert=function(elem,tree)
 {
  var nodes,oar,arr;
  function loop(node)
  {
   var b;
   if(!(node===null))
    if(node!=null?node.$==1:false)
     nodes.push(node.$0);
    else
     if(node!=null?node.$==2:false)
      {
       b=node.$1;
       loop(node.$0);
       loop(b);
      }
     else
      if(node!=null?node.$==3:false)
       node.$0(elem);
      else
       if(node!=null?node.$==4:false)
        oar.push(node.$0);
  }
  nodes=[];
  oar=[];
  loop(tree);
  arr=nodes.slice(0);
  return Dyn.New(elem,Attrs.Flags(tree),arr,oar.length===0?null:{
   $:1,
   $0:function(el)
   {
    Seq.iter(function(f)
    {
     f(el);
    },oar);
   }
  });
 };
 Attrs.Flags=function(a)
 {
  return(a!==null?a.hasOwnProperty("flags"):false)?a.flags:0;
 };
 SC$1.$cctor=Runtime.Cctor(function()
 {
  SC$1.Doc=Global.document;
  SC$1.$cctor=Global.ignore;
 });
 Docs.CreateElemNode=function(el,attr,children)
 {
  var attr$1;
  Docs.LinkElement(el,children);
  attr$1=Attrs.Insert(el,attr);
  return DocElemNode.New(attr$1,children,null,el,Fresh.Int(),Runtime.GetOptional(attr$1.OnAfterRender));
 };
 Docs.LinkElement=function(el,children)
 {
  var v;
  v=Docs.InsertDoc(el,children,null);
 };
 Docs.InsertDoc=function(parent,doc,pos)
 {
  var e,d,t,t$1,t$2,b,a;
  return doc.$==1?(e=doc.$0,Docs.InsertNode(parent,e.El,pos)):doc.$==2?(d=doc.$0,(d.Dirty=false,Docs.InsertDoc(parent,d.Current,pos))):doc.$==3?pos:doc.$==4?(t=doc.$0,Docs.InsertNode(parent,t.Text,pos)):doc.$==5?(t$1=doc.$0,Docs.InsertNode(parent,t$1,pos)):doc.$==6?(t$2=doc.$0,Arrays.foldBack(function($1,$2)
  {
   return $1.constructor===Global.Object?Docs.InsertDoc(parent,$1,$2):Docs.InsertNode(parent,$1,$2);
  },t$2.Els,pos)):(b=doc.$1,(a=doc.$0,Docs.InsertDoc(parent,a,Docs.InsertDoc(parent,b,pos))));
 };
 Docs.InsertNode=function(parent,node,pos)
 {
  DomUtility.InsertAt(parent,pos,node);
  return node;
 };
 Var.Create$1=function(v)
 {
  var _var;
  _var=null;
  _var=Var.New(false,v,Snap.CreateWithValue(v),Fresh.Int(),function()
  {
   return _var.s;
  });
  return _var;
 };
 Var=Next.Var=Runtime.Class({},null,Var);
 Var.New=function(Const,Current,Snap$1,Id,VarView)
 {
  return new Var({
   o:Const,
   c:Current,
   s:Snap$1,
   i:Id,
   v:VarView
  });
 };
 Snap.CreateForever=function(v)
 {
  return Snap.Make({
   $:0,
   $0:v
  });
 };
 Snap.Map2Unit=function(sn1,sn2)
 {
  var $1,$2,res,obs;
  function cont()
  {
   var $3,$4,f1,f2;
   if(!Snap.IsDone(res))
    {
     $3=Snap.ValueAndForever(sn1);
     $4=Snap.ValueAndForever(sn2);
     ($3!=null?$3.$==1:false)?($4!=null?$4.$==1:false)?(f1=$3.$0[1],f2=$4.$0[1],(f1?f2:false)?Snap.MarkForever(res,null):Snap.MarkReady(res,null)):void 0:void 0;
    }
  }
  $1=sn1.s;
  $2=sn2.s;
  return $1.$==0?$2.$==0?Snap.CreateForever():sn2:$2.$==0?sn1:(res=Snap.Create(),(obs=Snap.Obs(res),(Snap.When(sn1,cont,obs),Snap.When(sn2,cont,obs),res)));
 };
 Snap.CreateWithValue=function(v)
 {
  return Snap.Make({
   $:2,
   $0:v,
   $1:[]
  });
 };
 Snap.Make=function(st)
 {
  return{
   s:st
  };
 };
 Snap.IsForever=function(snap)
 {
  return snap.s.$==0?true:false;
 };
 Snap.WhenObsolete=function(snap,obsolete)
 {
  var m;
  m=snap.s;
  m.$==1?obsolete():m.$==2?m.$1.push(obsolete):m.$==3?m.$1.push(obsolete):void 0;
 };
 Snap.Create=function()
 {
  return Snap.Make({
   $:3,
   $0:[],
   $1:[]
  });
 };
 Snap.Obs=function(sn)
 {
  return function()
  {
   Snap.MarkObsolete(sn);
  };
 };
 Snap.IsDone=function(snap)
 {
  var m;
  m=snap.s;
  return m.$==0?true:m.$==2?true:false;
 };
 Snap.ValueAndForever=function(snap)
 {
  var m;
  m=snap.s;
  return m.$==0?{
   $:1,
   $0:[m.$0,true]
  }:m.$==2?{
   $:1,
   $0:[m.$0,false]
  }:null;
 };
 Snap.MarkForever=function(sn,v)
 {
  var m,q;
  m=sn.s;
  m.$==3?(q=m.$0,sn.s={
   $:0,
   $0:v
  },Seq.iter(function(k)
  {
   k(v);
  },q)):void 0;
 };
 Snap.MarkReady=function(sn,v)
 {
  var m,q2,q1;
  m=sn.s;
  m.$==3?(q2=m.$1,q1=m.$0,sn.s={
   $:2,
   $0:v,
   $1:q2
  },Seq.iter(function(k)
  {
   k(v);
  },q1)):void 0;
 };
 Snap.When=function(snap,avail,obsolete)
 {
  var m,v,q2;
  m=snap.s;
  m.$==1?obsolete():m.$==2?(v=m.$0,m.$1.push(obsolete),avail(v)):m.$==3?(q2=m.$1,m.$0.push(avail),q2.push(obsolete)):avail(m.$0);
 };
 Snap.Join=function(snap)
 {
  var res,obs;
  res=Snap.Create();
  obs=Snap.Obs(res);
  Snap.When(snap,function(x)
  {
   var y;
   y=x();
   Snap.When(y,function(v)
   {
    if(Snap.IsForever(y)?Snap.IsForever(snap):false)
     Snap.MarkForever(res,v);
    else
     Snap.MarkReady(res,v);
   },obs);
  },obs);
  return res;
 };
 Snap.Map=function(fn,sn)
 {
  var m,x,res,g;
  m=sn.s;
  return m.$==0?(x=m.$0,Snap.CreateForever(fn(x))):(res=Snap.Create(),(Snap.When(sn,(g=function(v)
  {
   Snap.MarkDone(res,sn,v);
  },function(x$1)
  {
   return g(fn(x$1));
  }),Snap.Obs(res)),res));
 };
 Snap.MarkObsolete=function(sn)
 {
  var m,$1;
  m=sn.s;
  (m.$==1?true:m.$==2?($1=m.$1,false):m.$==3?($1=m.$1,false):true)?void 0:(sn.s={
   $:1
  },Seq.iter(function(k)
  {
   k();
  },$1));
 };
 Snap.MarkDone=function(res,sn,v)
 {
  if(Snap.IsForever(sn))
   Snap.MarkForever(res,v);
  else
   Snap.MarkReady(res,v);
 };
 Dyn.New=function(DynElem,DynFlags,DynNodes,OnAfterRender)
 {
  var $1;
  $1={
   DynElem:DynElem,
   DynFlags:DynFlags,
   DynNodes:DynNodes
  };
  Runtime.SetOptional($1,"OnAfterRender",OnAfterRender);
  return $1;
 };
 List.head=function(l)
 {
  return l.$==1?l.$0:List.listEmpty();
 };
 List.tail=function(l)
 {
  return l.$==1?l.$1:List.listEmpty();
 };
 List.listEmpty=function()
 {
  return Operators.FailWith("The input list was empty.");
 };
 Enumerator.Get=function(x)
 {
  return x instanceof Global.Array?Enumerator.ArrayEnumerator(x):Unchecked.Equals(typeof x,"string")?Enumerator.StringEnumerator(x):x.GetEnumerator();
 };
 Enumerator.ArrayEnumerator=function(s)
 {
  return new T$1.New(0,null,function(e)
  {
   var i;
   i=e.s;
   return i<Arrays.length(s)?(e.c=Arrays.get(s,i),e.s=i+1,true):false;
  },void 0);
 };
 Enumerator.StringEnumerator=function(s)
 {
  return new T$1.New(0,null,function(e)
  {
   var i;
   i=e.s;
   return i<s.length?(e.c=s.charCodeAt(i),e.s=i+1,true):false;
  },void 0);
 };
 SC$2.$cctor=Runtime.Cctor(function()
 {
  SC$2.EmptyAttr=null;
  SC$2.$cctor=Global.ignore;
 });
 Fresh.Int=function()
 {
  Fresh.set_counter(Fresh.counter()+1);
  return Fresh.counter();
 };
 Fresh.set_counter=function($1)
 {
  SC$3.$cctor();
  SC$3.counter=$1;
 };
 Fresh.counter=function()
 {
  SC$3.$cctor();
  return SC$3.counter;
 };
 T$1=Enumerator.T=Runtime.Class({
  MoveNext:function()
  {
   return this.n(this);
  },
  Current:function()
  {
   return this.c;
  },
  Dispose:function()
  {
   if(this.d)
    this.d(this);
  }
 },null,T$1);
 T$1.New=Runtime.Ctor(function(s,c,n,d)
 {
  this.s=s;
  this.c=c;
  this.n=n;
  this.d=d;
 },T$1);
 Seq.iter=function(p,s)
 {
  var e;
  e=Enumerator.Get(s);
  try
  {
   while(e.MoveNext())
    p(e.Current());
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
 };
 SC$3.$cctor=Runtime.Cctor(function()
 {
  SC$3.counter=0;
  SC$3.$cctor=Global.ignore;
 });
 Client.Main();
}());


if (typeof IntelliFactory !=='undefined')
  IntelliFactory.Runtime.Start();
