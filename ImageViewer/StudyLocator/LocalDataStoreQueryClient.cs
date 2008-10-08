using System;
using System.Collections.Generic;
using System.ServiceModel;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.DataStore;
using ClearCanvas.Dicom.ServiceModel.Query;
using ClearCanvas.ImageViewer.Services.ServerTree;

namespace ClearCanvas.ImageViewer.StudyLocator
{
	internal class LocalDataStoreQueryClient : IStudyRootQuery
	{
		public LocalDataStoreQueryClient()
		{

		}

		public override string ToString()
		{
			return "<local>";
		}

		#region IStudyRootQuery Members

		public IList<StudyRootStudyIdentifier> StudyQuery(StudyRootStudyIdentifier queryQriteria)
		{
			return Query(queryQriteria);
		}

		public IList<SeriesIdentifier> SeriesQuery(SeriesIdentifier queryQriteria)
		{
			return Query(queryQriteria);
		}

		public IList<ImageIdentifier> ImageQuery(ImageIdentifier queryQriteria)
		{
			return Query(queryQriteria);
		}

		#endregion

		private static IList<T> Query<T>(T identifier) where T : Identifier, new()
		{
			if (identifier == null)
			{
				string message = "The query identifier cannot be null.";
				Platform.Log(LogLevel.Error, message);
				throw new FaultException(message);
			}

			DicomAttributeCollection queryCriteria;
			try
			{
				queryCriteria = identifier.ToDicomAttributeCollection();
			}
			catch (DicomException e)
			{
				DataValidationFault fault = new DataValidationFault();
				fault.Description = "Failed to convert contract object to DicomAttributeCollection.";
				Platform.Log(LogLevel.Error, e, fault.Description);
				throw new FaultException<DataValidationFault>(fault, fault.Description);
			}
			catch (Exception e)
			{
				DataValidationFault fault = new DataValidationFault();
				fault.Description = "Unexpected exception when converting contract object to DicomAttributeCollection.";
				Platform.Log(LogLevel.Error, e, fault.Description);
				throw new FaultException<DataValidationFault>(fault, fault.Description);
			}

			try
			{
				using (IDataStoreReader reader = DataAccessLayer.GetIDataStoreReader())
				{
					IEnumerable<DicomAttributeCollection> results = reader.Query(queryCriteria);

					List<T> queryResults = new List<T>();
					foreach (DicomAttributeCollection result in results)
					{
						T queryResult = Identifier.FromDicomAttributeCollection<T>(result);

						queryResult.InstanceAvailability = "ONLINE";
						queryResult.RetrieveAeTitle = ServerTree.GetClientAETitle();
						queryResults.Add(queryResult);
					}

					return queryResults;
				}
			}
			catch (Exception e)
			{
				QueryFailedFault fault = new QueryFailedFault();
				fault.Description = String.Format("Unexpected error while processing study root query.");
				Platform.Log(LogLevel.Error, e, fault.Description);
				throw new FaultException<QueryFailedFault>(fault, fault.Description);
			}
		}
	}
}