namespace todoweb.Server.Core
{
    using System;
    using System.Collections.Generic;
    using todoweb.Server.Contract;
    using todoweb.Server.Core.Contract;
    using todoweb.Server.Models;

    public class SessionManager : ISessionManager
    {
        // TODO(@jecollet): remove statics
        static Dictionary<Guid, Guid> sessionToUserMap_ = new Dictionary<Guid, Guid>();
        static Dictionary<Guid, Guid> userToSessionMap_ = new Dictionary<Guid, Guid>();
        IResourceManager<User> userManager_;

        public SessionManager(DatabaseContext<User> databaseContext)
        {
            this.userManager_ = new DatabaseResourceManager<User>(databaseContext);
        }

        public Guid CreateOrUpdateSession(User user)
        {
            if (SessionManager.userToSessionMap_.ContainsKey(user.Id))
            {
                return SessionManager.userToSessionMap_[user.Id];
            }

            var sessionId = Guid.NewGuid();
            SessionManager.sessionToUserMap_[sessionId] = user.Id;
            SessionManager.userToSessionMap_[user.Id] = sessionId;
            return sessionId;
        }

        public bool DeleteSession(Guid sessionId)
        {
            if (!SessionManager.sessionToUserMap_.ContainsKey(sessionId))
            {
                return false;
            }

            var userId = SessionManager.sessionToUserMap_[sessionId];
            SessionManager.sessionToUserMap_.Remove(sessionId);
            SessionManager.userToSessionMap_.Remove(userId);
            return true;
        }

        public User GetUserFromSession(Guid sessionId)
        {
            if (!SessionManager.sessionToUserMap_.ContainsKey(sessionId))
            {
                return null;
            }
            return this.userManager_.Get(SessionManager.sessionToUserMap_[sessionId]);
        }
    }
}
