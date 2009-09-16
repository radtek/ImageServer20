#region License

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

using System.Collections.Generic;
using System.ComponentModel;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Configuration;

namespace ClearCanvas.ImageViewer.Layout.Basic
{
	[ExtensionPoint]
	public sealed class DisplaySetCreationConfigurationComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	[AssociateView(typeof(DisplaySetCreationConfigurationComponentViewExtensionPoint))]
	public class DisplaySetCreationConfigurationComponent : ConfigurationApplicationComponent
	{
		private BindingList<StoredDisplaySetCreationOptions> _options;

		public DisplaySetCreationConfigurationComponent()
		{
		}

		public override void Start()
		{
			Initialize();
			base.Start();
		}

		public override void Save()
		{
			DisplaySetCreationSettings.Default.Save(_options);
		}

		private void Initialize()
		{
			List<StoredDisplaySetCreationOptions> sortedOptions = DisplaySetCreationSettings.Default.GetStoredOptions();
			sortedOptions = CollectionUtils.Sort(sortedOptions, 
					delegate(StoredDisplaySetCreationOptions options1, StoredDisplaySetCreationOptions options2)
						{ return options1.Modality.CompareTo(options2.Modality); });

			_options = new BindingList<StoredDisplaySetCreationOptions>(sortedOptions);

			foreach (StoredDisplaySetCreationOptions options in _options)
				options.PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.Modified = true;
		}

		public BindingList<StoredDisplaySetCreationOptions> Options
		{
			get { return _options; }
		}
	}
}