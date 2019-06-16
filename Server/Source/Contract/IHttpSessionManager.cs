namespace todoweb.Server.Contract
{
    using System;
    using Microsoft.AspNetCore.Http;

    using Server = Server.Models;

    public interface IHttpSessionManager
    {
        Server.User GetUserFromRequest(HttpRequest request);

        void CreateOrUpdateSession(Server.User user, HttpRequest request);

        bool DeleteSession(HttpRequest request);
    }
}
