<a name="segment2" />
### Segment 2: Windows Azure Media Services ###

In this segment, you will configure Media Services in the Windows Azure Management portal and then you will update the application to submit an encoding job to Media Services when a video is uploaded.

> **Important:**  Before proceeding with this segment you will either have to:

> - Use the begin solutions of segment #2, located in **[working directory]\BuildClipsMedia**, instead of continuing with the end solutions from segment #1.

> or

> - Continue with the solution from the previous segment after removing the **Microsoft.AspNet.WebApi.OData** NuGet package from the **BuildClips** project and the **[Queryable]** attribute from the **Get()** method in the **VideosController.cs** file. 

> The **Microsoft.AspNet.WebApi.OData** (required by segment #1) and **Microsoft.WindowsAzure.MediaServices** (required by this segment) are both pre-release software and are currently not compatible with each other.

--

> **Speaking point:** What I'm going to do now is scale that app even further. The first way we're going to do that is by integrating another service called Windows Azure Media Services. And what Media Services allows you to do is very easily ingest, encode or transcode video, and then set up a streaming end point that you can use in order to stream your video to the cloud. Now, instead of storing it directly to storage, we're going to fire it off to Media Services, which will then encode it for us automatically, and then we're going to stand up a media streaming endpoint, which is going to allow our clients to be able to go ahead and stream it from a scalable back end.
And the beauty about Windows Azure Media Services is that it exposes a REST API that makes it really easy for you as developers to integrate within your applications, and it takes care of all the infrastructure necessary for us. So we don't have to spin up VMs, we don't have to manage our own encoders or streaming servers. Instead, Windows Azure Media Services does all that for us.

1. Start in the Windows Azure Management Portal, in the **All Items** node.

	> **Speaking Point:**
	> So let's go ahead and look at some code and see how we do it. So you can see here this is the Windows Azure portal. Earlier, you saw how we could create a website.

	![](Images/portal-all-items.png?raw=true)

1. Show how to create a new Media Service by navigating to **New | App Services | Media Service**.  Enter the name of a new media service and select your pre-created storage account. **Do not click create**, just a talking point.

	> **Speaking Point:**
	> The cool thing here is I'm going to now just take advantage and click on this app services category, and I can easily create a media service. So this is a service I can use from within my application. And I can give it a name. I can choose where in the world I want to run it, and I just go ahead and create. And in about 45 seconds, I'll then have a media service of my own that I can then program against and use for all my media scenarios.

	![Quick Create Media Service](Images/quick-create-media-service.png?raw=true "Quick Create Media Service")
 
1. In the media services list on the left, select your  pre-created media service. Show the dashboard. Click the **Content** link to show the list of jobs.

	> **Speaking Point:**
	> Now, I happen to have one already created with some content that we uploaded earlier. So I'm going to click on it here, and you can see us drill in here. So, for example, I have a dashboard view here because I can program it with REST, I can download our SDK and then I just use our little manage keys button down here in order to get my developer key, and then I can just start coding against the service. Also, I can just go ahead and click this content link at the top, I can see I have a couple of jobs and videos already uploaded here.

	![Show Media service contents](Images/show-mediaservice-content.png?raw=true "Show Media service contents")

1. Click **Upload** in the command bar and browse to **[working directory]\Assets\videos** folder. Select a video and click Ok to start uploading the new video.

	> **Speaking Point:**
	> With the new Windows Azure Management Portal, I can simply click on Upload on the command bar and browse to a video file that I have on my local machine. And this is now going to upload the video directly from my dev machine up in terms of the Windows Azure Media Service account. And once it's uploaded, then, I can go ahead and do various jobs on it.

	![Upload video from portal](Images/upload-video-from-portal.png?raw=true "Upload Video from portal")

1. Select the video from the content list and click **Encode**. Highlight the options in the **Preset** list-box. Show that the encoding job begins.

	> **Speaking Point:**
	> Once the file is uploaded, I can then easily encode the file into multiple formats and support multiple devices. You can target Silverlight and Flash, you can target HTML5, you can even target iOS. I'll start to encode this video file using a preset encoding profile that will target playback in HTML5 browsers including IE, Chrome, and Safari.   

	![Encode video from portal](Images/encode-video-from-portal.png?raw=true "Encode video from portal")

	> **Speaking Point:**
	> So Windows Azure Media Services will automatically take that job, put it on dedicated machines that we run, and spin up and do that encoding job for you. Here you can see that my encoding job has started.  It will take a few minutes for this short video to finish encoding. In addition to uploading content and submitting encoding jobs from the new Windows Azure Management portal, I can also submit jobs to Windows Azure Media Services programmatically.

	![Encoding job starting](Images/encoding-job-starting.png?raw=true "Encoding job starting")

1. Select a previous video that is already encoded (ones starting with _JobOutputAsset_ prefix and currently not published). Click **Publish** and confirm to publish the video.

	> **Speaking Point:**
	> Once something is encoded, I can then choose to publish it. This is going to create a unique streaming end-point URL that I can use for this particular asset. And then I can go ahead and embed that within my application and start playing it directly. And then there's a media server that we're managing with Media Services that's doing all the back-end streaming for you. Since encoding will take a few minutes, here you can see a video that I encoded earlier. Once encoding is complete, I'll publish the video out to Windows Azure Storage. The publishing process makes the video available so it can be consumed from multiple applications and experiences.

	![Confirm Publish](Images/confirm-publish.png?raw=true)

1. Click the **Play** button to watch the video streaming.

	> **Speaking Point:**
	> And then the cool thing is when you actually want to play something, you can just go ahead, click on a video, click play, and even directly within the browser here, you can go ahead and test out your video, including with adaptive streaming. So very easy way you can test out and kind of learn how to use the product and quickly see the status of different jobs that you're working on.

	> **Note:**
	> Even when the publish operation seems to be done, it might take a few more seconds for the video to be really ready - 30-45 secs.  

	![Playing smooth streaming video](Images/playing-smooth-streaming-video.png?raw=true)

1. Switch to the Visual Studio 2012 instance that has the **BuildClips** Web Project already opened.

	> **Speaking Point:** 
	> But the cool thing about Media Services, again, isn't that you can do this all manually, it's the fact that you can actually code against it and just send REST calls and do all this from within your apps. Let's go ahead and do that. So I'm going to flip into Visual Studio. And what you're seeing here is that same project we were working on just a few minutes ago. So that same ASP.NET project.

1. Install the Media Services NuGet. To do this, right click the **BuildClips** solution  and select **Manage NuGet packages for Solution**. Search for **Windows Azure Media Services SDK** package and click **Install**. Make sure both projects are selected and click **Ok**. Click **I Accept**. Once the package installation is completed, click **Close**.

	> **Speaking Point:**
	> Now, I'm going to go ahead and click on manage NuGet and I'm just going to use the NuGet package manager in .NET in order to install a little SDK library that is going to provide a nice .NET-managed object model on top of those REST end points. And so this is just installing it within my solution, makes it a little bit easier for me to code against.   

	![Install the Media Services NuGet for solution](Images/install-package-mediaservices-solution.png?raw=true)

1. Right click the **Helpers** folder and select **Add Existing item**. Select the **MediaServicesHelper.cs** file located at **[working dir]\Assets\Segment2\Helpers** and click **Add**.

	> **Speaking Point:** So I'm going to add now a pre-cooked helper class that will provide some minor extensions to the Media Services SDK library, which are intended to help us on interfacing with our video service.

1. Open **VideoService.cs** and scroll down to the **CreateVideoAsync** method. Replace the **TODO** comment and the next four lines with the code below.
	
	(Code Snippet - _VideoService.cs - Create_)
	<!-- mark:1-13 -->
	````C#
	// Create an instance of the CloudMediaContext
	var mediaContext = new CloudMediaContext(
									 CloudConfigurationManager.GetSetting("MediaServicesAccountName"), 
									 CloudConfigurationManager.GetSetting("MediaServicesAccountKey"));

	// Create the Media Services asset from the uploaded video
	var asset = mediaContext.CreateAssetFromStream(name, title, type, dataStream);

	// Get the Media Services asset URL
	var videoUrl = mediaContext.GetAssetVideoUrl(asset);

	// Launch the smooth streaming encoding job and store its ID
	var jobId = mediaContext.ConvertAssetToSmoothStreaming(asset, true);
	````

	> **Speaking Point:** There we go. And then I'm just going to update some of the code I showed you earlier so that instead of storing that media inside a storage account, we're instead going to pass it off to Media Services to both store and code and publish. And doing that is really easy. So what I'm going to do here is just replace these four lines of code. This code right here. And what all this code is doing is connecting to my Windows Azure Media Services account. It's going to create a new asset in Media Services from the stream that was uploaded, get back a URL for it, and then kick off an encoding job to convert it to smooth streaming. Just a couple lines of code I can do that.

1. Place the cursor over the **CloudMediaContext** type and press **CTRL+.** to add the using statement. Do the same with **CloudConfigurationManager** on the next line.

1. Next, in the **Publish** method, replace the **TODO** comment and the next three lines of code with the following snippet.

	> **Speaking Point:** 
	> And then when I want to publish it so that people can stream it, I can just go ahead and call video services publisher, and this is then going to just call and publish that video on the job object that was encoded and get me back a URL that I can now pass off to my clients to play.

	(Code Snippet - _VideoService.cs - Publish_)
	<!-- mark:1-15 -->
	````C#
    var mediaContext = new CloudMediaContext(
                                     CloudConfigurationManager.GetSetting("MediaServicesAccountName"),
                                     CloudConfigurationManager.GetSetting("MediaServicesAccountKey"));

    string encodedVideoUrl, thumbnailUrl;
    if (mediaContext.PublishJobAsset(video.JobId, out encodedVideoUrl, out thumbnailUrl))
    {
        video.EncodedVideoUrl = encodedVideoUrl;
        video.ThumbnailUrl = thumbnailUrl;
        video.JobId = null;

        this.context.SaveChanges();
	}

	return video;
	````
1. Finally, insert a new method named **GetActiveJobs** into the class.

	> **Speaking Point:** 
	> Finally, let me add a new method to the video service that we'll be using shortly to retrieve the list of encoding jobs that are in progress and completed. This will allow me, for example, to run a background process to check the active jobs, report the status of the jobs in progress and also publish the videos already encoded.

	(Code Snippet - _VideoService.cs - GetActiveJobs_)
	<!-- mark:1-26 -->
	````C#
    public IEnumerable<Video> GetActiveJobs()
    {
        var activeJobs = this.context.Videos.Where(v => !string.IsNullOrEmpty(v.JobId));

        if (activeJobs.Any())
        {
            var mediaContext = new CloudMediaContext(
                                             CloudConfigurationManager.GetSetting("MediaServicesAccountName"),
                                             CloudConfigurationManager.GetSetting("MediaServicesAccountKey"));

            foreach (var video in activeJobs)
            {
                var job = mediaContext.GetJob(video.JobId);
                if (job != null)
                {
                    // The video status will be Encoding unless the encoding job is finished or error
                    video.JobStatus = (job.State == JobState.Finished || job.State == JobState.Error)
                                        ? JobStatus.Completed : JobStatus.Encoding;

                    yield return video;
                }
            }
        }

        yield break;
    }
	````

1. Right-click the **BuildClips** project and select **Publish**. 

	> **Note:** Please note that if you chose to open the begin solution for segment #2 you will need to import again the Web Site publish settings file.

	> **Speaking Point:** Now, that was basically all the code we needed to do. I can then right-click and publish this back to the cloud. One of the things that we support with Windows Azure websites is a very nice incremental publishing story.

	![Publish web site](Images/publish-web-site.png?raw=true)

1. Click the **Settings** page, expand **File Publish Options** and check the **Remove additional files at destination** option. Then click **Publish**.

	> **Speaking Point:** 
	> So before we start publishing, I'm going to configure the publishing to remove all files that won't be required anymore. And so you can see here, instead of having to redeploy the entire app, it's just showing me the differences between what's on my local dev machine and then what's in the cloud, so it makes deployment a lot faster. Those changes are now already live. 

	![Publish web site](Images/publish-web-settings.png?raw=true)

1. Once the app is deployed, switch the focused window to Internet Explorer to show the live running instance of the app and navigate to **http://{yourdeployedwebapp}.azurewebsites.net/help** in order to show the Web Api Help Page.

	> **Speaking Point:** 
	> So remember we have those Web APIs that we exposed earlier. We can also, then, hit this with a Windows 8 client.

	![](Images/webapi-help-page.png?raw=true)

1. Switch to the Windows 8 App.

	> **Speaking Point:** 
	> So let's go ahead and switch to the Windows 8 app. 

1. In the video list page, right-click on the screen and click the **Upload** button.

	![Upload Video Button](Images/windows8-upload-button.png?raw=true)

	> **Speaking Point:**
	> I'll upload a video that I have on my local machine.

1. Enter a title, description and select some tags, and then upload a video from the **[working dir]\Assets\videos** folder using the file picker.

	![Uploading a Video](Images/windows8-upload-video.png?raw=true)

	> **Speaking Point:**
	> Let me capture a title and description for my video, and then I'll press upload to pick the video. We're going to hit open, and this is now uploading this off to Windows Azure. We're going to use the exact same REST URL we published earlier with our Web APIs in the middle tier. And this is going to talk to that Web service on the back end, it's going to then fire off a REST call to Windows Azure Media Services and it's going to kick off an encoding job, which is then going to start encoding it. And then once it's done, and you can see it just completed, it will be able to be played through our streaming server in the cloud.

1. Switch back to the **Windows Azure Management Portal** and navigate to your Media Service account.

	> **Speaking Point:**
	> Now that my video is uploaded, let's switch back to the Windows Azure Management Portal.  

1. Go to the **Content** section of the media service and show the video uploaded and the running encoding job.

	> **Speaking Point:**
	> I'm going to go back into the portal, click the content tab, and you can see that my video has been, and hey, we've got two new files that have just shown up. One is the file we uploaded, and one is an adaptive streaming job that we just kicked off. And if I go ahead and hit play, what I should be able to do inside the portal is see all of you. 

	![Encoding the video](Images/encoding-the-video.png?raw=true)

