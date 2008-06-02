#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.ProtocollingWorkflow;

namespace ClearCanvas.Ris.Client.Reporting
{
	/// <summary>
	/// Extension point for views onto <see cref="ProtocolReasonComponent"/>
	/// </summary>
	[ExtensionPoint]
	public class ProtocolReasonComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	/// <summary>
	/// ProtocolReasonComponent class
	/// </summary>
	[AssociateView(typeof(ProtocolReasonComponentViewExtensionPoint))]
	public class ProtocolReasonComponent : ApplicationComponent
	{
		private EnumValueInfo _selectedReason;
		private List<EnumValueInfo> _availableReasons;
		private string _otherReason;

		public override void Start()
		{
			Platform.GetService<IProtocollingWorkflowService>(
				delegate(IProtocollingWorkflowService service)
				{
					GetSuspendRejectReasonChoicesResponse response =
						service.GetSuspendRejectReasonChoices(new GetSuspendRejectReasonChoicesRequest());
					_availableReasons = response.SuspendRejectReasonChoices;
				});

			// TODO prepare the component for its live phase
			base.Start();
		}

		#region PresentationModel

		public EnumValueInfo Reason
		{
			get { return _selectedReason; }
		}

		public string SelectedReasonChoice
		{
			get
			{
				return _selectedReason == null ? "" : _selectedReason.Value;
			}
			set
			{
				_selectedReason = (value == "")
					? null
					: CollectionUtils.SelectFirst<EnumValueInfo>(
						_availableReasons,
						delegate(EnumValueInfo reason) { return reason.Value == value; });
			}
		}

		public List<string> ReasonChoices
		{
			get { return EnumValueUtils.GetDisplayValues(_availableReasons); }
		}

		public string OtherReason
		{
			get { return _otherReason; }
			set { _otherReason = value; }
		}

		public bool OkayEnabled
		{
			get { return _selectedReason != null; }
		}

		public void Okay()
		{
			this.Exit(ApplicationComponentExitCode.Accepted);
		}

		public void Cancel()
		{
			this.Exit(ApplicationComponentExitCode.None);
		}

		#endregion
	}
}
