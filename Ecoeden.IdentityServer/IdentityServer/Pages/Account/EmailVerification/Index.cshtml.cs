using IdentityServer.Entity;
using IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using ILogger = Serilog.ILogger;

namespace IdentityServer.Pages.Account.EmailVerification
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public IndexModel(ILogger logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public bool EmailVerificationCompleted { get; set; }
        [BindProperty]
        public bool EmailVerificationFailed { get; set; }
        [BindProperty]
        public string FailureMessage { get; set; }

        public async Task<IActionResult> OnGet([FromQuery] string userId, [FromQuery] string token)
        {
            ViewData["current-page"] = "email-verification";
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                _logger.Here().Error("No user id found in the url. Url must be invalid");
                EmailVerificationFailed = true;
                FailureMessage = "Seems like the url was tampered. Please connect to support team";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                _logger.Here().Error("No user found with the given user id {id}", userId);
                EmailVerificationFailed = true;
                FailureMessage = "Seems like the url was tampered. Please connect to support team";
                return Page();
            }
            if (user.EmailConfirmed)
            {
                _logger.Here().Error("User email is already activated {email}", user.Email);
                EmailVerificationFailed = true;
                FailureMessage = "Seems like the url was tampered. Please connect to support team";
                return Page();
            }
            var result = await _userManager.ConfirmEmailAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)));
            if(!result.Succeeded)
            {
                _logger.Here().Error("Failed to confirm email for the user {username}", user.UserName);
                EmailVerificationFailed = true;
                FailureMessage = "Failed to confirm email. Please connect to our support team";
                return Page();
            }

            _logger.Here().Information("user email successfully verified - {userid}", userId);
            FailureMessage = "";
            EmailVerificationFailed = false;
            EmailVerificationCompleted = true;

            return Page();
        }
    }
}
