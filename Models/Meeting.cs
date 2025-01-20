namespace FriendsApp.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; } // Foreign key
        public DateTime MeetingDate { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public string Place { get; set; }

        public Friend Friend { get; set; } // Navigation property
    }


}
