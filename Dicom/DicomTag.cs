﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Text;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom
{
    /// <summary>
    /// The DicomTag class contains all DICOM information for a specific tag.
    /// </summary>
    /// <remarks>
    /// <para>The DicomTag class is used as described in the Flyweight pattern.  A single instance should only be allocated
    /// for each DICOM tag, and that instance will be shared in any <see cref="DicomAttributeCollection"/> 
    /// that references the specific tag.</para>
    /// <para>Note, however, that non standard DICOM tags (or tags not in stored in the <see cref="DicomTagDictionary"/>
    /// will have a specific instance allocated to store their information when they are encountered by the assembly.</para>
    /// </remarks>
    public class DicomTag
    {
        #region Static Members
        /// <summary>
        /// Return a uint with a tags value based on the input group and element.
        /// </summary>
        /// <param name="group">The Group for the tag.</param>
        /// <param name="element">The Element for the tag.</param>
        /// <returns></returns>
        public static uint GetTagValue(ushort group, ushort element)
        {
            return (uint)group << 16 | (uint)element;
        }

        /// <summary>
        /// Checks if a Group is private (odd).
        /// </summary>
        /// <param name="group">The Group to check.</param>
        /// <returns>true if the Group is private, false otherwise.</returns>
        public static bool IsPrivateGroup(ushort group)
        {
            return (group & 1) == 1;
        }
        /// <summary>
        /// Returns an instance of a private tag for a private creator code.
        /// </summary>
        /// <param name="group">The Group of the tag.</param>
        /// <param name="element">The Element for the tag.</param>
        /// <returns></returns>
        public static DicomTag GetPrivateCreatorTag(ushort group, ushort element)
        {
            return new DicomTag((uint)group << 16 | (uint)(element >> 8), "Private Creator", "PrivateCreator", DicomVr.LOvr, false, 1, 1, false);
        }

        /// <summary>(fffe,e0dd) VR= Sequence Delimitation Item</summary>
        internal static DicomTag SequenceDelimitationItem = new DicomTag(0xfffee0dd, "Sequence Delimitation Item");

        /// <summary>(fffe,e000) VR= Item</summary>
        internal static DicomTag Item = new DicomTag(0xfffee000, "Item");

        /// <summary>(fffe,e00d) VR= Item Delimitation Item</summary>
        internal static DicomTag ItemDelimitationItem = new DicomTag(0xfffee00d, "Item Delimitation Item");

        #endregion

        #region Private Members
        private uint _tag;
        private string _name;
        private string _varName;
        private DicomVr _vr;
        private uint _vmLow;
        private uint _vmHigh;
        private bool _isRetired;
        private bool _multiVrTag;
        #endregion

        #region Constructors
        /// <summary>
        /// Primary constructor for dictionary tags
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="name"></param>
        /// <param name="varName"></param>
        /// <param name="vr"></param>
        /// <param name="isMultiVrTag"></param>
        /// <param name="vmLow"></param>
        /// <param name="vmHigh"></param>
        /// <param name="isRetired"></param>
        public DicomTag(uint tag, String name, String varName, DicomVr vr, bool isMultiVrTag, uint vmLow, uint vmHigh, bool isRetired)
        {
            _tag = tag;
            _name = name;
            _varName = varName;
            _vr = vr;
            _multiVrTag = isMultiVrTag;
            _vmLow = vmLow;
            _vmHigh = vmHigh;
            _isRetired = isRetired;
        }

        private DicomTag(uint tag, String name)
        {
            _tag = tag;
            _name = name;
            _vr = DicomVr.UNvr;
            _multiVrTag = false;
            _vmLow = 1;
            _vmHigh = 1;
            _isRetired = false;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets the Group Number of the tag as a 16-bit unsigned integer.
        /// </summary>
        public ushort Group
        {
            get { return ((ushort)((_tag & 0xffff0000) >> 16)); }
        }

        /// <summary>
        /// Gets the Element Number of the tag as a 16-bit unsigned integer.
        /// </summary>
        public ushort Element
        {
            get { return ((ushort)(_tag & 0x0000ffff)); }
        }

        /// <summary>
        /// Gets a text description of the tag.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets a text description of the tag with spaces removed and proper .NET casing.
        /// </summary>
        public String VariableName
        {
            get { return _varName; }
        }

        /// <summary>
        /// Gets a boolean telling if the tag is retired.
        /// </summary>
        public bool Retired
        {
            get { return _isRetired; }
        }

        /// <summary>
        /// Gets a boolean telling if the tag supports multiple VRs.
        /// </summary>
        public bool MultiVR
        {
            get { return _multiVrTag; }
        }

        /// <summary>
        /// Returns a <see cref="DicomVr"/> object representing the Value Representation (VR) of the tag.
        /// </summary>
        public DicomVr VR
        {
            get { return _vr; }
        }

        /// <summary>
        /// Gets a uint representing the low Value of Multiplicity defined by DICOM for the tag. 
        /// </summary>
        public uint VMLow
        {
            get { return _vmLow; }
        }

        /// <summary>
        /// Gets a uint representing the high Value of Multiplicity defined by DICOM for the tag.
        /// </summary>
        public uint VMHigh
        {
            get { return _vmHigh; }
        }
        
        /// <summary>
        /// Gets a string representing the value of multiplicity defined by DICOM for the tag.
        /// </summary>
        public string VM
        {
            get
            {
                if (_vmLow == _vmHigh)
                    return _vmLow.ToString();
                if (_vmHigh == uint.MaxValue)
                    return _vmLow.ToString() + "-N";
                return _vmLow.ToString() + "-" + _vmHigh.ToString();
            }
        }

        /// <summary>
        /// Returns a uint DICOM Tag value for the object.
        /// </summary>
        public uint TagValue
        {
            get { return _tag; }
        }

        /// <summary>
        /// Returns a string with the tag value in Hex
        /// </summary>
        public String HexString
        {
            get
            {
                return _tag.ToString("X8");
            }
        }

        /// <summary>
        /// Returns a bool as true if the tag is private
        /// </summary>
        public bool IsPrivate
        {
            get { return (Group & 1) == 1; }
        }
        #endregion

        #region System.Object Overrides
        /// <summary>
        /// Provides a hash code that's more natural by using the
        /// Group and Element number of the tag.
        /// </summary>
        /// <returns>The Group and Element number as a 32-bit integer.</returns>
        public override int GetHashCode()
        {
            return ((int)_tag);
        }

        /// <summary>
        /// Provides a human-readable representation of the tag.
        /// </summary>
        /// <returns>The string representation of the Group and Element.</returns>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("({0:x4},{1:x4}) ", Group, Element);
            buffer.Append(_name);

            return buffer.ToString();
        }

        /// <summary>
        /// This override allows the comparison of two DicomTag objects
        /// for semantic equivalence. 
        /// </summary>
        /// <param name="obj">The other DicomTag object to compare this object to.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            DicomTag otherTag = obj as DicomTag;
            if (null == otherTag)
                return false;

            return (otherTag.GetHashCode() == this.GetHashCode());
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implicit cast to a String object, for ease of use.
        /// </summary>
        public static implicit operator String(DicomTag myTag)
        {
            return myTag.ToString();
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(DicomTag t1, DicomTag t2)
        {
            if ((object)t1 == null && (object)t2 == null)
                return true;
            if ((object)t1 == null || (object)t2 == null)
                return false;
            return t1.TagValue == t2.TagValue;
        }

        /// <summary>
        /// Not equal operator.
        /// </summary>
        public static bool operator !=(DicomTag t1, DicomTag t2)
        {
            return !(t1 == t2);
        }

        /// <summary>
        /// Less than operator.
        /// </summary>
        public static bool operator <(DicomTag t1, DicomTag t2)
        {
            if ((object)t1 == null || (object)t2 == null)
                return false;
            if (t1.Group == t2.Group && t1.Element < t2.Element)
                return true;
            if (t1.Group < t2.Group)
                return true;
            return false;
        }

        /// <summary>
        /// Greater than operator.
        /// </summary>
        public static bool operator >(DicomTag t1, DicomTag t2)
        {
            return !(t1 < t2);
        }

        /// <summary>
        /// Less than or equal to operator.
        /// </summary>
        public static bool operator <=(DicomTag t1, DicomTag t2)
        {
            if ((object)t1 == null || (object)t2 == null)
                return false;
            if (t1.Group == t2.Group && t1.Element <= t2.Element)
                return true;
            if (t1.Group < t2.Group)
                return true;
            return false;
        }
 
        /// <summary>
        /// Greater than or equal to operator.
        /// </summary>
        public static bool operator >=(DicomTag t1, DicomTag t2)
        {
            if ((object)t1 == null || (object)t2 == null)
                return false;
            if (t1.Group == t2.Group && t1.Element >= t2.Element)
                return true;
            if (t1.Group > t2.Group)
                return true;
            return false;
        }
        #endregion

        #region Public Methds
        /// <summary>
        /// This method creates a <see cref="DicomAttribute"/> derived class for the tag.
        /// </summary>
        /// <returns></returns>
        public DicomAttribute CreateDicomAttribute()
        {
            return _vr.CreateDicomAttribute(this);
        }
        /// <summary>
        /// This method creates a <see cref="DicomAttribute"/> derived class for the tag, and 
        /// sets the intial value of the tag to the value contains in <paramref name="bb"/>.
        /// </summary>
        /// <param name="bb">A ByteBuffer containing an intial raw value for the tag.</param>
        /// <returns></returns>
        public DicomAttribute CreateDicomAttribute(ByteBuffer bb)
        {
            return _vr.CreateDicomAttribute(this,bb);
        }
        #endregion
    }
}
