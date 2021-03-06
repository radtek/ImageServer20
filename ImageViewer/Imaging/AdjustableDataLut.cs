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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.ImageViewer.Imaging
{
	/// <summary>
	/// A class that wraps a <see cref="DataLut"/> inside an <see cref="IBasicVoiLutLinear"/>, in
	/// order to allow 'window/levelling' of the <see cref="DataLut"/>.  
	/// </summary>
	/// <remarks>
	/// Internally, this will be treated like any other linear lut, except that
	/// <see cref="GetDescription"/> expresses the Window Width/Center as a percentage of 
	/// the full window, since the true values won't necessarily have any real meaning.
	/// </remarks>
	[Cloneable]
	public class AdjustableDataLut : ComposableLut, IBasicVoiLutLinear, IDataLut
	{
		private class Memento
		{
			public readonly object DataLutMemento;
			public readonly object LinearLutMemento;
			
			public Memento(object dataLutMemento, object linearLutMemento)
			{
				DataLutMemento = dataLutMemento;
				LinearLutMemento = linearLutMemento;
			}

			public override int GetHashCode()
			{
				int dataLutHash = 0x7D60C4F1;
				int linearLutHash = 0x081B32C5;
				if (this.DataLutMemento != null)
					dataLutHash = this.DataLutMemento.GetHashCode();
				if (this.LinearLutMemento != null)
					linearLutHash = this.LinearLutMemento.GetHashCode();
				return dataLutHash ^ linearLutHash ^ 0x273FB457;
			}

			public override bool Equals(object obj)
			{
				if (Object.ReferenceEquals(obj, this))
					return true;

				if (obj is Memento)
				{
					Memento other = obj as Memento;
					return Object.Equals(other.DataLutMemento, DataLutMemento) && 
							Object.Equals(other.LinearLutMemento, LinearLutMemento);
				}

				return false;
			}
		}

		#region Private Fields

		private readonly DataLut _dataLut;
		private readonly BasicVoiLutLinear _linearLut;

		[CloneIgnore]
		private int[] _lutDataCache = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdjustableDataLut(DataLut dataLut)
		{
			Platform.CheckForNullReference(dataLut, "dataLut");

			_dataLut = dataLut;
			_dataLut.LutChanged += OnDataLutChanged;

			_linearLut = new BasicVoiLutLinear();
			_linearLut.LutChanged += OnLinearLutChanged;

			Reset();
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="source">The source object from which to clone.</param>
		/// <param name="context">The cloning context object.</param>
		protected AdjustableDataLut(AdjustableDataLut source, ICloningContext context)
		{
			context.CloneFields(source, this);

			Platform.CheckForNullReference(_dataLut, "_dataLut");
			Platform.CheckForNullReference(_linearLut, "_linearLut");

			_linearLut.LutChanged += OnLinearLutChanged;
			_dataLut.LutChanged += OnDataLutChanged;
		}

		#endregion

		#region Private Properties

		private double FullWindow
		{
			get { return _linearLut.MaxInputValue - _linearLut.MinInputValue + 1; }
		}

		private double BrightnessPercent
		{
			get { return 100 - (WindowCenter - _linearLut.MinInputValue) / FullWindow * 100; }
		}

		private double ContrastPercent
		{
			get { return WindowWidth / FullWindow * 100; }
		}

		#endregion

		#region Private Methods

		private void OnDataLutChanged(object sender, EventArgs e)
		{
			UpdateMinMaxInputLinear();
			this.OnLutChanged();
		}

		private void OnLinearLutChanged(object sender, EventArgs e)
		{
			this.OnLutChanged();
		}

		private void UpdateMinMaxInputLinear()
		{
			_linearLut.MinInputValue = _dataLut.MinOutputValue;
			_linearLut.MaxInputValue = _dataLut.MaxOutputValue;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the underlying data lut.
		/// </summary>
		public DataLut DataLut
		{
			get { return _dataLut; }	
		}

		/// <summary>
		/// Gets or sets the minimum input value.
		/// </summary>
		/// <remarks>
		/// This value should not be modified by your code.  It will be set internally by the framework.
		/// </remarks>
		public override int MinInputValue
		{
			get { return _dataLut.MinInputValue; }
			set { _dataLut.MinInputValue = value; }
		}

		/// <summary>
		/// Gets the maximum input value.
		/// </summary>
		/// <remarks>
		/// This value should not be modified by your code.  It will be set internally by the framework.
		/// </remarks>
		public override int MaxInputValue
		{
			get { return _dataLut.MaxInputValue; }
			set { _dataLut.MaxInputValue = value; }
		}

		/// <summary>
		/// Gets the minimum output value.
		/// </summary>
		public override int MinOutputValue
		{
			get { return _linearLut.MinOutputValue; }
			protected set { throw new InvalidOperationException(SR.ExceptionMinimumOutputValueIsNotSettable); } 
		}

		/// <summary>
		/// Gets the maximum output value.
		/// </summary>
		public override int MaxOutputValue
		{
			get { return _linearLut.MaxOutputValue; }
			protected set { throw new InvalidOperationException(SR.ExceptionMaximumOutputValueIsNotSettable); }
		}

		//TODO: later, add IContrastBrightnessLut and allow the properties to be set.

		#region IBasicVoiLutLinear Members

		/// <summary>
		/// Gets or sets the Window Width.
		/// </summary>
		public double WindowWidth
		{
			get { return _linearLut.WindowWidth; }
			set { _linearLut.WindowWidth = value; }
		}

		/// <summary>
		/// Gets or sets the Window Center.
		/// </summary>
		public double WindowCenter
		{
			get { return _linearLut.WindowCenter; }
			set { _linearLut.WindowCenter = value; }
		}

		#endregion

		#endregion

		/// <summary>
		/// Gets the output value of the lut at a given input index.
		/// </summary>
		public override int this[int index]
		{
			get
			{
				return _linearLut[_dataLut[index]];
			}
			protected set
			{
				throw new InvalidOperationException("This lut type is read-only.");
			}
		}

		#region Public Methods

		/// <summary>
		/// Resets the 
		/// </summary>
		public void Reset()
		{
			UpdateMinMaxInputLinear();

			_linearLut.WindowWidth = FullWindow;
			_linearLut.WindowCenter = _dataLut.MinOutputValue + FullWindow / 2;

			this.OnLutChanged();
		}

		/// <summary>
		/// Gets a string key that identifies this particular Lut's characteristics, so that 
		/// an image's <see cref="IComposedLut"/> can be more efficiently determined.
		/// </summary>
		/// <remarks>
		/// This method is not to be confused with <b>equality</b>, since some Luts can be
		/// dependent upon the actual image to which it belongs.
		/// </remarks>
		public override string GetKey()
		{
			return String.Format("{0}:{1}", _dataLut.GetKey(), _linearLut.GetKey());
		}

		/// <summary>
		/// Gets an abbreviated description of the Lut.
		/// </summary>
		public override string GetDescription()
		{
			return String.Format(SR.FormatAdjustableDataLutDescription, _dataLut.GetDescription(), ContrastPercent, BrightnessPercent);
		}

		/// <summary>
		/// Returns null.
		/// </summary>
		/// <remarks>
		/// Override this member only when necessary.  If this method is overridden, <see cref="ComposableLut.SetMemento"/> must also be overridden.
		///  </remarks>
		/// <returns>null, unless overridden.</returns>
		public override object CreateMemento()
		{
			return new Memento(_dataLut.CreateMemento(), _linearLut.CreateMemento());
		}

		/// <summary>
		/// Does nothing unless overridden.
		/// </summary>
		/// <remarks>
		/// If you override <see cref="ComposableLut.CreateMemento"/> to capture the Lut's state, you must also override this method
		/// to allow the state to be restored.
		/// </remarks>
		/// <param name="memento">The memento object from which to restore the Lut's state.</param>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="memento"/> is <B>not</B> null, 
		/// which would indicate that <see cref="ComposableLut.CreateMemento"/> has been overridden, but <see cref="ComposableLut.SetMemento"/> has not.</exception>
		public override void SetMemento(object memento)
		{
			Platform.CheckForNullReference(memento, "memento");
			Memento lutMemento = (Memento) memento;

			if (lutMemento.DataLutMemento != null)
				_dataLut.SetMemento(lutMemento.DataLutMemento);
			
			if (lutMemento.LinearLutMemento != null)
				_linearLut.SetMemento(lutMemento.LinearLutMemento);
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Fires the <see cref="ComposableLut.LutChanged"/> event.
		/// </summary>
		/// <remarks>
		/// Inheritors should call this method when any property of the Lut has changed.
		/// </remarks>
		protected override void OnLutChanged()
		{
			// when something changes, wipe the cached lut array
			_lutDataCache = null;

			base.OnLutChanged();
		}

		#endregion

		#region IDataLut Members

		int IDataLut.FirstMappedPixelValue
		{
			get { return _dataLut.FirstMappedPixelValue; }
		}

		int[] IDataLut.Data
		{
			get
			{
				if (_lutDataCache == null)
				{
					int lutLength = _dataLut.Length;
					int[] lutData = new int[lutLength];

					unsafe
					{
						fixed (int* output = lutData)
						{
							fixed (int* input = _dataLut.Data)
							{
								for (int n = 0; n < lutLength; n++)
									output[n] = _linearLut[input[n]];
							}
						}
					}

					_lutDataCache = lutData;
				}

				return _lutDataCache;
			}
		}

		#endregion
	}
}
