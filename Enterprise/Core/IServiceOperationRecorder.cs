﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ClearCanvas.Common.Audit;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Enterprise.Core
{
    /// <summary>
    /// Describes the invocation of a service operation, including the arguments, return value, and any exception thrown.
    /// </summary>
    public class ServiceOperationInvocationInfo
    {
        private readonly string _operationName;
        private readonly Type _serviceClass;
        private readonly MethodInfo _operationMethod;
        private readonly object[] _arguments;
        private readonly object _returnValue;
        private readonly Exception _exception;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="serviceClass"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <param name="returnValue"></param>
        /// <param name="exception"></param>
        internal ServiceOperationInvocationInfo(string operationName, Type serviceClass, MethodInfo operation, object[] args,
            object returnValue, Exception exception)
        {
            _operationName = operationName;
            _serviceClass = serviceClass;
            _operationMethod = operation;
            _arguments = args;
            _returnValue = returnValue;
            _exception = exception;
        }

        /// <summary>
        /// Gets the logical name of the operation.
        /// </summary>
        public string OperationName
        {
            get { return _operationName; }
        }

        /// <summary>
        /// Gets the class that provides the service implementation.
        /// </summary>
        public Type ServiceClass
        {
            get { return _serviceClass; }
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object describing the operation.
        /// </summary>
        public MethodInfo OperationMethodInfo
        {
            get { return _operationMethod; }
        }

        /// <summary>
        /// Gets the list of arguments passed to the operation.
        /// </summary>
        public object[] Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets the return value of the operation, or null if an exception was thrown.
        /// </summary>
        public object ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets any unhandled exception thrown from the service operation, or null if the 
        /// operation completed successfully.
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
        }
    }


    /// <summary>
    /// Defines an interface for writing an audit log entry that records
    /// information about the invocation of a service operation.
    /// </summary>
    public interface IServiceOperationRecorder
    {
		/// <summary>
		/// Gets the category of the audit log entries generated by this recorder.
		/// </summary>
		string Category { get; }

        /// <summary>
        /// Writes the specified service operation invocation to the specified audit log.
        /// </summary>
        void WriteLogEntry(ServiceOperationInvocationInfo invocationInfo, AuditLog auditLog);
    }
}
