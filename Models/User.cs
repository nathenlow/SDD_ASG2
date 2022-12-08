using System.ComponentModel.DataAnnotations;


namespace SDD_ASG2.Models


{
    public class User
    {
        [Display(Name = "User Id")]
        public int UserId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }


        [Display(Name = "Username")]
        public string Username { get; set; }

        public string SavedGameData { get; set; }

    }
}
