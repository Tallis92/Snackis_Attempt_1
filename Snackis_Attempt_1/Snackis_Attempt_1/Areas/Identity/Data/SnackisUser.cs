using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Snackis_Attempt_1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SnackisUser class
public class SnackisUser : IdentityUser
{
    [PersonalData] public string FirstName { get; set; }
    [PersonalData] public string LastName { get; set; }
    [PersonalData] public string NickName { get; set; }
    [PersonalData] public string? ProfilePic { get; set; }

}

