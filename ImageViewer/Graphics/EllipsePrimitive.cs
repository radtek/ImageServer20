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
using System.Drawing;
using System.Drawing.Drawing2D;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.ImageViewer.PresentationStates.Dicom;
using ClearCanvas.ImageViewer.PresentationStates.Dicom.GraphicAnnotationSerializers;
using ClearCanvas.ImageViewer.RoiGraphics;

namespace ClearCanvas.ImageViewer.Graphics
{
	/// <summary>
	/// A primitive ellipse graphic.
	/// </summary>
	[Cloneable(true)]
	[DicomSerializableGraphicAnnotation(typeof (EllipseGraphicAnnotationSerializer))]
	public class EllipsePrimitive : BoundableGraphic
	{
		/// <summary>
		/// Initializes a new instance of <see cref="EllipsePrimitive"/>.
		/// </summary>
		public EllipsePrimitive()
		{
				
		}

		/// <summary>
		/// Performs a hit test on the <see cref="EllipsePrimitive"/> at a given point.
		/// </summary>
		/// <param name="point">The mouse position in destination coordinates.</param>
		/// <returns>
		/// <b>True</b> if <paramref name="point"/> "hits" the <see cref="EllipsePrimitive"/>,
		/// <b>false</b> otherwise.
		/// </returns>
		/// <remarks>
		/// A "hit" is defined as when the mouse position is <see cref="VectorGraphic.HitTestDistance"/>
		/// screen pixels away from any point on the ellipse.
		/// </remarks>
		public override bool HitTest(Point point)
		{
			this.CoordinateSystem = CoordinateSystem.Source;
			bool result = HitTest(this.SpatialTransform.ConvertToSource(point), this.Rectangle, this.SpatialTransform);
			this.ResetCoordinateSystem();

			return result;
		}

		/// <summary>
		/// Gets the point where the ellipse intersects the line whose end points
		/// are the center of the ellipse and the specified point.
		/// </summary>
		/// <param name="point">A point in either source or destination coordinates.</param>
		/// <returns>The point on the graphic closest to the given <paramref name="point"/>.</returns>
		/// <remarks>
		/// <para>
		/// Depending on the value of <see cref="Graphic.CoordinateSystem"/>,
		/// the computation will be carried out in either source
		/// or destination coordinates.</para>
		/// </remarks>
		public override PointF GetClosestPoint(PointF point)
		{
			// Semi major/minor axes
			float a = this.Width/2;
			float b = this.Height/2;

			// Center of ellipse
			RectangleF rect = this.Rectangle;
			float x1 = rect.Left + a;
			float y1 = rect.Top + b;

			PointF center = new PointF(x1, y1);

			return IntersectEllipseAndLine(a, b, center, point);
		}

		/// <summary>
		/// Returns a value indicating whether the specified point is contained
		/// in the ellipse.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public override bool Contains(PointF point)
		{
			GraphicsPath path = new GraphicsPath();
			bool result;

			this.CoordinateSystem = CoordinateSystem.Source;
			path.AddEllipse(this.Rectangle);
			this.ResetCoordinateSystem();

			if (this.CoordinateSystem == CoordinateSystem.Destination)
				path.Transform(SpatialTransform.CumulativeTransform);

			result = path.IsVisible(point);

			path.Dispose();

			return result;
		}

		/// <summary>
		/// Gets an object describing the region of interest on the <see cref="Graphic.ParentPresentationImage"/> selected by this <see cref="Graphic"/>.
		/// </summary>
		/// <remarks>
		/// Graphic objects that do not describe a region of interest may return null.
		/// </remarks>
		/// <returns>A <see cref="Roi"/> describing this region of interest, or null if the graphic does not describe a region of interest.</returns>
		public override Roi GetRoi()
		{
			return new EllipticalRoi(this);
		}

		internal static bool HitTest(PointF point, RectangleF boundingBox, SpatialTransform transform)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(RectangleUtilities.ConvertToPositiveRectangle(boundingBox));

			Pen pen = new Pen(Brushes.White, HitTestDistance/transform.CumulativeScale);
			bool result = path.IsOutlineVisible(point, pen);

			path.Dispose();
			pen.Dispose();

			return result;
		}

		/// <summary>
		/// Finds the intersection between an ellipse and a line that starts at the
		/// center of the ellipse and ends at an aribtrary point.
		/// </summary>
		internal static PointF IntersectEllipseAndLine(float a, float b, PointF center, PointF point)
		{
			/*
			 * The point of intersection (P) between the center of the ellipse and the test point (Pt)
			 * where the center of the ellipse is at (0, 0) can be described by the vector equation:
			 * _     __ 
			 * P = m*Pt
			 * 
			 * which yields two equations:
			 * 
			 * x = m * xt (1)
			 * y = m * yt (2)
			 * 
			 * An ellipse centered at (0, 0) is described by the equation:
			 * 
			 * x^2/a^2 + y^2/b^2 = 1 (3)
			 * 
			 * substituting (1) and (2) into (3) gives:
			 * 
			 * m^2*xt^2/a^2 + m^2*yt^2/b^2 = 1
			 * m^2*(xt^2*b^2 + yt^2*a^2) = a*b
			 * 
			 * finally,
			 * 
			 * m = a*b/Sqrt(xt^2*b^2 + yt^2*a^2) (where a^2*yt^2 > 0 and/or b^2*yt^2 > 0)
			 * 
			 * which is a constant for a given ellipse.
			 * 
			 * The intersection point (x, y) can then be found by substituting m into (1) and (2).
			*/

			PointF testPoint = new PointF(point.X - center.X, point.Y - center.Y);

			float denominator = (float) Math.Sqrt(testPoint.X*testPoint.X*b*b +
			                                      testPoint.Y*testPoint.Y*a*a);

			if (FloatComparer.AreEqual(denominator, 0.0F, 0.001F))
				return center;

			float m = Math.Abs(a*b/denominator);

			return new PointF(center.X + m*testPoint.X, center.Y + m*testPoint.Y);
		}
	}
}
