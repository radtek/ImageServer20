#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

#if UNIT_TESTS
using ClearCanvas.Dicom.Codec;
using ClearCanvas.Dicom.Codec.Rle;
using ClearCanvas.Dicom.Tests;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Codec.Rle.Tests
{
	//TODO: this test won't work anymore because the codec registry uses extensions.
    [TestFixture]
    public class CodecTest : AbstractTest
    {
        [Test]
        public void RleTest()
        {
            DicomFile file = new DicomFile("RleCodecTest.dcm");

            SetupMR(file.DataSet);

            SetupMetaInfo(file);

            RleTest(file);

            file = new DicomFile("MultiframeRleCodecTest.dcm");

            this.SetupMultiframeXA(file.DataSet, 511, 511, 5);

            RleTest(file);


            file = new DicomFile("MultiframeRleCodecTest.dcm");

            this.SetupMultiframeXA(file.DataSet, 63, 63, 1);

            RleTest(file);

            file = new DicomFile("MultiframeRleCodecTest.dcm");

            this.SetupMultiframeXA(file.DataSet, 1024, 1024, 3);

            RleTest(file);

            file = new DicomFile("MultiframeRleCodecTest.dcm");

            this.SetupMultiframeXA(file.DataSet, 512, 512, 2);

            RleTest(file);
        }


        public void RleTest(DicomFile file)
        {
            // Make a copy of the source format
            DicomFile originalFile;
            DicomAttributeCollection originalDataSet = file.DataSet.Copy();
            DicomAttributeCollection originalMetaInfo = file.MetaInfo.Copy();
            originalFile = new DicomFile("", originalMetaInfo, originalDataSet);

            file.ChangeTransferSyntax(TransferSyntax.RleLossless);

            file.Save();

            DicomFile newFile = new DicomFile(file.Filename);

            newFile.Load();

            newFile.ChangeTransferSyntax(TransferSyntax.ExplicitVrLittleEndian);

            newFile.Filename = "Output" + file.Filename;
            newFile.Save();

            Assert.AreEqual(originalFile.DataSet.Equals(newFile.DataSet), true);
        }

        [Test]
        public void PartialFrameTest()
        {
            DicomFile file = new DicomFile("RlePartialFrameTest.dcm");

            this.SetupMultiframeXA(file.DataSet, 511, 511, 7);

            file.ChangeTransferSyntax(TransferSyntax.RleLossless);

            file.Save();

            DicomFile newFile = new DicomFile(file.Filename);

            newFile.Load(DicomReadOptions.StorePixelDataReferences);

            DicomPixelData pd;

            if (!newFile.TransferSyntax.Encapsulated)
                pd = new DicomUncompressedPixelData(newFile);
            else if (newFile.TransferSyntax.Equals(TransferSyntax.RleLossless))
                pd = new DicomCompressedPixelData(newFile);
            else
                throw new DicomCodecException("Unsupported transfer syntax: " + newFile.TransferSyntax);

            for (int i=0; i< pd.NumberOfFrames; i++)
            {
                byte[] frame = pd.GetFrame(i);

            }

        }


    }
}

#endif