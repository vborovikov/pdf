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

namespace MigraDoc.DocumentObjectModel.Shapes
{
    /// <summary>
    /// Base Class for all positionable Classes.
    /// </summary>
    public class Shape : DocumentObject
    {
        /// <summary>
        /// Initializes a new instance of the Shape class.
        /// </summary>
        public Shape()
        { }

        /// <summary>
        /// Initializes a new instance of the Shape class with the specified parent.
        /// </summary>
        internal Shape(DocumentObject parent) : base(parent) { }

        #region Methods
        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        public new Shape Clone()
        {
            return (Shape)DeepCopy();
        }

        /// <summary>
        /// Implements the deep copy of the object.
        /// </summary>
        protected override object DeepCopy()
        {
            Shape shape = (Shape)base.DeepCopy();
            if (shape._wrapFormat != null)
            {
                shape._wrapFormat = shape._wrapFormat.Clone();
                shape._wrapFormat._parent = shape;
            }
            if (shape._lineFormat != null)
            {
                shape._lineFormat = shape._lineFormat.Clone();
                shape._lineFormat._parent = shape;
            }
            if (shape._fillFormat != null)
            {
                shape._fillFormat = shape._fillFormat.Clone();
                shape._fillFormat._parent = shape;
            }
            return shape;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the wrapping format of the shape.
        /// </summary>
        public WrapFormat WrapFormat
        {
            get { return _wrapFormat ?? (_wrapFormat = new WrapFormat(this)); }
            set
            {
                SetParent(value);
                _wrapFormat = value;
            }
        }
        [DV]
        internal WrapFormat _wrapFormat;

        /// <summary>
        /// Gets or sets the reference point of the Top property.
        /// </summary>
        public RelativeVertical RelativeVertical
        {
            get { return (RelativeVertical)_relativeVertical.Value; }
            set { _relativeVertical.Value = (int)value; }
        }
        [DV(Type = typeof(RelativeVertical))]
        internal NEnum _relativeVertical = NEnum.NullValue(typeof(RelativeVertical));

        /// <summary>
        /// Gets or sets the reference point of the Left property.
        /// </summary>
        public RelativeHorizontal RelativeHorizontal
        {
            get { return (RelativeHorizontal)_relativeHorizontal.Value; }
            set { _relativeHorizontal.Value = (int)value; }
        }
        [DV(Type = typeof(RelativeHorizontal))]
        internal NEnum _relativeHorizontal = NEnum.NullValue(typeof(RelativeHorizontal));

        /// <summary>
        /// Gets or sets the position of the top side of the shape.
        /// </summary>
        public TopPosition Top
        {
            get { return _top; }
            set { _top = value; }
        }
        [DV]
        internal TopPosition _top = TopPosition.NullValue;

        /// <summary>
        /// Gets or sets the position of the left side of the shape.
        /// </summary>
        public LeftPosition Left
        {
            get { return _left; }
            set { _left = value; }
        }
        [DV]
        internal LeftPosition _left = LeftPosition.NullValue;

        /// <summary>
        /// Gets the line format of the shape's border.
        /// </summary>
        public LineFormat LineFormat
        {
            get { return _lineFormat ?? (_lineFormat = new LineFormat(this)); }
            set
            {
                SetParent(value);
                _lineFormat = value;
            }
        }
        [DV]
        internal LineFormat _lineFormat;

        /// <summary>
        /// Gets the background filling format of the shape.
        /// </summary>
        public FillFormat FillFormat
        {
            get { return _fillFormat ?? (_fillFormat = new FillFormat(this)); }
            set
            {
                SetParent(value);
                _fillFormat = value;
            }
        }
        [DV]
        internal FillFormat _fillFormat;

        /// <summary>
        /// Gets or sets the height of the shape.
        /// </summary>
        public Unit Height
        {
            get { return _height; }
            set { _height = value; }
        }
        [DV]
        internal Unit _height = Unit.NullValue;

        /// <summary>
        /// Gets or sets the width of the shape.
        /// </summary>
        public Unit Width
        {
            get { return _width; }
            set { _width = value; }
        }
        [DV]
        internal Unit _width = Unit.NullValue;
        #endregion

        #region Internal
        /// <summary>
        /// Converts Shape into DDL.
        /// </summary>
        internal override void Serialize(Serializer serializer)
        {
            if (!_height.IsNull)
                serializer.WriteSimpleAttribute("Height", Height);
            if (!_width.IsNull)
                serializer.WriteSimpleAttribute("Width", Width);
            if (!_relativeHorizontal.IsNull)
                serializer.WriteSimpleAttribute("RelativeHorizontal", RelativeHorizontal);
            if (!_relativeVertical.IsNull)
                serializer.WriteSimpleAttribute("RelativeVertical", RelativeVertical);
            if (!IsNull("Left"))
                _left.Serialize(serializer);
            if (!IsNull("Top"))
                _top.Serialize(serializer);
            if (!IsNull("WrapFormat"))
                _wrapFormat.Serialize(serializer);
            if (!IsNull("LineFormat"))
                _lineFormat.Serialize(serializer);
            if (!IsNull("FillFormat"))
                _fillFormat.Serialize(serializer);
        }

        /// <summary>
        /// Returns the meta object of this instance.
        /// </summary>
        internal override Meta Meta
        {
            get { return _meta ?? (_meta = new Meta(typeof(Shape))); }
        }
        static Meta _meta;
        #endregion
    }
}
