using System;
using System.Threading;
using System.Web;
using System.Web.Security;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.ImageServer.Common;
namespace ClearCanvas.ImageServer.Web.Common.Security
{
    public static class SessionManager
    {
        /// <summary>
        /// Returns the current session information
        /// </summary>
        /// <remarks>
        /// The session information is set by calling <see cref="InitializeSession"/>. It is null 
        /// if the <see cref="InitializeSession"/> hasn't been called or <see cref="TerminiateSession"/> has been called.
        /// </remarks>
        public static SessionInfo Current
        {
            get
            {
               if (Thread.CurrentPrincipal is CustomPrincipal)
               {
                   CustomPrincipal p = Thread.CurrentPrincipal as CustomPrincipal;
                   return new SessionInfo(p);
                   
               }
               else
               {
                   return null;
               }
            }
            set
            {
                Thread.CurrentPrincipal = value.User;
                HttpContext.Current.User = value.User;
            }
        }

        /// <summary>
        /// Sets up the principal for the thread and save the authentiction ticket if there's none.
        /// </summary>
        /// <param name="session"></param>
        public static void InitializeSession(SessionInfo session)
        {
            if (!session.Valid)
            {
                throw new ApplicationException("Attempt to initialize session with an invalid session object");
            }
            else
            {
                // this should throw exception if the session is no longer valid. It also loads the authority tokens}

                Current = session;

                string loginId = session.User.Identity.Name;
                string displayName = (session.User.Identity as CustomIdentity).DisplayName;
                SessionToken token = session.Credentials.SessionToken;
                string[] authorities = session.Credentials.Authorities;

                String data = String.Format("{0}|{1}|{2}", token.Id, displayName, authorities);
                FormsAuthenticationTicket authTicket = new
                    FormsAuthenticationTicket(1,  // version
                                 loginId,         // user name
                                 DateTime.Now,    // creation
                                 token.ExpiryTime,// Expiration
                                 false,           // Persistent
                                 data);           // User data

                // Now encrypt the ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                // Create a cookie with the encrypted data
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                HttpContext.Current.Response.Cookies.Set(authCookie);
            }

       }

        /// <summary>
        /// Terminates the current session and redirects users to the login page
        /// </summary>
        public static void TerminiateSession()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}