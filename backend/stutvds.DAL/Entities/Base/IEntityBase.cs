namespace StopStatAuth_6_0.Entities.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
