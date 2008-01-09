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

using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;



namespace ClearCanvas.Enterprise.Core
{
    public class AuditAdvice : ServiceOperationAdvice, IInterceptor
    {
        internal AuditAdvice()
        {
        }

        #region IInterceptor Members

        public object Intercept(IInvocation invocation, params object[] args)
        {
            ServiceOperationAttribute a = GetServiceOperationAttribute(invocation);
            if (a != null && a.Auditable)
            {
                IUpdateContext uctx = PersistenceScope.Current as IUpdateContext;

                // only install a TransactionLogger if the current context is an update context, and does not already have one
                if (uctx != null && uctx.TransactionLogger == null)
                {
                    string transactionName = string.Format("{0}.{1}", invocation.InvocationTarget.GetType().FullName, invocation.Method.Name);
                    uctx.TransactionLogger = new DefaultTransactionLogger(transactionName);
                }
            }
            return invocation.Proceed(args);
        }

        #endregion
    }
}
