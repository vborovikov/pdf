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

using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Drawing;

namespace MigraDoc.Rendering
{
    /// <summary>
    /// Formats a series of document elements from top to bottom.
    /// </summary>
    internal class TopDownFormatter
    {
        /// <summary>
        /// Returns the max of the given Margins, if both are positive or 0, the sum otherwise.
        /// </summary>
        /// <param name="prevBottomMargin">The bottom margin of the previous element.</param>
        /// <param name="nextTopMargin">The top margin of the next element.</param>
        /// <returns></returns>
        private XUnit MarginMax(XUnit prevBottomMargin, XUnit nextTopMargin)
        {
            if (prevBottomMargin >= 0 && nextTopMargin >= 0)
                return Math.Max(prevBottomMargin, nextTopMargin);
            return prevBottomMargin + nextTopMargin;
        }

        internal TopDownFormatter(IAreaProvider areaProvider, DocumentRenderer documentRenderer, DocumentElements elements)
        {
            _documentRenderer = documentRenderer;
            _areaProvider = areaProvider;
            _elements = elements;
        }

        readonly IAreaProvider _areaProvider;

        readonly DocumentElements _elements;

        /// <summary>
        /// Formats the elements on the areas provided by the area provider.
        /// </summary>
        /// <param name="gfx">The graphics object to render on.</param>
        /// <param name="topLevel">if set to <c>true</c> formats the object is on top level.</param>
        public void FormatOnAreas(XGraphics gfx, bool topLevel)
        {
            _gfx = gfx;
            XUnit prevBottomMargin = 0;
            XUnit yPos = prevBottomMargin;
            RenderInfo prevRenderInfo = null;
            FormatInfo prevFormatInfo = null;
            List<RenderInfo> renderInfos = new List<RenderInfo>();
            bool ready = _elements.Count == 0;
            bool isFirstOnPage = true;
            Area area = _areaProvider.GetNextArea();
            XUnit maxHeight = area.Height;
            if (ready)
            {
                _areaProvider.StoreRenderInfos(renderInfos);
                return;
            }
            int idx = 0;
            while (!ready && area != null)
            {
                DocumentObject docObj = _elements[idx];
                Renderer renderer = Renderer.Create(gfx, _documentRenderer, docObj, _areaProvider.AreaFieldInfos);
                if (renderer != null) // "Slightly hacked" for legends: see below
                    renderer.MaxElementHeight = maxHeight;

                if (topLevel && _documentRenderer.HasPrepareDocumentProgress)
                {
                    _documentRenderer.OnPrepareDocumentProgress(_documentRenderer.ProgressCompleted + idx + 1,
                      _documentRenderer.ProgressMaximum);
                }

                // "Slightly hacked" for legends: they are rendered as part of the chart.
                // So they are skipped here.
                if (renderer == null)
                {
                    ready = idx == _elements.Count - 1;
                    if (ready)
                        _areaProvider.StoreRenderInfos(renderInfos);
                    ++idx;
                    continue;
                }
                ///////////////////////////////////////////
                if (prevFormatInfo == null)
                {
                    LayoutInfo initialLayoutInfo = renderer.InitialLayoutInfo;
                    XUnit distance = prevBottomMargin;
                    if (initialLayoutInfo.VerticalReference == VerticalReference.PreviousElement &&
                        initialLayoutInfo.Floating != Floating.None)
                        distance = MarginMax(initialLayoutInfo.MarginTop, distance);

                    area = area.Lower(distance);
                }
                renderer.Format(area, prevFormatInfo);
                _areaProvider.PositionHorizontally(renderer.RenderInfo.LayoutInfo);
                bool pagebreakBefore = _areaProvider.IsAreaBreakBefore(renderer.RenderInfo.LayoutInfo) && !isFirstOnPage;
                pagebreakBefore = pagebreakBefore || !isFirstOnPage && IsForcedAreaBreak(idx, renderer, area);

                if (!pagebreakBefore && renderer.RenderInfo.FormatInfo.IsEnding)
                {
                    if (PreviousRendererNeedsRemoveEnding(prevRenderInfo, renderer.RenderInfo, area))
                    {
                        prevRenderInfo.RemoveEnding();
                        renderer = Renderer.Create(gfx, _documentRenderer, docObj, _areaProvider.AreaFieldInfos);
                        renderer.MaxElementHeight = maxHeight;
                        renderer.Format(area, prevRenderInfo.FormatInfo);
                    }
                    else if (NeedsEndingOnNextArea(idx, renderer, area, isFirstOnPage))
                    {
                        renderer.RenderInfo.RemoveEnding();
                        prevRenderInfo = FinishPage(renderer.RenderInfo, pagebreakBefore, ref renderInfos);
                        if (prevRenderInfo != null)
                            prevFormatInfo = prevRenderInfo.FormatInfo;
                        else
                        {
                            prevFormatInfo = null;
                            isFirstOnPage = true;
                        }
                        prevBottomMargin = 0;
                        area = _areaProvider.GetNextArea();
                        maxHeight = area.Height;
                    }
                    else
                    {
                        renderInfos.Add(renderer.RenderInfo);
                        isFirstOnPage = false;
                        _areaProvider.PositionVertically(renderer.RenderInfo.LayoutInfo);
                        if (renderer.RenderInfo.LayoutInfo.VerticalReference == VerticalReference.PreviousElement
                            && renderer.RenderInfo.LayoutInfo.Floating != Floating.None)
                        {
                            prevBottomMargin = renderer.RenderInfo.LayoutInfo.MarginBottom;
                            if (renderer.RenderInfo.LayoutInfo.Floating != Floating.None)
                                area = area.Lower(renderer.RenderInfo.LayoutInfo.ContentArea.Height);
                        }
                        else
                            prevBottomMargin = 0;

                        prevFormatInfo = null;
                        prevRenderInfo = null;

                        ++idx;
                    }
                }
                else
                {
                    if (renderer.RenderInfo.FormatInfo.IsEmpty && isFirstOnPage)
                    {
                        area = area.Unite(new Rectangle(area.X, area.Y, area.Width, double.MaxValue));

                        renderer = Renderer.Create(gfx, _documentRenderer, docObj, _areaProvider.AreaFieldInfos);
                        renderer.MaxElementHeight = maxHeight;
                        renderer.Format(area, prevFormatInfo);
                        prevFormatInfo = null;

                        _areaProvider.PositionHorizontally(renderer.RenderInfo.LayoutInfo);
                        _areaProvider.PositionVertically(renderer.RenderInfo.LayoutInfo);

                        ready = idx == _elements.Count - 1;

                        ++idx;
                    }
                    prevRenderInfo = FinishPage(renderer.RenderInfo, pagebreakBefore, ref renderInfos);
                    if (prevRenderInfo != null)
                        prevFormatInfo = prevRenderInfo.FormatInfo;
                    else
                    {
                        prevFormatInfo = null;
                    }
                    isFirstOnPage = true;
                    prevBottomMargin = 0;

                    if (!ready)
                    {
                        area = _areaProvider.GetNextArea();
                        maxHeight = area.Height;
                    }

                }
                if (idx == _elements.Count && !ready)
                {
                    _areaProvider.StoreRenderInfos(renderInfos);
                    ready = true;
                }
            }
        }

