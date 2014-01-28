namespace TicTacToe.Web.WebApi
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.Samples.SocialGames.Web.Services;

    public class AuthController : BaseApiController
    {
        private IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public HttpResponseMessage LoginSelector(string returnUrl)
        {
            try
            {
                var json = this.authService.LoginSelector(returnUrl);
                return HttpResponse<string>(json.ToString(), contentType: "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}