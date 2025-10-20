namespace profisys.Web.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public ICollection<DocumentItem> Items { get; set; }
    }
}
