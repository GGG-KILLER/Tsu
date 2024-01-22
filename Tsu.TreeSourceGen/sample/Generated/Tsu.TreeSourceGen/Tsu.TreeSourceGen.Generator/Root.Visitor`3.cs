﻿// <auto-generated/>

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tsu.TreeSourceGen.Sample
{
    interface IVisitor<TReturn, TArg1, TArg2>
    {
        [return: MaybeNull]
        TReturn VisitBinary(Tsu.TreeSourceGen.Sample.Binary binary, TArg1 arg1, TArg2 arg2);
        [return: MaybeNull]
        TReturn VisitConstant(Tsu.TreeSourceGen.Sample.Constant constant, TArg1 arg1, TArg2 arg2);
        [return: MaybeNull]
        TReturn VisitFunctionCall(Tsu.TreeSourceGen.Sample.FunctionCall functionCall, TArg1 arg1, TArg2 arg2);
    }
    
    partial class Visitor<TReturn, TArg1, TArg2> : IVisitor<TReturn, TArg1, TArg2> 
    {
        [return: MaybeNull]
        public virtual TReturn Visit(Tsu.TreeSourceGen.Sample.Root node, TArg1 arg1, TArg2 arg2)
        {
            if (node is not null)
                return node.Accept(this, arg1, arg2);
            
            return default;
        }
        
        [return: MaybeNull]
        protected virtual TReturn DefaultVisit(Tsu.TreeSourceGen.Sample.Root node, TArg1 arg1, TArg2 arg2) => default;
        
        [return: MaybeNull]
        public virtual TReturn VisitBinary(Tsu.TreeSourceGen.Sample.Binary binary, TArg1 arg1, TArg2 arg2) => DefaultVisit(binary, arg1, arg2);
        
        [return: MaybeNull]
        public virtual TReturn VisitConstant(Tsu.TreeSourceGen.Sample.Constant constant, TArg1 arg1, TArg2 arg2) => DefaultVisit(constant, arg1, arg2);
        
        [return: MaybeNull]
        public virtual TReturn VisitFunctionCall(Tsu.TreeSourceGen.Sample.FunctionCall functionCall, TArg1 arg1, TArg2 arg2) => DefaultVisit(functionCall, arg1, arg2);
    }
}
