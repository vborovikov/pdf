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
    /// Renders formatted text to RTF.
    /// </summary>
    internal class FormattedTextRenderer : RendererBase
    {
        internal FormattedTextRenderer(DocumentObject domObj, RtfDocumentRenderer docRenderer)
            : base(domObj, docRenderer)
        {
            _formattedText = domObj as FormattedText;
        }

        /// <summary>
        /// Renders a formatted text to RTF.
        /// </summary>
        internal override void Render()
        {
            _useEffectiveValue = true;

            _rtfWriter.StartContent();
            RenderStyleAndFont();
            foreach (DocumentObject docObj in _formattedText.Elements)
                RendererFactory.CreateRenderer(docObj, _docRenderer).Render();

            _rtfWriter.EndContent();
        }

        /// <summary>
        /// Renders the style if it is a character style and the font of the formatted text.
        /// </summary>
        void RenderStyleAndFont()
        {
            bool hasCharacterStyle = false;
            if (!_formattedText.IsNull("Style"))
            {
                Style style = _formattedText.Document.Styles[_formattedText.Style];
                if (style != null && style.Type == StyleType.Character)
                    hasCharacterStyle = true;
            }
            object font = GetValueAsIntended("Font");
            if (font != null)
            {
                if (hasCharacterStyle)
                    _rtfWriter.WriteControlWithStar("cs", _docRenderer.GetStyleIndex(_formattedText.Style));

                RendererFactory.CreateRenderer(_formattedText.Font, _docRenderer).Render();
            }
        }

        private readonly FormattedText _formattedText;
    }
}
