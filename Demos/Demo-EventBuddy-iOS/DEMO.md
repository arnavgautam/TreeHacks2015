<a name="title" />
# Event Buddy - iOS - Windows Azure Mobile Services #

---

<a name="Overview"/>
## Overview ##

This demo demonstrates an iOS client that connects to a Windows Azure Mobile Service.  In this scenario, you should have already completed and set up the primary [EventBuddy Windows 8 demo](https://github.com/WindowsAzure-TrainingKit/Demo-EventBuddy).

> **Note:** This demo was developed using Xcode 4.5 and iOS 6.  It may work with earlier or later versions of Xcode and iOS, though testing has not been performed.**.

<a name="goals" />
### Goals ###
This demo covers:

1. [Connecting the iOS app to Mobile Services](#Segment1)
2. [Adding pull to refresh code](#Segment2)
3. [Running the app](#Segment3)

<a name="KeyTechnologies" />
### Key Technologies ###
This demo uses the following technologies:

- [Xcode](https://developer.apple.com/xcode/)
- [Windows Azure Management Portal](http://manage.windowsazure.com/)

<a name="Setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up the primary Windows Store EventBuddy demo. This can be done by following the demo.md file located with the [EventBuddy Demo](https://github.com/WindowsAzure-TrainingKit/Demo-EventBuddy).

<a name="Push Notifications" />
### Push Notifications ###

Currently the only area missing from this demo that is present in the Windows Store application is enabling push notifications in the iOS app.  This may be added in the future, however, you can enable this scenario by following the [instructions for setting up push notifications for iOS](https://www.windowsazure.com/en-us/develop/mobile/tutorials/get-started-with-push-ios/) and enabling your server side script to push using WPNS or APNS depending on the device registered.

<a name="Demo" /> 
## Demo ##

<a name="Segment1" />
### Segment 1: Connecting the iOS app to Mobile Services ###

> **Speaking Point:** Full support for the iOS platform, for developing apps for iPhones and iPads, is also fully supported by Windows Azure Mobile Services.  Now we'll connect an iOS version of the EventBuddy application to the same Mobile Service we just used with our Windows Store / Windows Phone application.

1. Open the EventBuddy.xcodeproj file.

1. Open EventBuddyService.m in the editor.

1. Open the [Windows Azure Management Portal](http://manage.windowsazure.com).

1.  After logging in, navigate to the dashboard for the mobile service you created with the primary EventBuddy demo.  

	![manage-keys](images/mobile-service-settings-dashboard.png?raw=true)

1.  Now copy the **Application Key** value.

	![application-key](images/mobile-service-settings-keys.png?raw=true)

1.  Return to Xcode and EventBuddyService.m.

1.  Locate the **init** method and replace the mobile-service-name and mobile-service-key placeholders with the values from your mobile service.

	![placeholders](images/event-buddy-service-placeholders.png?raw=true)

<a name="Segment2" />
### Segment 2: Adding pull to refresh code ###

> **Speaking Point:** In iOS applications, the ability to "Pull to Refresh" is very common and expected.  I'm going to use some open source code to enable this in my application.

1.  Navigate to the [PullToRefresh library on GitHub](https://github.com/leah/PullToRefresh)

1.  Open the **Classes** folder.

1.  Save **PullRefreshTableViewController.h** and **PullRefreshTableViewController.m** to the **source/code/EventBuddy** folder.

1.  Right click on the **EventBuddy** folder in the **project navigator**.

	![add-files](images/xcode-add-files.png?raw=true)

1.  Navigate to the **EventBuddy** folder and select **PullRefreshTableViewController.h** and **PullRefreshTableViewController.m**.

1.  Click the **Add** button.

<a name="Segment3" />
### Segment 3: Running the app ###

> **Speaking Point:** Now we'll run the app and see the same capabilities in our iOS application.

1.  Tap the **Run** button in the top left of Xcode or hit **command+R** to run the app.

1.  You should first see the login to Twitter.

	![twitter-login](images/ios-twitter-login.png?raw=true)

1.  After logging in, you will see the list of events.

	![events](images/ios-event-list.png?raw=true)

1.  You can add a new event by tapping the plus in the top right of the simulator.

	![new-event](images/ios-add-event.png?raw=true)

1.  Tap on an event to view the list of sessions.

	![sessions](images/ios-session-list.png?raw=true)

1.  You can add a new session by tapping hte plus in the top right of the simulator.

	![new-session](images/ios-add-session.png?raw=true)

1.  Tap on a session to view the session details.

	![session-details](images/ios-session-details.png?raw=true)

1.  Tap on a star to rate the app.  This should trigger a push notification to the WIndows Store app.

<a name="Closing" />
### Closing ###

In just a few minutes we've connected an iOS app to the same mobile service we used for our Windows Store and Windows Phone 8 application.  Additionally, we've given users the ability to rate sessions which triggers a push notification to the session creator.