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
using System.Collections.Generic;
using ClearCanvas.Common;

namespace ClearCanvas.Desktop
{
    /// <summary>
	/// Defines an extension point for views onto the <see cref="SimpleComponentContainer"/>.
    /// </summary>
	public sealed class SimpleComponentContainerViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

	/// <summary>
	/// A simple container class for hosting <see cref="IApplicationComponent"/>s
	/// that provides Ok and Cancel buttons.
	/// </summary>
    [AssociateView(typeof(SimpleComponentContainerViewExtensionPoint))]
    public class SimpleComponentContainer : ApplicationComponentContainer
    {
        private class HostImpl : ContainedComponentHost
        {
            internal HostImpl(
                SimpleComponentContainer owner,
                IApplicationComponent component)
                : base(owner, component)
            {
            }

            #region ApplicationComponentHost overrides

			/// <summary>
			/// Contained components will use the comand history provided by the host that 
			/// owns the container.
			/// </summary>
			public override CommandHistory CommandHistory
			{
				get
				{
					return OwnerHost.CommandHistory;
				}
			}

			/// <summary>
            /// Gets or sets the title displayed in the user-interface.
            /// </summary>
            public override string Title
            {
                set { OwnerHost.Title = value; }
            }

            #endregion
        }


		private readonly IApplicationComponent _component;
        private readonly HostImpl _componentHost;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleComponentContainer(IApplicationComponent component)
		{
			_component = component;
            _componentHost = new HostImpl(this, _component);
		}

		/// <summary>
		/// The host object for the contained <see cref="IApplicationComponent"/>.
		/// </summary>
		public ApplicationComponentHost ComponentHost
        {
            get { return _componentHost; }
        }

        #region ApplicationComponent overrides

		/// <summary>
		/// Starts this component and the <see cref="ComponentHost"/>.
		/// </summary>
		///  <remarks>
		/// Override this method to implement custom initialization logic.  Overrides must be sure to call the base implementation.
		/// </remarks>
		public override void Start()
        {
			base.Start();

			_componentHost.StartComponent();
        }

		/// <summary>
		/// Stops this component and the <see cref="ComponentHost"/>.
		/// </summary>
		/// <remarks>
		/// Override this method to implement custom termination logic.  Overrides must be sure to call the base implementation.
		/// </remarks>
		public override void Stop()
        {
            _componentHost.StopComponent();

            base.Stop();
        }

		/// <summary>
		/// Gets a value indicating whether there are any data validation errors.
		/// </summary>
		public override bool HasValidationErrors
		{
			get { return _componentHost.Component.HasValidationErrors || base.HasValidationErrors; }
		}

		/// <summary>
		/// Sets the <see cref="ApplicationComponent.ValidationVisible"/> property and raises the 
		/// <see cref="ApplicationComponent.ValidationVisibleChanged"/> event.
		/// </summary>
		public override void ShowValidation(bool show)
		{
			base.ShowValidation(show);
			_componentHost.Component.ShowValidation(show);
		}

        #endregion

        #region ApplicationComponentContainer overrides

		/// <summary>
		/// Gets an enumeration of the contained components.
		/// </summary>
		public override IEnumerable<IApplicationComponent> ContainedComponents
        {
            get { return new IApplicationComponent[] { _componentHost.Component }; }
        }

		/// <summary>
		/// Gets an enumeration of the components that are currently visible.
		/// </summary>
		public override IEnumerable<IApplicationComponent> VisibleComponents
        {
            get { return this.ContainedComponents; }
        }

		/// <summary>
		/// Does nothing, since the hosted component is started by default.
		/// </summary>
		public override void EnsureStarted(IApplicationComponent component)
        {
            if (!this.IsStarted)
                throw new InvalidOperationException(SR.ExceptionContainerNeverStarted);

            // nothing to do, since the hosted component is started by default
        }

		/// <summary>
		/// Does nothing, since the hosted component is visible by default.
		/// </summary>
		public override void EnsureVisible(IApplicationComponent component)
        {
            if (!this.IsStarted)
                throw new InvalidOperationException(SR.ExceptionContainerNeverStarted);

            // nothing to do, since the hosted component is visible by default
        }

        #endregion

        #region Presentation Model

		/// <summary>
		/// Called by the view to indicate the user dismissed the dialog with "Ok"; the <see cref="ApplicationComponent.ExitCode"/>
		/// is set to <see cref="ApplicationComponentExitCode.Accepted"/>.
		/// </summary>
		public void OK()
		{
			if (this.HasValidationErrors)
			{
				this.ShowValidation(true);
				return;
			}
			base.ExitCode = ApplicationComponentExitCode.Accepted;
			base.Host.Exit();
		}

		/// <summary>
		/// Called by the view to indicate the user dismissed the dialog with "Cancel"; the <see cref="ApplicationComponent.ExitCode"/>
		/// is set to <see cref="ApplicationComponentExitCode.None"/>.
		/// </summary>
		public void Cancel()
		{
			base.ExitCode = ApplicationComponentExitCode.None;
            base.Host.Exit();
        }

        #endregion
    }
}
