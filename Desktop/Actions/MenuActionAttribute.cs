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

using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Desktop.Actions
{
    /// <summary>
    /// Declares a menu button action with the specifed action identifier and path hint.
    /// </summary>
    public class MenuActionAttribute : ClickActionAttribute
    {
        /// <summary>
        /// Attribute constructor.
        /// </summary>
        /// <param name="actionID">The logical action identifier to associate with this action.</param>
        /// <param name="pathHint">The suggested location of this action in the menu model.</param>
        public MenuActionAttribute(string actionID, string pathHint)
            : base(actionID, pathHint)
        {
        }

        /// <summary>
        /// Attribute constructor.
        /// </summary>
        /// <param name="actionID">The logical action identifier to associate with this action.</param>
        /// <param name="pathHint">The suggested location of this action in the toolbar model.</param>
        /// <param name="clickHandler">Name of the method that will be invoked when the button is clicked.</param>
        public MenuActionAttribute(string actionID, string pathHint, string clickHandler)
            : base(actionID, pathHint, clickHandler)
        {
        }

    	/// <summary>
		/// Creates the <see cref="MenuAction"/> represented by this attribute.
    	/// </summary>
    	/// <param name="actionID">The logical action ID.</param>
    	/// <param name="path">The action path.</param>
    	/// <param name="flags">Flags that specify the click behaviour of the action.</param>
    	/// <param name="resolver">The object used to resolve the action path and icons.</param>
    	protected override ClickAction CreateAction(string actionID, ActionPath path, ClickActionFlags flags, IResourceResolver resolver)
        {
            return new MenuAction(actionID, path, flags, resolver);
        }
    }
}
