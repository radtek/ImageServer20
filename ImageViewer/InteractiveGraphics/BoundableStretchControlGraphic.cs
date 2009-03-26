using System;
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Graphics;

namespace ClearCanvas.ImageViewer.InteractiveGraphics
{
	[Cloneable]
	public sealed class BoundableStretchControlGraphic : ControlPointsGraphic
	{
		private const int _top = 0;
		private const int _bottom = 1;
		private const int _left = 2;
		private const int _right = 3;

		[CloneIgnore]
		private bool _bypassControlPointChangedEvent = false;

		public BoundableStretchControlGraphic(IGraphic subject)
			: base(subject)
		{
			Platform.CheckExpectedType(base.Subject, typeof(IBoundableGraphic));

			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				RectangleF rf = this.Subject.Rectangle;
				float halfWidth = rf.Width/2;
				float halfHeight = rf.Height/2;
				base.ControlPoints.Add(new PointF(rf.Left + halfWidth, rf.Top));
				base.ControlPoints.Add(new PointF(rf.Left + halfWidth, rf.Bottom));
				base.ControlPoints.Add(new PointF(rf.Left, rf.Top + halfHeight));
				base.ControlPoints.Add(new PointF(rf.Right, rf.Top + halfHeight));
			}
			finally
			{
				this.ResetCoordinateSystem();
			}

			Initialize();
		}

		private BoundableStretchControlGraphic(BoundableStretchControlGraphic source, ICloningContext context)
			: base(source, context)
		{
			context.CloneFields(source, this);
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			Initialize();
		}

		public new IBoundableGraphic Subject
		{
			get { return base.Subject as IBoundableGraphic; }
		}

		private void Initialize()
		{
			this.Subject.BottomRightChanged += OnSubjectChanged;
			this.Subject.TopLeftChanged += OnSubjectChanged;
		}

		protected override void Dispose(bool disposing)
		{
			this.Subject.BottomRightChanged -= OnSubjectChanged;
			this.Subject.TopLeftChanged -= OnSubjectChanged;

			base.Dispose(disposing);
		}

		protected override void OnControlPointChanged(int index, PointF point)
		{
			if (!_bypassControlPointChangedEvent)
			{
				IBoundableGraphic subject = this.Subject;
				RectangleF rect = subject.Rectangle;
				switch (index)
				{
					case _top:
						subject.TopLeft = new PointF(rect.Left, point.Y);
						break;
					case _bottom:
						subject.BottomRight = new PointF(rect.Right, point.Y);
						break;
					case _left:
						subject.TopLeft = new PointF(point.X, rect.Top);
						break;
					case _right:
						subject.BottomRight = new PointF(point.X, rect.Bottom);
						break;
				}
			}
			base.OnControlPointChanged(index, point);
		}

		private void OnSubjectChanged(object sender, PointChangedEventArgs e)
		{
			_bypassControlPointChangedEvent = true;
			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				RectangleF rect = this.Subject.Rectangle;
				float halfWidth = rect.Width / 2;
				float halfHeight = rect.Height / 2;
				this[_top] = new PointF(rect.Left + halfWidth, rect.Top);
				this[_bottom] = new PointF(rect.Left + halfWidth, rect.Bottom);
				this[_left] = new PointF(rect.Left, rect.Top + halfHeight);
				this[_right] = new PointF(rect.Right, rect.Top + halfHeight);
			}
			finally
			{
				this.ResetCoordinateSystem();
				_bypassControlPointChangedEvent = false;
			}
		}
	}
}