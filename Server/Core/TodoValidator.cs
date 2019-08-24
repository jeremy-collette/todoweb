namespace todoweb.Server
{
    using System;
    using todoweb.Server.Core.Contract;

    using Server = Server.Models;

    public class TodoValidator : IModelValidator<Server.Todo>
    {
        public bool Validate(Server.Todo model)
        {
            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Owner) || string.IsNullOrEmpty(model.Title))
            {
                return false;
            }

            if (model.Id == Guid.Empty.ToString())
            {
                return false;
            }
            return true;
        }
    }
}
