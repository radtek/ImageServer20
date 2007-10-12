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

using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer
{
	internal class ImageOperationApplicatorMemento : IMemento
	{
		private IList<ImageOriginatorMemento> _imageOriginatorMementos;

		public ImageOperationApplicatorMemento(
			IList<ImageOriginatorMemento> imageOriginatorMementos)
		{
			Platform.CheckForNullReference(imageOriginatorMementos, "imageOriginatorMementos");

			_imageOriginatorMementos = imageOriginatorMementos;
		}

		public IList<ImageOriginatorMemento> ImageOriginatorMementos
		{
			get { return _imageOriginatorMementos; }
		}

		public override bool Equals(object obj)
		{
			Platform.CheckForNullReference(obj, "obj");
			ImageOperationApplicatorMemento imageOperationApplicatorMemento = obj as ImageOperationApplicatorMemento;
			Platform.CheckForInvalidCast(imageOperationApplicatorMemento, "obj", "ImageOperationApplicatorMemento");

			if (this.ImageOriginatorMementos.Count !=
				imageOperationApplicatorMemento.ImageOriginatorMementos.Count)
				return false;

			for (int i = 0; i < this.ImageOriginatorMementos.Count; i++)
			{
				if (!this.ImageOriginatorMementos[i].Memento.Equals(
					imageOperationApplicatorMemento.ImageOriginatorMementos[i].Memento))
					return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			// Normally, you would calculate a hash code dependent on immutable
			// member fields since we've given this object value type semantics
			// because of how we've overridden Equals().  However, this is a memento
			// and thus the actualy contents of the memento are irrelevant if they 
			// were ever put in a hashtable.  We are in fact interested in the instance.
			return base.GetHashCode();
		}
	}
}
