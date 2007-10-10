using System;
using System.Windows.Forms;
using ClearCanvas.Common;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.Desktop.View.WinForms
{
    public class ActiveMenuItem : ToolStripMenuItem
    {
        private IClickAction _action;
        private EventHandler _actionEnabledChangedHandler;
        private EventHandler _actionCheckedChangedHandler;
		private EventHandler _actionVisibleChangedHandler;
		private EventHandler _actionLabelChangedHandler;

        public ActiveMenuItem(IClickAction action)
        {
            _action = action;

            _actionEnabledChangedHandler = new EventHandler(OnActionEnabledChanged);
            _actionCheckedChangedHandler = new EventHandler(OnActionCheckedChanged);
			_actionVisibleChangedHandler = new EventHandler(OnActionVisibleChanged);
			_actionLabelChangedHandler = new EventHandler(OnActionLabelChanged);

            _action.EnabledChanged += _actionEnabledChangedHandler;
            _action.CheckedChanged += _actionCheckedChangedHandler;
			_action.VisibleChanged += _actionVisibleChangedHandler;
			_action.LabelChanged += _actionLabelChangedHandler;
			
            this.Text = _action.Label;
            this.Checked = _action.Checked;

            UpdateVisibility();
            UpdateEnablement();
			

            this.Click += delegate(object sender, EventArgs e)
            {
                _action.Click();
            };

            if (_action.IconSet != null && _action.ResourceResolver != null)
            {
                try
                {
                    this.Image = IconFactory.CreateIcon(_action.IconSet.SmallIcon, _action.ResourceResolver);
                }
                catch (Exception e)
                {
                    // the icon was either null or not found - log some helpful message
                    Platform.Log(LogLevel.Error, e);
                }
            }

            try
            {
                this.ShortcutKeys = (Keys)_action.KeyStroke;
            }
            catch (Exception e)
            {
                Platform.Log(LogLevel.Error, e);
            }
        }

        private void OnActionCheckedChanged(object sender, EventArgs e)
        {
            this.Checked = _action.Checked;
        }

        private void OnActionEnabledChanged(object sender, EventArgs e)
        {
            UpdateEnablement();
        }

		private void OnActionVisibleChanged(object sender, EventArgs e)
		{
            UpdateVisibility();
		}

		private void OnActionLabelChanged(object sender, EventArgs e)
		{
			this.Text = _action.Label;
		}

		protected override void Dispose(bool disposing)
        {
            if (disposing && _action != null)
            {
                // VERY IMPORTANT: instances of this class will be created and discarded frequently
                // throughout the lifetime of the application
                // therefore is it extremely important that the event handlers are disconnected
                // from the underlying _action events
                // otherwise, this object will hang around for the entire lifetime of the _action object,
                // even though this object is no longer needed
                _action.EnabledChanged -= _actionEnabledChangedHandler;
                _action.CheckedChanged -= _actionCheckedChangedHandler;
				_action.VisibleChanged -= _actionVisibleChangedHandler;
				_action.LabelChanged -= _actionLabelChangedHandler;

                _action = null;
            }
            base.Dispose(disposing);
        }

        private void UpdateVisibility()
        {
            this.Visible = _action.Visible && (_action.Permissible || DesktopViewSettings.Default.ShowNonPermissibleActions);
        }

        private void UpdateEnablement()
        {
            this.Enabled = _action.Enabled && (_action.Permissible || DesktopViewSettings.Default.EnableNonPermissibleActions);
        }
        
    }
}
