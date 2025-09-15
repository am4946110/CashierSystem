using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models
{
    public class ViewModels
    {
        [Table("RoleModel", Schema = "SUR")]
        public class RoleModel
        {
            [NotMapped]
            public string EncryptedId { get; set; }
            [Key]
            public Guid? Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; }
        }

        [Table("LoginModel", Schema = "SUR")]
        public class LoginModel
        {
            [Key]
            public Guid? Id { get; set; } = Guid.NewGuid();

            [Display(Name = "Email Address")]
            [Required(ErrorMessage = "Please enter your email address.")]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            public string Email { get; set; }

            [Display(Name = "Password")]
            [Required(ErrorMessage = "Please enter your password.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

        }

        [Table("RegisterModel", Schema = "SUR")]
        public class RegisterModel
        {
            [NotMapped]
            public string EncryptedId { get; set; }
            [Key]
            public Guid? Id { get; set; } = Guid.NewGuid();

            [Display(Name = "User Name")]
            [Required(ErrorMessage = "Please enter your user name.")]
            public string UserName { get; set; }

            [Display(Name = "Email Address")]
            [Required(ErrorMessage = "Please enter your email address.")]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            public string Email { get; set; }

            [Display(Name = "Password")]
            [Required(ErrorMessage = "Please enter a password.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Confirm Password")]
            [Required(ErrorMessage = "Please confirm your password.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The new password and confirmation do not match.")]
            public string ConfirmPassword { get; set; }
            public string Country { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime? BirthDate { get; set; }
            public byte[]? ImagePath { get; set; }
            [NotMapped]
            public IFormFile ImageFile { get; set; }
        }

        [Table("UserRoleModel", Schema = "SUR")]
        public class UserRoleModel
        {
            [NotMapped]
            public string EncryptedId { get; set; }
            [Key]
            public Guid? Id { get; set; } = Guid.NewGuid();

            [Required]
            public string UserId { get; set; }

            [Required]
            public string RoleId { get; set; }

            public string UserName { get; set; }
            public string SelectedRole { get; set; }
        }

        [Table("EditPasswordModel", Schema = "SUR")]
        public class EditPasswordModel
        {
            [Key]
            public Guid? Id { get; set; } = Guid.NewGuid();

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string OldPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match.")]
            public string ConfirmPassword { get; set; }
            public string EncryptedId { get; internal set; }
        }
    }
}
