using System.ComponentModel.DataAnnotations;

namespace Snackis_Attempt_1.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Password)] public string Password { get; set; }

        [DataType(DataType.EmailAddress)] public string EmailAddress { get; set;}

        public string? ProfilePic {  get; set; }

        
    }
}
