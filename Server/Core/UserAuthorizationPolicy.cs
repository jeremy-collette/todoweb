namespace todoweb.Server
{
    using todoweb.Server.Core.Contract;
    using todoweb.Server.Models;

    public class UserAuthorizationPolicy : IAuthorizationPolicy<User>
    {
        public bool CanCreate(User creator)
        {
            return creator == null;
        }

        public bool CanDelete(User accessor, User resource)
        {
            return accessor?.Id == resource.Id;
        }

        public bool CanRead(User accessor, User resource)
        {
            return accessor?.Id == resource.Id;
        }

        public bool CanWrite(User accessor, User resource)
        {
            return accessor?.Id == resource.Id;
        }
    }
}
