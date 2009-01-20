using System;
using System.Collections.Generic;
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Mathematics;

namespace ClearCanvas.ImageViewer.Tools.Standard
{
	[MenuAction("showHide", "global-menus/MenuTools/MenuStandard/MenuShowHideScaleOverlay", "ShowHide")]
	[ButtonAction("showHide", "global-toolbars/ToolbarStandard/ToolbarShowHideScaleOverlay", "ShowHide")]
	[Tooltip("showHide", "TooltipShowHideScaleOverlay")]
	[GroupHint("showHide", "Tools.Image.Overlays.Scale.ShowHide")]
	[IconSet("showHide", IconScheme.Colour, "Icons.ScaleOverlayToolSmall.png", "Icons.ScaleOverlayToolMedium.png", "Icons.ScaleOverlayToolLarge.png")]
	[ExtensionOf(typeof (ImageViewerToolExtensionPoint))]
	public class ScaleOverlayTool : ImageViewerTool
	{
		private event EventHandler _visibleChanged;
		private bool _visible;

		public ScaleOverlayTool()
		{
			_visible = true;
		}

		public bool Visible
		{
			get { return _visible; }
			set
			{
				if (_visible != value)
				{
					_visible = value;
					OnVisibleChanged();
				}
			}
		}

		public event EventHandler VisibleChanged
		{
			add { _visibleChanged += value; }
			remove { _visibleChanged -= value; }
		}

		public void ShowHide()
		{
			this.Visible = !this.Visible;
		}

		private void RefreshGraphics()
		{
			if (_visible)
			{
				foreach (IPresentationImage image in EnumerateVisiblePresentationImages(base.ImageViewer))
				{
					CompositeScaleGraphic scale = GetCompositeScaleGraphic(image, true);
					if (scale != null)
					{
						scale.Visible = true;
						scale.Draw();
					}
				}
			}
			else
			{
				foreach (IPresentationImage image in EnumerateVisiblePresentationImages(base.ImageViewer))
				{
					CompositeScaleGraphic scale = GetCompositeScaleGraphic(image, false);
					if (scale != null)
					{
						scale.Visible = false;
						scale.Draw();
					}
				}
			}
		}

