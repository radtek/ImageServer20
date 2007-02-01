using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.ImageViewer.InteractiveGraphics
{
	public class StandardGraphicState : GraphicState
	{
		protected StandardGraphicState(IStandardStatefulGraphic interactiveGraphic)
			: base(interactiveGraphic)
		{

		}

		protected IStandardStatefulGraphic StandardStatefulGraphic
		{
			get { return this.StatefulGraphic as IStandardStatefulGraphic; }
		}
	}
}
