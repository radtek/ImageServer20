using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace ClearCanvas.Common.Configuration
{
    [ExtensionPoint]
    public class ConfigurationStoreExtensionPoint : ExtensionPoint<IConfigurationStore>
    {
    }

    /// <summary>
    /// This class is the standard settings provider that should be used by all settings classes that operate
    /// within the ClearCanvas framework.  Internally, this class will delegate the storage of settings between
    /// the local file system and an implemetation of <see cref="EnterpriseConfigurationStoreExtensionPoint"/>,
    /// if an extension is found.  All methods on this class are thread-safe, as per MSDN guidelines.
    /// </summary>
    public class StandardSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private string _appName;
        private SettingsProvider _sourceProvider;
        private object _syncLock = new object();


        public StandardSettingsProvider()
        {
            // according to MSDN recommendation, use the name of the executing assembly here
            _appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        #region SettingsProvider overrides

        public override string ApplicationName
        {
            get
            {
                return _appName;
            }
            set
            {
                _appName = value;
            }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            lock (_syncLock)
            {
                // obtain a source provider
                try
                {
                    IConfigurationStore ecs = (IConfigurationStore)(new ConfigurationStoreExtensionPoint()).CreateExtension();
                    _sourceProvider = new ConfigurationStoreSettingsProvider(ecs);
                }
                catch (NotSupportedException)
                {
                    Platform.Log(SR.LogConfigurationStoreNotFound, LogLevel.Warn);

                    // default to LocalFileSettingsProvider as a last resort
                    _sourceProvider = new LocalFileSettingsProvider();
                }

                // init source provider
                // according to sample implementations, use the application name here
                _sourceProvider.Initialize(this.ApplicationName, config);
                base.Initialize(this.ApplicationName, config);
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
        {
            lock (_syncLock)
            {
                Type settingsClass = (Type)context["SettingsClassType"];

                SettingsPropertyValueCollection values = _sourceProvider.GetPropertyValues(context, props);
                foreach (SettingsPropertyValue value in values)
                {
					if (value.SerializedValue == null || (value.SerializedValue is string) && ((string)value.SerializedValue) == ((string)value.Property.DefaultValue))
					{
						value.SerializedValue = SettingsClassMetaDataReader.TranslateDefaultValue(settingsClass,
							(string)value.Property.DefaultValue);
					}
                }
                return values;
            }
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection settings)
        {
            lock (_syncLock)
            {
                _sourceProvider.SetPropertyValues(context, settings);
            }
        }

        #endregion


        #region IApplicationSettingsProvider Members

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            lock (_syncLock)
            {
                if (_sourceProvider is IApplicationSettingsProvider)
                {
                    return (_sourceProvider as IApplicationSettingsProvider).GetPreviousVersion(context, property);
                }
                else
                {
                    // fail silently as per MSDN 
                    return new SettingsPropertyValue(property);
                }
            }
        }

        public void Reset(SettingsContext context)
        {
            lock (_syncLock)
            {
                if (_sourceProvider is IApplicationSettingsProvider)
                {
                    (_sourceProvider as IApplicationSettingsProvider).Reset(context);
                }
                else
                {
                    // fail silently as per MSDN 
                }
            }
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            lock (_syncLock)
            {
                if (_sourceProvider is IApplicationSettingsProvider)
                {
                    (_sourceProvider as IApplicationSettingsProvider).Upgrade(context, properties);
                }
                else
                {
                    // fail silently as per MSDN 
                }
            }
        }

        #endregion


    }
}
