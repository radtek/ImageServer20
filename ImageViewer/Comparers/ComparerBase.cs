namespace ClearCanvas.ImageViewer
{
	public abstract class ComparerBase
	{
		private int _returnValue;

		protected ComparerBase()
		{
			Reverse = false;
		}

		protected ComparerBase(bool reverse)
		{
			this.Reverse = reverse;
		}

		public bool Reverse
		{
			get
			{ 
				return ( this.ReturnValue == 1); 
			}
			set
			{
				if (!value)
					this.ReturnValue = -1;
				else
					this.ReturnValue = 1;
			}
		}

		protected int ReturnValue
		{
			get { return _returnValue; }
			set { _returnValue = value; }
		}
	}
}
