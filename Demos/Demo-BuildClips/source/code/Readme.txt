Before running the solutions provided in this folder make sure you perform the following actions:

For setting up solutions from Segments 1 and 2, follow the steps described in the "Setup and Configuration" section of the demo.

Segment 3
=========
- For BuildClips.Web:
	- In the BuildClips project, open the Web.config file and update the Facebook, Twitter and Media Services account configuration settings.
	- In the BuildClips project, open the Web.Release.config file and update the Facebook, Twitter and storage connection string configuration settings.
	- In the BackgroundService project, open the app.config file and update the Media Services account settings.
	- Set BuildClips.Azure as the startup project.

- For BuildClips.Win8App:
	- In the BuildClips project, open the config.js file located inside the js folder, update the Facebook and Twitter configuration settings, and set the value of ApiBaseUrl to your Azure website's URL.

Segment 4
=========
- Perform the tasks described in Segment 3.

- For BuildClips.Web:
	- In the BuildClips project, open the Web.config file and update the Service Bus configuration settings.

Segment 5
=========
- Perform the tasks described in Segments 3 and 4.