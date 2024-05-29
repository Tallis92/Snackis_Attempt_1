// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis_Attempt_1.Areas.Identity.Data;

namespace Snackis_Attempt_1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        

        public IndexModel(
            UserManager<SnackisUser> userManager,
            SignInManager<SnackisUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }
        
        [BindProperty] public string ProfilePic { get; set; }
        public static Areas.Identity.Data.SnackisUser MyUser { get; set;} //Ta bort om inga referenser efter bilder är avklarade
        
        public IFormFile UploadedImage { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefonnummer")]
            public string PhoneNumber { get; set; }

            [PersonalData]
            [Display(Name = "Profilbild")]
            [BindProperty]
            public string ProfilePic { get; set; }
        }

        private async Task LoadAsync(SnackisUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            

            Username = userName;

            if(user.ProfilePic == null || user.ProfilePic == "defaultpf.jpg")
            {
                ProfilePic = "defaultpf.jpg";
                Methods.Singleton.SetProfilePic(ProfilePic);
            }
            else
            {

                ProfilePic = user.ProfilePic;
            }

            //MyUser = await _userManager.GetUserAsync(User);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ProfilePic = user.ProfilePic
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var image = UploadedImage;
            string fileName = "";
            if(image != null)
            {
                Random rnd = new Random();
                fileName = rnd.Next(1, 100000).ToString() + image.FileName;
                using (var fileStream = new FileStream("./wwwroot/images/" + fileName, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            var profilePic = Methods.Singleton.GetProfilePic();
            if (fileName != profilePic)
            {
                user.ProfilePic = fileName;              
            }
            
            user.ProfilePic = fileName;
            await _signInManager.RefreshSignInAsync(user);
            await _userManager.UpdateAsync(user);
            

            StatusMessage = "Your profile has been updated";
            
            return RedirectToPage();
        }

        private static string SetProfilePic(Areas.Identity.Data.SnackisUser user, string profilePic)
        {
            return profilePic;
        }
    }
}
