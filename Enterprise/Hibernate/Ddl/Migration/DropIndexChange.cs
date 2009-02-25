using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Enterprise.Hibernate.Ddl.Migration
{
    class DropIndexChange : Change
    {
    	private readonly IndexInfo _index;

        public DropIndexChange(TableInfo table, IndexInfo index)
			: base(table)
		{
        	_index = index;
        }

		public IndexInfo Index
		{
			get { return _index; }
		}

		public override Statement[] GetStatements(IRenderer renderer)
		{
			return renderer.Render(this);
		}
	}
}
