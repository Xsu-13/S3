namespace EFTraining.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int? AuthorId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        
        public Author Author { get; set; }
    }
}
