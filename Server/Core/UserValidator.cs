namespace todoweb.Server
{
    using todoweb.Server.Core.Contract;

    using Server = Server.Models;

    public class UserValidator : IModelValidator<Server.User>
    {
        public bool Validate(Server.User model)
        {
            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Owner)
                || model.PasswordHash == null)
            {
                return false;
            }

            if (model.Id != model.Email || model.Email != model.Owner)
            {
                return false;
            }
            return true;
        }
    }
}
