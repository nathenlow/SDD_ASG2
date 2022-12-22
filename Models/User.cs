using System.ComponentModel.DataAnnotations;


namespace SDD_ASG2.Models


{
    public class User
    {
        public int UserId { get; set; }
        
        public string Email { get; set; }
        
        public string Username { get; set; }

        public string SavedGameData { get; set; }

    }
}
