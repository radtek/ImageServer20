#if UNIT_TESTS

namespace ClearCanvas.Dicom.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using ClearCanvas.Dicom.Network;
    using NUnit.Framework;

    [TestFixture]
    public class DcmNetTest
    {   
        private Process _dicomServer = null;

        [TestFixtureSetUp]
        public void Init()
        {
            // check to see if OFFIS storescp.exe exists
            string programToRun = "C:\\Windows\\storescp.exe";
            string arguments = " -v 104";

            if (!System.IO.File.Exists(programToRun))
                throw new Exception("Could not find the DICOM server program to run tests against");
            
            /* Note: I haven't figured out how to determine whether the dcmnet.dll 
             * dependency is somewhere that NUnit can access and properly load into
             * memory
             * 
            // check to see if the dependent C++ library file exists
            string dependentLibraryFile = "\\dcmnet.dll";
            if (!System.IO.File.Exists(dependentLibraryFile))
                throw new Exception("Could not find the dependent C++ library");
             */

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(programToRun);
                startInfo.Arguments = arguments;

                _dicomServer = Process.Start(startInfo);
            }
            catch (System.Exception e)
            {
                throw new Exception("Could not start the DICOM server", e);
            }
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            if (null != _dicomServer)
            {
                _dicomServer.Kill();
            }
        }

        [Test]
        public void Verify()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(110));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("localhost"),
                new AETitle("STORESCP"), new ListeningPort(104));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            bool successVerify = dicomClient.Verify(serverAE);

            Assert.IsTrue(successVerify);
        }
    }
}

#endif