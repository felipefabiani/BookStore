namespace BookStore.Database.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string NickName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset DateAdded { get; set; } = DateTimeOffset.MinValue;
    }
}
