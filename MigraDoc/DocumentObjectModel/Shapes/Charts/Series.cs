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

namespace MigraDoc.DocumentObjectModel.Shapes.Charts
{
    /// <summary>
    /// Represents a series of data on the chart.
    /// </summary>
    public class Series : ChartObject
    {
        /// <summary>
        /// Initializes a new instance of the Series class.
        /// </summary>
        public Series()
        { }

        #region Methods
        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        public new Series Clone()
        {
            return (Series)DeepCopy();
        }

        /// <summary>
        /// Implements the deep copy of the object.
        /// </summary>
        protected override object DeepCopy()
        {
            Series series = (Series)base.DeepCopy();
            if (series._seriesElements != null)
            {
                series._seriesElements = series._seriesElements.Clone();
                series._seriesElements._parent = series;
            }
            if (series._lineFormat != null)
            {
                series._lineFormat = series._lineFormat.Clone();
                series._lineFormat._parent = series;
            }
            if (series._fillFormat != null)
            {
                series._fillFormat = series._fillFormat.Clone();
                series._fillFormat._parent = series;
            }
            if (series._dataLabel != null)
            {
                series._dataLabel = series._dataLabel.Clone();
                series._dataLabel._parent = series;
            }
            return series;
        }

        /// <summary>
        /// Adds a blank to the series.
        /// </summary>
        public void AddBlank()
        {
            Elements.AddBlank();
        }

        /// <summary>
        /// Adds a real value to the series.
        /// </summary>
        public Point Add(double value)
        {
            return Elements.Add(value);
        }

        /// <summary>
        /// Adds an array of real values to the series.
        /// </summary>
        public void Add(params double[] values)
        {
            Elements.Add(values);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The actual value container of the series.
        /// </summary>
        public SeriesElements Elements
        {
            get { return _seriesElements ?? (_seriesElements = new SeriesElements(this)); }
            set
            {
                SetParent(value);
                _seriesElements = value;
            }
        }
        [DV]
        internal SeriesElements _seriesElements;

        /// <summary>
        /// Gets or sets the name of the series which will be used in the legend.
        /// </summary>
        public string Name
        {
            get { return _name.Value; }
            set { _name.Value = value; }
        }
        [DV]
        internal NString _name = NString.NullValue;

        /// <summary>
        /// Gets the line format of the border of each data.
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
        /// Gets the background filling of the data.
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
        /// Gets or sets the size of the marker in a line chart.
        /// </summary>
        public Unit MarkerSize
        {
            get { return _markerSize; }
            set { _markerSize = value; }
        }
        [DV]
        internal Unit _markerSize = Unit.NullValue;

        /// <summary>
        /// Gets or sets the style of the marker in a line chart.
        /// </summary>
        public MarkerStyle MarkerStyle
        {
            get { return (MarkerStyle)_markerStyle.Value; }
            set { _markerStyle.Value = (int)value; }
        }
        [DV(Type = typeof(MarkerStyle))]
        internal NEnum _markerStyle = NEnum.NullValue(typeof(MarkerStyle));

        /// <summary>
        /// Gets or sets the foreground color of the marker in a line chart.
        /// </summary>
        public Color MarkerForegroundColor
        {
            get { return _markerForegroundColor; }
            set { _markerForegroundColor = value; }
        }
        [DV]
        internal Color _markerForegroundColor = Color.Empty;

        /// <summary>
        /// Gets or sets the background color of the marker in a line chart.
        /// </summary>
        public Color MarkerBackgroundColor
        {
            get { return _markerBackgroundColor; }
            set { _markerBackgroundColor = value; }
        }
        [DV]
        internal Color _markerBackgroundColor = Color.Empty;

        /// <summary>
        /// Gets or sets the chart type of the series if it's intended to be different than the global chart type.
        /// </summary>
        public ChartType ChartType
        {
            get { return (ChartType)_chartType.Value; }
            set { _chartType.Value = (int)value; }
        }
        [DV(Type = typeof(ChartType))]
        internal NEnum _chartType = NEnum.NullValue(typeof(ChartType));

        /// <summary>
        /// Gets the DataLabel of the series.
        /// </summary>
        public DataLabel DataLabel
        {
            get { return _dataLabel ?? (_dataLabel = new DataLabel(this)); }
            set
            {
                SetParent(value);
                _dataLabel = value;
            }
        }
        [DV]
        internal DataLabel _dataLabel;

        /// <summary>
        /// Gets or sets whether the series has a DataLabel.
        /// </summary>
        public bool HasDataLabel
        {
            get { return _hasDataLabel.Value; }
            set { _hasDataLabel.Value = value; }
        }
        [DV]
        internal NBool _hasDataLabel = NBool.NullValue;

        /// <summary>
        /// Gets the elementcount of the series.
        /// </summary>
        public int Count
        {
            get
            {
                if (_seriesElements != null)
                    return _seriesElements.Count;

                return 0;
            }
        }
        #endregion

        #region Internal
        /// <summary>
        /// Converts Series into DDL.
        /// </summary>
        internal override void Serialize(Serializer serializer)
        {
            serializer.WriteLine("\\series");

            int pos = serializer.BeginAttributes();

            if (!_name.IsNull)
                serializer.WriteSimpleAttribute("Name", Name);

            if (!_markerSize.IsNull)
                serializer.WriteSimpleAttribute("MarkerSize", MarkerSize);
            if (!_markerStyle.IsNull)
                serializer.WriteSimpleAttribute("MarkerStyle", MarkerStyle);

            if (!_markerBackgroundColor.IsNull)
                serializer.WriteSimpleAttribute("MarkerBackgroundColor", MarkerBackgroundColor);
            if (!_markerForegroundColor.IsNull)
                serializer.WriteSimpleAttribute("MarkerForegroundColor", MarkerForegroundColor);

            if (!_chartType.IsNull)
                serializer.WriteSimpleAttribute("ChartType", ChartType);

            if (!_hasDataLabel.IsNull)
                serializer.WriteSimpleAttribute("HasDataLabel", HasDataLabel);

            if (!IsNull("LineFormat"))
                _lineFormat.Serialize(serializer);
            if (!IsNull("FillFormat"))
                _fillFormat.Serialize(serializer);
            if (!IsNull("DataLabel"))
                _dataLabel.Serialize(serializer);

            serializer.EndAttributes(pos);

            serializer.BeginContent();
            _seriesElements.Serialize(serializer);
            serializer.WriteLine("");
            serializer.EndContent();
        }

        /// <summary>
        /// Returns the meta object of this instance.
        /// </summary>
        internal override Meta Meta
        {
            get { return _meta ?? (_meta = new Meta(typeof(Series))); }
        }
        static Meta _meta;
        #endregion
    }
}
