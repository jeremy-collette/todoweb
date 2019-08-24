namespace todoweb.Server.Core.Contract
{
    public interface IModelValidator<TModel>
    {
        bool Validate(TModel model);
    }
}
