namespace Snackis_Attempt_1.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public DateTime SentDate { get; set; }
        public bool Flagged { get; set; }

    }
}