        /// <summary>
        /// Finishes rendering for the page.
        /// </summary>
        /// <param name="lastRenderInfo">The last render info.</param>
        /// <param name="pagebreakBefore">set to <c>true</c> if there is a pagebreak before this page.</param>
        /// <param name="renderInfos">The render infos.</param>
        /// <returns>
        /// The RenderInfo to set as previous RenderInfo.
        /// </returns>
        RenderInfo FinishPage(RenderInfo lastRenderInfo, bool pagebreakBefore, ref List<RenderInfo> renderInfos)
        {
            RenderInfo prevRenderInfo;
            if (lastRenderInfo.FormatInfo.IsEmpty || pagebreakBefore)
            {
                prevRenderInfo = null;
            }
            else
            {
                prevRenderInfo = lastRenderInfo;
                renderInfos.Add(lastRenderInfo);
                if (lastRenderInfo.FormatInfo.IsEnding)
                    prevRenderInfo = null;
            }
            _areaProvider.StoreRenderInfos(renderInfos);
            renderInfos = new List<RenderInfo>();
            return prevRenderInfo;
        }

        /// <summary>
        /// Indicates that a break between areas has to be performed before the element with the given idx.
        /// </summary>
        /// <param name="idx">Index of the document element.</param>
        /// <param name="renderer">A formatted renderer for the document element.</param>
        /// <param name="remainingArea">The remaining area.</param>
        bool IsForcedAreaBreak(int idx, Renderer renderer, Area remainingArea)
        {
            FormatInfo formatInfo = renderer.RenderInfo.FormatInfo;
            LayoutInfo layoutInfo = renderer.RenderInfo.LayoutInfo;

            if (formatInfo.IsStarting && !formatInfo.StartingIsComplete)
                return true;

            if (layoutInfo.KeepTogether && !formatInfo.IsComplete)
                return true;

            if (layoutInfo.KeepTogether && layoutInfo.KeepWithNext)
            {
                Area area = remainingArea.Lower(layoutInfo.ContentArea.Height);
                return NextElementsDontFit(idx, area, layoutInfo.MarginBottom);
            }
            return false;
        }

