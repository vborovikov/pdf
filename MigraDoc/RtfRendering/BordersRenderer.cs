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

namespace MigraDoc.RtfRendering
{
    /// <summary>
    /// Class to render a Borders object to RTF.
    /// </summary>
    internal class BordersRenderer : BorderRendererBase
    {
        internal BordersRenderer(DocumentObject domObj, RtfDocumentRenderer docRenderer)
            : base(domObj, docRenderer)
        {
            _borders = domObj as Borders;
        }

        /// <summary>
        /// Renders a Borders object to RTF.
        /// </summary>
        internal override void Render()
        {
            _useEffectiveValue = true;

            object visible = GetValueAsIntended("Visible");
            object borderStyle = GetValueAsIntended("Style");
            object borderColor = GetValueAsIntended("Color");
            bool isCellBorders = (_parentCell != null);
            bool isFormatBorders = (_parentFormat != null);

            object obj = null;
            if (LeaveAwayTop)
                _rtfWriter.WriteControl(GetBorderControl(BorderType.Top));
            else
            {
                obj = GetValueAsIntended("Top");
                if (obj != null)
                {
                    CreateBorderRenderer((Border)obj, BorderType.Top).Render();
                }
                else
                    RenderBorder(GetBorderControl(BorderType.Top));
            }
            if (!isCellBorders)
                Translate("DistanceFromTop", "brsp");
            //REVIEW: Andere Renderer verfahren glaube ich genauso, da ja sonst doppelt gemoppelt

            if (LeaveAwayLeft)
                _rtfWriter.WriteControl(GetBorderControl(BorderType.Top));
            else
            {
                obj = GetValueAsIntended("Left");
                if (obj != null)
                    CreateBorderRenderer((Border)obj, BorderType.Left).Render();
                else
                    RenderBorder(GetBorderControl(BorderType.Left));
            }
            if (!isCellBorders)
                Translate("DistanceFromLeft", "brsp");

            if (LeaveAwayRight)
                _rtfWriter.WriteControl(GetBorderControl(BorderType.Right));
            else
            {
                obj = GetValueAsIntended("Right");
                if (obj != null)
                    CreateBorderRenderer((Border)obj, BorderType.Right).Render();
                else
                    RenderBorder(GetBorderControl(BorderType.Right));
            }
            if (!isCellBorders)
                Translate("DistanceFromRight", "brsp");

            if (LeaveAwayBottom)
                _rtfWriter.WriteControl(GetBorderControl(BorderType.Bottom));
            else
            {
                obj = GetValueAsIntended("Bottom");
                if (obj != null)
                    CreateBorderRenderer((Border)obj, BorderType.Bottom).Render();
                else
                    RenderBorder(GetBorderControl(BorderType.Bottom));
            }
            // All renderers ignore DistanceFrom for table cells.
            // This would lead to problems with Word. (Padding is the substitute.)
            if (!isCellBorders)
                Translate("DistanceFromBottom", "brsp");

            if (isCellBorders)
            {
                obj = GetValueAsIntended("DiagonalDown");
                if (obj != null)
                    CreateBorderRenderer((Border)obj, BorderType.DiagonalDown).Render();

                obj = GetValueAsIntended("DiagonalUp");
                if (obj != null)
                    CreateBorderRenderer((Border)obj, BorderType.DiagonalUp).Render();
            }
        }

        BorderRenderer CreateBorderRenderer(Border border, BorderType borderType)
        {
            BorderRenderer brdrRndrr = new BorderRenderer(border, _docRenderer);
            brdrRndrr.ParentCell = _parentCell;
            brdrRndrr.ParentFormat = _parentFormat;
            brdrRndrr.BorderType = borderType;
            return brdrRndrr;
        }

        Borders _borders;

        internal bool LeaveAwayTop;
        internal bool LeaveAwayLeft;
        internal bool LeaveAwayRight;
        internal bool LeaveAwayBottom;
    }
}
