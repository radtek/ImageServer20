#if UNIT_TESTS
using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using ClearCanvas.ImageServer.Dicom;
using ClearCanvas.ImageServer.Dicom.Exceptions;

namespace ClearCanvas.ImageServer.Dicom.Tests
{
    [TestFixture]
    public class FileTest : AbstractTest
    {
        [Test]
        public void ConstructorTests()
        {
            DicomFile file = new DicomFile(null);

            file = new DicomFile("filename");

            file = new DicomFile(null, new AttributeCollection(), new AttributeCollection());


        }

        private void SetupMetaInfo(DicomFile theFile)
        {
            AttributeCollection theSet = theFile.MetaInfo;

            theSet[DicomTags.MediaStorageSOPClassUID].SetStringValue(theFile.DataSet[DicomTags.SOPClassUID].ToString());
            theSet[DicomTags.MediaStorageSOPInstanceUID].SetStringValue(theFile.DataSet[DicomTags.SOPInstanceUID].ToString());
            theFile.TransferSyntax = TransferSyntax.ExplicitVRLittleEndian; 

            theSet[DicomTags.ImplementationClassUID].SetStringValue("1.1.1.1.1.11.1");
            theSet[DicomTags.ImplementationVersionName].SetStringValue("CC ImageServer 1.0");
        }


        public void WriteOptionsTest(DicomFile sourceFile, DicomWriteOptions options)
        {
            bool result = sourceFile.Save(options);

            Assert.AreEqual(result, true);

            DicomFile newFile = new DicomFile("CreateFileTest.dcm");

            DicomReadOptions readOptions = DicomReadOptions.Default;
            newFile.Load(readOptions);

            Assert.AreEqual(sourceFile.DataSet.Equals(newFile.DataSet), true);

            // update a tag, and make sure it shows they're different
            newFile.DataSet[DicomTags.PatientsName].Values = "NewPatient^First";
            Assert.AreEqual(sourceFile.DataSet.Equals(newFile.DataSet), false);

            // Now update the original file with the name, and they should be the same again
            sourceFile.DataSet[DicomTags.PatientsName].Values = "NewPatient^First";
            Assert.AreEqual(sourceFile.DataSet.Equals(newFile.DataSet), true);

            // Return the original string value.
            sourceFile.DataSet[DicomTags.PatientsName].SetStringValue("Patient^Test");

        }

        [Test]
        public void CreateFileTest()
        {
            DicomFile file = new DicomFile("CreateFileTest.dcm");

            AttributeCollection dataSet = file.DataSet;

            AttributeCollection metaInfo = file.DataSet;


            SetupMR(dataSet);

            SetupMetaInfo(file);

            DicomWriteOptions writeOptions = DicomWriteOptions.Default;
            WriteOptionsTest(file, writeOptions);

            writeOptions = DicomWriteOptions.ExplicitLengthSequence;
            WriteOptionsTest(file, writeOptions);

            writeOptions = DicomWriteOptions.ExplicitLengthSequenceItem;
            WriteOptionsTest(file, writeOptions);

            writeOptions = DicomWriteOptions.ExplicitLengthSequence | DicomWriteOptions.ExplicitLengthSequenceItem;
            WriteOptionsTest(file, writeOptions);

            writeOptions = DicomWriteOptions.None;
            WriteOptionsTest(file, writeOptions);

        }
    }
}
#endif