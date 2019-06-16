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
            if (!Guid.TryParse(request.HttpContext.Session.GetString(SessionKey), out var sessionId))
            {
                return null;
            }
            return this.sessionManager_.GetUserFromSession(sessionId);
        }

        public void CreateOrUpdateSession(User user, HttpRequest request)
        {
            var sessionId = this.sessionManager_.CreateOrUpdateSession(user);
            request.HttpContext.Session.SetString(SessionKey, sessionId.ToString());
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
