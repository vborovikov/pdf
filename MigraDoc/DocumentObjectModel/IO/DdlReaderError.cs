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

namespace MigraDoc.DocumentObjectModel.IO
{
    /// <summary>
    /// Represents an error or diagnostic message reported by the DDL reader.
    /// </summary>
    public class DdlReaderError
    {
        /// <summary>
        /// Initializes a new instance of the DdlReaderError class.
        /// </summary>
        public DdlReaderError(DdlErrorLevel errorLevel, string errorMessage, int errorNumber,
          string sourceFile, int sourceLine, int sourceColumn)
        {
            ErrorLevel = errorLevel;
            ErrorMessage = errorMessage;
            ErrorNumber = errorNumber;
            SourceFile = sourceFile;
            SourceLine = sourceLine;
            SourceColumn = sourceColumn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DdlReaderError"/> class.
        /// </summary>
        public DdlReaderError(DdlErrorLevel errorLevel, string errorMessage, int errorNumber)
        {
            ErrorLevel = errorLevel;
            ErrorMessage = errorMessage;
            ErrorNumber = errorNumber;
        }

        //    public DdlReaderError(string errorName, DdlReaderError _level, DomMsgID _error, string message, string msg2,
        //      string DocumentFileName, int CurrentLine, int CurrentLinePos)
        //    {
        //    }
        //
        //    public DdlReaderError(string errorName, int _level, string _error, string message, string adf,
        //      string  DocumentFileName,  int CurrentLine, int CurrentLinePos)
        //    {
        //    }
        //
        //    public DdlReaderError(string errorName, DdlErrorLevel errorInfo , string _error, string message, string adf,
        //      string  DocumentFileName,  int CurrentLine, int CurrentLinePos)
        //    {
        //    }
        //
        //    public DdlReaderError(string errorName, DdlErrorLevel errorInfo , DomMsgID _error, string message, string adf,
        //      string  DocumentFileName,  int CurrentLine, int CurrentLinePos)
        //    {
        //    }

        //public const int NoErrorNumber = -1;

        /// <summary>
        /// Returns a String that represents the current DdlReaderError.
        /// </summary>
        public override string ToString()
        {
            return String.Format("[{0}({1},{2}):] {3} DDL{4}: {5}",
              SourceFile, SourceLine, SourceColumn, "xxx", ErrorNumber, ErrorMessage);
        }

        /// <summary>
        /// Specifies the severity of this diagnostic.
        /// </summary>
        public readonly DdlErrorLevel ErrorLevel;

        /// <summary>
        /// Specifies the diagnostic message text.
        /// </summary>
        public readonly string ErrorMessage;

        /// <summary>
        /// Specifies the diagnostic number.
        /// </summary>
        public readonly int ErrorNumber;

        /// <summary>
        /// Specifies the filename of the DDL text that caused the diagnostic,
        /// or an empty string ("").
        /// </summary>
        public readonly string SourceFile;

        /// <summary>
        /// Specifies the line of the DDL text that caused the diagnostic (1 based),
        /// or 0 if there is no line information. 
        /// </summary>
        public readonly int SourceLine;

        /// <summary>
        /// Specifies the column of the source text that caused the diagnostic (1 based),
        /// or 0 if there is no column information.
        /// </summary>
        public readonly int SourceColumn;
    }
}
