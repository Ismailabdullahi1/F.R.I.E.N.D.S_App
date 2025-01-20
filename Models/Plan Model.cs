using System;
using System.Collections.Generic;

namespace FriendsApp.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public DateTime PlanDate { get; set; }

        public List<Friend> Friends { get; set; } = new List<Friend>();
    }

}
