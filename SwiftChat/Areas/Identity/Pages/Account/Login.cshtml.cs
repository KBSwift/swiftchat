﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SwiftChat.Controllers;
using SwiftChat.Models.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace SwiftChat.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager; // adding UserManager for dynamic login
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, // Injecting UserManager
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager; // Setting UserManager
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        //public string EmailConfirmationUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
             
            // Modified to accommodate username or email for login logic
            [Required]
            [Display(Name = "Username or Email")]
            public string UsernameOrEmail { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/home");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/home");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid)
			{
				var userName = Input.UsernameOrEmail;
				ApplicationUser user = null;

				if (new EmailAddressAttribute().IsValid(userName))
				{
					user = await _userManager.FindByEmailAsync(userName);
					if (user != null)
					{
						userName = user.UserName;
					}
				}
				else
				{
					user = await _userManager.FindByNameAsync(userName);
				}

				if (user != null)
				{
					var passwordCheck = await _userManager.CheckPasswordAsync(user, Input.Password);
					if (passwordCheck)
					{
						if (!await _userManager.IsEmailConfirmedAsync(user))
						{
							// Redirect to a page informing the user to confirm their email
							return RedirectToPage("RegisterConfirmation", new { email = user.Email, returnUrl });
						}

						// Sign in the user
						await _signInManager.SignInAsync(user, Input.RememberMe);
						_logger.LogInformation("User logged in.");
						TempData.Clear();
						TempData["SuccessMessage"] = "You have successfully logged in.";
						return LocalRedirect(returnUrl);
					}
				}

				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}



	}
}
