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
using MigraDoc.DocumentObjectModel.Internals;
using MigraDoc.DocumentObjectModel.Visitors;
using MigraDoc.DocumentObjectModel.Tables;

namespace MigraDoc.RtfRendering
{
    /// <summary>
    /// Class to render a Row to RTF.
    /// </summary>
    internal class RowRenderer : RendererBase
    {
        internal RowRenderer(DocumentObject domObj, RtfDocumentRenderer docRenderer)
            : base(domObj, docRenderer)
        {
            _row = domObj as Row;
        }

        /// <summary>
        /// Render a Row to RTF.
        /// </summary>
        internal override void Render()
        {
            _useEffectiveValue = true;
            _rtfWriter.WriteControl("trowd");
            new RowsRenderer(DocumentRelations.GetParent(_row) as Rows, _docRenderer).Render();
            RenderRowHeight();
            //MigraDoc always keeps together table rows.
            _rtfWriter.WriteControl("trkeep");
            Translate("HeadingFormat", "trhdr");

            // trkeepfollow is intended to keep table rows together.
            // Unfortunalte, this does not work in word.
            int thisRowIdx = _row.Index;
            for (int rowIdx = 0; rowIdx <= _row.Index; ++rowIdx)
            {
                object keepWith = _row.Table.Rows[rowIdx].GetValue("KeepWith");
                if (keepWith != null && (int)keepWith + rowIdx > thisRowIdx)
                    _rtfWriter.WriteControl("trkeepfollow");
            }
            RenderTopBottomPadding();

            //Cell borders etc. are written before the contents.
            for (int idx = 0; idx < _row.Table.Columns.Count; ++idx)
            {
                Cell cell = _row.Cells[idx];
                CellFormatRenderer cellFrmtRenderer =
                new CellFormatRenderer(cell, _docRenderer);
                cellFrmtRenderer.CellList = _cellList;
                cellFrmtRenderer.Render();
            }
            foreach (Cell cell in _row.Cells)
            {
                CellRenderer cellRndrr = new CellRenderer(cell, _docRenderer);
                cellRndrr.CellList = _cellList;
                cellRndrr.Render();
            }

            _rtfWriter.WriteControl("row");
        }

        private void RenderTopBottomPadding()
        {
            string rwPadCtrl = "trpadd";
            string rwPadUnit = "trpaddf";
            object rwPdgVal = _row.GetValue("TopPadding", GV.GetNull);
            if (rwPdgVal == null)
                rwPdgVal = Unit.FromCentimeter(0);

            //Word bug: Top and leftpadding are being confused in word.
            _rtfWriter.WriteControl(rwPadCtrl + "t", ToRtfUnit((Unit)rwPdgVal, RtfUnit.Twips));
            //Tells the RTF reader to take it as Twips:
            _rtfWriter.WriteControl(rwPadUnit + "t", 3);
            rwPdgVal = _row.GetValue("BottomPadding", GV.GetNull);
            if (rwPdgVal == null)
                rwPdgVal = Unit.FromCentimeter(0);

            _rtfWriter.WriteControl(rwPadCtrl + "b", ToRtfUnit((Unit)rwPdgVal, RtfUnit.Twips));
            _rtfWriter.WriteControl(rwPadUnit + "b", 3);
        }
        private void RenderRowHeight()
        {
            object heightObj = GetValueAsIntended("Height");
            object heightRlObj = GetValueAsIntended("HeightRule");
            if (heightRlObj != null)
            {
                switch ((RowHeightRule)heightRlObj)
                {
                    case RowHeightRule.AtLeast:
                        Translate("Height", "trrh", RtfUnit.Twips, "0", false);
                        break;
                    case RowHeightRule.Auto:
                        _rtfWriter.WriteControl("trrh", 0);
                        break;

                    case RowHeightRule.Exactly:
                        if (heightObj != null)
                            RenderUnit("trrh", -((Unit)heightObj).Point);
                        break;
                }
            }
            else
                Translate("Height", "trrh", RtfUnit.Twips, "0", false); //treat it like "AtLeast".
        }

        /// <summary>
        /// Sets the merged cell list. This property is set by the table renderer.
        /// </summary>
        internal MergedCellList CellList
        {
            set
            {
                _cellList = value;
            }
        }
        MergedCellList _cellList = null;
        readonly Row _row;
    }
}
