using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Common.Statistics;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Services.Streaming.ImageStreaming;

namespace ClearCanvas.ImageServer.Services.Streaming.ImageStreaming
{
    /// <summary>
    /// Represents a Dicom WADO request processor.
    /// </summary>
    public class WADORequestProcessor : IDisposable
    {
        #region Private Members
        private int _readBufferSize = 0;
        private WADORequestProcessorStatistics _statistics;
        private readonly WADORequestTypeHandlerManager _handlerManager = new WADORequestTypeHandlerManager();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the buffer used by the processor for reading.
        /// </summary>
        public int ReadBufferSize
        {
            get { return _readBufferSize; }
            set { _readBufferSize = value; }
        }

        /// <summary>
        /// Gets the statistics generated by the processor.
        /// </summary>
        public WADORequestProcessorStatistics Statistics
        {
            get { return _statistics; }
        }

        #endregion

        

        #region Private Methods

        private string GetClientAcceptTypes(HttpListenerContext context)
        {
            Platform.CheckForNullReference(context, "context");

            if (context.Request.AcceptTypes == null)
                return null;

            StringBuilder mimes = new StringBuilder();
            foreach (string mime in context.Request.AcceptTypes)
            {
                if (mimes.Length > 0)
                    mimes.Append(",");
                mimes.Append(mime);
            }
            return mimes.ToString();
        }

        private void LogRequest(HttpListenerContext context)
        {
            Platform.CheckForNullReference(context, "context");

            StringBuilder info = new StringBuilder();

            info.AppendFormat("\n\tAgents={0}", context.Request.UserAgent);
            info.AppendFormat("\n\tRequestType={0}", context.Request.QueryString["RequestType"]);
            info.AppendFormat("\n\tStudyUid={0}", context.Request.QueryString["StudyUid"]);
            info.AppendFormat("\n\tSeriesUid={0}", context.Request.QueryString["SeriesUid"]);
            info.AppendFormat("\n\tObjectUid={0}", context.Request.QueryString["ObjectUid"]);
            info.AppendFormat("\n\tAccepts={0}", GetClientAcceptTypes(context));

            Platform.Log(LogLevel.Debug, info);

        }


        private void SendWADOResponse(WADOResponse response, HttpListenerContext context)
        {
            SetResponseStatus(context, (int)HttpStatusCode.OK); //TODO: what does the protocol say about how error that occurs after OK status has been sent should  be handled?

            context.Response.ContentType = response.ContentType;

            if (response.Stream == null)
            {
                context.Response.ContentLength64 = 0;

            }
            else
            {
                context.Response.ContentLength64 = response.Stream.Length;

                OptimizeBufferSize(context);

                Platform.Log(LogLevel.Debug, "Starting streaming image...");

                int count = 0;
                byte[] buffer = new byte[ReadBufferSize];
                do
                {
                    count = response.Stream.Read(buffer, 0, buffer.Length);
                    Statistics.DiskAccessCount++;
                    if (count > 0)
                    {
                        Statistics.TransmissionTime.Start();
                        context.Response.OutputStream.Write(buffer, 0, count);
                        Statistics.TransmissionTime.End();
                    }
                } while (count > 0);

            }
        }

        /// <summary>
        /// Adjust the buffer size
        /// </summary>
        /// <param name="context"></param>
        private void OptimizeBufferSize(HttpListenerContext context)
        {
            if (ReadBufferSize <= 0)
            {
                const int KILOBYTES = 1024;
                const int MEGABYTES = 1024 * KILOBYTES;

                long contentSize = context.Response.ContentLength64;

                // This is very simple optimization algorithm: the buffer size is set according to the content size
                // Other factors may be considered in the future: disk access speed, available physical memory, network speed, cpu usage
                if (contentSize > 3 * MEGABYTES)
                    ReadBufferSize = 3 * MEGABYTES;
                else if (contentSize > 1 * MEGABYTES)
                    ReadBufferSize = 1 * MEGABYTES;
                else if (contentSize > 500 * KILOBYTES)
                    ReadBufferSize = 256 * KILOBYTES;
                else if (contentSize > 100 * KILOBYTES)
                    ReadBufferSize = 128 * KILOBYTES;
                else
                    ReadBufferSize = 64 * KILOBYTES;


            }

            Statistics.ImageSize = (ulong)context.Response.ContentLength64;
            Statistics.BufferSize = (ulong)ReadBufferSize;
        }

        private static void SetResponseStatus(HttpListenerContext context, int statusCode)
        {
            context.Response.StatusCode = statusCode;
        }

        private IWADORequestTypeHandler GetHandler(HttpListenerContext context)
        {

            string requestType = context.Request.QueryString["requestType"];

            if (String.IsNullOrEmpty(requestType))
            {
                throw new WADOException((int)HttpStatusCode.BadRequest, "RequestType parameter is required");
            }
            else
            {
                return _handlerManager.GetHandler(requestType);
            }
        }


        #endregion

        #region Public Methods

        public void Process(HttpListenerContext context)
        {
            Platform.CheckForNullReference(context, "context");

            _statistics = new WADORequestProcessorStatistics(context.Request.RemoteEndPoint.Address.ToString());

            Statistics.TotalProcessTime.Add(delegate()
                {
                    LogRequest(context);

                    using(IWADORequestTypeHandler typeHandler = GetHandler(context))
                    {
                        WADOResponse response = typeHandler.Process(context.Request);
                        if (response!=null)
                        {
                            SendWADOResponse(response, context);
                            response.Dispose();
                        }
                            
                    }
                });

            StatisticsLogger.Log(LogLevel.Info, Statistics);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}