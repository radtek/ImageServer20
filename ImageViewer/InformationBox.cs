#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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
using System.Drawing;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.ImageViewer
{
	public class InformationBox
	{
		private string _data;
		private Point _destinationPoint;
		private bool _visible;

		private event EventHandler _updated;

		public InformationBox()
		{
			_visible = false;
		}

		public event EventHandler Updated
		{
			add { _updated += value; }
			remove { _updated -= value; }
		}

		public string Data
		{
			get { return _data; }
			set
			{
				if (_data == value)
					return;

				_data = value;

				EventsHelper.Fire(_updated, this, new EventArgs());
			}
		}

		public Point DestinationPoint
		{
			get { return _destinationPoint; }
			set
			{
				if (value == _destinationPoint)
					return;

				_destinationPoint = value;

				EventsHelper.Fire(_updated, this, new EventArgs());
			}
		}

		public bool Visible
		{
			get
			{ return _visible; }
			set
			{
				if (value == _visible)
					return;

				_visible = value;

				EventsHelper.Fire(_updated, this, new EventArgs());
			}
		}

		public void Update(string data, Point destinationPoint)
		{
			bool changed = false;

			if (!_visible || data != _data || destinationPoint != _destinationPoint)
				changed = true;

			_visible = true;
			_data = data;
			_destinationPoint = destinationPoint;
			
			if (changed)
				EventsHelper.Fire(_updated, this, new EventArgs());
		}
	}
}
