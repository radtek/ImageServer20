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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.InputManagement;
using ClearCanvas.ImageViewer.Mathematics;

namespace ClearCanvas.ImageViewer.InteractiveGraphics
{
	/// <summary>
	/// An interactive graphic that controls the vertices of an <see cref="IPointsGraphic"/>.
	/// </summary>
	[Cloneable]
	public class VerticesControlGraphic : ControlPointsGraphic
	{
		private bool _canAddRemoveVertices = false;

		[CloneIgnore]
		private bool _suspendSubjectPointChangeEvents = false;

		[CloneIgnore]
		private PointF _lastContextMenuPoint = PointF.Empty;

		/// <summary>
		/// Constructs a new <see cref="VerticesControlGraphic"/>.
		/// </summary>
		/// <param name="subject">An <see cref="IPointsGraphic"/> or an <see cref="IControlGraphic"/> chain whose subject is an <see cref="IPointsGraphic"/>.</param>
		public VerticesControlGraphic(IGraphic subject)
			: this(false, subject) {}

		/// <summary>
		/// Constructs a new <see cref="VerticesControlGraphic"/>.
		/// </summary>
		/// <param name="canAddRemoveVertices">A value indicating whether or not the user can dynamically add or remove vertices on the subject.</param>
		/// <param name="subject">An <see cref="IPointsGraphic"/> or an <see cref="IControlGraphic"/> chain whose subject is an <see cref="IPointsGraphic"/>.</param>
		public VerticesControlGraphic(bool canAddRemoveVertices, IGraphic subject)
			: base(subject)
		{
			Platform.CheckExpectedType(base.Subject, typeof (IPointsGraphic));

			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				foreach(PointF point in this.Subject.Points)
				{
					base.ControlPoints.Add(point);
				}
			}
			finally
			{
				this.ResetCoordinateSystem();
			}

			_canAddRemoveVertices = canAddRemoveVertices;

			Initialize();
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="source">The source object from which to clone.</param>
		/// <param name="context">The cloning context object.</param>
		protected VerticesControlGraphic(VerticesControlGraphic source, ICloningContext context)
			: base(source, context)
		{
			context.CloneFields(source, this);
		}

		/// <summary>
		/// Gets the subject graphic that this graphic controls.
		/// </summary>
		public new IPointsGraphic Subject
		{
			get { return base.Subject as IPointsGraphic; }
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
			this.Subject.Points.PointChanged += OnSubjectPointChanged;
			this.Subject.Points.PointAdded += OnSubjectPointsChanged;
			this.Subject.Points.PointRemoved += OnSubjectPointsChanged;
			this.Subject.Points.PointsCleared += OnSubjectPointsChanged;
		}

		/// <summary>
		/// Releases all resources used by this <see cref="IControlGraphic"/>.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			this.Subject.Points.PointChanged -= OnSubjectPointChanged;
			this.Subject.Points.PointAdded -= OnSubjectPointsChanged;
			this.Subject.Points.PointRemoved -= OnSubjectPointsChanged;
			this.Subject.Points.PointsCleared -= OnSubjectPointsChanged;
			base.Dispose(disposing);
		}

		#region Add/Remove Vertices

		/// <summary>
		/// Gets or sets a value indicating whether or not the user can dynamically add or remove vertices on the subject.
		/// </summary>
		public bool CanAddRemoveVertices
		{
			get { return _canAddRemoveVertices; }
			set
			{
				if(_canAddRemoveVertices!=value)
				{
					_canAddRemoveVertices = value;
					OnCanAddRemoveVerticesChanged();
				}
			}
		}

		/// <summary>
		/// Called to insert a vertex at the point where the context menu was last invoked.
		/// </summary>
		protected virtual void InsertVertex()
		{
			if (!_canAddRemoveVertices)
				return;

			object memento = this.CreateMemento();

			IPointsGraphic subject = this.Subject;
			subject.CoordinateSystem = CoordinateSystem.Destination;
			try
			{
				int index = this.HitTestControlPoint(Point.Round(_lastContextMenuPoint));

				if (index < 0)
				{
					// if inserting in middle of line, find which index to insert at
					index = IndexOfNextClosestPoint(subject, _lastContextMenuPoint);
				}
				else if(index == subject.Points.Count - 1)
				{
					// if inserting on last point, append instead of inserting before
					index++;
				}

				if (index >= 0)
				{
					subject.Points.Insert(index, _lastContextMenuPoint);
				}
			}
			finally
			{
				subject.ResetCoordinateSystem();
			}

			AddToCommandHistory(this, memento, this.CreateMemento());
		}

		/// <summary>
		/// Called to remove the vertex at the point where the context menu was last invoked.
		/// </summary>
		protected virtual void DeleteVertex()
		{
			if (!_canAddRemoveVertices)
				return;

			object memento = this.CreateMemento();

			IPointsGraphic subject = this.Subject;
			if (subject.Points.Count > 1)
			{
				int index = this.HitTestControlPoint(Point.Round(_lastContextMenuPoint));
				if (index >= 0 && index < subject.Points.Count)
				{
					subject.Points.RemoveAt(index);
				}
			}

			AddToCommandHistory(this, memento, this.CreateMemento());
		}

		/// <summary>
		/// Computes the index for insertion of a new point on an existing <see cref="IPointsGraphic"/>.
		/// </summary>
		private int IndexOfNextClosestPoint(IPointsGraphic subject, PointF point)
		{
			int index = 0;
			double best = double.MaxValue;
			PointF closestPoint = this.GetClosestPoint(point);
			PointF temp = PointF.Empty;
			for (int n = 0; n < subject.Points.Count - 1; n++)
			{
				double distance = Vector.DistanceFromPointToLine(closestPoint, subject.Points[n], subject.Points[n + 1], ref temp);
				if (distance < best)
				{
					best = distance;
					index = n + 1;
				}
			}
			return index;
		}

