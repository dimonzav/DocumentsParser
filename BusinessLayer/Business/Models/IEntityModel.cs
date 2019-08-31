namespace Business.Models
{
    public interface IEntityModel<TEntity>
    {
        TEntity ToEntity();

        void ToModel(TEntity entity);
    }
}
