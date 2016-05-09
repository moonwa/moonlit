namespace Moonlit.Mvc
{
    public interface IFromEntity<T>
    {
        void OnFromEntity(T entity, FromEntityContext context);
    }
}