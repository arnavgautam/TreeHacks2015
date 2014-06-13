using System;
using System.Drawing;
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace ADALBinding
{
	public delegate void CompletionBlock(NSString result);

	[BaseType (typeof (NSObject))]
	public partial interface ADALWrapper {

		[Static, Export ("getToken:authority:redurectUri:resourceId:clientId:completionHandler:")]
		void GetToken (bool clearCache, string authority, string redirectUriString, string resourceId, string clientId, CompletionBlock completionBlock);
	}

	[BaseType (typeof (NSError))]
	public partial interface ADAuthenticationError
	{
		[Export ("protocolCode")]
		string ProtocolCode { get; }

		[Export ("errorDetails")]
		string ErrorDetails { get; }

		[Static, Export ("errorFromArgument:argumentName:")]
		ADAuthenticationError ErrorFromArgument (NSObject argument, string argumentName);

		[Static, Export ("errorFromUnauthorizedResponse:errorDetails:")]
		ADAuthenticationError ErrorFromUnauthorizedResponse (int responseCode, string errorDetails);

		[Static, Export ("errorFromNSError:errorDetails:")]
		ADAuthenticationError ErrorFromNSError (NSError error, string errorDetails);

		[Static, Export ("errorFromAuthenticationError:protocolCode:errorDetails:")]
		ADAuthenticationError ErrorFromAuthenticationError (int code, string protocolCode, string errorDetails);

		[Static, Export ("unexpectedInternalError:")]
		ADAuthenticationError UnexpectedInternalError (string errorDetails);
	}

	[BaseType (typeof (NSObject))]
	public partial interface ADAuthenticationResult
	{
		[Export ("status")]
		ADAuthenticationResultStatus Status { get; }

		[Export ("accessToken")]
		string AccessToken { get; }

		[Export ("tokenCacheStoreItem")]
		ADTokenCacheStoreItem TokenCacheStoreItem { get; }

		[Export ("error")]
		ADAuthenticationError Error { get; }

		[Export ("multiResourceRefreshToken")]
		bool MultiResourceRefreshToken { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface ADTokenCacheStoreItem 
	{
		[Export ("resource")]
		string Resource { get; set; }

		[Export ("authority")]
		string Authority { get; set; }

		[Export ("clientId")]
		string ClientId { get; set; }

		[Export ("accessToken")]
		string AccessToken { get; set; }

		[Export ("accessTokenType")]
		string AccessTokenType { get; set; }

		[Export ("refreshToken")]
		string RefreshToken { get; set; }

		[Export ("expiresOn")]
		NSDate ExpiresOn { get; set; }

		[Export ("userInformation")]
		ADUserInformation UserInformation { get; set; }

		[Export ("multiResourceRefreshToken")]
		bool MultiResourceRefreshToken { [Bind ("isMultiResourceRefreshToken")] get; }

		[Export ("extractKeyWithError:")]
		ADTokenCacheStoreKey ExtractKeyWithError (out ADAuthenticationError error);

		[Export ("isExpired")]
		bool IsExpired { get; }

		[Export ("isEmptyUser")]
		bool IsEmptyUser { get; }

		[Export ("isSameUser:")]
		bool IsSameUser (ADTokenCacheStoreItem other);
	}

	[BaseType (typeof (NSObject))]
	public partial interface ADUserInformation 
	{
		[Static, Export ("userInformationWithUserId:error:")]
		ADUserInformation UserInformationWithUserId (string userId, out ADAuthenticationError error);

		[Static, Export ("userInformationWithIdToken:error:")]
		ADUserInformation UserInformationWithIdToken (string idToken, out ADAuthenticationError error);

		[Export ("userId")]
		string UserId { get; }

		[Export ("userIdDisplayable")]
		bool UserIdDisplayable { get; set; }

		[Export ("givenName")]
		string GivenName { get; set; }

		[Export ("familyName")]
		string FamilyName { get; set; }

		[Export ("identityProvider")]
		string IdentityProvider { get; set; }

		[Export ("eMail")]
		string EMail { get; set; }

		[Export ("uniqueName")]
		string UniqueName { get; set; }

		[Export ("upn")]
		string Upn { get; set; }

		[Export ("tenantId")]
		string TenantId { get; set; }

		[Export ("subject")]
		string Subject { get; set; }

		[Export ("userObjectId")]
		string UserObjectId { get; set; }

		[Export ("guestId")]
		string GuestId { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface ADTokenCacheStoreKey 
	{
		[Static, Export ("keyWithAuthority:resource:clientId:error:")]
		ADTokenCacheStoreKey KeyWithAuthority (string authority, string resource, string clientId, out ADAuthenticationError error);

		[Export ("authority")]
		string Authority { get; }

		[Export ("resource")]
		string Resource { get; }

		[Export ("clientId")]
		string ClientId { get; }
	}

	public delegate void ADAuthenticationCallback(ADAuthenticationResult result);

	[BaseType (typeof (NSObject))]
	public partial interface ADAuthenticationContext
	{
		[Export ("initWithAuthority:validateAuthority:tokenCacheStore:error:")]
		IntPtr Constructor (string authority, bool validateAuthority, ADTokenCacheStoring tokenCache, out ADAuthenticationError error);

		[Static, Export ("authenticationContextWithAuthority:error:")]
		ADAuthenticationContext AuthenticationContextWithAuthority (string authority, out ADAuthenticationError error);

		[Static, Export ("authenticationContextWithAuthority:validateAuthority:error:")]
		ADAuthenticationContext AuthenticationContextWithAuthority (string authority, bool validate, out ADAuthenticationError error);

		[Static, Export ("authenticationContextWithAuthority:tokenCacheStore:error:")]
		ADAuthenticationContext AuthenticationContextWithAuthority (string authority, ADTokenCacheStoring tokenCache, out ADAuthenticationError error);

		[Static, Export ("authenticationContextWithAuthority:validateAuthority:tokenCacheStore:error:")]
		ADAuthenticationContext AuthenticationContextWithAuthority (string authority, bool validate, ADTokenCacheStoring tokenCache, out ADAuthenticationError error);

		[Export ("authority")]
		string Authority { get; }

		[Export ("validateAuthority")]
		bool ValidateAuthority { get; set; }

		[Export ("tokenCacheStore")]
		ADTokenCacheStoring TokenCacheStore { get; set; }

		[Export ("correlationId")]
		Guid CorrelationId { get; set; }

		[Export ("acquireTokenWithResource:clientId:redirectUri:completionBlock:")]
		void AcquireTokenWithResource (string resource, string clientId, NSUrl redirectUri, ADAuthenticationCallback completionBlock);

		[Export ("acquireTokenWithResource:clientId:redirectUri:userId:completionBlock:")]
		void AcquireTokenWithResource (string resource, string clientId, NSUrl redirectUri, string userId, ADAuthenticationCallback completionBlock);

		[Export ("acquireTokenWithResource:clientId:redirectUri:userId:extraQueryParameters:completionBlock:")]
		void AcquireTokenWithResource (string resource, string clientId, NSUrl redirectUri, string userId, string queryParams, ADAuthenticationCallback completionBlock);

		[Export ("acquireTokenWithResource:clientId:redirectUri:promptBehavior:userId:extraQueryParameters:completionBlock:")]
		void AcquireTokenWithResource (string resource, string clientId, NSUrl redirectUri, ADPromptBehavior promptBehavior, string userId, string queryParams, ADAuthenticationCallback completionBlock);

		[Export ("acquireTokenByRefreshToken:clientId:resource:completionBlock:")]
		void AcquireTokenByRefreshToken (string refreshToken, string clientId, string resource, ADAuthenticationCallback completionBlock);
	}

	[BaseType (typeof (NSObject))]
	public partial interface ADTokenCacheStoring
	{
		[Export ("allItems")]
		NSObject [] AllItems { get; }

		[Export ("getItemWithKey:userId:")]
		ADTokenCacheStoreItem GetItemWithKey (ADTokenCacheStoreKey key, string userId);

		[Export ("getItemsWithKey:")]
		NSObject [] GetItemsWithKey (ADTokenCacheStoreKey key);

		[Export ("addOrUpdateItem:error:")]
		void AddOrUpdateItem (ADTokenCacheStoreItem item, out ADAuthenticationError error);

		[Export ("removeItemWithKey:userId:")]
		void RemoveItemWithKey (ADTokenCacheStoreKey key, string userId);

		[Export ("removeAll")]
		void RemoveAll ();
	}
}

