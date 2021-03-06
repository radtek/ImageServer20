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
using System.Threading;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.ImageViewer.Annotations;

namespace ClearCanvas.ImageViewer.Thumbnails
{
	public partial class ThumbnailComponent
	{
		public delegate void ThumbnailLoadedCallback(Thumbnail thumbnail);

		public class Thumbnail : IGalleryItem, IDisposable
		{
			private static readonly int DefaultIconWidth = 100;
			private static readonly int DefaultIconHeight = 100;

			private readonly IDisplaySet _displaySet;

			private volatile IPresentationImage _image;
			private volatile Image _icon;

			private volatile SynchronizationContext _uiThreadContext;
			private volatile bool _loading = false;
			private volatile bool _disposed = false;

			private ThumbnailLoadedCallback _loadedCallback;

			private int _iconWidth;
			private int _iconHeight;

			public Thumbnail(IDisplaySet displaySet, ThumbnailLoadedCallback loadedCallback)
				: this(displaySet, loadedCallback, DefaultIconWidth, DefaultIconHeight)
			{
			}

			public Thumbnail(IDisplaySet displaySet, ThumbnailLoadedCallback loadedCallback, int width, int height)
			{
				_displaySet = displaySet;

				_iconWidth = width;
				_iconHeight = height;

				_image = GetMiddlePresentationImage(displaySet);
				if (_image != null)
				{
					_image = _image.CreateFreshCopy();
					_icon = CreateDummyBitmap(SR.MessageLoading, _iconWidth, _iconHeight);
				}
				else
				{
					_icon = CreateDummyBitmap(SR.MessageNoImages, _iconWidth, _iconHeight);
				}

				_uiThreadContext = SynchronizationContext.Current;
				_loadedCallback = loadedCallback;
				_loading = false;
				_disposed = false;
			}

			#region IGalleryItem Members

			public Image Image
			{
				get { return _icon; }
			}

			public string Name
			{
				get
				{
					string name = _displaySet.Name;
					name = name.Replace("\r\n", " ");
					name = name.Replace("\r", " ");
					name = name.Replace("\n", " ");

					int number = _displaySet.PresentationImages.Count;
					if (number <= 1)
						return String.Format(SR.FormatThumbnailName1Image, name);
					
					return String.Format(SR.FormatThumbnailName, number, name);
				}
				set { throw new NotSupportedException("Renaming thumbnails is not allowed."); }
			}

			public string Description
			{
				get { return String.Empty; }
			}

			public object Item
			{
				get { return _displaySet; }
			}

			#endregion

			public void LoadAsync()
			{
				if (_image == null)
					return;

				_loading = true;
				ThreadPool.QueueUserWorkItem(LoadAsync);
			}

			#region IDisposable Members

			public void Dispose()
			{
				_disposed = true;
				if (_loading)
					return;

				DisposeInternal();
			}

			#endregion

			#region Private Methods

			private void DisposeInternal()
			{
				_disposed = true;

				_uiThreadContext = null;
				_loadedCallback = null;

				if (_image != null)
				{
					_image.Dispose();
					_image = null;
				}

				if (_icon != null)
				{
					_icon.Dispose();
					_icon = null;
				}
			}

			private void LoadAsync(object ignored)
			{
				Bitmap icon;
				try
				{
					icon = CreateBitmap(_image, _iconWidth, _iconHeight);
				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Error, e);
					icon = CreateDummyBitmap(SR.MessageLoadFailed, _iconWidth, _iconHeight);
				}

				_uiThreadContext.Post(this.OnLoaded, icon);
			}

			private void OnLoaded(object icon)
			{
				_icon.Dispose();
				_icon = (Bitmap) icon;

				_loading = false;

				if (!_disposed)
					_loadedCallback(this);
				else
					DisposeInternal();
			}

			private static Bitmap CreateDummyBitmap(string message, int width, int height)
			{
				Bitmap bmp = new Bitmap(width, height);
				System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bmp);

				Brush brush = new SolidBrush(Color.WhiteSmoke);
				Font font = new Font("Arial", 10.0f);

				StringFormat format = new StringFormat();
				format.Alignment = StringAlignment.Center;
				format.LineAlignment = StringAlignment.Center;

				graphics.DrawString(message, font, brush, new Rectangle(0, 0, width, height), format);
				DrawBorder(graphics, width, height);
				graphics.Dispose();

				format.Dispose();
				font.Dispose();
				brush.Dispose();

				return bmp;
			}

			private static Bitmap CreateBitmap(IPresentationImage image, int width, int height)
			{
				image = image.CreateFreshCopy();

				if (image is IAnnotationLayoutProvider)
					((IAnnotationLayoutProvider) image).AnnotationLayout.Visible = false;

				Bitmap bmp = image.DrawToBitmap(width, height);
				System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bmp);

				DrawBorder(graphics, width, height);

				image.Dispose();
				graphics.Dispose();

				return bmp;
			}

			private static void DrawBorder(System.Drawing.Graphics graphics, int width, int height)
			{
				using (Pen pen = new Pen(Color.DarkGray))
				{
					graphics.DrawRectangle(pen, 0, 0, width - 1, height - 1);
				}
			}

			private static IPresentationImage GetMiddlePresentationImage(IDisplaySet displaySet)
			{
				if (displaySet.PresentationImages.Count == 0)
					return null;

				if (displaySet.PresentationImages.Count <= 2)
					return displaySet.PresentationImages[0];

				return displaySet.PresentationImages[displaySet.PresentationImages.Count/2];
			}

			#endregion
		}
	}
}