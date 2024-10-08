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

using MigraDoc.DocumentObjectModel;
using PdfSharp.Drawing;

namespace MigraDoc.Rendering
{
    /// <summary>
    /// Abstract base class for all classes that store rendering information.
    /// </summary>
    public abstract class RenderInfo
    {
        /// <summary>
        /// Gets the format information in a specific derived type. For a table, for example, this will be a TableFormatInfo with information about the first and last row showing on a page.
        /// </summary>
        public abstract FormatInfo FormatInfo { get; internal set; }

        /// <summary>
        /// Gets the layout information.
        /// </summary>
        public LayoutInfo LayoutInfo
        {
            get { return _layoutInfo; }
        }
        readonly LayoutInfo _layoutInfo = new LayoutInfo();

        /// <summary>
        /// Gets the document object to which the layout information applies. Use the Tag property of DocumentObject to identify an object.
        /// </summary>
        public abstract DocumentObject DocumentObject { get; internal set; }

        internal virtual void RemoveEnding()
        {
            System.Diagnostics.Debug.Assert(false, "Unexpected call of RemoveEnding");
        }

        internal static XUnit GetTotalHeight(RenderInfo[] renderInfos)
        {
            if (renderInfos == null || renderInfos.Length == 0)
                return 0;

            int lastIdx = renderInfos.Length - 1;
            RenderInfo firstRenderInfo = renderInfos[0];
            RenderInfo lastRenderInfo = renderInfos[lastIdx];
            LayoutInfo firstLayoutInfo = firstRenderInfo.LayoutInfo;
            LayoutInfo lastLayoutInfo = lastRenderInfo.LayoutInfo;
            XUnit top = firstLayoutInfo.ContentArea.Y - firstLayoutInfo.MarginTop;
            XUnit bottom = lastLayoutInfo.ContentArea.Y + lastLayoutInfo.ContentArea.Height;
            bottom += lastLayoutInfo.MarginBottom;
            return bottom - top;
        }
    }
}
