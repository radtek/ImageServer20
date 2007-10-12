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

namespace ClearCanvas.ImageViewer.Mathematics
{
	/// <summary>
	/// A rectangle based
	/// </summary>
	public class RectF
	{
		private float _top;
		private float _left;
		private float _bottom;
		private float _right;

		public RectF()
		{
		}

		public float Top
		{
			get { return _top; }
			set { _top = value; }
		}

		public float Left
		{
			get { return _left; }
			set { _left = value; }
		}

		public float Bottom
		{
			get { return _bottom; }
			set { _bottom = value; }
		}

		public float Right
		{
			get { return _right; }
			set { _right = value; }
		}

		public PointF TopLeft
		{
			get { return new PointF(_top, _left); }
			set
			{
				_left = value.X;
				_top = value.Y;
			}
		}

		public PointF BottomRight
		{
			get { return new PointF(_bottom, _right); }
			set
			{
				_right = value.X;
				_bottom = value.Y;
			}
		}

		public float Width
		{
			get { return _right - _left; }
			set { _right = _left + value; }
		}

		public float Height
		{
			get { return _bottom - _top; }
			set { _bottom = _top + value; }
		}

		public bool Contains(PointF point)
		{
			return Contains(point.X, point.Y);
		}

		public bool Contains(float x, float y)
		{
			bool inXRange = false;
			bool inYRange = false;

			if (this.Width > 0)
			{
				if (x > _left && x < _right)
					inXRange = true;
			}
			else
			{
				if (x > _right && x < _left)
					inXRange = true;
			}

			if (this.Height > 0)
			{
				if (y > _top && y < _bottom)
					inYRange = true;
			}
			else
			{
				if (y > _bottom && y < _top)
					inYRange = true;
			}

			return inXRange && inYRange;
		}

		public override string ToString()
		{
			return String.Format("{{t={0},l={1},b={2},r={3}}}",
						 _top, _left, _bottom, _right);
		}

	}
}
