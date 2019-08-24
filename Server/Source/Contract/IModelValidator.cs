namespace todoweb.Server.Contract
{
    public interface IModelValidator<TModel>
    {
        bool Validate(TModel model);
    }
}
