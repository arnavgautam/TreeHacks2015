?<a name="title" />
# Big Compute on Microsoft Azure: Weather as a Service Demo #

<a name="Overview" />
## Overview ##
This demo shows how we turned one of the most complex super computing tasks, weather prediction, into a pay-as-you-go service.

### Key Messages: ###
- Traditionally, only large institutions with dedicated hardware have been able to run complex research software. Microsoft Azure enables even individuals to do the same with on-demand compute power.
- Complex software systems can be moved online using off-the-shelf Microsoft web software and robust pay-as-you-go cloud technologies to reach the masses. 
- Microsoft Azure Storage  is a scalable and economical way to share your valuable data.


> **Note:** This is a Big Compute scenario based demo that requires about 10 minutes to learn with no prior HPC knowledge assumed.

<a name="setup" />
### Setup and Configuration ###
Run through the demo once, perhaps submit a weather forecast the night before your demo. Please read this optional [blog on technical details](http://blogs.msdn.com/b/hpctrekker/archive/2012/11/26/running-weather-research-forecast-as-a-service-on-windows-azure.aspx). 

> **Note:**  Please note that Step 11 - 13 are optional.   You may use screenshots from the deck [AdditionalScreenshots.pptx](https://github.com/WindowsAzure-TrainingKit/Demo-WeatherService/blob/master/AdditionalScreenshots.pptx) instead of using the live Microsoft Azure Portal to show the actual deployment.

<a name="Demo" />
## Demo Steps ##


**1. Go to the Weather Service Site**: <http://weatherservice.cloudapp.net/Home/Index> 
  
> **Speaking Point** 
> This is a weather forecast portal that runs compute intensive weather prediction software called WRF that is commonly used by National Weather Service.

**2. Show the Gallery** <http://weatherservice.cloudapp.net/Gallery/List> page.
 
  > **Speaking Point** These are animations produced by a mathematical weather modeling/forecast program.  Here you can see some of the recent simulation results.  We have five metrics being shown here, accumulative precipitation, temperature, snow fall, water vapor, and surface wind plots.  The simulation produces a large amounts of raw data that allows you to visualize many more simulation properties beyond these 5 properties.  These computer simulations are very complex and do take a long time to compute.  

![gallery](images/gallery.jpg?raw=true "Gallery")

> **Background:** Before we go further, here’s more background on this topic; Weather forecast programs have always required the power of supercomputers.  In fact, the world’s first ever CRAY super computer was created in 1976 for the sole purpose of computing weather models.  That very same $6 million super computer still sits in the basement of NCAR (National Center of Atmospheric research).  It had 1 MB worth of memory.  To put the importance of Weather and Climate Modeling into perspective, the NOAA (National Oceanic and Atmospheric Administration) states that at least 1/3 of the US GDP is weather and climate sensitive, a potential impact of $4 trillion / year.  Now with the power of Microsoft Cloud Technology you can do what super computers do for the cost of lunch!

   
![cray1](images/cray1.jpg?raw=true "Cray 1 in 1976")

![1976](images/1976.jpg?raw=true "1976")

**3. Click on any of the simulation tiles.** 

> **Speaking Point** Let’s take a look at one of these prediction models in detail.   It takes a tremendous amount of computing power for us to run a simulation model: 1km resolution at 60x60km at the core center, 3km x 3km at 180 x 180km, or a larger area, and 9km resolution at 540km x 540km.  The simulation predicts the weather conditions 3 days out, and is capable of a maximum of 8 days.  The modeling has 3 dimensions, the third being the atmosphere, which is divided into 27 layers to form a large 3D grid.  The simulation will run for 8 hours on an extra-large (8-core) Azure instance. Each simulation produces 5 GB worth of raw data.

**4. Play it back for Snow, Rain, and Temperature simulations** 

> **Speaking Point** This is the weather prediction from last night. E.g., you can pick Rainier <http://weatherservice.cloudapp.net/Forecast/Display/afaa503b-29d4-42ff-b6bf-703d2b2b80e2?what=precip>.    These overlay maps are created using a series of applications pipelined together.  The process starts by retrieving initial parameter data from the NOAA, runs the CPU-intensive simulation, prepares the data, and **generates plots for overlaying with Bing Maps**.   It’s a complex set of tasks that only trained scientists can understand and do.  It is often the case that one scientist only has expertise in 1-2 of the steps.
 
>**Speaking Point**  So how easy would it be to create a prediction like this using Microsoft Azure and Bing Maps?  We’ve automated the process on Microsoft Azure with a small set of PowerShell scripts, so the only required skill is the ability to point and click on a map in a browser. Our weather service integrates a set of publically available tools from weather research institutions like NOAA and NCAR.  The computation runs on a Microsoft Azure Platform as a Service(PaaS) deployment.

![image005](images/image005.jpg?raw=true "PrecipitationTile Bing Map Overlay")
 
**5. Click on New Forecast, and allow BingMap to get your location. Move map to position your forecast location** 

> **Speaking Point** Now, let’s try to submit a request for our own 3 day weather forecast. Let’s run it for Mt. Rainier (or pick your own), which receives a lot of precipitation due to its unique geo features.
 
 ![newforecast](images/newforecast.jpg?raw=true "New Forecast")

>**Note:** You will see the Detailed Status Page, but **the simulation will take the next 8 hours** to run.
![detailed status](images/detailed-status.png?raw=true "detailed status")

**6. Now list the forecasts by clicking on Processing Queue Menu, showing that the job has been submitted**



> **Note:**  Might take a few seconds for it to show up. If the scheduler isn't busy on other jobs, it will goto running state and show up at the front of the queue.  You can simply click on the **processing queue" Menu item.
 
![processingqueue](images/processingqueue.PNG?raw=true "Processing queue")

**7. Navigate to the last page of the queue to see new items**

![processingqueuelast](images/processingqueuelast.PNG?raw=true "navigate to the last page of the queue to see new items")

> **Speaking Point** Since Microsoft Azure is elastic, having the ability to provide computing power dynamically, we can run 1 simulation using 1 machine, or 40 simulations using 40 machines for 40 cities around the world.   Let me show you some of the runs we have done on this system. Since the service started running in February 2012, Microsoft Azure has been working hard.  We have spent approximately 100,000 core hours as of Sept 1st and have collected over **8** million objects in the Microsoft Azure blob storage system.  

<http://weatherservice.cloudapp.net/Gallery/Map>

**8. Click on the Map View**
Show the map view, move the map around to show all the simulations we have done around the world by zooming to different levels.

 ![mapview](images/mapview.png?raw=true "Map View")

**9. Zoom in and Click also show how you get access to individual simulations**

 ![mapviewItem](images/mapviewitem.png?raw=true "Map View Item")

**10. Go to timeline, click on 6/28, 29, 30.** 
To show that we can scale more instances on-demand, pick June 28, 29, 30 of 2012.  This was run by Tech-ED workshop attendees where we scaled up 15 machines to allow simulations done on major European cities. On June 30 2012, there were 34 finished simulations that day. 

 ![timelineview](images/timelineview.jpg?raw=true "Time Line") 


 
> **Speaking Point** To add more nodes to the weather service, you can log into your WindowsAzure Portal and simply move the slider. 

> **Note:** For all the screens below, you may use screenshots from the **[AdditionalScreenshots.pptx](https://github.com/WindowsAzure-TrainingKit/Demo-WeatherService/blob/master/AdditionalScreenshots.pptx)** in the root directory of the github repo if you do not access to the portal.

**11. Show the DashBoard in Microsoft Azure Portal, or a screenshot slide.** 

> **Speaking Point** This is the Microsoft Azure Portal.  For the Weather Service we deployed, the DashBoard is currently shows the Compute Node has been using  100% CPU constantly for the last hour or so.

![Image 14](images/image-14.png?raw=true)

**12. Show the Scale Tab, and move the slider to the right, or a screenshot slide of this.** 

> **Speaking Point** To add more nodes to the weather service, you can log into your WindowsAzure Portal and simply move the slider. 

![Image 13](images/image-13.png?raw=true)

**13. Under the Instances tab, show that there are 3 types of instances in the Microsoft Azure HPC Scheduler deployment.**

> **Speaking Point** 
There are three types of instances in this deployment. HeadNode which controls the cluster. A FrontEnd Web role which hosts the MVC website.
Extra Large Compute Nodes which can scale to meet computing needs.

![Image 15](images/image-15.png?raw=true)

> **Speaking Point** Closing:  With Microsoft Azure, we’ve simplified the task of providing compute resources to complex compute intensive software, a task that previously would have required expensive hardware and domain specialists. Using an extra-large 8-core instance for the computation, the simulation takes about 8 hours. Since WRF uses Message Passing library for parallelism, it can easily scale multiple HPC capable machines with fast interconnects.  

>As we make new HPC hardware available in Microsoft Azure, more cores can be applied simultaneously to one simulation run and it should easily reduce the run time down to minutes from 8 hours. The compute cost comes down to about a few dollars per simulation, it is something that just about anyone can do and afford.  

>Complex software systems can be moved online using off-the-shelf Microsoft web software and robust pay-as-you-go cloud technologies to reach the masses.  We can see this enables a new paradigm of collaborative research and knowledge sharing. In the near future, weather forecasts will be used to plan and operate search & rescue missions and wind farms.  They will shape hurricane preparation timetables and financial assessments of weather-related damages, helping businesses and communities remove and manage risks and unknowns posed by nature.

>Technical References [Blog](http://blogs.msdn.com/b/hpctrekker/archive/2012/11/26/running-weather-research-forecast-as-a-service-on-windows-azure.aspx)
