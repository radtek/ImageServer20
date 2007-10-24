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
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.ImageViewer.Annotations
{
	[ExtensionPoint()]
	public class AnnotationItemProviderExtensionPoint : ExtensionPoint<IAnnotationItemProvider>
	{ 
	}

	// TODO: Remove this class
	internal sealed class AnnotationItemProviderManager
	{
		private static readonly AnnotationItemProviderManager _instance = new AnnotationItemProviderManager();
		private List<IAnnotationItemProvider> _providers;

		private AnnotationItemProviderManager()
		{
			_providers = null;
		}

		private void Initialize()
		{
			if (_providers == null)
			{
				_providers = new List<IAnnotationItemProvider>();

				foreach (object extension in new AnnotationItemProviderExtensionPoint().CreateExtensions())
				{
					try
					{
						_providers.Add((IAnnotationItemProvider)extension);
					}
					catch (Exception e)
					{
						Platform.Log(LogLevel.Warn, e);
					}
				}
			}
		}

		public static AnnotationItemProviderManager Instance
		{
			get { return _instance; }
		}

		public IEnumerable<IAnnotationItemProvider> Providers
		{
			get
			{
				Initialize();
				return _providers;
			}
		}

		public IAnnotationItem GetAnnotationItem(string annotationItemIdentifier)
		{
			foreach (IAnnotationItemProvider provider in this.Providers)
			{
				foreach (IAnnotationItem item in provider.GetAnnotationItems())
				{
					if (item.GetIdentifier() == annotationItemIdentifier)
						return item;
				}
			}

			return null;
		}
	}
}
