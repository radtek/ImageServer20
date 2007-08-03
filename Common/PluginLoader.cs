using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Common
{
	internal class PluginLoader 
	{
		// Private attributes
        private List<Assembly> _pluginList = new List<Assembly>();

		// Constructor
		public PluginLoader() {	}

		// Properties
        public Assembly[] PluginAssemblies
        {
            get { return _pluginList.ToArray(); }
        }

		// Public methods
		public void LoadPlugin(string path)
		{
			Platform.CheckForNullReference(path, "path");
			Platform.CheckForEmptyString(path, "path");

			try
			{
				Assembly asm = Assembly.LoadFrom(path);
                _pluginList.Add(asm);

				Platform.Log(LogLevel.Info, SR.LogPluginLoaded, path);
			}
			catch (FileNotFoundException e)
			{
				Platform.Log(LogLevel.Error, e);
				throw e;
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e);
				throw e;
			}
		}

		private Type GetPluginType(Assembly asm)
		{
			Platform.CheckForNullReference(asm, "asm");

			foreach (Type type in asm.GetExportedTypes())
			{
				if (typeof(PluginInfo).IsAssignableFrom(type))
					return type;
			}

			return null;
		}
	}
}
