namespace Eventures.Web.Areas.Identity.Pages.Account
{
    using Eventures.Domain;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<EventuresUser> _signInManager;
        private readonly UserManager<EventuresUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        // private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<EventuresUser> userManager,
            SignInManager<EventuresUser> signInManager,
            ILogger<RegisterModel> logger/*,
            IEmailSender emailSender*/)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = @"
                The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = @"
                The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Unique citizen number")]
            public string UCN { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this._signInManager
                .GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this._signInManager
                .GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                EventuresUser user = new EventuresUser
                {
                    UserName = this.Input.Username,
                    Email = this.Input.Email,
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    UCN = this.Input.UCN
                };
                IdentityResult result = await this._userManager.CreateAsync(user, this.Input.Password);



                if (result.Succeeded)
                {
                    this._logger.LogInformation("User created a new account with password.");

                    if ((await _userManager.GetUsersInRoleAsync("Admin")).Count()==0)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this._userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this._signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }
                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
