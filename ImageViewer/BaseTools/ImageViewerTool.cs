using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.ImageViewer.Annotations;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.InputManagement;

namespace ClearCanvas.ImageViewer.BaseTools
{
	/// <summary>
	/// A base class for image viewer tools.
	/// </summary>
	public abstract class ImageViewerTool : Tool<IImageViewerToolContext>, IMouseWheelHandler
	{
		private bool _enabled = true;
		private event EventHandler _enabledChanged;

		private MouseWheelShortcut _mouseWheelShortcut;
		private event EventHandler _mouseWheelShortcutChanged;
		private uint _mouseWheelStopDelayMilliseconds;

		public override void Initialize()
		{
			this.Context.Viewer.EventBroker.TileSelected += new EventHandler<TileSelectedEventArgs>(OnTileSelected);
			this.Context.Viewer.EventBroker.PresentationImageSelected += new EventHandler<PresentationImageSelectedEventArgs>(OnPresentationImageSelected);

			base.Initialize();

			ImageViewerToolAttributeProcessor.Process(this);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the tool is enabled.
		/// </summary>
		public bool Enabled
		{
			get { return _enabled; }
			protected set
			{
				if (_enabled != value)
				{
					_enabled = value;
					EventsHelper.Fire(_enabledChanged, this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Occurs when the <see cref="Enabled"/> property has changed.
		/// </summary>
		public event EventHandler EnabledChanged
		{
			add { _enabledChanged += value; }
			remove { _enabledChanged -= value; }
		}
		
		/// <summary>
		/// Gets the <see cref="IImageViewer"/> associated with this tool.
		/// </summary>
		protected IImageViewer ImageViewer
		{
			get { return this.Context.Viewer; }
		}

		/// <summary>
		/// Gets the selected <see cref="IPresentationImage"/>.
		/// </summary>
		/// <value>The selected <see cref="IPresentationImage"/> or <b>null</b>
		/// if no <see cref="IPresentationImage"/> is currently selected.</value>
		protected IPresentationImage SelectedPresentationImage
		{
			get
			{
				if (this.ImageViewer != null)
					return this.ImageViewer.SelectedPresentationImage;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="IImageGraphicProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="IImageGraphicProvider"/> or <b>null</b>
		/// if no <see cref="IImageGraphicProvider"/> is currently selected.</value>
		protected IImageGraphicProvider SelectedImageGraphicProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
					return this.SelectedPresentationImage as IImageGraphicProvider;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="IImageSopProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="IImageSopProvider"/> or <b>null</b>
		/// if no <see cref="IImageSopProvider"/> is currently selected.</value>
		protected IImageSopProvider SelectedImageSopProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
					return this.SelectedPresentationImage as IImageSopProvider;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="ISpatialTransformProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="ISpatialTransformProvider"/> or <b>null</b>
		/// if no <see cref="ISpatialTransformProvider"/> is currently selected.</value>
		protected ISpatialTransformProvider SelectedSpatialTransformProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
					return this.SelectedPresentationImage as ISpatialTransformProvider;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="IVOILUTLinearProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="IVOILUTLinearProvider"/> or <b>null</b>
		/// if no <see cref="IVOILUTLinearProvider"/> is currently selected.</value>
		protected IVOILUTLinearProvider SelectedVOILUTLinearProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
				{
					return this.SelectedPresentationImage as IVOILUTLinearProvider;
				}
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="IAutoLutApplicatorProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="IAutoLutApplicatorProvider"/> or <b>null</b>
		/// if no <see cref="IAutoLutApplicatorProvider"/> is currently selected.</value>
		protected IAutoVoiLutApplicatorProvider SelectedAutoLutApplicatorProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
				{
					return this.SelectedPresentationImage as IAutoVoiLutApplicatorProvider;
				}
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="IOverlayGraphicsProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="IOverlayGraphicsProvider"/> or <b>null</b>
		/// if no <see cref="IOverlayGraphicsProvider"/> is currently selected.</value>
		protected IOverlayGraphicsProvider SelectedOverlayGraphicsProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
					return this.SelectedPresentationImage as IOverlayGraphicsProvider;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="IAnnotationLayoutProvider"/>.
		/// </summary>
		/// <value>The selected <see cref="IAnnotationLayoutProvider"/> or <b>null</b>
		/// if no <see cref="IAnnotationLayoutProvider"/> is currently selected.</value>
		protected IAnnotationLayoutProvider SelectedAnnotationLayoutProvider
		{
			get
			{
				if (this.SelectedPresentationImage != null)
					return this.SelectedPresentationImage as IAnnotationLayoutProvider;
				else
					return null;
			}
		}

		protected virtual void OnTileSelected(object sender, TileSelectedEventArgs e)
		{
			if (e.SelectedTile.PresentationImage == null)
				this.Enabled = false;
			else
				this.Enabled = true;
		}

		protected virtual void OnPresentationImageSelected(object sender, PresentationImageSelectedEventArgs e)
		{
			if (e.SelectedPresentationImage == null)
				this.Enabled = false;
			else
				this.Enabled = true;
		}

		public MouseWheelShortcut MouseWheelShortcut
		{
			get { return _mouseWheelShortcut; }
			set 
			{
				if (_mouseWheelShortcut != null && _mouseWheelShortcut.Equals(value))
					return;

				_mouseWheelShortcut = value;
				EventsHelper.Fire(_mouseWheelShortcutChanged, this, EventArgs.Empty);
			}
		}

		public event EventHandler MouseWheelShortcutChanged
		{
			add { _mouseWheelShortcutChanged += value; }
			remove { _mouseWheelShortcutChanged -= value; }
		}

		#region IMouseWheelHandler Members

		void IMouseWheelHandler.Start()
		{
			this.StartWheel();
		}

		void IMouseWheelHandler.Wheel(int wheelDelta)
		{
			this.Wheel(wheelDelta);
		}

		void IMouseWheelHandler.Stop()
		{
			this.StopWheel();
		}

		uint IMouseWheelHandler.StopDelayMilliseconds
		{
			get { return _mouseWheelStopDelayMilliseconds; }
		}

		#endregion

		protected virtual void StartWheel()
		{
		}

		protected virtual void Wheel(int wheelDelta)
		{
			if (wheelDelta > 0)
				WheelUp();
			else if (wheelDelta < 0)
				WheelDown();
		}

		protected virtual void WheelUp()
		{
		}

		protected virtual void WheelDown()
		{
		}

		protected virtual void StopWheel()
		{
		}

		internal uint MouseWheelStopDelayMilliseconds
		{
			set { _mouseWheelStopDelayMilliseconds = value; }
		}
	}
}
