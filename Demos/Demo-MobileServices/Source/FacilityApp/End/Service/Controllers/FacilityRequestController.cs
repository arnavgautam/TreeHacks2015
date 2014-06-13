using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MobileService.DataObjects;
using MobileService.Models;

namespace MobileService.Controllers
{
    using Microsoft.WindowsAzure.Mobile.Service.Security;
    using MobileService.Common.Providers;

    [AuthorizeLevel(AuthorizationLevel.User)]
    public class FacilityRequestController : TableController<FacilityRequest>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<FacilityRequest>(context, Request, Services);
        }

        // GET tables/FacilityRequest
        public IQueryable<FacilityRequest> GetAllFacilityRequest()
        {
            return Query(); 
        }

        // GET tables/FacilityRequest/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FacilityRequest> GetFacilityRequest(string id)
        {
            return Lookup(id);
        }

		// PATCH tables/FacilityRequest/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<FacilityRequest> PatchFacilityRequest(string id, Delta<FacilityRequest> patch)
        {
            var sharePointUri = SharePointProvider.SharePointUri;
            if (sharePointUri == null)
                Services.Settings.TryGetValue("SharePointUri", out sharePointUri);

            SharePointProvider.SharePointUri = sharePointUri;
            var facilityRequest = patch.GetEntity();

            sharePointUri = SharePointProvider.SharePointUri + string.Format(@"/getfolderbyserverrelativeurl('Documents')/Folders('Requests')/Files/Add(url='{0}.docx', overwrite=true)", facilityRequest.DocId);

            string authority;
            string sharePointResource;
            string activeDirectoryClientId;
            string activeDirectoryClientSecret;

            Services.Settings.TryGetValue("Authority", out authority);
            Services.Settings.TryGetValue("SharePointResource", out sharePointResource);
            Services.Settings.TryGetValue("ActiveDirectoryClientId", out activeDirectoryClientId);
            Services.Settings.TryGetValue("ActiveDirectoryClientSecret", out activeDirectoryClientSecret);

            var token = await SharePointProvider.RequestAccessToken((ServiceUser)this.User, authority, sharePointResource, activeDirectoryClientId, activeDirectoryClientSecret);                      
            var document = SharePointProvider.BuildDocument(facilityRequest);

            await SharePointProvider.UploadFile(sharePointUri, document, token, activeDirectoryClientId);

            return await this.UpdateAsync(id, patch);
        }

		// POST tables/FacilityRequest/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<IHttpActionResult> PostFacilityRequest(FacilityRequest item)
        {
			FacilityRequest current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

		// DELETE tables/FacilityRequest/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFacilityRequest(string id)
        {
             return DeleteAsync(id);
        }

    }
}