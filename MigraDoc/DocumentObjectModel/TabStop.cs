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

using MigraDoc.DocumentObjectModel.Internals;

namespace MigraDoc.DocumentObjectModel
{
    /// <summary>
    /// Represents a tab inside a paragraph.
    /// </summary>
    public class TabStop : DocumentObject
    {
        /// <summary>
        /// Initializes a new instance of the TabStop class.
        /// </summary>
        public TabStop()
        { }

        /// <summary>
        /// Initializes a new instance of the TabStop class with the specified parent.
        /// </summary>
        internal TabStop(DocumentObject parent) : base(parent) { }

        /// <summary>
        /// Initializes a new instance of the TabStop class with the specified position.
        /// </summary>
        public TabStop(Unit position)
            : this()
        {
            _position = position;
        }

        #region Methods
        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        public new TabStop Clone()
        {
            return (TabStop)DeepCopy();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the tab stop position.
        /// </summary>
        public Unit Position
        {
            get { return _position; }
        }
        [DV]
        internal Unit _position = Unit.NullValue;  // always defined
        // useful enhancement: 'Position = Center' and 'Position = Right'

        /// <summary>
        /// Gets or sets the alignment of the tabstop.
        /// </summary>
        public TabAlignment Alignment
        {
            get { return (TabAlignment)_alignment.Value; }
            set { _alignment.Value = (int)value; }
        }
        [DV(Type = typeof(TabAlignment))]
        internal NEnum _alignment = NEnum.NullValue(typeof(TabAlignment));

        /// <summary>
        /// Gets or sets the character which is used as a leader for the tabstop.
        /// </summary>
        public TabLeader Leader
        {
            get { return (TabLeader)_leader.Value; }
            set { _leader.Value = (int)value; }
        }
        [DV(Type = typeof(TabLeader))]
        internal NEnum _leader = NEnum.NullValue(typeof(TabLeader));

        /// <summary>
        /// Generates a '+=' in DDL if it is true, otherwise '-='.
        /// </summary>
        internal bool AddTab = true;
        #endregion

        #region Internal
        /// <summary>
        /// Converts TabStop into DDL.
        /// </summary>
        internal override void Serialize(Serializer serializer)
        {
            if (AddTab)
            {
                serializer.WriteLine("TabStops +=");
                serializer.BeginContent();
                serializer.WriteSimpleAttribute("Position", Position);
                if (!_alignment.IsNull)
                    serializer.WriteSimpleAttribute("Alignment", Alignment);
                if (!_leader.IsNull)
                    serializer.WriteSimpleAttribute("Leader", Leader);
                serializer.EndContent();
            }
            else
                serializer.WriteLine("TabStops -= \"" + Position + "\"");
        }

        /// <summary>
        /// Returns the meta object of this instance.
        /// </summary>
        internal override Meta Meta
        {
            get { return _meta ?? (_meta = new Meta(typeof(TabStop))); }
        }
        static Meta _meta;
        #endregion
    }
}
