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

using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Internals;

namespace MigraDoc.DocumentObjectModel.Visitors
{
    /// <summary>
    /// Represents a merged list of cells of a table.
    /// </summary>
    public class MergedCellList : List<Cell>
    {
        /// <summary>
        /// Enumeration of neighbor positions of cells in a table.
        /// </summary>
        enum NeighborPosition
        {
            Top,
            Left,
            Right,
            Bottom
        }

        /// <summary>
        /// Initializes a new instance of the MergedCellList class.
        /// </summary>
        public MergedCellList(Table table)
        {
            Init(table);
        }

        /// <summary>
        /// Initializes this instance from a table.
        /// </summary>
        private void Init(Table table)
        {
            for (int rwIdx = 0; rwIdx < table.Rows.Count; ++rwIdx)
            {
                for (int clmIdx = 0; clmIdx < table.Columns.Count; ++clmIdx)
                {
                    Cell cell = table[rwIdx, clmIdx];
                    if (!IsAlreadyCovered(cell))
                        Add(cell);
                }
            }
        }

        /// <summary>
        /// Returns whether the given cell is already covered by a preceding cell in this instance.
        /// </summary>
        /// <remarks>
        /// Help function for Init().
        /// </remarks>
        private bool IsAlreadyCovered(Cell cell)
        {
            for (int index = Count - 1; index >= 0; --index)
            {

                Cell currentCell = this[index];
                if (currentCell.Column.Index <= cell.Column.Index && currentCell.Column.Index + currentCell.MergeRight >= cell.Column.Index)
                {
                    if (currentCell.Row.Index <= cell.Row.Index && currentCell.Row.Index + currentCell.MergeDown >= cell.Row.Index)
                        return true;

                    if (currentCell.Row.Index + currentCell.MergeDown == cell.Row.Index - 1)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the cell at the specified position.
        /// </summary>
        public new Cell this[int index]
        {
            get { return base[index]; }
        }

        /// <summary>
        /// Gets a borders object that should be used for rendering.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when the cell is not in this list.
        ///   This situation occurs if the given cell is merged "away" by a previous one.
        /// </exception>
        public Borders GetEffectiveBorders(Cell cell)
        {
            //Borders borders = cell.GetValue("Borders", GV.ReadOnly) as Borders;
            Borders borders = cell._borders;
            if (borders != null)
            {
                //Document doc = borders.Document;
                borders = borders.Clone();
                borders._parent = cell;
                //doc = borders.Document;
            }
            else
                borders = new Borders(cell._parent);

            int cellIdx = BinarySearch(cell, new CellComparer());
            if (!(cellIdx >= 0 && cellIdx < Count))
                throw new ArgumentException("cell is not a relevant cell", "cell");

            if (cell._mergeRight > 0)
            {
                Cell rightBorderCell = cell.Table[cell.Row.Index, cell.Column.Index + cell._mergeRight];
                if (rightBorderCell._borders != null && rightBorderCell._borders._right != null)
                    borders.Right = rightBorderCell._borders._right.Clone();
                else
                    borders._right = null;
            }

            if (cell._mergeDown > 0)
            {
                Cell bottomBorderCell = cell.Table[cell.Row.Index + cell._mergeDown, cell.Column.Index];
                if (bottomBorderCell._borders != null && bottomBorderCell._borders._bottom != null)
                    borders.Bottom = bottomBorderCell._borders._bottom.Clone();
                else
                    borders._bottom = null;
            }

            // For BorderTypes Top, Right, Bottom and Left update the width with the neighbours touching border where required.
            // In case of rounded corners this should not be done.

            Cell leftNeighbor = GetNeighbor(cellIdx, NeighborPosition.Left);
            if (leftNeighbor != null && leftNeighbor.RoundedCorner != RoundedCorner.TopRight && leftNeighbor.RoundedCorner != RoundedCorner.BottomRight)
            {
                Borders nbrBrdrs = leftNeighbor.GetValue("Borders", GV.ReadWrite) as Borders;
                if (nbrBrdrs != null && GetEffectiveBorderWidth(nbrBrdrs, BorderType.Right) >= GetEffectiveBorderWidth(borders, BorderType.Left))
                    borders.SetValue("Left", GetBorderFromBorders(nbrBrdrs, BorderType.Right));
            }

            Cell rightNeighbor = GetNeighbor(cellIdx, NeighborPosition.Right);
            if (rightNeighbor != null && rightNeighbor.RoundedCorner != RoundedCorner.TopLeft && rightNeighbor.RoundedCorner != RoundedCorner.BottomLeft)
            {
                Borders nbrBrdrs = rightNeighbor.GetValue("Borders", GV.ReadWrite) as Borders;
                if (nbrBrdrs != null && GetEffectiveBorderWidth(nbrBrdrs, BorderType.Left) > GetEffectiveBorderWidth(borders, BorderType.Right))
                    borders.SetValue("Right", GetBorderFromBorders(nbrBrdrs, BorderType.Left));
            }

            Cell topNeighbor = GetNeighbor(cellIdx, NeighborPosition.Top);
            if (topNeighbor != null && topNeighbor.RoundedCorner != RoundedCorner.BottomLeft && topNeighbor.RoundedCorner != RoundedCorner.BottomRight)
            {
                Borders nbrBrdrs = topNeighbor.GetValue("Borders", GV.ReadWrite) as Borders;
                if (nbrBrdrs != null && GetEffectiveBorderWidth(nbrBrdrs, BorderType.Bottom) >= GetEffectiveBorderWidth(borders, BorderType.Top))
                    borders.SetValue("Top", GetBorderFromBorders(nbrBrdrs, BorderType.Bottom));
            }

            Cell bottomNeighbor = GetNeighbor(cellIdx, NeighborPosition.Bottom);
            if (bottomNeighbor != null && bottomNeighbor.RoundedCorner != RoundedCorner.TopLeft && bottomNeighbor.RoundedCorner != RoundedCorner.TopRight)
            {
                Borders nbrBrdrs = bottomNeighbor.GetValue("Borders", GV.ReadWrite) as Borders;
                if (nbrBrdrs != null && GetEffectiveBorderWidth(nbrBrdrs, BorderType.Top) > GetEffectiveBorderWidth(borders, BorderType.Bottom))
                    borders.SetValue("Bottom", GetBorderFromBorders(nbrBrdrs, BorderType.Top));
            }
            return borders;
        }

        /// <summary>
        /// Gets the cell that covers the given cell by merging. Usually the cell itself if not merged.
        /// </summary>
        public Cell GetCoveringCell(Cell cell)
        {
            int cellIdx = BinarySearch(cell, new CellComparer());
            if (cellIdx >= 0 && cellIdx < Count)
                return this[cellIdx];

            // Binary Search returns the complement of the next value, therefore, "~cellIdx - 1" is the previous cell.
            cellIdx = ~cellIdx - 1;

            for (int index = cellIdx; index >= 0; --index)
            {
                Cell currCell = this[index];
                if (currCell.Column.Index <= cell.Column.Index &&
                  currCell.Column.Index + currCell.MergeRight >= cell.Column.Index &&
                  currCell.Row.Index <= cell.Row.Index &&
                  currCell.Row.Index + currCell.MergeDown >= cell.Row.Index)
                    return currCell;
            }
            return null;
        }

        /// <summary>
        /// Returns the border of the given borders-object of the specified type (top, bottom, ...).
        /// If that border doesn't exist, it returns a new border object that inherits all properties from the given borders object
        /// </summary>
        private static Border GetBorderFromBorders(Borders borders, BorderType type)
        {
            Border returnBorder = borders.GetBorderReadOnly(type);
            if (returnBorder == null)
            {
                returnBorder = new Border();
                returnBorder._style = borders._style;
                returnBorder._width = borders._width;
                returnBorder._color = borders._color;
                returnBorder._visible = borders._visible;
            }
            return returnBorder;
        }

        /// <summary>
        /// Returns the width of the border at the specified position.
        /// </summary>
        private static Unit GetEffectiveBorderWidth(Borders borders, BorderType type)
        {
            if (borders == null)
                return 0;

            Border border = borders.GetBorderReadOnly(type);

            DocumentObject relevantDocObj = border;
            if (relevantDocObj == null || relevantDocObj.IsNull("Width"))
                relevantDocObj = borders;

            // Avoid unnecessary GetValue calls.
            object visible = relevantDocObj.GetValue("visible", GV.GetNull);
            if (visible != null && !(bool)visible)
                return 0;

            object width = relevantDocObj.GetValue("width", GV.GetNull);
            if (width != null)
                return (Unit)width;

            object color = relevantDocObj.GetValue("color", GV.GetNull);
            if (color != null)
                return 0.5;

            object style = relevantDocObj.GetValue("style", GV.GetNull);
            if (style != null)
                return 0.5;
            return 0;
        }

        /// <summary>
        /// Gets the specified cell's uppermost neighbor at the specified position.
        /// </summary>
        private Cell GetNeighbor(int cellIdx, NeighborPosition position)
        {
            Cell cell = this[cellIdx];
            if (cell.Column.Index == 0 && position == NeighborPosition.Left ||
              cell.Row.Index == 0 && position == NeighborPosition.Top ||
              cell.Row.Index + cell.MergeDown == cell.Table.Rows.Count - 1 && position == NeighborPosition.Bottom ||
              cell.Column.Index + cell.MergeRight == cell.Table.Columns.Count - 1 && position == NeighborPosition.Right)
                return null;

            switch (position)
            {
                case NeighborPosition.Top:
                case NeighborPosition.Left:
                    for (int index = cellIdx - 1; index >= 0; --index)
                    {
                        Cell currCell = this[index];
                        if (IsNeighbor(cell, currCell, position))
                            return currCell;
                    }
                    break;

                case NeighborPosition.Right:
                    if (cellIdx + 1 < Count)
                    {
                        Cell cell2 = this[cellIdx + 1];
                        if (cell2.Row.Index == cell.Row.Index)
                            return cell2;
                    }
                    for (int index = cellIdx - 1; index >= 0; --index)
                    {
                        Cell currCell = this[index];
                        if (IsNeighbor(cell, currCell, position))
                            return currCell;
                    }
                    break;

                case NeighborPosition.Bottom:
                    for (int index = cellIdx + 1; index < Count; ++index)
                    {
                        Cell currCell = this[index];
                        if (IsNeighbor(cell, currCell, position))
                            return currCell;
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Returns whether cell2 is a neighbor of cell1 at the specified position.
        /// </summary>
        private bool IsNeighbor(Cell cell1, Cell cell2, NeighborPosition position)
        {
            bool isNeighbor = false;
            switch (position)
            {
                case NeighborPosition.Bottom:
                    int bottomRowIdx = cell1.Row.Index + cell1.MergeDown + 1;
                    isNeighbor = cell2.Row.Index == bottomRowIdx &&
                      cell2.Column.Index <= cell1.Column.Index &&
                      cell2.Column.Index + cell2.MergeRight >= cell1.Column.Index;
                    break;

                case NeighborPosition.Left:
                    int leftClmIdx = cell1.Column.Index - 1;
                    isNeighbor = cell2.Row.Index <= cell1.Row.Index &&
                      cell2.Row.Index + cell2.MergeDown >= cell1.Row.Index &&
                      cell2.Column.Index + cell2.MergeRight == leftClmIdx;
                    break;

                case NeighborPosition.Right:
                    int rightClmIdx = cell1.Column.Index + cell1.MergeRight + 1;
                    isNeighbor = cell2.Row.Index <= cell1.Row.Index &&
                      cell2.Row.Index + cell2.MergeDown >= cell1.Row.Index &&
                      cell2.Column.Index == rightClmIdx;
                    break;

                case NeighborPosition.Top:
                    int topRowIdx = cell1.Row.Index - 1;
                    isNeighbor = cell2.Row.Index + cell2.MergeDown == topRowIdx &&
                      cell2.Column.Index + cell2.MergeRight >= cell1.Column.Index &&
                      cell2.Column.Index <= cell1.Column.Index;
                    break;
            }
            return isNeighbor;
        }
    }
}
