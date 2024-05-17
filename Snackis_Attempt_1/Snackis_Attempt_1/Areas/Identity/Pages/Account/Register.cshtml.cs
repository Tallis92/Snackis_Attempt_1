// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Snackis_Attempt_1.Areas.Identity.Data;
using Snackis_Attempt_1.Pages.RoleAdmin;

namespace Snackis_Attempt_1.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly IUserStore<SnackisUser> _userStore;
        private readonly IUserEmailStore<SnackisUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<SnackisUser> userManager,
            IUserStore<SnackisUser> userStore,
            SignInManager<SnackisUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
			RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Epost")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} måste vara minst {2} och får max vara {1} tecken långt.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bekräfta lösenord")]
            [Compare("Password", ErrorMessage = "Lösenordet matchar inte.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Förnamn")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Efternamn")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Visningsnamn")]
            public string Nickname { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
			var roles = _roleManager.Roles.ToList();

			if (roles.Count == 0)
			{
                await CreateRole("Användare");
                await CreateRole("Admin");
                await CreateRole("Huvud Admin");

			}
            
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.NickName = Input.Nickname;


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                string userid = user.Id;

				//**This piece of code makes it so that whenever an account is made the new users role is set to "Användare" by default.**
				var users = _userManager.Users.ToList();
				
				var result = await _userManager.CreateAsync(user, Input.Password);
				if (users.Count == 0)
				{
					await _userManager.AddToRoleAsync(user, "Huvud Admin");
					await _userManager.AddToRoleAsync(user, "Admin");
					await _userManager.AddToRoleAsync(user, "Användare");
				}
                else
                {
					await _userManager.AddToRoleAsync(user, "Användare");
				}
				

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private SnackisUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<SnackisUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(SnackisUser)}'. " +
                    $"Ensure that '{nameof(SnackisUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<SnackisUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<SnackisUser>)_userStore;
        }

		public async Task CreateRole(string roleName)
		{
			bool roleExist = await _roleManager.RoleExistsAsync(roleName);

			if (!roleExist)
			{
				var role = new IdentityRole
				{
					Name = roleName,
				};

				await _roleManager.CreateAsync(role);
				//UpdateAsync(role) för att ändra/uppdatera en befintlig post

			}
		}
	}
}
