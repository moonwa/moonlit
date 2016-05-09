namespace Moonlit.Mvc
{
    public interface IToEntity<T>
    {
        void OnToEntity(T entity, ToEntityContext context);
    }
}