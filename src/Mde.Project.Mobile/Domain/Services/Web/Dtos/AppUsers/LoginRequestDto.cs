

using System.ComponentModel.DataAnnotations;
namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        public string Password { get; set; }
    }

