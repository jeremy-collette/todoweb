namespace todoweb.Server.Contract
{
    using System;
    using Server = Server.Models;

    public interface ISessionManager
    {
        Server.User GetUserFromSession(Guid sessionId);

        Guid CreateOrUpdateSession(Server.User user);

        bool DeleteSession(Guid sessionId);
    }
}
