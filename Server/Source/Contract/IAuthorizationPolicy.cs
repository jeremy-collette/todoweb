namespace todoweb.Server.Contract
{
    using todoweb.Server.Models;

    public interface IAuthorizationPolicy<TServerResource>
        where TServerResource : IServerResource
    {
        bool CanCreate(User creator);

        bool CanDelete(User accessor, TServerResource resource);

        bool CanRead(User accessor, TServerResource resource);

        bool CanWrite(User accessor, TServerResource resource);
    }
}
