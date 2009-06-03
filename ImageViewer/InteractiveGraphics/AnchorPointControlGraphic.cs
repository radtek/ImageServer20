using System;
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Graphics;

namespace ClearCanvas.ImageViewer.InteractiveGraphics
{
	/// <summary>
	/// An interactive graphic that controls the single point of an <see cref="IPointGraphic"/>.
	/// </summary>
	[Cloneable]
	public class AnchorPointControlGraphic : ControlPointsGraphic
	{
		[CloneIgnore]
		private bool _suspendSubjectPointChangeEvents = false;

		/// <summary>
		/// Constructs a new <see cref="AnchorPointControlGraphic"/>.
		/// </summary>
		/// <param name="subject">An <see cref="IPointGraphic"/> or an <see cref="IControlGraphic"/> chain whose subject is an <see cref="IPointGraphic"/>.</param>
		public AnchorPointControlGraphic(IGraphic subject)
			: base(subject)
		{
			Platform.CheckExpectedType(base.Subject, typeof (IPointGraphic));

			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				base.ControlPoints.Add(this.Subject.Point);
			}
			finally
			{
				this.ResetCoordinateSystem();
			}

			Initialize();
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="source">The source object from which to clone.</param>
		/// <param name="context">The cloning context object.</param>
		protected AnchorPointControlGraphic(AnchorPointControlGraphic source, ICloningContext context)
			: base(source, context)
		{
			context.CloneFields(source, this);
		}

		/// <summary>
		/// Gets the subject graphic that this graphic controls.
		/// </summary>
		public new IPointGraphic Subject
		{
			get { return base.Subject as IPointGraphic; }
		}

		/// <summary>
		/// Gets a string that describes the type of control operation that this graphic provides.
		/// </summary>
		public override string CommandName
		{
			get { return SR.CommandChange; }
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			Initialize();
		}

		private void Initialize()
		{
			this.Subject.PointChanged += OnSubjectPointChanged;
		}

		/// <summary>
		/// Releases all resources used by this <see cref="IControlGraphic"/>.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			this.Subject.PointChanged -= OnSubjectPointChanged;
			base.Dispose(disposing);
		}

		/// <summary>
		/// Captures the current state of this <see cref="AnchorPointControlGraphic"/>.
		/// </summary>
		public override object CreateMemento()
		{
			this.Subject.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				return new PointMemento(this.Subject.Point);
			}
			finally
			{
				this.Subject.ResetCoordinateSystem();
			}
		}

		/// <summary>
		/// Restores the state of this <see cref="AnchorPointControlGraphic"/>.
		/// </summary>
		/// <param name="memento">The object that was originally created with <see cref="AnchorPointControlGraphic.CreateMemento"/>.</param>
		public override void SetMemento(object memento)
		{
			PointMemento pointMemento = memento as PointMemento;
			if (pointMemento == null)
				throw new ArgumentException("The provided memento is not the expected type.", "memento");

			_suspendSubjectPointChangeEvents = true;
			this.Subject.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				this.Subject.Point = pointMemento.Point;
			}
			finally
			{
				this.Subject.ResetCoordinateSystem();
				_suspendSubjectPointChangeEvents = false;
				this.OnSubjectPointChanged(this, EventArgs.Empty);
			}
		}

		private void OnSubjectPointChanged(object sender, EventArgs e)
		{
			if (_suspendSubjectPointChangeEvents)
				return;

			this.SuspendControlPointEvents();
			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				this.ControlPoints[0] = this.Subject.Point;
			}
			finally
			{
				this.ResetCoordinateSystem();
				this.ResumeControlPointEvents();
			}
		}

		/// <summary>
		/// Called to notify the derived class of a control point change event.
		/// </summary>
		/// <param name="index">The index of the point that changed.</param>
		/// <param name="point">The value of the point that changed.</param>
		protected override void OnControlPointChanged(int index, PointF point)
		{
			this.Subject.Point = point;
			base.OnControlPointChanged(index, point);
		}
	}
}