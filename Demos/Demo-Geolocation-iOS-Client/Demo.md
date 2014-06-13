<a name="title" />
# Connecting an iOS Geolocation App to a Backend in Microsoft Azure Websites #

---

<a name="Overview" />
## Overview ##

This demonstration shows how to connect a iOS client to a backend service running in Microsoft Azure Websites.  

Widows Azure Websites enables developers to quickly get up and running with websites.  Websites may be developed in ASP.NET, Node.js and PHP.  In addition, websites may use SQL or MySQL for data storage.  Deployment can be accomplished in several ways including TFS, FTP, and GIT.

<a id="goals" />
### Goals ###
In this demo, you will see:

1. How easy is to connect an iOS client to a backend running in Microsoft Azure Websites.

<a name="technologies" />
### Key Technologies ###

- [Microsoft Azure Websites](https://www.windowsazure.com/en-us/home/scenarios/web-sites/)
- [XCode](https://developer.apple.com/xcode/)

<a name="prerequisities" />
### Prerequisities ###

1.  Prior to starting this demo, you should have already completed the Geolocation - PHP demo.  This site functions as the mobile client's backend and is required.
2.  Visit http://<your-subdomain>.azurewebsites.net/api/Location/AddTestContainer in a browser prior to starting this demo.  This will generate a container in your storage account which will be used to add new images and videos.
3.  Run the iPhone Simulator and in the browser navigate somewhere so you can save an appropriate image for later use in the demo.  Once you have an image in the browser, long tap on it to open the save image dialog.

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
> During this demo we're going to connect an iOS client to the URL Shortening service that was just pushed to Microsoft Azure Websites.
>
> Lets start by opening the iOS application in XCode.

1. Navigate to the iOS Client demo's source folder.

1.  Open the ShortifierDemo.xcodeproj.

1.  Open Constants.m in the project navigator.

	![Opening Constants.m ](Images/constantsInProject.png?raw=true "Opening Constants")

	_Opening Constants_

1.  Replace <your-subdomain> with site URL you set up for the PHP URL Shortener.

	![Constant URLs](Images/yourSubdomainBefore.png?raw=true "Constant URLs")

	_Constant URLs_

1.  Afterwards, the URls should point to your site.

	![Constant URLs After Setting](Images/yourSubdomainAfter.png?raw=true "Constant URLs After Setting")

	_Constant URLs After Setting_

> **Speaking Point**
>
> That's all we need to do to get this app ready to talk to our backend.  Next, let's walk through some of the functionality.

<a name="segment2" />
### Reviewing the App's Functionality ###

1.  In XCode, go to the top right and ensure an iPhone simulator (e.g. iPhone 5.1 Simulator) is selected.

	![Selecting the Simulator](Images/xcodeBuildSettings.png?raw=true "Selecting the Simulator")

	_Selecting the Simulator_

1.  Go to the Product menu and select Build.

1.  Ensure the top build status shows a success.

	![Build Success](Images/buildSucceeded.png?raw=true "Build Success")

	_Build Success_

1.  Go to the Product menu and select Run.

1.  The iPhone Simulator should start and after a moment, you should be asked if Geodemo can use your location.  

	![Using your Location](Images/useYourLocation.png?raw=true "Using your Location")

	_Using your Location_

1.  Click ok to allow.  

1.  Depending on if the location has been set in your iPhone simulator before, a different location may show up.  Feel free to use your own coordinates going forward (preferebly located near your session).

1.  Go to the Debug menu and choose Location, then Custom Location. For Latitude, enter 47.610069 and for longitude, enter -122.342941.

	![Setting a Custom Location](Images/settingCustomLocation.png?raw=true "Setting Custom Location")

	_Setting Custom Location_

1.  The app should now center around the coordinates you entered.

	![Map Centered on Coordinates](Images/mapCenteredOnCoordinates.png?raw=true "Map Centered on Coordinates")

	_Map Centered on Coordinates_

	> **Speaking Point**
	>
	> At this point there aren't any pins on the map becuase we haven't added any (unless you did before this demo).  Let's add some now.

1.  Tap the Add POI (Point of Interest) button.

	![Tapping Add POI](Images/tappedAddPoi.png?raw=true "Tapping Add POI")

	_Tapping Add POI_

1.  Tap the Select Image button.

	![Tapping Select Image](Images/tappedSelectImage.png?raw=true "Tapping Select Image")

	_Tapping Select Image_

1.  Tap a saved image from the gallery.

	![Tapping Saved Image](Images/tappedImage.png?raw=true "Tapping Saved Image")

	_Tapping Saved Image_

1.  Tap the Get SAS URL button.

	![Tapping Get SAS URL](Images/tappedGetSasUrl.png?raw=true "Tapping Get SAS URL")

	_Tapping Get SAS URl_

	> **Speaking Point**
	>
	> SAS stands for Shared Access Signature.  A SAS URL gives you the ability to upload files to BLOB storage without having the account name and key in your app.  This is done for security purposes as if your key is in your app, other people could get access to it and upload whatever they wanted.  With the SAS URL, there is only a limited amount of time to upload file to the specific URL.

1.  In just a moment you should see a SAS URL appear on the screen.

	![Showing SAS URL](Images/showingSasUrl.png?raw=true "Showing SAS URL")

	_Showing SAS URL_

1.  Tap the Post POI button.

	![Tapping Post POI](Images/tappedPostPOI.png?raw=true "Tapping Post POI")

	_Tapping POST POI_

1.  After successful posting, the screen will be updated (briefly to say it was a success).

	![Succesful Posting](Images/savedPOI.png?raw=true "Successful Posting")

	_Successful Posting_

1.  This screen will change quickly back to the map.  When you return to the map, you'll see a new pin has been dropped on the map.

	![Pin Dropped](Images/pinDropped.png?raw=true "Pin Dropped")

	_Pin Dropped_

1.  Tap on the PIN for more information on the post.

	![Pin Info](Images/pinInfo.png?raw=true "Pin Info")

	_Pin Info_

<a name="segment3" />
### Reviewing the App's Code Base ###

> **Speaking Point**
>
> Now that we've seen the functionality of the application.  Let's look through some of the code that accomplishes the communications with the web service.

1.  Return to XCode.

1.  Return to the Project Navigator by clicking the button in the top left.

	![Returning to the Project Navigator](Images/returnToProjectNavigator.png?raw=true "Returning to the Project Navigator")

	_Returning to the Project Navigator_

1.  Open ViewController.m in the project navigator.

1.  Point out the following code in the ViewController class.

	````C#
	(void)getCurrentPointsOfInterest {
		 NSURL *requestUrl = [NSURL URLWithString:[NSString stringWithFormat:@"%@?latitude=%f&longitude=%f&radiusInMeters=1000",kGetPOIUrl, [currentLocation coordinate].latitude, [currentLocation coordinate].longitude]];
		 //Get our POI
		 dispatch_async(kBgQueue, ^{
			  NSData* data = [NSData dataWithContentsOfURL: 
									requestUrl];
			  [self performSelectorOnMainThread:@selector(fetchedData:) 
											 withObject:data waitUntilDone:YES];
		 });
	}
	````

	> **Speaking Point**
	>
	> This method builds a request URL with a constant, kGetPOIUrl, and the current location and latitude of the user as well as a radius.  We then use a background queue to poll the server and call the fetchedData method when we hear back from the server.  Let's look at the fetchedData method.

	````C#
	(void)fetchedData:(NSData *)responseData {
		 NSError* error;
		 //Build a JSON object from the response Data
		 NSArray* json = [NSJSONSerialization 
									  JSONObjectWithData:responseData //1
									  
									  options:kNilOptions 
									  error:&error];
		 
		 //Go through each POI in the JSON data and pull out the important fields
		 for (NSDictionary *pointOfInterest in json) {
			  NSLog(@"POI:%@", pointOfInterest);

			  
			  CLLocationCoordinate2D mapPoint = mapView.centerCoordinate;
			  NSString *latString = [pointOfInterest valueForKey:@"Latitude"];
			  NSString *longString = [pointOfInterest valueForKey:@"Longitude"];
			  NSString *description = [pointOfInterest valueForKey:@"Description"];
			  NSString *url = [pointOfInterest valueForKey:@"Url"];
			  mapPoint.latitude = [latString doubleValue];
			  mapPoint.longitude = [longString doubleValue];
					
			  //Add a new map annotation for each POI
			  MKPointAnnotation *anny = [[MKPointAnnotation alloc] init];
			  anny.coordinate = mapPoint;
			  anny.title = description;
			  anny.subtitle = url;
			  [mapView addAnnotation:anny];        
		 }
	}    
	````

	> **Speaking Point**
	>
	> Here we're parsing out the individual points of interest and creating new map annotations for each one.  Those annotations are then added to the map view.

1.  Open the NewPOIViewControllerViewController.m file.  Look first at the getSasURL method.

	````C#
	(IBAction)getSasUrl:(id)sender {
		 NSString *time = [NSString stringWithFormat:@"%i", -CFAbsoluteTimeGetCurrent()];
		 
		 //Just pass the call over to our generic serviceCaller
		 [serviceCaller postToUrl:[NSString stringWithFormat:@"%@?container=%@&blobname=%@"
											,kGetSASUrl, kContainerName, time]
							  withBody:nil andPostType:@"GET" andContentType:nil withCallback:^(NSString *response) {
									//This is the callback code that the ServiceCaller will call upon success
									labelSasUrlInfo.text = response;
									sasURL = [response stringByReplacingOccurrencesOfString:@"\"" withString:@""];
									NSLog(@"SAS URL: %@", sasURL);
									buttonPostPOI.enabled = YES;
									buttonGetSasURL.enabled = NO;
							  }];
	}
	````

	> **Speaking Point**
	>
	> Here we are building a url from the constant kGetSASUrl as well as a container name (we just use a constant for that, and a name (we're using the current time in ticks).  We do a GET call on that and when done, we enable the user to tap Post POI.

1.  Look at the postPOI method next (not shown here for brevity).

	> **Speaking Point**
	>
	> The PostPOI method really does two different things.  First it uploads our image file to BLOB storage using the SAS URL.  Remember that this URL only works for a limited amount of time and has to be authorized by the server.  Next, provided that was a success, we post the details of the point of interest (the latitude and location, the image name, the image url, etc) to another endpoint in the service.  When that comes back a success, we return back to the map view and trigger a refresh of the points of interest within our radius.

---

<a name="summary" />
## Summary ##

In this demo, you saw how easy it is to communicate with a service layer running in Microsoft Azure Websites.  You took a prebuilt iOS application and pointed it at a web service that you previously deployed and demonstrated how it works.  In addition, you made use of Blob storage to store files.
