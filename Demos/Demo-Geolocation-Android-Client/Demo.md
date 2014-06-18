<a name="title" />
# Connecting an Android Geolocation App to a Backend in Microsoft Azure Websites #

---

<a name="Overview" />
## Overview ##

This demonstration shows how to connect a Android client to a backend service running in Microsoft Azure Websites.  

Widows Azure Websites enables developers to quickly get up and running with websites.  Websites may be developed in ASP.NET, Node.js and PHP.  In addition, websites may use SQL or MySQL for data storage.  Deployment can be accomplished in several ways including TFS, FTP, and GIT.

<a id="goals" />
### Goals ###
In this demo, you will see:

1. How easy is to connect an Android client to a backend running in Microsoft Azure Websites.

<a name="technologies" />
### Key Technologies ###

- [Microsoft Azure Websites](https://www.windowsazure.com/en-us/home/scenarios/web-sites/)
- [Eclipse](http://eclipse.org/)
- [Android](http://developer.android.com/index.html)

<a name="prerequisities" />
### Prerequisities ###

1.  Prior to starting this demo, you should have already completed the Geolocation - PHP demo.  This site functions as the mobile client's backend and is required.
2.  This demo also requires the installation of Eclipse and the Android SDK.  Instructions on this are beyond the scope of this demo.
3.  This demo also requires creation of an Android Virtual Device.  Instructions on this are beyond the scope of this demo.

---

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Connecting the Mobile Client to the Web Service](#segment1).
1. [Reviewing the App's Functionality](#segment2).
1. [Reviewing the App's Code Base](#segment3).

<a name="segment1" />
### Connecting the Mobile Client to the Web Service ###

> **Speaking Point**
>
> During this demo we're going to connect an Android client to the Geolocation service that was just pushed to Microsoft Azure Websites.
>
> Lets start by opening the Android application in Eclipse.

1. Open Eclipse.

1. Go to the File menu and choose Import.  

1. Choose "Existing Projects into Workspace".

1. Browse to the source directory of the GeoLocation-Android-Client folder.

1. Select the GeoDemo project and continue to import.

	![Importing GeoDemo](Images/importProject.png?raw=true "Importing GeoDemo")

	_Importing GeoDemo_

1. Expand the GeoDemo project in the Package Explorer and find Constants.java under src/com.msdpe.geodemo.misc.

	![Opening Constants](Images/openConstants.png?raw=true "Opening Constants)

	_Opening Constants_

1. Replace <your-subdomain> with site URL you set up for the PHP URL Shortener.

	````C#
	public class Constants {

		public static final String kFindPOIUrl = "http://<Your Subdomain>.azurewebsites.net/api/Location/FindPointsOfInterestWithinRadius";
		public static final String kBlobSASUrl = "http://<Your Subdomain>.azurewebsites.net/api/blobsas/get?container=%s&blobname=%s";
		public static final String kAddPOIUrl = "http://<Your Subdomain>.azurewebsites.net/api/location/postpointofinterest/";
		
		public static final String kContainerName = "test";
	}
	````

1. Afterwards, the URls should point to your site.

	````C#
	public class Constants {

		public static final String kFindPOIUrl = "http://phpgeo.azurewebsites.net/api/Location/FindPointsOfInterestWithinRadius";
		public static final String kBlobSASUrl = "http://phpgeo.azurewebsites.net/api/blobsas/get?container=%s&blobname=%s";
		public static final String kAddPOIUrl = "http://phpgeo.azurewebsites.net/api/location/postpointofinterest/";
		
		public static final String kContainerName = "test";
	}

	````

	> **Speaking Point**
	>
	> That's all we need to do to get this app ready to talk to our backend.  Next, let's walk through some of the functionality.

<a name="segment2" />
### Reviewing the App's Functionality ###

1.  In Eclipse, right click on the Geo project and go to Run As and choose Android Application.

	![Running the Application](Images/runProject.png?raw=true "Running the Application")

	_Running the Application_

1.  The Android AVD should start.  It may first load a locked screen.  If it does, swipe right on the lock.

	![Locked Android](Images/unlockAVD.png?raw=true "Locked Android")

	_Locked Android AVD_

1.  After loading, or unlocking, the Android emulator should bring up the geodemo.

	![First Run of GeoDemo](Images/firstRun.png?raw=true "First Run of GeoDemo")

	_First Run of GeoDemo_


	> **Speaking Point**
	>
	> The Android emulator doens't natively pull your location at all.  For this reason, we will fake our location.

1.  Return to Eclipse.

1.  Go to the Window menu, select Open Perspective, and then select DDMS.  

1.  In the top left of the new display, select your emulator AVD.

	![Selecting AVD in DDMS](Images/ddmsSelectingAVD.png?raw=true "Selecting AVD in DDMS")

	_Selecting AVD in DDMS_

1.  Scroll down in the Emulator Control panel beneath where you selected the AVD running.  Enter a longitude and latitude and then select send.

	![Setting the Location](Images/settingLocation.png?raw=true "Setting the Location")

	_Setting the Location_

1.  The app should now center around the coordinates you entered.

	![Map Centered on Coordinates](Images/mapCenteredOnCoordinates.png?raw=true "Map Centered on Coordinates")

	_Map Centered on Coordinates_

	> **Speaking Point**
	>
	> At this point there aren't any pins on the map becuase we haven't added any (unless you did before this demo).  Let's add some now.

1.  Tap the Menu Button.

	![Tapping Menu](Images/tappingMenu.png?raw=true "Tapping Menu")

	_Tapping Menu_

1.  Tap the Add POI (Point of Interest) button in the menu that appears at the bottom.

	![Tapping Add POI](Images/tappingAddPoi.png?raw=true "Tapping Add POI")

	_Tapping Add POI_

1.  Tap the Select Image button.

	![Tapping Select Image](Images/tappingSelectImage.png?raw=true "Tapping Select Image")

	_Tapping Select Image_

1.  Tap a saved image from the gallery.

	![Tapping Saved Image](Images/tappedImage.png?raw=true "Tapping Saved Image")

	_Tapping Saved Image_

1.  Tap the Get SAS URL button.

	![Tapping Get SAS URL](Images/tappingGetSasUrl.png?raw=true "Tapping Get SAS URL")

	_Tapping Get SAS URl_

	> **Speaking Point**
	>
	> SAS stands for Shared Access Signature.  A SAS URL gives you the ability to upload files to BLOB storage without having the account name and key in your app.  This is done for security purposes as if your key is in your app, other people could get access to it and upload whatever they wanted.  With the SAS URL, there is only a limited amount of time to upload file to the specific URL.

1.  In just a moment you should see a SAS URL appear on the screen.

	![Showing SAS URL](Images/showingSasUrl.png?raw=true "Showing SAS URL")

	_Showing SAS URL_

1.  Scroll down if necessary and tap the Post POI button.

	![Tapping Post POI](Images/tappingSavePoi.png?raw=true "Tapping Post POI")

	_Tapping POST POI_

1.  This screen will change quickly back to the map.  When you return to the map, you'll see a new pin has been dropped on the map.

	![Pin Dropped](Images/pinDropped.png?raw=true "Pin Dropped")

	_Pin Dropped_

1.  Tap on the PIN for more information on the post.

	![Pin Info](Images/pinInfo.png?raw=true "Pin Info")

	_Pin Info_

1.  Tap the View Image button to see the image retrieved from the server.

<a name="segment3" />
### Reviewing the App's Code Base ###

> **Speaking Point**
>
> Now that we've seen the functionality of the application.  Let's look through some of the code that accomplishes the communications with the web service.

1.  Return to Eclipse.

1.  If you are still in the DDMS perspective, click the arrows pointing to the right at the top right of Eclipse and choose Java from the drop down.

	![Returning to Java Perspective](Images/javaPerspective.png?raw=true "Returning to Java Perspective")

	_Returning to Java Perspective_

1.  Open the GeoDemoActivity.java file under src/com.msdpe.geodemo.ui.

1.  Look at the loadPointsFromServer method.

	````C#
	String fetchUrl = Constants.kFindPOIUrl + "?latitude="
			+ location.getLatitude() + "&longitude="
			+ location.getLongitude() + "&radiusInMeters=1000";
	URL url = new URL(fetchUrl);
	HttpURLConnection urlConnection = (HttpURLConnection) url
			.openConnection();
	try {
		InputStream in = new BufferedInputStream(
				urlConnection.getInputStream());

		BufferedReader r = new BufferedReader(new InputStreamReader(in));
		StringBuilder stringBuilderResult = new StringBuilder();
		String line;
		while ((line = r.readLine()) != null) {
			stringBuilderResult.append(line);
		}
		Log.w(TAG, stringBuilderResult.toString());

		JSONArray jsonArray = new JSONArray(
				stringBuilderResult.toString());
		for (int i = 0; i < jsonArray.length(); i++) {
			JSONObject jsonObject = jsonArray.getJSONObject(i);
			Log.i(TAG, "Obj: " + jsonObject.toString());
			Double latitude = jsonObject.getDouble("Latitude");
			Double longitude = jsonObject.getDouble("Longitude");
			String description = jsonObject.getString("Description");
			String itemUrl = jsonObject.getString("Url");
			// The item URL comes back with quotes at the beginning,
			// so we strip them out
			itemUrl = itemUrl.replace("\"", "");

			// Create a new geo point with this information and add it
			// to the overlay
			GeoPoint point = coordinatesToGeoPoint(new double[] {
					latitude, longitude });
			OverlayItem overlayitem = new OverlayItem(point,
					description, itemUrl);
			mItemizedOverlay.addOverlay(overlayitem);
		}
	````

	> **Speaking Point**
	>
	> This method builds a url using the constant, kFindPOIUrl, which we set earlier, along with the current latitude and longitude as well as a radius.  A HttpURLConnection is then used to communicate with the service end point at that URL.  The data is read back in JSON format, put into a JSON Array, and looped through pulling out each Point of Interest's information.  Overlay items are then created and added to the map view.  

1.  Open the AddPointOfInterestActivity.java from src/com.msdpe.geodemo.ui.

1.  Pull up the getSas method.

	````C#
	String fetchUrl = String.format(Constants.kBlobSASUrl, Constants.kContainerName,
			System.currentTimeMillis());
	Log.i(TAG, "FetchURL: " + fetchUrl);
	URL url = new URL(fetchUrl);
	HttpURLConnection urlConnection = (HttpURLConnection) url
			.openConnection();

	InputStream in = new BufferedInputStream(
			urlConnection.getInputStream());

	BufferedReader r = new BufferedReader(new InputStreamReader(in));
	StringBuilder total = new StringBuilder();
	String line;
	while ((line = r.readLine()) != null) {
		total.append(line);
	}
	Log.w(TAG, total.toString());
	_blobImagePostString = total.toString();
	````

	> **Note:** This method is building a URL from the kBlobSASURL constant we set up earlier as well as a container name (set to "test" for now) and the current time in milliseconds.  We then use a HttpURLConnection to hit the server and request our SAS.

1.  Now look at the postPointOfInterestToServer method (not shown here for brevity).

	> **Speaking Point**
	>
	> The PostPOI method really does two different things.  First it uploads our image file to BLOB storage using the SAS URL.  Remember that this URL only works for a limited amount of time and has to be authorized by the server.  Next, provided that was a success, we post the details of the point of interest (the latitude and location, the image name, the image url, etc) to another endpoint in the service.  When that comes back a success, we return back to the map view and trigger a refresh of the points of interest within our radius.

---

<a name="summary" />
## Summary ##

In this demo, you saw how easy it is to communicate with a service layer running in Microsoft Azure Websites.  You took a prebuilt Android application and pointed it at a web service that you previously deployed and demonstrated how it works.  In addition, you made use of Blob storage to store files.
