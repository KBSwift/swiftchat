using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwiftChat.Models.Dtos;
using SwiftChat.Models.Entities;

namespace SwiftChat.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Populating dto with user data
            var userProfileDto = new UserProfileDto
            {
                UserName = user.UserName,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
                // Map other properties as needed
            };

            return View(userProfileDto);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Updating the user properties
            user.Bio = model.Bio;
            user.DateOfBirth = model.DateOfBirth; // now nullable

            // Handle profile picture update WORK IN PROGRESS. Will use VAR BIN
            //if (profilePictureFile != null && profilePictureFile.Length > 0)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await profilePictureFile.CopyToAsync(memoryStream);
            //        user.ProfilePicture = memoryStream.ToArray();
            //    }
            //}

            // Handle email update WIP
            if (!string.IsNullOrWhiteSpace(model.NewEmail) && model.NewEmail != user.Email)
            {
                // Checking if the new email is already in use by another user
                var existingUser = await _userManager.FindByEmailAsync(model.NewEmail);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("NewEmail", "Email is already in use.");
                    return View("Index", model); // Return to the view with the error message
                }

                // Updating the email
                var setEmailResult = await _userManager.SetEmailAsync(user, model.NewEmail);
                if (!setEmailResult.Succeeded)
                {
                    // Error handling
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Index", model); // Return to the view with the error messages
                }

                // Send email confirmation. NEEDS REVIEW!
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action(
                //    "ConfirmEmail",
                //    "Account",
                //    new { userId = user.Id, code = code },
                //    protocol: HttpContext.Request.Scheme);
            }

            // Handle password update. Need to revisit Core implementation
            if (!string.IsNullOrWhiteSpace(model.CurrentPassword) && !string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    // Handle errors (e.g., current password is wrong, new password is weak)
                    // Add model errors or handle as needed
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Handle errors during user update
                // Add model errors or handle as needed
            }

            // Redirecting to index action method
            return RedirectToAction("Index");
        }
    }
}
