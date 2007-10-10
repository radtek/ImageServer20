using System;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Forms;

namespace ClearCanvas.Desktop.View.WinForms
{
    /// <summary>
    /// Form used by the <see cref="DialogView"/> class.
    /// </summary>
    /// <remarks>
    /// This class may be subclassed.
    /// </remarks>
    public partial class DialogBoxForm : DotNetMagicForm
    {
        private Control _content;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public DialogBoxForm(string title, Control content)
        {
            InitializeComponent();
            this.Text = title;

            _content = content;

            // important - if we do not set a minimum size, the full content may not be displayed
            _content.MinimumSize = _content.Size;
            _content.Dock = DockStyle.Fill;

            // force the dialog client size to the size of the content
            this.ClientSize = _content.Size;
            _contentPanel.Controls.Add(_content);

            // Resize the dialog if size of the underlying content changed
            _content.SizeChanged += new EventHandler(OnContentSizeChanged);
        }

        private void OnContentSizeChanged(object sender, EventArgs e)
        {
            this.ClientSize = _content.Size;
        }
    }
}