using System.Drawing;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities.StudyBuilder;

namespace ClearCanvas.ImageViewer.Utilities.StudyComposer
{
	/// <summary>
	/// A <see cref="StudyComposerItemBase{T}"/> that wraps a <see cref="SopInstanceNode"/> in a <see cref="StudyBuilder"/> tree.
	/// </summary>
	public class ImageItem : StudyComposerItemBase<SopInstanceNode>
	{
		private Image _baseIcon;
		private string _name = SR.FormatStudyComposerGenericImageLabelCaption;

		public ImageItem(SopInstanceNode sopInstance, IPresentationImage pImage)
		{
			base.Node = sopInstance;

			int num = sopInstance.DicomData[DicomTags.InstanceNumber].GetInt32(0, -1);
			if (num >= 0)
				_name = string.Format(SR.FormatStudyComposerImageLabelCaption, num);

			string seriesDesc = sopInstance.DicomData[DicomTags.SeriesDescription].GetString(0, "");
			if (seriesDesc.Length > 0)
				_name = string.Format("{0}: {1}", seriesDesc, _name);

			if (pImage != null)
				base.Icon = _baseIcon = _helper.CreateImageIcon(pImage);
		}

		private ImageItem(ImageItem source) : this(source.Node.Copy(), null)
		{
			this._name = source._name;
			this._baseIcon = (Image)source._baseIcon.Clone();
			this.Icon = (Image)source.Icon.Clone();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public ImageItem Copy()
		{
			return new ImageItem(this);
		}

		#region Overrides

		/// <summary>
		/// Gets or sets the name label of this item.
		/// </summary>
		public override string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets a short, multi-line description of the item that contains ancillary information.
		/// </summary>
		public override string Description
		{
			get { return base.Node.InstanceUid; }
		}

		/// <summary>
		/// Regenerates the icon for a specific icon size.
		/// </summary>
		/// <param name="iconSize">The <see cref="Size"/> of the icon to generate.</param>
		public override void UpdateIcon(Size iconSize)
		{
			if (_baseIcon.Size != iconSize)
			{
				// if requesting a new icon size
				_helper.IconSize = iconSize;
				base.Icon = _helper.CreateImageIcon(_baseIcon);
			}
			else
			{
				base.Icon = _baseIcon;
			}
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public override StudyComposerItemBase<SopInstanceNode> Clone()
		{
			return this.Copy();
		}

		protected override void OnNodePropertyChanged(string propertyName)
		{
			base.OnNodePropertyChanged(propertyName);
			if (propertyName == "InstanceUid")
				FirePropertyChanged("Description");
		}

		#endregion

		#region Statics

		private static readonly IconHelper _helper = new IconHelper(64, 64);

		static ImageItem()
		{
			_helper.IconSize = new Size(64, 64);
		}

		#endregion
	}
}