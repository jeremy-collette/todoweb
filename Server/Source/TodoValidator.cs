namespace todoweb.Server
{
    using todoweb.Server.Contract;

    using Server = Server.Models;

    public class TodoValidator : IModelValidator<Server.Todo>
    {
        public bool Validate(Server.Todo model)
        {
            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Owner) || string.IsNullOrEmpty(model.Title))
            {
                return false;
            }
            return true;
        }
    }
}
