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
using ClearCanvas.Desktop;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.Graphics
{
	/// <summary>
	/// An <see cref="UndoableCommand"/> for inserting an <see cref="IGraphic"/> into a <see cref="GraphicCollection"/>.
	/// </summary>
	public class InsertGraphicUndoableCommand : UndoableCommand
	{
		private Command _command;

		/// <summary>
		/// Constructor.
		/// </summary>
		public InsertGraphicUndoableCommand(IGraphic graphic, GraphicCollection parentCollection, int index)
		{
			_command = new InsertGraphicCommand(graphic, parentCollection, index);
		}

		/// <summary>
		/// <see cref="Execute"/>s the insert command.
		/// </summary>
		public override void Execute()
		{
			if (_command is InsertGraphicCommand)
			{
				_command.Execute();
				_command = ((InsertGraphicCommand)_command).GetUndoCommand();
			}
		}

		/// <summary>
		/// <see cref="Unexecute"/>s the insert command (e.g. removes the graphic).
		/// </summary>
		public override void Unexecute()
		{
			if (_command is RemoveGraphicCommand)
			{
				_command.Execute();
				_command = ((RemoveGraphicCommand)_command).GetUndoCommand();
			}
		}
	}

	internal class InsertGraphicCommand : Command
	{
		private GraphicCollection _parentCollection;
		private IGraphic _graphic;
		private int _index;
		
		private RemoveGraphicCommand _undoCommand;

		public InsertGraphicCommand(IGraphic graphic, GraphicCollection parentCollection, int index)
		{
			Platform.CheckForNullReference(graphic, "graphic");
			Platform.CheckForNullReference(parentCollection, "parentCollection");
			Platform.CheckTrue(index >= 0, "Insert index positive");

			_parentCollection = parentCollection;
			_graphic = graphic;
			_index = index;
		}

		private void Validate()
		{
			if (_graphic.ParentGraphic != null)
				throw new InvalidOperationException("The graphic already has a parent.");

			if (_parentCollection.Contains(_graphic))
				throw new InvalidOperationException("The graphic is already in the collection.");

			Platform.CheckArgumentRange(_index, 0, _parentCollection.Count, "Insert index is out of range.");
		}

		public RemoveGraphicCommand GetUndoCommand()
		{
			if (_undoCommand == null)
				throw new InvalidOperationException("The command must be executed first.");

			return _undoCommand;
		}

		public override void Execute()
		{
			if (_undoCommand != null)
				throw new InvalidOperationException("The command has already been executed.");

			Validate();

			_parentCollection.Insert(_index, _graphic);

			_undoCommand = new RemoveGraphicCommand(_graphic);

			_parentCollection = null;
			_graphic = null;
			_index = -1;
		}
	}
}
