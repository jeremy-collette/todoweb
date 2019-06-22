namespace todoweb.Server
{
    using System;

    using Microsoft.AspNetCore.Http;

    using todoweb.Server.Contract;
    using todoweb.Server.Models;

    public class HttpSessionManager : IHttpSessionManager
    {
        private const string SessionKey = "sessionId";
        private ISessionManager sessionManager_;

        public HttpSessionManager(ISessionManager sessionManager)
        {
            this.sessionManager_ = sessionManager;
        }
        public User GetUserFromRequest(HttpRequest request)
        {
            if (!request.Cookies.Keys.Contains(SessionKey))
            {
                return null;
            }

            if (!Guid.TryParse(request.Cookies[SessionKey].ToString(), out var sessionId))
            {
                return null;
            }

            // If we have a sessionId but no user, delete the session
            var user = this.sessionManager_.GetUserFromSession(sessionId);
            if (user == null)
            {
                this.DeleteSession(request);
            }
            return user;
        }

        public void CreateOrUpdateSession(User user, HttpRequest request)
        {
            var sessionId = this.sessionManager_.CreateOrUpdateSession(user);
            request.HttpContext.Response.Cookies.Append(SessionKey, sessionId.ToString());
        }

        public bool DeleteSession(HttpRequest request)
        {
            var sessionId = Guid.Parse(request.HttpContext.Session.GetString(SessionKey));
            if (sessionId == null)
            {
                return false;
            }
            return this.sessionManager_.DeleteSession(sessionId);
        }
    }
}
