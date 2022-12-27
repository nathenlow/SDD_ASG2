using Google.Protobuf.WellKnownTypes;
using MailKit.Net.Imap;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using SDD_ASG2.DAL;

namespace SDD_ASG2.ViewModels
{
    public class UserRegister
    {
        private string username;
        private string email;
        private string password;
        private string retypePassword;

        [Required]
        [ValidateUsernameExists]
        [StringLength(50,ErrorMessage = "Username have exceeded the 50 character limit")]
        public string Username
        {
            get { return username; }
            set
            {
                if (value != null)
                {
                    username = value.Trim().ToLower();
                }
            }
        }

        [Required]
        [ValidateEmailExists]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address")]
        [StringLength(320, ErrorMessage = "Email address have exceeded the 320 character limit")]
        public string Email
        {
            get { return email; }
            set {
                if (value != null)
                {
                    email = value.Trim().ToLower();
                }
            }
        }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z/\\d]{8,}$", ErrorMessage = "Password must have minimum eight characters, at least one uppercase letter, one lowercase letter and one number")]
        [StringLength(128, ErrorMessage = "Password have exceeded the 128 character limit")]
        public string Password
        {
            get { return password; }
            set
            {
                if (value != null)
                {
                    password = value.Trim();
                }
            }
        }

        [Required]
        [ValidatePassword]
        public string RetypePassword
        {
            get { return retypePassword;}
            set {
                if (value != null)
                {
                    retypePassword = value.Trim();
                }
            }
        }

    }
    public class ValidateEmailExists : ValidationAttribute
    {
        private UserDAL userContext = new UserDAL();
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            if (userContext.IsEmailExist(email))
            {
                // validation failed
                return new ValidationResult("Email address already exists!");
            }
            else
            {
                // validation passed
                return ValidationResult.Success;
            }

        }
    }

    public class ValidateUsernameExists : ValidationAttribute
    {
        private UserDAL userContext = new UserDAL();
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string username = Convert.ToString(value);
            if (userContext.IsUsernameExist(username))
            {
                // validation failed
                return new ValidationResult("Username already exists!");
            }
            else
            {
                // validation passed
                return ValidationResult.Success;
            }

        }
    }

    public class ValidatePassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string password = Convert.ToString(value);
            UserRegister user = (UserRegister)validationContext.ObjectInstance;
            if (user.Password == password)
            {
                // validation passed
                return ValidationResult.Success;
            }
            else
            {
                // validation failed
                return new ValidationResult("Password do not match!");
            }

        }
    }
}
