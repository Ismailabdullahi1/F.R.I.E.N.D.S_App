namespace FriendsApp.Models
{
    public class Friend
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public string FirstMeetingPlace { get; set; }

     public List<Plan> Plans { get; set; } = new List<Plan>();
     public List<Meeting> Meetings { get; set; } = new List<Meeting>();
    }



}
