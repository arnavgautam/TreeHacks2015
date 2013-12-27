namespace AutomatedHelloWorld.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutomatedHelloWorld.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;

    [Authorize]
    public class AccountController : Controller
    {
        public AccountController() 
        {
            this.IdentityStore = new IdentityStoreManager();
            this.AuthenticationManager = new IdentityAuthenticationManager(this.IdentityStore);
        }

        public AccountController(IdentityStoreManager storeManager, IdentityAuthenticationManager authManager)
        {
            this.IdentityStore = storeManager;
            this.AuthenticationManager = authManager;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        public IdentityStoreManager IdentityStore { get; private set; }

        public IdentityAuthenticationManager AuthenticationManager { get; private set; }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Validate the user password
                if (await this.AuthenticationManager.CheckPasswordAndSignIn(this.HttpContext, model.UserName, model.Password, model.RememberMe))
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            return this.View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create a profile, password, and link the local login before signing in the user
                    User user = new User(model.UserName);
                    if (await this.IdentityStore.CreateLocalUser(user, model.Password))
                    {
                        await this.AuthenticationManager.SignIn(this.HttpContext, user.Id, isPersistent: false);
                        return this.RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to register user name: " + model.UserName);
                    }
                }
                catch (IdentityException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            string userId = User.Identity.GetUserId();
            if (await this.IdentityStore.RemoveLogin(User.Identity.GetUserId(), loginProvider, providerKey))
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }

            return this.RedirectToAction("Manage", new { Message = message });
        }

        // GET: /Account/Manage
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : string.Empty;
            ViewBag.HasLocalPassword = await this.IdentityStore.HasLocalLogin(User.Identity.GetUserId());
            ViewBag.ReturnUrl = Url.Action("Manage");
            return this.View();
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            string userId = User.Identity.GetUserId();
            bool hasLocalLogin = await this.IdentityStore.HasLocalLogin(userId);
            ViewBag.HasLocalPassword = hasLocalLogin;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalLogin)
            {               
                if (ModelState.IsValid)
                {
                    bool changePasswordSucceeded = await this.IdentityStore.ChangePassword(User.Identity.GetUserName(), model.OldPassword, model.NewPassword);
                    if (changePasswordSucceeded)
                    {
                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Create the local login info and link the local account to the user
                        if (await this.IdentityStore.CreateLocalLogin(userId, User.Identity.GetUserName(), model.NewPassword))
                        {
                            return this.RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to set password");
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError(string.Empty, e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { loginProvider = provider, ReturnUrl = returnUrl }), this.AuthenticationManager);
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string loginProvider, string returnUrl)
        {
            ClaimsIdentity id = await this.AuthenticationManager.GetExternalIdentity(HttpContext);
            if (!this.AuthenticationManager.VerifyExternalIdentity(id, loginProvider))
            {
                return this.View("ExternalLoginFailure");
            }

            // Sign in this external identity if its already linked
            if (await this.AuthenticationManager.SignInExternalIdentity(this.HttpContext, id, loginProvider)) 
            {
                return this.RedirectToLocal(returnUrl);
            }
            else if (User.Identity.IsAuthenticated)
            {
                // Try to link if the user is already signed in
                if (await this.AuthenticationManager.LinkExternalIdentity(id, User.Identity.GetUserId(), loginProvider))
                {
                    return this.RedirectToLocal(returnUrl);
                }
                else 
                {
                    return this.View("ExternalLoginFailure");
                }
            }
            else
            {
                // Otherwise prompt to create a local user
                ViewBag.ReturnUrl = returnUrl;
                return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = id.Name, LoginProvider = loginProvider });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Manage");
            }
            
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                try
                {
                    if (await this.AuthenticationManager.CreateAndSignInExternalUser(this.HttpContext, model.LoginProvider, new User(model.UserName)))
                    {
                        return this.RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return this.View("ExternalLoginFailure");
                    }
                }
                catch (IdentityException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return this.View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut(this.HttpContext);
            return this.RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return (ActionResult)PartialView("_ExternalLoginsListPartial", new List<AuthenticationDescription>(this.AuthenticationManager.GetExternalAuthenticationTypes(HttpContext)));
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            return Task.Run(async () =>
            {
                var linkedAccounts = await this.IdentityStore.GetLogins(User.Identity.GetUserId());
                ViewBag.ShowRemoveButton = linkedAccounts.Count > 1;
                return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
            }).Result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.IdentityStore != null)
            {
                this.IdentityStore = null;
                this.IdentityStore.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUrl, IdentityAuthenticationManager manager)
            {
                this.LoginProvider = provider;
                this.RedirectUrl = redirectUrl;
                this.Manager = manager;
            }

            public string LoginProvider { get; set; }

            public string RedirectUrl { get; set; }

            public IdentityAuthenticationManager Manager { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                this.Manager.Challenge(context.HttpContext, this.LoginProvider, this.RedirectUrl);
            }
        }
        
        #endregion
    }
}