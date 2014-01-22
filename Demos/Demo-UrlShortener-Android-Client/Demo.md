<a name="title" />
# Connecting an Android URL Shortener to a Backend in Windows Azure Websites #

---

<a name="Overview" />
## Overview ##

This demonstration shows how to connect an Android client to a backend service running in Windows Azure Websites.  

Widows Azure Websites enables developers to quickly get up and running with websites.  Websites may be developed in ASP.NET, Node.js and PHP.  In addition, websites may use SQL or MySQL for data storage.  Deployment can be accomplished in several ways including TFS, FTP, and GIT.

<a id="goals" />
### Goals ###
In this demo, you will see:

1. How easy is to connect an Android client to a backend running in Windows Azure Websites.

<a name="technologies" />
### Key Technologies ###

- [Windows Azure Web Sites](https://www.windowsazure.com/en-us/home/scenarios/web-sites/)
- [Eclipse](http://eclipse.org/)
- [Android](http://developer.android.com/index.html)

<a name="prerequisities" />
### Prerequisities ###

1.  Prior to starting this demo, you should have already completed the URL Shortener - PHP demo.  This site functions as the mobile client's backend and is required.
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
> During this demo we're going to connect an Android client to the URL Shortening service that was just pushed to Windows Azure Websites.
>
> Lets start by opening the Android application in Eclipse.

1.  Open Eclipse.

1.  Go to the File menu and choose Import.  

1.  Choose "Existing Projects into Workspace".

1.  Browse to the source directory of the UrlShortener-Android-Client folder.

1.  Select the ShortifierDemo project and continue to import.

	![Importing ShortifierDemo](Images/importProject.png?raw=true "Importing ShortifierDemo")

	_Importing ShortifierDemo_

1.  Expand the ShortifierDemo project in the Package Explorer and find Constants.java under src/com.msdpe.shortifierdemo.misc.

	![Opening Constants](Images/openConstants.png?raw=true "Opening Constants")

	_Opening Constants_

1.  Replace <your-subdomain> with site URL you set up for the PHP URL Shortener.

	````C#
	public class Constants {
		public static final String kShortifierRootUrl = "http://<your-subdomain>.azurewebsites.net/";
		public static final String kGetAllUrl = "http://<your-subdomain>.azurewebsites.net/api-getall";
		public static final String kAddUrl = "http://<your-subdomain>.azurewebsites.net/api-add";
	}
	````

1.  Afterwards, the URls should point to your site.

	````C#
	public class Constants {
		public static final String kShortifierRootUrl = "http://urlshortenertest.azurewebsites.net/";
		public static final String kGetAllUrl = "http://urlshortenertest.azurewebsites.net/api-getall";
		public static final String kAddUrl = "http://urlshortenertest.azurewebsites.net/api-add";
	}

	````

	> **Speaking Point**
	>
	> That's all we need to do to get this app ready to talk to our backend.  Next, let's walk through some of the functionality.

<a name="segment2" />
### Reviewing the App's Functionality ###

1.  In Eclipse, right click on the ShortifierDemo project and go to Run As and choose Android Application.

	![Running the Application](Images/runProject.png?raw=true "Running the Application")

	_Running the Application_

1.  The Android AVD should start.  It may first load a locked screen.  If it does, swipe right on the lock.

	![Locked Android](Images/unlockAVD.png?raw=true "Locked Android")

	_Locked Android AVD_

1.  After loading (or unlocking), you should see the URL(s) you generated via the URL in the previous demo.

	![URL List](Images/urlList.png?raw=true "URL List")

	_URL List_

1.  Tap a shortened URL.	

	![Tapping a Shortened URL](Images/tappingShortenedUrl.png?raw=true "Tapping Shortened URL")

	_Tapping Shortened URL_

1.  You should now be viewing the details for a URL.

	![Viewing URL Details](Images/urlDetails.png?raw=true "Viewing URL Details")

	_Viewing URL Details_

1.  Tap the "Go to URL" button.

	![Tapping Go to URL](Images/tappingGoToUrl.png?raw=true "Tapping Go to URL")

	_Tapping Go to URL_

1.  You should now be taken to the URL Shortener website for the URL you tapped.  This should just show the full URL for the one tapped.

	![Viewing the URL from the Web](Images/viewingShortenedUrl.png?raw=true "Viewing the URL from the Web")

	_Viewing the URL from the Web_

1.  Return to the App (hold the home button for two seconds and then select MainActivity from the recent app list).

	![Returning to the App](Images/recentApps.png?raw=true "Returning to the App")

	_Returning to the App_

1.  From the URL Details screen, tap the back button.

	![Going Back to the List View](Images/goBack.png?raw=true "Going Back to the List View")

	_Going Back to the List View_

1.  From the list view, tap the menu button in the navigation bar.

	![Tapping Menu](Images/tappingMenu.png?raw=true "Tapping Menu")

	_Tapping Menu_

1.  In the menu that appears at the bottom, tap "Add URL"

	![Tapping Add](Images/tappingAdd.png?raw=true "Tapping Add")

	_Tapping Add_

1.  Enter a shortened URL (e.g. "so") and full URL (e.g. "http://www.stackoverflow.com") into the details screen.

	![Entering URL Details](Images/enteringUrlDetails.png?raw=true "Entering URL Details")

	_Entering URL Details_

	> **NOTE:** You MUST enter "http://" at the beginning of the URL.

1.  Tap the save button.

	![Tapping Save](Images/tappingSave.png?raw=true "Tapping Save")

	_Tapping Save_

1.  After tapping save, you will return to the list view and should see your new shortened URL.

	![After Saving New URL](Images/newUrlSlug.png?raw=true "After Saving New URL")

	_After Saving New URL_

<a name="segment3" />
### Reviewing the App's Code Base ###

> **Speaking Point**
>
> Now that we've seen the functionality of the application.  Let's look through some of the code that accomplishes the communications with the web service.

1.  Return to Eclipse.

1.  Open MainActivity.java in the Package Explorer.

	![Opening MainActivity](Images/openingMainActivity.png?raw=true "Opening Main Activity")

	_Opening MainActivity_

1.  Point out the following code in the MainActivity class.

	````C#
		private void startUrlFetchService() {
			final Intent serviceIntent = new Intent(Intent.ACTION_SYNC, null, 
					  getApplicationContext(), UrlFetchService.class);
			// put the specifics for the submission service commands
			serviceIntent.putExtra(UrlFetchService.RECEIVER_KEY, mReceiver);
			serviceIntent.putExtra(UrlFetchService.COMMAND_KEY, UrlFetchService.PERFORM_SERVICE_ACTIVITY);
			//Start the service
			startService(serviceIntent);
		}
	````
	> **Speaking Point**
	>
	>Here we're starting an Intent Service.  We're passing over some information including a reference to a receiver that is part of the MainActivity and will be used to make call backs from the service.  

1.  Open the UrlFecthService.java class and look at the fetchUrls method.

	````C#
URL url = new URL(Constants.kGetAllUrl);
	HttpURLConnection urlConnection = (HttpURLConnection) url
			.openConnection();
	try {
		InputStream in = new BufferedInputStream(
				urlConnection.getInputStream());				
		BufferedReader bufferReader = new BufferedReader(new InputStreamReader(in));
		StringBuilder stringBuilderResponse = new StringBuilder();
		String line;
		while ((line = bufferReader.readLine()) != null) {
			stringBuilderResponse.append(line);
		}
		//Java needs brackets to surround the JSON so we're adding them manually
		JSONArray jsonArray = new JSONArray("[" + stringBuilderResponse.toString()
				+ "]");
		//Get the array of URLs
		JSONObject urls = jsonArray.getJSONObject(0).getJSONObject(
				"Urls");
		//Iterate over all of the URLs and add them to the URL hashmap
		Iterator iter = urls.keys();
		while (iter.hasNext()) {
			String key = (String) iter.next();
			String value = urls.getString(key);
			urlMap.put(key, value);
		}	
	} 
````

	> **Speaking Point**
	>
	>Here we're creating a HttpURLConnection and using the kGetAllUrl that we defined in Constants.  We're reading the response from the URL connection and then converting that to a JSONArray.  We then pull all of the URLs out of that JSON data and put them into a hashmap (urlMap).  

1.  Continue on in the FetchUrls method.

	````C#
		if (fetchFailed) { // error
			mReceiver.send(STATUS_ERROR, Bundle.EMPTY);
			this.stopSelf();
			mReceiver.send(STATUS_FINISHED, Bundle.EMPTY);
		} else {
			Bundle bundle = new Bundle();
			bundle.putBoolean(SERVICE_WAS_SUCCESS_KEY, true);
			//put the urlMap into the bundle
			bundle.putSerializable("urlMap", urlMap);
			mReceiver.send(STATUS_SUCCESS, bundle);
			this.stopSelf();
			mReceiver.send(STATUS_FINISHED, Bundle.EMPTY);
		}
	````

	> **Speaking Point**
	>
	> After pulling down the data we use the receiver that was sent in when the service was created to call back to the UI.

1.  Return to MainActivity.java and look at the onReceiveResult method.

	````C#
		//Success, update the ListView
		mUrlMap = (HashMap<String, String>) 
				resultBundle.getSerializable("urlMap");
		showUrlsInListView(mUrlMap);

	````

	> **Speaking Point**
	>
	> When a success occurs, the hashmap is returned from the service intent and stored in the local activity.  Then the showUrlsInListView is called.  

1.  Look at the showUrlsInListView method

	````C#
		private void showUrlsInListView(HashMap<String, String> urlMap) {				
			TreeSet<String> treeSetKeys = new TreeSet<String>(urlMap.keySet());
			String[] keys = (String[]) treeSetKeys.toArray(new String[treeSetKeys.size()]);
			ArrayAdapter adapter = new ArrayAdapter<String>(this,
					android.R.layout.simple_list_item_1, keys);
			setListAdapter(adapter);
		}
	````

	> **Speaking Point**
	>
	> This method builds a TreeSet with the keys of the hash map.  A tree set is used to get an alphabetized list of the URL slugs.  An ArrayAdapter is then created with those keys and used as the ListAdapter for the ListView.

1.  Open UrlDetailsActivity.java in the Package Explorer.

1.  Look at the SaveUrl method.

	````C#
		protected void SaveUrl(String urlSlug, String fullUrl) {		
			new AddUrlTask(this).execute(urlSlug, fullUrl);
		}
	````

	> **Speaking Point**
	>
	> This method is called when the user taps the Save URL button.  It starts the AddUrlTask which is an AsyncTask which will run in the background.

1.  Look at the doInBackground method.

	````C#
	JSONObject jsonUrl = new JSONObject();
	try {
		jsonUrl.put("key", "my_key");
		jsonUrl.put("url_slug", params[0]);
		jsonUrl.put("url", params[1]);
	} catch (JSONException e) {
		Log.e("UrlDetailsActivity", "Error creating JSON object: " 
				+ e.getMessage());
	}
	Log.i("UrlDetailsActivity", "JSON: " + jsonUrl.toString());

	HttpURLConnection urlConnection = null;
	try {
		URL url = new URL(Constants.kAddUrl);
		 urlConnection= (HttpURLConnection) url//
				.openConnection();
		urlConnection.setDoOutput(true);
		urlConnection.setDoInput(true);
		urlConnection.setRequestMethod("POST");
		urlConnection.addRequestProperty("Content-Type", "application/json");
		urlConnection.setRequestProperty("Content-Length", "" + 
						Integer.toString(jsonUrl.toString().getBytes().length));			
		byte[] bytes = jsonUrl.toString().getBytes("UTF-8");			
		//Write JSON to Server
			DataOutputStream wr = new DataOutputStream (
							urlConnection.getOutputStream ());
			wr.writeBytes(jsonUrl.toString());
			wr.flush ();
			wr.close ();
		//Get response code
		int response = urlConnection.getResponseCode();
		//Read response
		InputStream inputStream = 
				new BufferedInputStream(urlConnection.getInputStream());
		BufferedReader bufferedReader = 
				new BufferedReader(new InputStreamReader(inputStream));
		StringBuilder stringBuilderResult = new StringBuilder();
		String line;
		while ((line = bufferedReader.readLine()) != null) {
			stringBuilderResult.append(line);
		}
		JSONObject statusObject = new JSONObject(stringBuilderResult.toString());
		String status = statusObject.getString("Status");
		return status;
	````

	> **Speaking Point**
	>
	> This method builds a JSON object with the data for the new shortened URL in it.  That data is then sent over to the server with a HttpURLConnection.  The response is then read back in and parsed into JSON.  

1.  Look at the onPostExecute method.

	> **Speaking Point**
	>
	> The onPostExecute method interpretes the response from the server and returns back to the MainActivity.

1.  Open MainActivity.java and look at the onActivityResult method.

	````C#
		protected void onActivityResult(int requestCode, int resultCode, Intent data) {
			if (requestCode == 1) {
				startUrlFetchService();
			}
			else 
				super.onActivityResult(requestCode, resultCode, data);
		 }
	````

	> **Speaking Point**
	>
	> This method just kicks off the URL fetch service again which will reload the List View with the new URL.

---

<a name="summary" />
## Summary ##

In this demo, you saw how easy it is to communicate with a service layer running in Windows Azure Websites.  You took a prebuilt Android application and pointed it at a web service that you previously deployed and demonstrated how it works.