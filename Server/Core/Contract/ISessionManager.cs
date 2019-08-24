namespace todoweb.Server.Core.Contract
{
    using System;

    using Server = todoweb.Server.Models;

    public interface ISessionManager
    {
        Server.User GetUserFromSession(Guid sessionId);

        Guid CreateOrUpdateSession(Server.User user);

        bool DeleteSession(Guid sessionId);
    }
}
