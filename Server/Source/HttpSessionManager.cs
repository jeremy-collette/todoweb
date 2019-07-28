namespace todoweb.Server
{
    using System;

    using Microsoft.AspNetCore.Http;

    using todoweb.Server.Contract;
    using todoweb.Server.Core;
    using todoweb.Server.Models;

    public class HttpSessionManager : IHttpSessionManager
    {
        private const string SessionKey = "sessionId";
        private ISessionManager sessionManager_;

        public HttpSessionManager(DatabaseContext<User> databaseContext)
        {
            this.sessionManager_ = new SessionManager(databaseContext);
        }

        public User GetUserFromRequest(HttpRequest request)
        {
            var sessionId = this.GetSessionId(request);

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
            this.SetSessionId(request, sessionId);
        }

        public bool DeleteSession(HttpRequest request)
        {
            var sessionId = this.GetSessionId(request);
            if (sessionId == Guid.Empty)
            {
                return false;
            }
            return this.sessionManager_.DeleteSession(sessionId);
        }

        private Guid GetSessionId(HttpRequest request)
        {
            if (!request.Cookies.Keys.Contains(SessionKey))
            {
                return Guid.Empty;
            }

            if (!Guid.TryParse(request.Cookies[SessionKey].ToString(), out var sessionId))
            {
                return Guid.Empty;
            }

            return sessionId;
        }

        private void SetSessionId(HttpRequest request, Guid sessionId)
        {
            request.HttpContext.Response.Cookies.Append(SessionKey, sessionId.ToString());
        }
    }
}
