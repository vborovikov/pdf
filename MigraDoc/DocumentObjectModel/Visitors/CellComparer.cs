#region MigraDoc - Creating Documents on the Fly
//
// Authors:
//   Stefan Lange
//   Klaus Potzesny
//   David Stephensen
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

namespace MigraDoc.DocumentObjectModel.Visitors
{
    /// <summary>
    /// Comparer for the cell positions within a table.
    /// It compares the cell positions from top to bottom and left to right.
    /// </summary>
    public class CellComparer : IComparer<Cell>
    {
        // AG_HACK
        //public int Compare(object lhs, object rhs)
        //{
        //  if (!(lhs is Cell))
        //    throw new ArgumentException(DomSR.CompareJustCells, "lhs");

        //  if (!(rhs is Cell))
        //    throw new ArgumentException(DomSR.CompareJustCells, "rhs");

        //  Cell cellLhs = lhs as Cell;
        //  Cell cellRhs = rhs as Cell;
        //  int rowCmpr = cellLhs.Row.Index - cellRhs.Row.Index;
        //  if (rowCmpr != 0)
        //    return rowCmpr;

        //  return cellLhs.Column.Index - cellRhs.Column.Index;
        //}

        //int IComparer<object>.Compare(object lhs, object rhs)
        //{
        //  if (!(lhs is Cell))
        //    throw new ArgumentException(DomSR.CompareJustCells, "lhs");

        //  if (!(rhs is Cell))
        //    throw new ArgumentException(DomSR.CompareJustCells, "rhs");

        //  Cell cellLhs = lhs as Cell;
        //  Cell cellRhs = rhs as Cell;
        //  int rowCmpr = cellLhs.Row.Index - cellRhs.Row.Index;
        //  if (rowCmpr != 0)
        //    return rowCmpr;

        //  return cellLhs.Column.Index - cellRhs.Column.Index;
        //}


        /// <summary>
        /// Compares the specified cells.
        /// </summary>
        /// <returns></returns>
        public int Compare(Cell cellLhs, Cell cellRhs)
        {
            int rowCmpr = cellLhs.Row.Index - cellRhs.Row.Index;
            if (rowCmpr != 0)
                return rowCmpr;

            return cellLhs.Column.Index - cellRhs.Column.Index;
        }
    }
}
