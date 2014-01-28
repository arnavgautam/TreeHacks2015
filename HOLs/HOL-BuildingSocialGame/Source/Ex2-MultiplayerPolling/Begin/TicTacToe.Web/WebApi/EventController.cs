namespace TicTacToe.Web.WebApi
{
    using System;
    using System.Json;
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.Samples.SocialGames.Web.Services;

    public class EventController : BaseApiController
    {
        private IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostEvent([FromUri]string id)
        {
            dynamic formContent = this.Request.Content.ReadAsAsync<JsonValue>().Result;

            try
            {
                this.eventService.PostEvent(id, formContent);
                return SuccessResponse;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}