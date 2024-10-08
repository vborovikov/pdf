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
using MigraDoc.DocumentObjectModel.Visitors;
using MigraDoc.DocumentObjectModel.Tables;

namespace MigraDoc.RtfRendering
{
    /// <summary>
    /// Renders a cell to RTF.
    /// </summary>
    internal class CellRenderer : StyleAndFormatRenderer
    {
        internal CellRenderer(DocumentObject domObj, RtfDocumentRenderer docRenderer)
            : base(domObj, docRenderer)
        {
            _cell = domObj as Cell;
        }

        internal override void Render()
        {
            _useEffectiveValue = true;
            Cell cvrgCell = _cellList.GetCoveringCell(_cell);
            if (_cell.Column.Index != cvrgCell.Column.Index)
                return;

            bool writtenAnyContent = false;
            if (!_cell.IsNull("Elements"))
            {
                if (_cell == cvrgCell)
                {
                    foreach (DocumentObject docObj in _cell.Elements)
                    {
                        RendererBase rndrr = RendererFactory.CreateRenderer(docObj, _docRenderer);
                        if (rndrr != null)
                        {
                            rndrr.Render();
                            writtenAnyContent = true;
                        }
                    }
                }
            }
            if (!writtenAnyContent)
            {
                //Format attributes need to be set here to satisfy Word 2000.
                _rtfWriter.WriteControl("pard");
                RenderStyleAndFormat();
                _rtfWriter.WriteControl("intbl");
                EndStyleAndFormatAfterContent();
            }
            _rtfWriter.WriteControl("cell");
        }

        internal MergedCellList CellList
        {
            set
            {
                _cellList = value;
            }
        }
        MergedCellList _cellList = null;
        readonly Cell _cell;
    }
}
