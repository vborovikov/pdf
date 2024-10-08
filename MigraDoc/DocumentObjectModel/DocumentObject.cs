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
using System.Diagnostics;
using MigraDoc.DocumentObjectModel.Internals;

namespace MigraDoc.DocumentObjectModel
{
    /// <summary>
    /// Base class of all objects of the MigraDoc Document Object Model.
    /// </summary>
    public abstract class DocumentObject : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the DocumentObject class.
        /// </summary>
        internal DocumentObject()
        { }

        /// <summary>
        /// Initializes a new instance of the DocumentObject class with the specified parent.
        /// </summary>
        internal DocumentObject(DocumentObject parent)
        {
            Debug.Assert(parent != null, "Parent must not be null.");
            _parent = parent;
        }

        /// <summary>
        /// Creates a deep copy of the DocumentObject. The parent of the new object is null.
        /// </summary>
        public object Clone()
        {
            return DeepCopy();
        }

        /// <summary>
        /// Implements the deep copy of the object.
        /// </summary>
        protected virtual object DeepCopy()
        {
            DocumentObject value = (DocumentObject)MemberwiseClone();
            value._parent = null;
            return value;
        }

        /// <summary>
        /// Creates an object using the default constructor.
        /// </summary>
        public object CreateValue(string name)
        {
            ValueDescriptor vd = Meta[name];
            if (vd != null)
                return vd.CreateValue();
            return null;
        }

        /// <summary>
        /// Gets the parent object.
        /// </summary>
        internal DocumentObject Parent
        {
            get { return _parent; }
        }
        [DV(RefOnly = true)]
        internal DocumentObject _parent;

        /// <summary>
        /// Gets the document of the object, or null, if the object is not associated with a document.
        /// </summary>
        public Document Document
        {
            // TODO Have to reset _document when _parent changes.
            get
            {
                if (_document != null)
                    return _document;
                DocumentObject doc = Parent;
                Document document = doc as Document;
                if (document != null)
                    return _document = document;
                // Call document at the parent - recursive call instead of while loop - and all parent objects will also update their _document fields.
                // Next call to Document from a sibbling will succeed without climbing all the way up.
                if (doc != null)
                    return _document = doc.Document;
                return null;
            }
        }
        private Document _document;

        /// <summary>
        /// Gets the section of the object, or null, if the object is not associated with a section.
        /// </summary>
        public Section Section
        {
            // TODO Have to reset _section when _parent changes.
            get
            {
                if (_section != null)
                    return _section;
                DocumentObject doc = Parent;
                if (doc != null)
                {
                    Section section = doc as Section;
                    if (section != null)
                        return _section = section;
                    return _section = doc.Section;
                }
                return null;
            }
        }
        private Section _section;

        /// <summary>
        /// Converts DocumentObject into DDL.
        /// </summary>
        internal abstract void Serialize(Serializer serializer);

        /// <summary>
        /// Returns the value with the specified name.
        /// </summary>
        public virtual object GetValue(string name)
        {
            return GetValue(name, GV.ReadWrite);
        }

        /// <summary>
        /// Returns the value with the specified name and value flags.
        /// </summary>
        public virtual object GetValue(string name, GV flags)
        {
            return Meta.GetValue(this, name, flags);
        }

        /// <summary>
        /// Sets the given value and sets its parent afterwards.
        /// </summary>
        public virtual void SetValue(string name, object val)
        {
            Meta.SetValue(this, name, val);
            DocumentObject documentObject = val as DocumentObject;
            if (documentObject != null)
                (documentObject)._parent = this;
        }

        /// <summary>
        /// Determines whether this instance has a value of the given name.
        /// </summary>
        public virtual bool HasValue(string name)
        {
            return Meta.HasValue(name);
        }

        /// <summary>
        /// Determines whether the value of the given name is null.
        /// </summary>
        public virtual bool IsNull(string name)
        {
            return Meta.IsNull(this, name);
        }

        /// <summary>
        /// Resets the value of the given name, i.e. IsNull(name) will return true afterwards.
        /// </summary>
        public virtual void SetNull(string name)
        {
            Meta.SetNull(this, name);
        }

        /// <summary>
        /// Determines whether this instance is null (not set).
        /// </summary>
        public virtual bool IsNull()
        {
            return Meta.IsNull(this);
        }

        /// <summary>
        /// Resets this instance, i.e. IsNull() will return true afterwards.
        /// </summary>
        public virtual void SetNull()
        {
            Meta.SetNull(this);
        }

        /// <summary>
        /// Gets or sets a value that contains arbitrary information about this object.
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        object _tag;

        /// <summary>
        /// Returns the meta object of this instance.
        /// </summary>
        internal abstract Meta Meta
        {
            get;
        }

        /// <summary>
        /// Sets the parent of the specified value.
        /// If a parent is already set, an ArgumentException will be thrown.
        /// </summary>
        protected void SetParent(DocumentObject val)
        {
            if (val != null)
            {
                if (val.Parent != null)
                    throw new ArgumentException(DomSR.ParentAlreadySet(val, this));

                val._parent = this;
                val._document = null;
                val._section = null;
            }
        }

        /// <summary>
        /// When overridden in a derived class resets cached values
        /// (like column index).
        /// </summary>
        internal virtual void ResetCachedValues()
        {
            _document = null;
            _section = null;
        }
    }
}
