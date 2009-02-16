using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer
{
	public partial class ImageViewerComponent
	{
		internal class SingleStudyLoader : IDisposable
		{
			private readonly LoadStudyArgs _args;
			private readonly List<Sop> _sops;
			private int _total;
			private int _failed;

			public SingleStudyLoader(LoadStudyArgs args)
			{
				_args = args;
				_sops = new List<Sop>();
				_total = 0;
				_failed = 0;
			}

			public int Total
			{
				get { return _total; }
			}

			public int Failed
			{
				get { return _failed; }
			}

			public void LoadStudy(ImageViewerComponent viewer)
			{
				LoadSops(viewer.StudyLoaders);
				AddSops(viewer);
			}

			public void LoadSops(StudyLoaderMap studyLoaders)
			{
				IStudyLoader studyLoader = studyLoaders[_args.StudyLoaderName];
				int total;
				try
				{
					total = studyLoader.Start(new StudyLoaderArgs(_args.StudyInstanceUid, _args.Server));
				}
				catch (Exception e)
				{
					OpenStudyException ex = new OpenStudyException("Failed to load any of the requested images.", e);
					throw ex;
				}

				try
				{
					while (true)
					{
						Sop sop = studyLoader.LoadNextSop();
						if (sop == null)
							break;

						_sops.Add(sop);
					}
				}
				catch (Exception e)
				{
					string message = String.Format("Failed to load Sops for study with uid '{0}'", _args.StudyInstanceUid);
					Platform.Log(LogLevel.Error, e, message);

					foreach (Sop sop in _sops)
						sop.Dispose();

					_sops.Clear();

					OpenStudyException ex = new OpenStudyException(message, e);
					ex.TotalImages = total;
					ex.FailedImages = total;
					throw ex;
				}
			}

			public void AddSops(ImageViewerComponent viewer)
			{
				List<Sop> sops = new List<Sop>(_sops);
				_sops.Clear();
				foreach (Sop sop in sops)
				{
					try
					{
						viewer.StudyTree.AddSop(sop);
					}
					catch(SopValidationException e)
					{
						++_failed;
						Platform.Log(LogLevel.Error, e);
						sop.Dispose();
					}
					catch (Exception e)
					{
						++_failed;
						Platform.Log(LogLevel.Error, e);
					}

					++_total;
				}

				Study study = viewer.StudyTree.GetStudy(_args.StudyInstanceUid);
				if (study != null)
					viewer.EventBroker.OnStudyLoaded(new ItemEventArgs<Study>(study));

				VerifyLoad(Total, Failed);

				IPrefetchingStrategy prefetchingStrategy = viewer.StudyLoaders[_args.StudyLoaderName].PrefetchingStrategy;
				if (prefetchingStrategy != null)
					prefetchingStrategy.Start(viewer);
			}

			#region IDisposable Members

			public void Dispose()
			{
				try
				{
					foreach (Sop sop in _sops)
						sop.Dispose();

					_sops.Clear();

					GC.SuppressFinalize(this);
				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Error, e);
				}
			}

			#endregion
		}
	}
}