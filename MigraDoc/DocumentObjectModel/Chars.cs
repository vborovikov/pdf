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

#pragma warning disable 1591

namespace MigraDoc.DocumentObjectModel
{
    /// <summary>
    /// Character table by name.
    /// </summary>
    public sealed class Chars
    {
        // ReSharper disable InconsistentNaming
        public const char Null = '\0';   // EOF
        public const char CR = '\x0D'; // ignored by scanner
        public const char LF = '\x0A';
        public const char BEL = '\a';   // Bell
        public const char BS = '\b';   // Backspace
        public const char FF = '\f';   // Formfeed
        public const char HT = '\t';   // Horizontal tab
        public const char VT = '\v';   // Vertical tab
        public const char NonBreakableSpace = (char)160;  // char(160)
        // ReSharper restore InconsistentNaming

        // The following names come from "PDF Reference Third Edition"
        // Appendix D.1, Latin Character Set and Encoding
        public const char Space = ' ';
        public const char QuoteDbl = '"';
        public const char QuoteSingle = '\'';
        public const char ParenLeft = '(';
        public const char ParenRight = ')';
        public const char BraceLeft = '{';
        public const char BraceRight = '}';
        public const char BracketLeft = '[';
        public const char BracketRight = ']';
        public const char Less = '<';
        public const char Greater = '>';
        public const char Equal = '=';
        public const char Period = '.';
        public const char Semicolon = ';';
        public const char Colon = ':';
        public const char Slash = '/';
        public const char Bar = '|';
        public const char BackSlash = '\\';
        public const char Percent = '%';
        public const char Dollar = '$';
        public const char At = '@';
        public const char NumberSign = '#';
        public const char Question = '?';
        public const char Hyphen = '-';  // char(45)
        public const char SoftHyphen = '�';  // char(173)
        public const char Currency = '�';
    }
}