        /// <summary>
        /// Indicates that the Ending of the element has to be removed.
        /// </summary>
        /// <param name="prevRenderInfo">The prev render info.</param>
        /// <param name="succedingRenderInfo">The succeding render info.</param>
        /// <param name="remainingArea">The remaining area.</param>
        bool PreviousRendererNeedsRemoveEnding(RenderInfo prevRenderInfo, RenderInfo succedingRenderInfo, Area remainingArea)
        {
            if (prevRenderInfo == null)
                return false;
            LayoutInfo layoutInfo = succedingRenderInfo.LayoutInfo;
            FormatInfo formatInfo = succedingRenderInfo.FormatInfo;
            LayoutInfo prevLayoutInfo = prevRenderInfo.LayoutInfo;
            if (formatInfo.IsEnding && !formatInfo.EndingIsComplete)
            {
                Area area = _areaProvider.ProbeNextArea();
                if (area.Height > prevLayoutInfo.TrailingHeight + layoutInfo.TrailingHeight + Renderer.Tolerance)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// The maximum number of elements that can be combined via keepwithnext and keeptogether
        /// </summary>
        internal static readonly int MaxCombineElements = 10;
        bool NextElementsDontFit(int idx, Area remainingArea, XUnit previousMarginBottom)
        {
            XUnit elementDistance = previousMarginBottom;
            Area area = remainingArea;
            for (int index = idx + 1; index < _elements.Count; ++index)
            {
                // Never combine more than MaxCombineElements elements
                if (index - idx > MaxCombineElements)
                    return false;

                DocumentObject obj = _elements[index];
                Renderer currRenderer = Renderer.Create(_gfx, _documentRenderer, obj, _areaProvider.AreaFieldInfos);
                elementDistance = MarginMax(elementDistance, currRenderer.InitialLayoutInfo.MarginTop);
                area = area.Lower(elementDistance);

                if (area.Height <= 0)
                    return true;

                currRenderer.Format(area, null);
                FormatInfo currFormatInfo = currRenderer.RenderInfo.FormatInfo;
                LayoutInfo currLayoutInfo = currRenderer.RenderInfo.LayoutInfo;

                if (currLayoutInfo.VerticalReference != VerticalReference.PreviousElement)
                    return false;

                if (!currFormatInfo.StartingIsComplete)
                    return true;

                if (currLayoutInfo.KeepTogether && !currFormatInfo.IsComplete)
                    return true;

                if (!(currLayoutInfo.KeepTogether && currLayoutInfo.KeepWithNext))
                    return false;

                area = area.Lower(currLayoutInfo.ContentArea.Height);
                if (area.Height <= 0)
                    return true;

                elementDistance = currLayoutInfo.MarginBottom;
            }
            return false;
        }

        bool NeedsEndingOnNextArea(int idx, Renderer renderer, Area remainingArea, bool isFirstOnPage)
        {
            LayoutInfo layoutInfo = renderer.RenderInfo.LayoutInfo;
            if (isFirstOnPage && layoutInfo.KeepTogether)
                return false;
            FormatInfo formatInfo = renderer.RenderInfo.FormatInfo;

            if (!formatInfo.EndingIsComplete)
                return false;

            if (layoutInfo.KeepWithNext)
            {
                remainingArea = remainingArea.Lower(layoutInfo.ContentArea.Height);
                return NextElementsDontFit(idx, remainingArea, layoutInfo.MarginBottom);
            }

            return false;
        }

        readonly DocumentRenderer _documentRenderer;
        XGraphics _gfx;
    }
}
