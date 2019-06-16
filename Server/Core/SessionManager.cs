namespace todoweb.Server.Core
{
    using System;
    using System.Collections.Generic;
    using todoweb.Server.Contract;
    using todoweb.Server.Models;

    public class SessionManager : ISessionManager
    {
        Dictionary<Guid, Guid> sessionToUserMap_;
        Dictionary<Guid, Guid> userToSessionMap_;
        IResourceManager<User> userManager_;

        public SessionManager(IResourceManager<User> userManager)
        {
            this.sessionToUserMap_ = new Dictionary<Guid, Guid>();
            this.userToSessionMap_ = new Dictionary<Guid, Guid>();
            this.userManager_ = userManager;
        }

        public Guid CreateOrUpdateSession(User user)
        {
            if (this.userToSessionMap_.ContainsKey(user.Id))
            {
                return this.userToSessionMap_[user.Id];
            }

            var sessionId = Guid.NewGuid();
            this.sessionToUserMap_[sessionId] = user.Id;
            this.userToSessionMap_[user.Id] = sessionId;
            return sessionId;
        }

        public bool DeleteSession(Guid sessionId)
        {
            if (!this.sessionToUserMap_.ContainsKey(sessionId))
            {
                return false;
            }

            var userId = this.sessionToUserMap_[sessionId];
            this.sessionToUserMap_.Remove(sessionId);
            this.userToSessionMap_.Remove(userId);
            return true;
        }

        public User GetUserFromSession(Guid sessionId)
        {
            if (!this.sessionToUserMap_.ContainsKey(sessionId))
            {
                return null;
            }
            return this.userManager_.Get(this.sessionToUserMap_[sessionId]);
        }
    }
}
