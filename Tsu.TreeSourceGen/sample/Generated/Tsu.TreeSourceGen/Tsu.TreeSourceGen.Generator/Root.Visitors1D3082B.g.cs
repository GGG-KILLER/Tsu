﻿// <auto-generated/>

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tsu.TreeSourceGen.Sample
{
    partial class Root 
    {
        public void Accept(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitRoot(this);
        public TReturn Accept<TReturn>(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitRoot(this);
        public TReturn Accept<TReturn, TArg1>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1) => visitor.VisitRoot(this, arg1);
        public TReturn Accept<TReturn, TArg1, TArg2>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1, TArg2 arg2) => visitor.VisitRoot(this, arg1, arg2);
    }
}

namespace Tsu.TreeSourceGen.Sample
{
    partial class Binary 
    {
        public void Accept(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitBinary(this);
        public TReturn Accept<TReturn>(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitBinary(this);
        public TReturn Accept<TReturn, TArg1>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1) => visitor.VisitBinary(this, arg1);
        public TReturn Accept<TReturn, TArg1, TArg2>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1, TArg2 arg2) => visitor.VisitBinary(this, arg1, arg2);
    }
}

namespace Tsu.TreeSourceGen.Sample
{
    partial class Constant 
    {
        public void Accept(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitConstant(this);
        public TReturn Accept<TReturn>(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitConstant(this);
        public TReturn Accept<TReturn, TArg1>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1) => visitor.VisitConstant(this, arg1);
        public TReturn Accept<TReturn, TArg1, TArg2>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1, TArg2 arg2) => visitor.VisitConstant(this, arg1, arg2);
    }
}

namespace Tsu.TreeSourceGen.Sample
{
    partial class FunctionCall 
    {
        public void Accept(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitFunctionCall(this);
        public TReturn Accept<TReturn>(Tsu.TreeSourceGen.Sample.Visitor visitor) => visitor.VisitFunctionCall(this);
        public TReturn Accept<TReturn, TArg1>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1) => visitor.VisitFunctionCall(this, arg1);
        public TReturn Accept<TReturn, TArg1, TArg2>(Tsu.TreeSourceGen.Sample.Visitor visitor, TArg1 arg1, TArg2 arg2) => visitor.VisitFunctionCall(this, arg1, arg2);
    }
}
