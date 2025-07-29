namespace WatchCenter.Domain.Entites
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Content>? Contents { get; set; } = new List<Content>();
        
    }
}