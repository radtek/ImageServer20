#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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

using System.Threading;
using System.Collections.Generic;
using ClearCanvas.Enterprise.Authentication;
using ClearCanvas.Enterprise.Authentication.Brokers;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Alert;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Application.Services
{
    public abstract class ApplicationServiceBase : IApplicationServiceLayer
    {
        private static Staff _currentUserStaff;
        private static User _currentUser;

        /// <summary>
        /// Obtains the staff associated with the current user.  If no <see cref="Staff"/> is associated with the current user,
        /// a <see cref="RequestValidationException"/> is thrown.
        /// </summary>
        protected Staff CurrentUserStaff
        {
            get
            {
                try
                {
                    _currentUserStaff = PersistenceContext.GetBroker<IStaffBroker>().FindStaffForUser(Thread.CurrentPrincipal.Identity.Name);
                }
                catch (EntityNotFoundException)
                {
                    throw new RequestValidationException(SR.ExceptionNoStaffForUser);
                }

                return _currentUserStaff;
            }
        }

        /// <summary>
        /// Obtains the <see cref="User"/> entity for the current user.
        /// </summary>
        /// <exception cref="RequestValidationException">Thrown if no <see cref="User"/> is found, though this should not happen.</exception>
        protected User CurrentUser
        {
            get
            {
                try
                {
                    UserSearchCriteria criteria = new UserSearchCriteria();
                    criteria.UserName.EqualTo(Thread.CurrentPrincipal.Identity.Name);
                    _currentUser = this.PersistenceContext.GetBroker<IUserBroker>().FindOne(criteria);
                }
                catch (EntityNotFoundException)
                {
                    throw new RequestValidationException(SR.ExceptionNoUser);
                }

                return _currentUser;
            }
        }

        protected IPersistenceContext PersistenceContext
        {
            get { return PersistenceScope.Current; }
        }

        /// <summary>
        /// Helper method to test a Patient or Patient Profile with alerts that implement the PatientAlertExtensionPoint and PatientProfileAlertExtensionPoint
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="context"></param>
        /// <returns>a list of alert notification detail if each alert test succeeds</returns>
        protected static List<AlertNotificationDetail> GetAlertNotifications(Entity subject, IPersistenceContext context)
        {
            AlertAssembler assembler = new AlertAssembler();
            List<AlertNotificationDetail> results = new List<AlertNotificationDetail>();

            if (subject.Is<Patient>())
            {
                foreach (IPatientAlert patientAlertTests in PatientAlertHelper.Instance.GetAlertTests())
                {
                    IAlertNotification testResult = patientAlertTests.Test(subject.Downcast<Patient>(), context);
                    if (testResult != null)
                    {
                        results.Add(assembler.CreateAlertNotification(testResult));
                    }
                }
            }
            else if (subject.Is<PatientProfile>())
            {
                foreach (IPatientProfileAlert profileAlertTests in PatientProfileAlertHelper.Instance.GetAlertTests())
                {
                    IAlertNotification testResult = profileAlertTests.Test(subject.Downcast<PatientProfile>(), context);
                    if (testResult != null)
                    {
                        results.Add(assembler.CreateAlertNotification(testResult));
                    }
                }
            }
            else if (subject.Is<Order>())
            {
                foreach (IOrderAlert orderAlertTests in OrderAlertHelper.Instance.GetAlertTests())
                {
                    IAlertNotification testResult = orderAlertTests.Test(subject.Downcast<Order>(), context);
                    if (testResult != null)
                    {
                        results.Add(assembler.CreateAlertNotification(testResult));
                    }
                }
            }

            return results;
        }
    }
}
