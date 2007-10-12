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

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Helps clients raise events safely.
	/// </summary>
	public class EventsHelper
	{
		/// <summary>
		/// Raises events safely.
		/// </summary>
		/// <param name="del">Delegate.</param>
		/// <param name="args">Parameters to be passed to the delegate.</param>
		/// <remarks>Use this method to safely invoke user code via delegates.
		/// Because there is no guarantee that user code will not throw
		/// exceptions, it has to be executed safely.  That is, any exceptions thrown
		/// must be handled.  This method will log any exceptions thrown in user code.
		/// Whether the exception is rethrown depends on how the exception policy
		/// has been configured.  The typical usage is shown below.</remarks>
		/// <example>
		/// <code>
		/// [C#]
		/// public class PresentationImage
		/// {
		///    private event EventHandler _imageDrawingEvent;
		///    
		///    public void Draw()
		///    {
		///       EventsHelper.Fire(_imageDrawingEvent, this, new DrawImageEventArgs());
		///    }
		/// }
		/// </code>
		/// </example>
		public static void Fire(Delegate del, params object[] args)
		{
			if (del == null)
				return;

			Delegate[] delegates = del.GetInvocationList();

			foreach(Delegate sink in delegates)
			{
				try
				{
					sink.DynamicInvoke(args);
				}
				catch (Exception e)
				{
                    Platform.Log(LogLevel.Error, e);
					throw e;
				}
			}
		}
	}
}
