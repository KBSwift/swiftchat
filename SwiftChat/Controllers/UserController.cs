using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwiftChat.Models.Dtos;
using SwiftChat.Models.Entities;
using SixLabors.ImageSharp;

namespace SwiftChat.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private const long MaxFileSize = 10485760; // 10 MB

		public UserController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		[Route("user/")]
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
				// possibly more properties
			};

			return View(userProfileDto);
		}


		[HttpPost]
		public async Task<IActionResult> UpdateProfileDetails(UserProfileDto model)
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

			// establishing user properties from model
			user.Bio = model.Bio;
			user.DateOfBirth = model.DateOfBirth; // now nullable

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				// Create a response object with updated details from the user object
				var updatedProfile = new
				{
					Bio = user.Bio,
					DateOfBirth = user.DateOfBirth.HasValue ? user.DateOfBirth.Value.ToString("yyyy-MM-dd") : null
					// Add other fields from the user object as needed
				};

				return Json(new { success = true, message = "Details updated successfully.", data = updatedProfile });
			}
			else
			{
				return Json(new { success = false, message = "Error updating details. Try again." });
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProfileCredentials(UserProfileDto model)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { success = false, message = "Multiple errors. Check submission and try again." });
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Json(new { success = false, message = "User not found." });
			}

			if (string.IsNullOrWhiteSpace(model.CurrentPassword))
			{
				return Json(new { success = false, message = "Current password cannot be blank." });
			}

			if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
			{
				return Json(new { success = false, message = "Current password is incorrect." });
			}

			if (!string.IsNullOrWhiteSpace(model.NewEmail) && model.NewEmail != user.Email)
			{
				var existingUser = await _userManager.FindByEmailAsync(model.NewEmail);
				if (existingUser != null && existingUser.Id != user.Id)
				{
					return Json(new { success = false, message = "Email is already in use." });
				}

				var setEmailResult = await _userManager.SetEmailAsync(user, model.NewEmail);
				if (!setEmailResult.Succeeded)
				{
					return Json(new { success = false, message = "Failed to update email." });
				}
			}

			if (!string.IsNullOrWhiteSpace(model.NewPassword))
			{
				if (model.NewPassword != model.ConfirmNewPassword)
				{
					return Json(new { success = false, message = "New password and confirmation password do not match." });
				}

				var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
				if (!changePasswordResult.Succeeded)
				{
					return Json(new { success = false, message = "Failed to change password." });
				}
			}

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				return Json(new { success = false, message = "Error updating profile." });
			}

			// Optionally, you can return updated user data here
			return Json(new { success = true, message = "Credentials updated successfully.", data = new { user.Email, user.UserName } });
		}

		[HttpPost]
		public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
		{
			if (profilePicture == null || profilePicture.Length == 0)
			{
				return Json(new { success = false, message = "No file selected." });
			}

			if (profilePicture.Length > MaxFileSize)
			{
				return Json(new { success = false, message = $"Max filesize allowed is {MaxFileSize / 1024} MB." });
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			using (var memoryStream = new MemoryStream())
			{
				await profilePicture.CopyToAsync(memoryStream);

				// Check if the file is an image (optional)
				if (!IsImage(memoryStream))
				{
					return Json(new { success = false, message = "Invalid file format. Please upload an image." });
				}

				user.ProfilePicture = memoryStream.ToArray();
			}

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				string base64Image = Convert.ToBase64String(user.ProfilePicture);
				return Json(new
				{
					success = true,
					message = "Profile picture updated successfully.",
					data = base64Image,
					mimeType = profilePicture.ContentType
				});
			}
			else
			{
				return Json(new { success = false, message = "Error updating profile picture." });
			}
		}

		private bool IsImage(Stream stream)
		{
			try
			{
				stream.Position = 0;
				using var image = Image.Load(stream);
				return image.Width > 0 && image.Height > 0;
			}
			catch
			{
				return false;
			}
		}
	}



}