		private void PerformInsertVertex()
		{
			this.InsertVertex();
			this.Draw();
		}

		private void PerformDeleteVertex()
		{
			this.DeleteVertex();
			this.Draw();
		}

		/// <summary>
		/// Called when the value of <see cref="CanAddRemoveVertices"/> changes.
		/// </summary>
		protected void OnCanAddRemoveVerticesChanged() {}

		/// <summary>
		/// Gets a set of exported <see cref="IAction"/>s.
		/// </summary>
		/// <param name="site">The action model site at which the actions should reside.</param>
		/// <param name="mouseInformation">The mouse input when the action model was requested, such as in response to a context menu request.</param>
		/// <returns>A set of exported <see cref="IAction"/>s.</returns>
		public override IActionSet GetExportedActions(string site, IMouseInformation mouseInformation)
		{
			IActionSet actions = base.GetExportedActions(site, mouseInformation);
			_lastContextMenuPoint = mouseInformation.Location;

			if (!_canAddRemoveVertices)
				return actions;

			if (!base.Subject.HitTest(Point.Round(_lastContextMenuPoint)))
				return actions;

			int count = this.Subject.Points.Count;
			bool hit = base.ControlPoints.HitTest(Point.Round(_lastContextMenuPoint));

			IResourceResolver resolver = new ResourceResolver(this.GetType(), true);
			string @namespace = typeof (VerticesControlGraphic).FullName;

			MenuAction insertAction = new MenuAction(@namespace + ":insert", new ActionPath(site + "/MenuInsertVertex", resolver), ClickActionFlags.None, resolver);
			insertAction.GroupHint = new GroupHint("Tools.Graphics.Edit");
			insertAction.Label = SR.MenuInsertVertex;
			insertAction.Persistent = true;
			insertAction.SetClickHandler(this.PerformInsertVertex);

			MenuAction deleteAction = new MenuAction(@namespace + ":delete", new ActionPath(site + "/MenuDeleteVertex", resolver), ClickActionFlags.None, resolver);
			deleteAction.GroupHint = new GroupHint("Tools.Graphics.Edit");
			deleteAction.Label = SR.MenuDeleteVertex;
			deleteAction.Visible = hit && count > 1;
			deleteAction.Persistent = true;
			deleteAction.SetClickHandler(this.PerformDeleteVertex);

			return actions.Union(new ActionSet(new IAction[] {insertAction, deleteAction}));
		}

		#endregion

		/// <summary>
		/// Captures the current state of this <see cref="VerticesControlGraphic"/>.
		/// </summary>
		public override object CreateMemento()
		{
			PointsMemento pointsMemento = new PointsMemento();

			this.Subject.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				foreach (PointF point in this.Subject.Points)
					pointsMemento.Add(point);
			}
			finally
			{
				this.Subject.ResetCoordinateSystem();
			}

			return pointsMemento;
		}

		/// <summary>
		/// Restores the state of this <see cref="VerticesControlGraphic"/>.
		/// </summary>
		/// <param name="memento">The object that was originally created with <see cref="VerticesControlGraphic.CreateMemento"/>.</param>
		public override void SetMemento(object memento)
		{
			PointsMemento pointsMemento = memento as PointsMemento;
			if (pointsMemento == null)
				throw new ArgumentException("The provided memento is not the expected type.", "memento");

			_suspendSubjectPointChangeEvents = true;
			this.Subject.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				int numPoints = Math.Min(this.Subject.Points.Count, pointsMemento.Count);
				for (int n = 0; n < numPoints; n++)
					this.Subject.Points[n] = pointsMemento[n];
				for (int n = numPoints; n < this.Subject.Points.Count; n++)
					this.Subject.Points.RemoveAt(numPoints);
				for (int n = numPoints; n < pointsMemento.Count; n++)
					this.Subject.Points.Add(pointsMemento[n]);
			}
			finally
			{
				this.Subject.ResetCoordinateSystem();
				_suspendSubjectPointChangeEvents = false;
				this.OnSubjectPointsChanged(this, new EventArgs());
			}
		}

		private void OnSubjectPointsChanged(object sender, EventArgs e)
		{
			if (_suspendSubjectPointChangeEvents)
				return;

			this.OnSubjectPointsChanged();
		}

		/// <summary>
		/// Called to notify that multiple vertices in the subject graphic have changed.
		/// </summary>
		protected virtual void OnSubjectPointsChanged()
		{
			this.SuspendControlPointEvents();
			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				// resync control points
				this.ControlPoints.Clear();
				foreach (PointF point in this.Subject.Points)
				{
					base.ControlPoints.Add(point);
				}
			}
			finally
			{
				this.ResetCoordinateSystem();
				this.ResumeControlPointEvents();
			}
		}

		private void OnSubjectPointChanged(object sender, IndexEventArgs e)
		{
			if (_suspendSubjectPointChangeEvents)
				return;

			this.OnSubjectPointChanged(e.Index);
		}

		/// <summary>
		/// Called to notify that a vertex in the subject graphic has changed.
		/// </summary>
		/// <param name="index">The index of the vertex that changed.</param>
		protected virtual void OnSubjectPointChanged(int index)
		{
			this.SuspendControlPointEvents();
			this.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				this.ControlPoints[index] = this.Subject.Points[index];
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
			this.Subject.Points[index] = point;
			base.OnControlPointChanged(index, point);
		}
	}
}