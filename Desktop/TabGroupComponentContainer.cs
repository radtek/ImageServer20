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
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.Desktop
{
    /// <summary>
	/// Defines an extension point for views onto the <see cref="TabGroupComponentContainer"/>
    /// </summary>
	public sealed class TabbedGroupsComponentContainerViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

	/// <summary>
	/// An enumeration describing the available layout directions of the <see cref="TabGroupComponentContainer"/>.
	/// </summary>
    public enum LayoutDirection
    {
		/// <summary>
		/// The layout should be horizontal.
		/// </summary>
        Horizontal = 0,

		/// <summary>
		/// The layout should be vertical.
		/// </summary>
        Vertical = 1
    }

	/// <summary>
	/// An application component that serves as a container for other components, hosted in <see cref="TabGroup"/>s.
	/// </summary>
    [AssociateView(typeof(TabbedGroupsComponentContainerViewExtensionPoint))]
    public class TabGroupComponentContainer : ApplicationComponentContainer
    {
		/// <summary>
		/// A host for <see cref="TabGroup"/>s.
		/// </summary>
        private class TabGroupHost : ApplicationComponentHost
        {
            private TabGroupComponentContainer _owner;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="owner">The container that owns this host.</param>
            /// <param name="tabGroup">The <see cref="TabGroup"/> that is hosted by this object.</param>
			internal TabGroupHost(TabGroupComponentContainer owner, TabGroup tabGroup)
                : base(tabGroup.Component)
            {
                Platform.CheckForNullReference(owner, "owner");
                _owner = owner;
            }

			#region ApplicationComponentHost overrides

			/// <summary>
			/// Gets the title of the parent container.
			/// </summary>
            public override string Title
            {
                get { return _owner.Host.Title; }
                // individual components cannot set the title for the container
                set { throw new NotSupportedException(); }
            }

			/// <summary>
			/// Gets the <see cref="IDesktopWindow"/> that owns the parent container.
			/// </summary>
            public override DesktopWindow DesktopWindow
            {
                get { return _owner.Host.DesktopWindow; }
            }

            #endregion
        }

        private List<TabGroup> _tabGroups;
        private LayoutDirection _layoutDirection;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TabGroupComponentContainer(LayoutDirection layoutDirection)
        {
            _tabGroups = new List<TabGroup>();
            _layoutDirection = layoutDirection;
        }

		/// <summary>
		/// Adds a <see cref="TabGroup"/> to the container.
		/// </summary>
        public void AddTabGroup(TabGroup tg)
        {
            //if (tg != null && tg.ComponentHost != null && tg.ComponentHost.IsStarted)
            //    throw new InvalidOperationException(SR.ExceptionCannotSetTabGroupAfterContainerStarted);

            tg.ComponentHost = new TabGroupHost(this, tg);
            _tabGroups.Add(tg);
        }

		/// <summary>
		/// Gets a list of the <see cref="TabGroup"/>s in the container.
		/// </summary>
		public IList<TabGroup> TabGroups
        {
            get { return _tabGroups.AsReadOnly(); ; }
        }

		/// <summary>
		/// Gets the <see cref="LayoutDirection"/> of the container.
		/// </summary>
        public LayoutDirection LayoutDirection
        {
            get { return _layoutDirection; }
        }

		/// <summary>
		/// Gets the <see cref="TabGroup"/> that owns a particular <see cref="TabPage"/>.
		/// </summary>
        public TabGroup GetTabGroup(TabPage page)
        {
            foreach (TabGroup tg in _tabGroups)
            {
                if (CollectionUtils.Contains<TabPage>(tg.Component.Pages,
                    delegate(TabPage tp) { return tp == page; }))
                {
                    return tg;
                }
            }

            return null;
        }

        #region ApplicationComponent overrides

		/// <summary>
		/// Starts this component and all the contained <see cref="TabGroup"/>s.
		/// </summary>
        public override void Start()
        {
            base.Start();

            foreach (TabGroup tabGroup in _tabGroups)
            {
                tabGroup.ComponentHost.StartComponent();
            }
        }

		/// <summary>
		/// Stops this component and all the contained <see cref="TabGroup"/>s.
		/// </summary>
		public override void Stop()
        {
            foreach (TabGroup tabGroup in _tabGroups)
            {
                tabGroup.ComponentHost.StopComponent();
            }

            base.Stop();
        }

		/// <summary>
		/// Unless overridden, returns the union of all actions in the contained <see cref="TabGroup"/>s.
		/// </summary>
        public override IActionSet ExportedActions
        {
            get
            {
                IActionSet exportedActionSet = new ActionSet(); ;

                // export the actions from all subcomponents
                foreach (TabGroup tabGroup in _tabGroups)
                {
                    exportedActionSet.Union(tabGroup.Component.ExportedActions);
                }

                return exportedActionSet;
            }
        }

        #endregion

        #region ApplicationComponentContainer overrides

		/// <summary>
		/// Enumerates all the <see cref="IApplicationComponent"/>s hosted in the contained <see cref="TabGroup"/>s.
		/// </summary>
        public override IEnumerable<IApplicationComponent> ContainedComponents
        {
            get 
            {
                List<IApplicationComponent> components = new List<IApplicationComponent>();
                foreach (TabGroup tabGroup in _tabGroups)
                {
                    components.AddRange(tabGroup.Component.ContainedComponents);
                }
                return components;
            }
        }

		/// <summary>
		/// Enumerates all the <see cref="IApplicationComponent"/>s hosted 
		/// in the contained <see cref="TabGroup"/>s that are currently visible.
		/// </summary>
		public override IEnumerable<IApplicationComponent> VisibleComponents
        {
            get 
            {
                List<IApplicationComponent> components = new List<IApplicationComponent>();
                foreach (TabGroup tabGroup in _tabGroups)
                {
                    components.AddRange(tabGroup.Component.VisibleComponents);
                }
                return components;
            }
        }

		/// <summary>
		/// Does nothing, since all contained <see cref="IApplicationComponent" />s are already visible.
		/// </summary>
		public override void EnsureVisible(IApplicationComponent component)
        {
            if (!this.IsStarted)
                throw new InvalidOperationException(SR.ExceptionContainerNeverStarted);

            // nothing to do, since the hosted components are started by default
        }

		/// <summary>
		/// Does nothing, since all contained <see cref="IApplicationComponent" />s have already been started.
		/// </summary>
		public override void EnsureStarted(IApplicationComponent component)
        {
            if (!this.IsStarted)
                throw new InvalidOperationException(SR.ExceptionContainerNeverStarted);

            // nothing to do, since the hosted components are visible by default
        }

        #endregion

    }
}
