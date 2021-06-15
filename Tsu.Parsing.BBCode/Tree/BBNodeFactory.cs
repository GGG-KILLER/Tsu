// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

namespace Tsu.Parsing.BBCode.Tree
{
    /// <summary>
    /// A factory for <see cref="BBTextNode" /> and <see cref="BBTagNode" />
    /// </summary>
    public static class BBNodeFactory
    {
        /// <summary>
        /// Initializes a new <see cref="BBTextNode" /> with the provided <paramref name="value" />.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BBTextNode Text(string value) =>
            new BBTextNode(value);

        /// <summary>
        /// Initializes a new self-closing <see cref="BBTagNode" /> with the provided
        /// <paramref name="name" />.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BBTagNode SelfClosingTag(string name) =>
            new BBTagNode(true, name, null);

        /// <summary>
        /// Initializes a new self-closing <see cref="BBTagNode" /> with the provided
        /// <paramref name="name" /> and <paramref name="value" />.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BBTagNode SelfClosingTag(string name, string value) =>
            new BBTagNode(true, name, value);

        /// <summary>
        /// Initializes a new <see cref="BBTagNode" /> with the provided <paramref name="name" />.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static BBTagNode Tag(string name, params BBNode[] children)
        {
            var tag = new BBTagNode(false, name, null);
            foreach (var child in children)
                tag.AddChild(child);
            return tag;
        }

        /// <summary>
        /// Initializes a new <see cref="BBTagNode" /> with the provided <paramref name="name" /> and
        /// <paramref name="value" />.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static BBTagNode Tag(string name, string value, params BBNode[] children)
        {
            var tag = new BBTagNode(false, name, value);
            foreach (var child in children)
                tag.AddChild(child);
            return tag;
        }
    }
}
