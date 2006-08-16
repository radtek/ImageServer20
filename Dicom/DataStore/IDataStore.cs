using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Dicom;

namespace ClearCanvas.Dicom.DataStore
{
    public interface IDataStore
    {
        bool StudyExists(Uid referencedUid);
        bool SeriesExists(Uid referencedUid);
        bool SopInstanceExists(Uid referencedUid);
        ISopInstance GetSopInstance(Uid referencedUid);
        ISeries GetSeries(Uid referenceUid);
        IStudy GetStudy(Uid referenceUid);
        ReadOnlyQueryResultCollection StudyQuery(QueryKey queryKey);
    }
}
