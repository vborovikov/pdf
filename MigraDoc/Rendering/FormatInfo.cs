#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   Klaus Potzesny
//
// Copyright (c) 2001-2017 empira Software GmbH, Cologne Area (Germany)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

namespace MigraDoc.Rendering
{
    /// <summary>
    /// Abstract base class for formatting information received by calling Format() on a renderer.
    /// </summary>
    public abstract class FormatInfo
    {
        /// <summary>
        /// Indicates that the formatted object is starting.
        /// </summary>
        internal abstract bool IsStarting
        {
            get;
        }

        /// <summary>
        /// Indicates that the formatted object is ending.
        /// </summary>
        internal abstract bool IsEnding
        {
            get;
        }

        /// <summary>
        /// Indicates that the formatted object is complete.
        /// </summary>
        internal abstract bool IsComplete
        {
            get;
        }

        /// <summary>
        /// Indicates that the starting of the element is completed
        /// </summary>
        internal abstract bool StartingIsComplete
        {
            get;
        }

        /// <summary>
        /// Indicates that the ending of the element is completed
        /// </summary>
        internal abstract bool EndingIsComplete
        {
            get;
        }

        internal abstract bool IsEmpty
        {
            get;
        }
    }
}
