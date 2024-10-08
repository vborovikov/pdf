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

using System.Collections.Generic;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Visitors;
using PdfSharp.Drawing;

namespace MigraDoc.Rendering
{
    /// <summary>
    /// Formatting information for tables.
    /// </summary>
    public class TableFormatInfo : FormatInfo
    {
        internal TableFormatInfo()
        { }

        internal override bool EndingIsComplete
        {
            get { return _isEnding; }
        }

        internal override bool StartingIsComplete
        {
            get { return !IsEmpty && StartRow > LastHeaderRow; }
        }

        internal override bool IsComplete
        {
            get { return false; }
        }

        internal override bool IsEmpty
        {
            get { return StartRow < 0; }
        }

        internal override bool IsEnding
        {
            get { return _isEnding; }
        }
        internal bool _isEnding;

        internal override bool IsStarting
        {
            get { return StartRow == LastHeaderRow + 1; }
        }

        internal int StartColumn = -1;
        internal int EndColumn = -1;

        /// <summary>
        /// The first row of the table that is showing on a page.
        /// </summary>
        public int StartRow = -1;
        /// <summary>
        /// The last row of the table that is showing on a page.
        /// </summary>
        public int EndRow = -1;

        internal int LastHeaderRow = -1;
        /// <summary>
        /// The formatted cells.
        /// </summary>
        public Dictionary<Cell, FormattedCell> FormattedCells; //Sorted_List formattedCells;
        internal MergedCellList MergedCells;
        internal Dictionary<int, XUnit> BottomBorderMap; //Sorted_List bottomBorderMap;
        internal Dictionary<int, int> ConnectedRowsMap; //Sorted_List connectedRowsMap;
    }
}
