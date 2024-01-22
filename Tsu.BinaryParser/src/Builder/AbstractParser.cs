// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Tsu.BinaryParser.Parsers;

namespace Tsu.BinaryParser.Builder
{
    /// <summary>
    /// A parser builder for complex objects.
    /// </summary>
    /// <typeparam name="T">The type of object being parsed.</typeparam>
    public abstract class AbstractParser<[DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor
        | DynamicallyAccessedMemberTypes.PublicFields
        | DynamicallyAccessedMemberTypes.PublicProperties)] T> : IBinaryParser<T>
    {
        private readonly List<ParserStep> _steps = new();

        /// <summary>
        /// Initializes a new parser builder
        /// </summary>
        public AbstractParser()
        {
            if (!typeof(T).GetConstructors().Any(ctor => ctor.IsPublic && ctor.GetParameters().Length == 0))
                throw new InvalidOperationException("Parsed types must have a public parameterless constructor.");
        }

        /// <summary>
        /// All steps currently registered in this parser.
        /// </summary>
        public IEnumerable<ParserStep> Steps => _steps;

        public abstract Option<int> MinimumByteCount { get; }
        public abstract Option<int> MaxmiumByteCount { get; }
        public abstract bool IsFixedSize { get; }

        /// <summary>
        /// Adds a new <see cref="MemberBindingStep" /> to the parser.
        /// </summary>
        /// <typeparam name="TMember">The type of the member being read/written to.</typeparam>
        /// <param name="expression">
        /// The member to be used in this step.
        /// </param>
        /// <param name="parser">
        /// The parser that is able to handle the type of this member.
        /// </param>
        /// <returns>
        /// The parser itself.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        protected AbstractParser<T> Add<TMember>(Expression<Func<T, TMember>> expression, IBinaryParser<T> parser)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Provided expression is not a member access expression.", nameof(expression));

            var memberExpression = (MemberExpression) expression.Body;
            if (memberExpression.Expression?.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("Only direct members of the class can be used.", nameof(expression));

            if (memberExpression.Member is FieldInfo fieldInfo)
            {
                if (fieldInfo.IsInitOnly)
                    throw new ArgumentException("The provided field cannot be written to.", nameof(expression));
                _steps.Add(new MemberBindingStep(fieldInfo, parser));
            }
            else if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                if (!propertyInfo.CanRead)
                    throw new ArgumentException("The provided property cannot be read from.");
                if (!propertyInfo.CanWrite)
                    throw new ArgumentException("The provided property cannot be written to.");
                _steps.Add(new MemberBindingStep(propertyInfo, parser));
            }
            else
            {
                throw new ArgumentException("Member being accessed is not a field nor property.");
            }

            return this;
        }

        public abstract long CalculateSize(T value);
        public abstract T Deserialize(IBinaryReader reader, IBinaryParsingContext context);
        public abstract ValueTask<T> DeserializeAsync(IBinaryReader reader, IBinaryParsingContext context, CancellationToken cancellationToken = default);
        public abstract void Serialize(Stream stream, IBinaryParsingContext context, T value);
        public abstract ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, T value, CancellationToken cancellationToken = default);
    }
}