		private void RefreshGraphic(IPresentationImage image)
		{
			if (_visible)
			{
				CompositeScaleGraphic scale = GetCompositeScaleGraphic(image, true);
				if (scale != null)
				{
					scale.Visible = true;
					scale.Draw();
				}
			}
			else
			{
				CompositeScaleGraphic scale = GetCompositeScaleGraphic(image, false);
				if (scale != null)
				{
					scale.Visible = false;
					scale.Draw();
				}
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			base.ImageViewer.EventBroker.DisplaySetChanged += OnDisplaySetChanged;
		}

		protected override void Dispose(bool disposing)
		{
			base.ImageViewer.EventBroker.DisplaySetChanged -= OnDisplaySetChanged;
			base.Dispose(disposing);
		}

		private void OnVisibleChanged()
		{
			RefreshGraphics();
			EventsHelper.Fire(_visibleChanged, this, new EventArgs());
		}

		private void OnDisplaySetChanged(object sender, DisplaySetChangedEventArgs e)
		{
			RefreshGraphics();
		}

		protected override void OnPresentationImageSelected(object sender, PresentationImageSelectedEventArgs e)
		{
			base.OnPresentationImageSelected(sender, e);
			RefreshGraphic(e.SelectedPresentationImage);
		}

		private static CompositeScaleGraphic GetCompositeScaleGraphic(IPresentationImage image, bool createIfNull)
		{
			if (image is IOverlayGraphicsProvider)
			{
				GraphicCollection overlayGraphics = ((IOverlayGraphicsProvider) image).OverlayGraphics;
				CompositeScaleGraphic scale = CollectionUtils.SelectFirst(overlayGraphics,
				                                                          delegate(IGraphic graphic) { return graphic is CompositeScaleGraphic; }
				                              	) as CompositeScaleGraphic;

				if (scale == null && createIfNull)
					overlayGraphics.Insert(0, scale = new CompositeScaleGraphic());

				return scale;
			}

			return null;
		}

		private static IEnumerable<IPresentationImage> EnumerateVisiblePresentationImages(IImageViewer imageViewer)
		{
			foreach (IImageBox imageBox in imageViewer.PhysicalWorkspace.ImageBoxes)
			{
				foreach (ITile tile in imageBox.Tiles)
				{
					if (tile.PresentationImage != null)
						yield return tile.PresentationImage;
				}
			}
		}

		private class CompositeScaleGraphic : CompositeGraphic
		{
			private event EventHandler _changed;
			private readonly ScaleGraphic _horizontalScale;
			private readonly ScaleGraphic _verticalScale;

			public CompositeScaleGraphic()
			{
				base.Graphics.Add(_horizontalScale = new ScaleGraphic());
				base.Graphics.Add(_verticalScale = new ScaleGraphic());

				_horizontalScale.Visible = false;
				_horizontalScale.IsMirrored = true;

				_verticalScale.Visible = false;
				_verticalScale.IsMirrored = true;
			}

			/// <summary>
			/// Indicates that the ScaleGraphic have changed.
			/// </summary>
			public event EventHandler Changed
			{
				add { _changed += value; }
				remove { _changed -= value; }
			}

			/// <summary>
			/// Gets or sets a value indicating the visibility of the <see cref="IGraphic"/>.
			/// </summary>
			public override bool Visible
			{
				get { return base.Visible; }
				set
				{
					if (base.Visible != value)
					{
						base.Visible = value;
						OnChanged();
					}
				}
			}

			/// <summary>
			/// Fires the <see cref="Graphic.Drawing"/> event.  Should be called by an <see cref="IRenderer"/>
			/// for each object just before it is drawn/rendered, hence the reason it is public.
			/// </summary>
			public override void OnDrawing()
			{
				base.CoordinateSystem = CoordinateSystem.Destination;
				_horizontalScale.CoordinateSystem = CoordinateSystem.Destination;
				_verticalScale.CoordinateSystem = CoordinateSystem.Destination;

				try
				{
					Rectangle hScaleBounds = ComputeScaleBounds(base.ParentPresentationImage, 0.10f, 0.05f);
					_horizontalScale.SetEndPoints(new PointF(hScaleBounds.Left, hScaleBounds.Bottom), new SizeF(hScaleBounds.Width, 0));
					_horizontalScale.Visible = true;

					Rectangle vScaleBounds = ComputeScaleBounds(base.ParentPresentationImage, 0.05f, 0.10f);
					_verticalScale.SetEndPoints(new PointF(vScaleBounds.Right, vScaleBounds.Top), new SizeF(0, vScaleBounds.Height));
					_verticalScale.Visible = true;
				}
				catch (ScaleGraphic.UncalibratedImageException)
				{
					_horizontalScale.Visible = false;
					_verticalScale.Visible = false;
				}
				finally
				{
					base.ResetCoordinateSystem();
					_horizontalScale.ResetCoordinateSystem();
					_verticalScale.ResetCoordinateSystem();
				}

				base.OnDrawing();
			}

			protected virtual void OnChanged()
			{
				EventsHelper.Fire(_changed, this, EventArgs.Empty);
				base.Draw();
			}

			/// <summary>
			/// Computes the maximum bounds for scales on a given <see cref="IPresentationImage"/>.
			/// </summary>
			/// <param name="presentationImage">The image to compute bounds for.</param>
			/// <param name="horizontalReduction">The percentage of width to subtract from both the client bounds and the source image bounds.</param>
			/// <param name="verticalReduction">The percentage of height to subtract from both the client bounds and the source image bounds.</param>
			/// <returns>The maximum scale bounds.</returns>
			private static Rectangle ComputeScaleBounds(IPresentationImage presentationImage, float horizontalReduction, float verticalReduction)
			{
				RectangleF clientBounds = presentationImage.ClientRectangle;
				float hReduction = horizontalReduction*Math.Min(1000f, clientBounds.Width);
				float vReduction = verticalReduction*Math.Min(1000f, clientBounds.Height);

				clientBounds = new RectangleF(clientBounds.X + hReduction, clientBounds.Y + vReduction, clientBounds.Width - 2*hReduction, clientBounds.Height - 2*vReduction);

				if (presentationImage is IImageGraphicProvider)
				{
					ImageGraphic imageGraphic = ((IImageGraphicProvider) presentationImage).ImageGraphic;
					Rectangle srcRectangle = new Rectangle(0, 0, imageGraphic.Columns, imageGraphic.Rows);

					RectangleF imageBounds = imageGraphic.SpatialTransform.ConvertToDestination(srcRectangle);
					imageBounds = RectangleUtilities.ConvertToPositiveRectangle(imageBounds);
					hReduction = horizontalReduction*imageBounds.Width;
					vReduction = verticalReduction*imageBounds.Height;

					imageBounds = new RectangleF(imageBounds.X + hReduction, imageBounds.Y + vReduction, imageBounds.Width - 2*hReduction, imageBounds.Height - 2*vReduction);
					return Rectangle.Round(RectangleUtilities.Intersect(imageBounds, clientBounds));
				}
				else
				{
					return Rectangle.Round(clientBounds);
				}
			}
		}
	}
}