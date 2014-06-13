<a name="title" />
# Connecting an iOS URL Shortener to a Backend in Microsoft Azure Websites #

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

1.  Prior to starting this demo, you should have already completed the URL Shortener - PHP demo.  This site functions as the mobile client's backend and is required.

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

1.  The iPhone Simulator should start and after a moment, you should see the URL(s) you generated via the URL in the previous demo.

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

1.  Return to the App (tap the home button twice and then select ShortifierDemo).

	![Returning to the App](Images/homeScreen.png?raw=true "Returning to the App")

	_Returning to the App_

1.  From the URL Details screen, tap the back button in the navigation bar.

	![Going Back to the Table View](Images/goBack.png?raw=true "Going Back to the Table View")

	_Going Back to the Table View_

1.  From the table view, tap the add button in the navigation bar.

	![Tapping Add](Images/tappingAdd.png?raw=true "Tapping Add")

	_Tapping Add_

1.  Enter a shortened URL (e.g. "so") and full URL (e.g. "http://www.stackoverflow.com") into the details screen.

	![Entering URL Details](Images/enteringUrlDetails.png?raw=true "Entering URL Details")

	_Entering URL Details_

	> **NOTE:** You MUST enter "http://" at the beginning of the URL.

1.  Tap the save button.

	![Tapping Save](Images/tappingSave.png?raw=true "Tapping Save")

	_Tapping Save_

1.  After tapping save, you will return to the table view and should see your new shortened URL.

	![After Saving New URL](Images/newUrlSlug.png?raw=true "After Saving New URL")

	_After Saving New URL_

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
	(void)viewDidLoad
	{
		 [super viewDidLoad];
		 //Hit the server for URL data
		 dispatch_queue_t backgroundQueue = dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0);
		 dispatch_async(backgroundQueue, ^{
			  NSData* data = [NSData dataWithContentsOfURL: 
									[NSURL URLWithString: kGetAllUrl]];
			  [self performSelectorOnMainThread:@selector(fetchedData:) 
											 withObject:data waitUntilDone:YES];
		 });
	}
	````
	> **Speaking Point**
	>
	>Here we're creating a background queue and dispatching it to run asynchronously in the background.  We're telling it to fetch the contents of the URL "kGetAllUrl" which was defined in the Constants.m file.  Finally, we're telling it to call the **fetchedData** method when it's complete.

1.  Continue down in the file and show the **fetchedData** method.

	````C#
	(void)fetchedData:(NSData *)responseData {
		 //parse out the json data
		 NSError* error;
		 NSDictionary* json = [NSJSONSerialization 
					  JSONObjectWithData:responseData
					  
					  options:kNilOptions 
					  error:&error];
		 
		 NSString* status =[json objectForKey:@"Status"];
		 NSLog(@"status: %@", status);
		 _success = [status isEqualToString:@"SUCCESS"];
		 
		 //If we successfuly pulled the URLs, show them
		 if (_success) {
			  NSDictionary* urls = [json objectForKey:@"Urls"];
			  NSLog(@"urls: %@", urls);
			  AppDelegate *appDelegate = (AppDelegate *)[[UIApplication sharedApplication] delegate];
			  appDelegate.urls = [urls mutableCopy];
			  
			  [self.tableView reloadData];
		 } else {
			  //Otherwise, show an error
			  UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" 
							  message:@"There was an error loading the URL data.  Please try again later." 
							 delegate:self 
				 cancelButtonTitle:@"OK"
				 otherButtonTitles:nil];
			  [alert show];
		 }
	}    
	````

	> **Speaking Point**
	>
	>Here, we're attempting to deserialize the data that came back back into a NSDictionary.  We then check the Status flag to see if polling the server was a success.  If it was, we put the URLs that came from the server into an NSDictionary and set that dictionary to a property on the AppDelegate.  We then tell the Table View to reload (it uses the object on the AppDelegate as it's datasource).  

1.  Scroll down in the class and look at the didAddUrlWith slug method.

	````C#
	(void)urlDetailsViewController:(UrlDetailsViewController *)controller didAddUrlWithSlug:
											  (NSString *)urlSlug andFullUrl:(NSString *)fullUrl {
		 
		 // Create the request.
		 NSMutableURLRequest *theRequest=[NSMutableURLRequest 
													 requestWithURL:
													 [NSURL URLWithString: kAddUrl]
													 cachePolicy:NSURLRequestUseProtocolCachePolicy
													 timeoutInterval:60.0];
		 [theRequest setHTTPMethod:@"POST"];
		 [theRequest addValue:@"application/json" forHTTPHeaderField:@"Content-Type"];    
		 //build an info object and convert to json
		 NSDictionary* jsonDictionary = [NSDictionary dictionaryWithObjectsAndKeys:
													@"my_key", @"key",
													fullUrl, @"url",
													urlSlug, @"url_slug",
													nil];
		 //convert JSON object to data
		 NSError *error;
		 NSData* jsonData = [NSJSONSerialization dataWithJSONObject:jsonDictionary 
															  options:NSJSONWritingPrettyPrinted error:&error];    
		 [theRequest setHTTPBody:jsonData];        
		 //prints out JSON
		 NSString *jsonText =  [[NSString alloc] initWithData:jsonData                                        
																	encoding:NSUTF8StringEncoding];
		 NSLog(@"JSON: %@", jsonText);
		 
		 // create the connection with the request and start loading the data
		 NSURLConnection *theConnection=[[NSURLConnection alloc] initWithRequest:theRequest delegate:self];
		 if (theConnection) {
			  // Create the NSMutableData to hold the received data.
			  // receivedData is an instance variable declared elsewhere.
			  receivedData = [NSMutableData data];
		 } else {
			  // We should inform the user that the connection failed.
		 }
		 
		 AppDelegate *appDelegate = (AppDelegate *)[[UIApplication sharedApplication] delegate];
		 
		 //Add shortened URL locally
		 [appDelegate.urls setObject:fullUrl forKey:urlSlug];
		 NSIndexPath *indexPath = [NSIndexPath indexPathForRow:appDelegate.urls.count -1 inSection:0];
		 [self.tableView insertRowsAtIndexPaths:
							  [NSArray arrayWithObject:indexPath] 
							  withRowAnimation:UITableViewRowAnimationAutomatic];
		 [self.navigationController popViewControllerAnimated:YES];
		 [self.navigationController dismissViewControllerAnimated:YES completion:nil];
		 [self.tableView reloadData];    
	}
	````

	> **Speaking Point**
	>
	>This is a delagate method that is called from the URL Details screen when the user adds a new URL.  
	>
	>In it, we generate a NSMutableURLRequest which we piont at kAddURL (from Constants.m).  We then bulid a NSDictionary which is filled with the details of the Shortened URL.  This dictionary is serialized to JSON and then passed via the NSURLConnection to the server.  
	>Lastly, the new shortened URL is added to the appDelegate's dictionary of shortened URLs and the TableView is told to reload.
	>The rest of the code is app functionality and doens't directly relate to communications with the server.

---

<a name="summary" />
## Summary ##

In this demo, you saw how easy it is to communicate with a service layer running in Microsoft Azure Websites.  You took a prebuilt iOS application and pointed it at a web service that you previously deployed and demonstrated how it works.
