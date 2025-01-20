namespace FriendsApp.Models
{
    public class DashboardViewModel
    {
        public List<Friend> Friends { get; set; }
        public List<Meeting> Meetings { get; set; }
        public List<Plan> Plans { get; set; }
        public Dictionary<string, int> FriendCategories { get; set; }
    }

}
