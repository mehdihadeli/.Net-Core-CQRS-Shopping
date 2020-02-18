namespace Common.Core.Domain
{
    public class Entity : Entity<int>
    {
    }

    public class Entity<TKey>
    {
        public TKey Id { get; set; }
    }
}