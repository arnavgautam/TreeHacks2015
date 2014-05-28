<a name="demo2" />
# ClipMeme Demo : BUILD Clips#

## Overview ##

In this demo, we will ...

<a name="Goals" />
### Goals ###
In this demo, you will see how to:

1. This...

<a name="Technologies" />
### Key Technologies ###

- Windows Azure Webistes

<a name="Prerequisites" />
### System Prerequisites ###
- Visual Studio 2013 Express for Web
- Visual Studio 2013 Express for Windows 8

<a name="Setup" />
### Setup and Configuration ###

In order to execute this demo, you first need to set up your environment by completing the following tasks: 

1. [Visual Studio and Azure Provisioning](#segment1)

1. [Web Essentials Features](#segment2)

1. [Azure Web Jobs](#segment3)

1. [Azure Auto Scale, Traffic Manager and Backup](#segment4)

<a name="segment1" />
###Visual Studio and Azure Provisioning###

	File / New Experience
		Database
		Website
		Virtual machine
	Provisioning and PowerShell

<a name="segment2" />
### Web Essentials Features ###
	Quick Site Demo
		Show ClipMeme site
		Upload new gif
	BrowserLink Features
		Design in Browser 
			Fix Wording issue 
		Edit CSS with F12 Tooling
			Change Background color 
	Image Sprites (Not in Keynote)
	Javascript editing
		Js hint
	Angular JS refactoring (Not in Keynote)
		Show angular Intellisense 
	Azure Staged Published

<a name="segment3" />
### Azure Web Jobs ###

1. Switch to the production site of ClipMeme, click **Create Meme**.

1. Drag the animated GIF from your working directory into the _drop zone_ in the browser.

1. Add a caption and click **Submit**.

1. Wait until the meme is fully created (the image legend will change to your caption).

1. Switch to the ClipMeme Azure Website in the Management Portal and go to **WEBJOBS**.

	> **Speaking Point:** This allows me to run background tasks.

1. Switch back to Visual Studio.

1. Open **Program.cs** and scroll down to show the **ProcessImage** method.

1. In Solution Explorer, right-click the **ClipMeme** project node and select **Add > Windows Azure Webjob...**.

	> **Speaking Point:** I've already associated my Console project with my Website. When I publish my Website the Webjob is published too.

1. Switch to the Web Jobs Dashboard in the Management Portal.

1. Show the **Invocation Log** and explain that everytime a Job runs or fails, it will add a new entry to this log.

	> **Note:** If you generated errors in your log, click on the item to show the details of the error. Then click the **Toggle Output** button to expand the exception details

<a name="segment4" />
### Site Swap, Azure Auto Scale, Traffic Manager and Backup ###
	Site Swap
	Auto Scale
	Traffic Manager
	Backup