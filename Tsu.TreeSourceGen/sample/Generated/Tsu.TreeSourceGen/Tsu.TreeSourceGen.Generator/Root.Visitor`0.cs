﻿// <auto-generated/>

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tsu.TreeSourceGen.Sample
{
    interface IVisitor
    {
        void VisitBinary(Tsu.TreeSourceGen.Sample.Binary binary);
        void VisitConstant(Tsu.TreeSourceGen.Sample.Constant constant);
        void VisitFunctionCall(Tsu.TreeSourceGen.Sample.FunctionCall functionCall);
    }
    
    partial class Visitor : IVisitor 
    {
        public virtual void Visit(Tsu.TreeSourceGen.Sample.Root node)
        {
            if (node is not null)
                node.Accept(this);
            }
            
            protected virtual void DefaultVisit(Tsu.TreeSourceGen.Sample.Root node)
            {
            }
            
            void VisitBinary(Tsu.TreeSourceGen.Sample.Binary binary) => DefaultVisit(
            binary);
            
            void VisitConstant(Tsu.TreeSourceGen.Sample.Constant constant) => DefaultVisit(
            constant);
            
            void VisitFunctionCall(Tsu.TreeSourceGen.Sample.FunctionCall functionCall) => DefaultVisit(
            functionCall);
        }
    }
}
