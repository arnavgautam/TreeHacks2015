<a name="Title" />
#Introduction to Microsoft Azure HDInsight#

---
<a name="Overview" />
## Overview ##

This hands-on lab shows two ways to run MapReduce programs in a HDInsight on Azure cluster and how to analyze data imported into the cluster from Excel using Hive-based connectivity. 

The first way to run a MapReduce program is with a HDInsight jar file using the **Create Job** UI. The second way is with a query using the fluent API layered on Pig that is provided by the **Interactive Console**. The first approach uses a MapReduce program written in Java; the second uses a script written in JavaScript. This lab also shows how to upload files to the HDFS cluster that are needed as input for a MapReduce program and how to read the MapReduce output files from the HDFS cluster to examine the results of an analysis.

The Microsoft Azure Marketplace collects data, imagery, and real-time web services from leading commercial data providers and authoritative public data sources. It simplifies the purchase and consumption of a wide variety of data including demographic, environment, financial, retail and sports. This lab shows how to upload this data into a HDInsight on Azure cluster and query it using Hive scripts.

A key feature of Microsoft’s Big Data Solution is the integration of HDInsight with Microsoft Business Intelligence (BI) components. A good example of this is the ability for Excel to connect to the Hive data warehouse framework in the HDInsight cluster. This lab shows how to use Excel via the Hive ODBC driver to access and view data in the cluster. 

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

* Run a basic Java MapReduce program using a HDInsight jar file

* Upload input files to the HDFS cluster and read output files from the HDFS cluster

* Run a JavaScript MapReduce script with a query using the fluent API on Pig that is provided by the Interactive JavaScript Console

* Import data from the Microsoft Azure Marketplace into a HDInsight on Azure cluster using the Interactive Hive Console

* Use Excel to query data stored in a HDInsight on Azure cluster

<a name="Prerequisites" />
### Prerequisites ###

You must have an account to access HDInsight on Azure. You may request for an invitation [here](http://www.hadooponazure.com). To create a HDInsight cluster, follow the instructions outlined in the _Getting Started_ section of this lab.  You will also need Microsoft Excel to install the Hive Add-in.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

- [Getting Started: Requesting a new cluster](#GettingStarted)
- [Executing a basic Java MapReduce program using a HDInsight jar file](#Exercise1)
- [Executing a JavaScript MapReduce script using the Interactive Console](#Exercise2)
- [Importing data from the Microsoft Azure Marketplace using the Hive Interactive Console](#Exercise3)
- [Connecting to and querying Hive data in a cluster using Excel](#Exercise4)


<a name="GettingStarted" />
### Getting Started: Requesting a new cluster ###

In this exercise, you will request a new HDInsight cluster through the Microsoft Azure portal.

1. In order to access HDInsight you must first enable it as a preview feature from the Microsoft Azure Account screen.  <https://account.windowsazure.com/PreviewFeatures/> **This might take a couple of days depending on approval quene length.** 

	![preview features](images/preview features.png?raw=true "Enabling HDInsight as a preview feature in your Microsoft Azure account")

1. Once the HDInsight feature has been enabled, go to the [Microsoft Azure](http://manage.windowsazure.com) portal, login using your credentials and select the **HDInsight** option on the left-hand menu.

	![hdinsight on Microsoft Azure portal](images/hdinsight on Microsoft Azure portal.png?raw=true "List of HDInsight clusters")
	

1. Before creating a new HDInsight cluster you must create a new Storage account in the 'East US' location for the cluster.

	![create hdinsight storage account](images/create hdinsight storage account.png?raw=true "Creating a new storage account for HDInsight")

1.  Next create the HDInsight cluster using the Quick Create wizard, entering the DNS name, choosing a small sized cluster with 4 data nodes, entering login credentials and selecting the storage account we just created before clicking **Create HDInsight Cluster**

	![create an hdinsight account](images/create an hdinsight account.png?raw=true "Creating an HDInsight cluster")

	_Requesting a new cluster_


1. The process will take a few minutes.  While your cluster is requested, you will see feedback messages like the following.

	![provisioning cluster 1](images/provisioning cluster 1.png?raw=true "Provisioning cluster")

	_Cluster creation and allocation in progress_

1. After the cluster has been successfully allocated you will see it listed under your HDInsight menu.


	![hdinsight complete](images/hdinsight complete.png?raw=true "HDInsight cluster successfully created")

1. Click on the newly created cluster to view its details in the Azure portal.

	![manage cluster](images/manage cluster.png?raw=true "Viewing the cluster details")

1. Click the link to open the HDInsight portal for the newly created cluster and login using your credentials.

	![HDInsight login](images/HDInsight login.png?raw=true "Logging in to the HDInsight portal")

1. You can now manage your cluster from the HDInsight portal account page.

	![Microsoft Azure HDInsight portal](images/Microsoft Azure HDInsight portal.png?raw=true "HDInsight portal")

<a name="Exercise1" />
### Exercise 1: Executing a basic Java MapReduce program using a HDInsight jar file ###

In this exercise, you will learn how to execute a simple Java MapReduce program using a HDInsight JAR file.

<a name="Ex1Task1" />
#### Task 1 - Deploying and Executing the Pi Estimator Sample  ####

In this task, you will deploy and execute the Pi Estimator sample, available in the HDInsight on Microsoft Azure portal.

1. From your **Account** page, click on the **Create Job** icon in the **Your Tasks** section.

1. This brings up the **Create Job** UI.

	![CreateJobUI](images/createjobui.png?raw=true "The Create Job UI")

	_The Create Job UI_

	>**Note:** To run a MapReduce program you need to specify the Job Name and the JAR File to use. Parameters are added to specify the name of the MapReduce program to run, the location of input and code files, and an output directory.

1. To see a simple example of how this interface is used to run the MapReduce job, let's look at the Pi Estimator sample. Return to your **Account** page. Scroll down to the **Samples** icon in the **Manage your account** section and click on it. 

1. From your **Account** page, scroll down to the **Samples** icon in the **Manage your account** section and click on it.   

	![Microsoft Azure HDInsight Account page](images/Microsoft Azure HDInsight Account page.png?raw=true "Selecting Samples")

	_Selecting Samples_

1. Click on the **Pi Estimator** sample icon in the HDInsight Sample Gallery.

	![SelectingThePiEstimatorSample](images/selecting-the-piestimator-sample.png?raw=true "Pi Estimator Create Job UI")

	_Selecting the Pi Estimator sample_

	> **Note:** On the **Pi Estimator** page, information is provided about the application and downloads are make available for Java MapReduce programs and the jar file that contains the files needed by HDInsight on Azure to deploy the application.

1. Click on the **Deploy to your cluster** button on the right side to deploy the files to the cluster.

	![deploying-the-piestimator-sample](images/deploying-the-piestimator-sample.png?raw=true "Deploying the PI estimator sample")
	
	_Deploying the Pi Estimator sample_

1. Once the **Create Job** page opens, check that the fields are already populated.	
	

	![PiEstimatorCreateJob](images/PiEstimatorCreateJob.PNG?raw=true "Pi Estimator Create Job UI")

	_The Pi Estimator Create Job page_


	**Note:** The fields on the **Create Job** page are populated for you in this example. The first parameter value defaults to "pi 16 10000000". The first number indicates how many maps to create (default is 16) and the second number indicates how many samples are generated per map (10 million by default). So this program uses 160 million random points to make its estimate of Pi. The **Final Command** is automatically constructed for you from the specified parameters and jar file.

1. To run the program on the HDInsight cluster, simply click on the blue **Execute job** button on the right side of the page. The status of the job is displayed on the page and will change to  _Completed Successfully_ when it is done. The result is displayed at the bottom of the Output (stdout) section. For the default parameters, the result is Pi = 3.14159155000000000000 which is accurate to 8 decimal place, when rounded.

	![Viewing the Pi Estimator results](images/viewing-the-piestimator-results.PNG?raw=true "Viewing the Pi Estimator results")

	_Viewing the Pi Estimator results_

---

<a name="Exercise2" />
### Exercise 2: Executing a JavaScript MapReduce Script using the Interactive Console  ###

In this exercise, you will learn how to run a MapReduce job with a  query using the fluent API layered on Pig that is provided by the **Interactive Console**. This example requires an input data file. The WordCount sample that we will use here has already had this file uploaded to the cluster. But the sample does require that the .js script be uploaded to the cluster and we will use this step to show the procedure for uploading files to HDFS from the **Interactive Console**.

<a name="Ex2Task1" />
#### Task 1 - Deploying and Executing the WordCount Sample ####

In this task, you will deploy and execute the WordCount sample available in the portal.

1. First we need to download a copy of the WordCount.js script to your local machine. We need to store it locally before we upload it to the cluster. Right-click [here](http://isoprodstore.blob.core.windows.net/isotopectp/examples/WordCount.js "WordCount.js") and save a copy of the WordCount.js file to your local ../downloads directory. In addition we need to download the _The Notebooks of Leonardo Da Vinci_ file, available [here](http://isoprodstore.blob.core.windows.net/isotopectp/examples/davinci.txt). 

1. To get to the Interactive JavaScript console, return to your [Account](https://www.hadooponazure.com/Account) page. Scroll down to the **Your Cluster** section and click on the **Interactive Console** icon to bring up the [Interactive JavaScript console](https://www.hadooponazure.com/Cluster/InteractiveJS).

	![Selecting the Interactive Console](images/selecting-interactive-console.png?raw=true "Selecting the Interactive Console")

	_Selecting the Interactive Console_

	![The Interactive JavaScript console](images/the-interactive-javascript-console.png?raw=true "The Interactive JavaScript console")

	_The Interactive JavaScript console_

1. To upload the WordCount.js file to the cluster, enter the upload command `fs.put()` at the **js>** console and select the WordCount.js from your downloads folder, for the Destination parameter use **./WordCount.js/**.

	![Running Commands from the JavaScript Console](images/running-command-from-jsconsole.png?raw=true "Running Commands from the JavaScript Console")

	_Running Commands from the JavaScript Console_

	![Uploading the WordCount.js file](images/uploading-the-wordcountjs-file.png?raw=true "Uploading the WordCount.js file")	

	_Uploading the WordCount.js file_

1. Repeat the previous step to upload the **davinci.txt** file using **./example/data/** for the Destination.

1. Execute the MapReduce program from the **js>** console using the following command:

	`
pig.from("/example/data/davinci.txt").mapReduce("WordCount.js", "word, count:long").orderBy("count DESC").take(10).to("DaVinciTop10Words")`

1. Scroll to the right and click on **View Log** if you want to observe the details of the job's progress. This log will also provide diagnostics if the job fails to complete. 

	![View Log](images/javascript-console-show-log.png?raw=true "View Log")

	_View Log_

1. To display the results in the DaVinciTop10Words directory once the job completes, use the `file = fs.read("DaVinciTop10Words")`	command at the **js>** prompt.

	![Viewing the WordCount sample results](images/viewing-the-wordcount-sample-results.png?raw=true "Viewing the WordCount sample results")

	_Viewing the WordCount sample results_

---

<a name="Exercise3"/>
### Exercise 3: Importing data from the Microsoft Azure Marketplace using the Hive Interactive Console ###

In this exercise, you will learn how to subscribe and import data from the Microsoft Azure Marketplace.  You will also learn how to query data using the Hive Interactive Console.

<a name="Ex3Task1" />
#### Task 1 -  Registering a new Account ####

In this task, you will register a new account in the Microsoft Azure Marketplace.  You will then use this account to subscribe and import data.

1. Open the [Microsoft Azure Marketplace](https://datamarket.azure.com/ "DataMarket") page in a browser and sign in with a valid Windows Live ID.

	![The Microsoft Azure Marketplace home page](images/the-windows-azure-marketplace-home-page.png?raw=true "The Microsoft Azure Marketplace home page")

	_The Microsoft Azure Marketplace home page_

	Complete the Registration form to open a subscription account.

	![Registering a new account](images/registering-a-new-account.png?raw=true "Registering a new account")	

	_Registering a new account_

1. Click on the **MyAccount** tab & select **Account Keys** from the left-hand menu.
	
	
	![Obtaining the account keys](images/obtaining-the-account-keys.png?raw=true "Obtaining the account keys")

	_Obtaining the account keys_

	> **Note:** Account keys are used by applications to access your Microsoft Azure Marketplace dataset subscriptions.

<a name="Ex3Task2" />
#### Task 2 -  Subscribing to Crime Data and Building a Query ####

In this task, you will subscribe to crime data in the Microsoft Azure Marketplace, and you will build a query that will be used in the following tasks for importing data.

1. Click on the **Data** menu icon in the middle of the menu bar near the top of the page. Enter "crime" into the search the marketplace box on the upper right of the page and **Enter**.

	![Searching for Crime Data](images/searching-crime-data.png?raw=true "Searching for Crime Data")

	_Searching for Crime Data_

1. Select the **2006-2008 Crime in the United States (Data.gov)** data.

	![Selecting Crime Data](images/selecting-crime-data.png?raw=true "Selecting Crime Data")

	_Selecting Crime Data_

1. Press the **SIGN UP** button on the right side of the page. Note that there is no cost for subscribing. Agree to the conditions on the Sign Up page and click the **Sign Up** button.

	![Subscribing to crime data](images/subscribing-to-crime-data.png?raw=true "Subscribing to crime data")

	_Subscribing to crime data_

1. This brings up the RECEIPT page. Press the **EXPLORE THIS DATASET** button to bring up a window where you can build your query.

	![Confirmation page](images/confirmation-page.png?raw=true "Confirmation page")

	_Confirmation page_

1. Press the **RUN QUERY** button on the right side of the page to run the query without any parameters. Note the name of the query and the name of the table, and then click on the **DEVELOP** tab to reveal the query that was auto generated.  Copy this query.

	![Executing the crime data query](images/executing-the-crime-data-query.png?raw=true "Executing the crime data query")

	_Executing the crime data query_


	![Obtaining crime query results](images/obtaining-crime-query-results.png?raw=true "Obtaining crime query results")

	_Obtaining crime query results_


<a name="Ex3Task3" />
#### Task 3 - Importing Data from the Microsoft Azure Marketplace ####

In this task, you will use the query you built in the previous task to import data to your cluster.

1. Return to your **HDInsight on Azure** portal, scroll down to the **Your Cluster** section and click on **Remote Desktop** icon.

	![Remote Desktop](images/rdp.png?raw=true "Remote Desktop")

	_Remoting to the Cluster Head Node_

1. Open the **Remote Desktop Profile** file (.rdp file) which your browser downloads & login using your HDInsight credentials.

1. Download **Curl** from http://curl.haxx.se/download.html using your browser & save it to C:\curl. If you have permission issues with the browser on the headnode, download to local machine and then copy, paste into your RDP session Window to upload it. 

![Image 49](images/image-49.png?raw=true) 
	
1. Open the **HDInsight Command Line** console.

1. Navigate to C:\curl and run the following command to download the data from the data market using the curl utility.
 
	`curl "https://datamarket.azure.com/offer/download?endpoint=https%3A%2F%2Fapi.datamarket.azure.com%2Fdata.gov%2FCrimes%2Fv1%2F&query=CityCrime&accountKey=kYhmXxxmCej9%2B3wH5Mw23Q4cYbUB8JcTD1%2FoTAUG40o%3D&title=2006+-+2008+Crime+in+the+United+States+(Data.gov)&name=2006+-+2008+Crime+in+the+United+States+(Data.gov)-CityCrime" -u yourUsername@live.com:yourPasskey  --insecure > C:\hivedata\crimedata.csv`

	Note that **yourUsername** and **yourPasskey** are obtained from your Microsoft Azure Data Market account. Your user name is the email used for you Live ID. The value for the passkey is the account key default value assigned to you when you opened your Marketplace account. It can also be found as the Primary Account Key value on you Marketplace Account Details page.  

1. Now that the crime data is stored on the local filesystem of the head node, we must copy the file into **HDFS**.
	1. Ensure that all of the quotes are removed from the crimedata.csv file. To do so open a powershell console on the remote desktop and type the following command.
       `(Get-Content C:\hivedata\crimedata.csv) | % {$_ -replace '"', ""} | out-file -FilePath C:\hivedata\crimedata.csv -Force -Encoding ascii`
 
   1. In the **HDInsight Command Line** prompt, use the following HDInsight command:

		`hadoop fs -copyFromLocal "C:\hivedata\crimedata.csv" "/hivedata/crimedata.csv"`

	1. To ensure the file has been correctly loaded into **HDFS** run the following command: 
 
		``hadoop fs -ls "/hivedata/"``
		and you should see your crimedata file listed.

1. Now create a table in **Hive** with a schema appropriate for the crime data by following these steps...

	1. Go into the **Interactive Console** of the HDInsight portal & select **Hive**.

		![Using Hive](images/hive.png?raw=true "Using Hive")

	1. Enter the following command in the textbox & click **Evaluate**.  Make sure you copy the entire SQL statement.

		`create external table crime_data(ROWID INT, State STRING, City STRING, Year INT, Population INT, ViolentCrime INT, MurderAndNonEgligentManslaughter INT, ForcibleRape INT, Robbery INT, AggravatedAssault INT, PropertyCrime INT, Burglary INT, LarcenyTheft INT, MotorVehicleTheft INT, Arson INT) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LINES TERMINATED BY '\n' STORED AS TEXTFILE LOCATION '/hivedata/';`
	
		![Creating a table in Hive](images/interactive-hive.png?raw=true "Creating a table in Hive")

	1. Now the crime data is loaded into **Hive** as an external table and the existence of the data can be verified via the following command in the **Interactive Hive Console**: 

		`select * from crime_data;`
	
		![Crime Data Results](images/crime_data.png?raw=true "Crime Data Results")

<a name="Ex3Task4" />
#### Task 4 - Querying Imported Data ####

In this task, you will query the imported data in the previous task using the Interactive Hive console.

1. When the import task completes, return to your Account page and select the **Interactive Console** icon from the **Your Cluster** section. 

	![Selecting the Interactive Console](images/selecting-interactive-console.png?raw=true "Selecting the Interactive Console")

	_Selecting the Interactive Console_

1. Press the **Hive** option on the console page and enter the following command: 

	````
	create table crime_results as select city, max(violentcrime) 
		as maxviolentcrime from crime_data 
		group by city order by maxviolentcrime desc limit 10
	````

	![The Interactive Hive console](images/the-interactive-hive-console.png?raw=true "The Interactive Hive console")

	_The Interactive Hive console_

1. Press the **Evaluate** button and wait a few moments for the process to finish.

	![Evaluating a Hive query](images/evaluating-a-hive-query.png?raw=true "Evaluating a Hive query")

	_Evaluating a Hive query_

1. You can now query the **crime_results** table.  Just enter **select * from crime_results** and then press **Evaluate**.  You should see the results as follows:

	![Querying crime_results table](images/querying-crime-results-table.png?raw=true "Querying crime_results table")

	_Querying crime_results table_

---

<a name="Exercise4" />
### Exercise 4: Connecting to and Querying Hive Data in a Cluster using Excel ###

In this exercise, you will learn how to connect and query data in a cluster using Excel.

<a name="Ex4Task1" />
#### Task 1 - Installing the Hive Panel ####

In this task, you will configure your cluster, download and install the Hive ODBC driver and Hive Add-in for Excel.

1. Return to your Account page and select the **Open Ports** icon from the **Your Cluster** section to open the Configure Ports page. 

	![Selecting Open Ports Option](images/selecting-the-open-ports-option.png?raw=true "Selecting Open Ports Option")

	_Selecting Open Ports Option_

1. Open the ODBC Server on port 10000 by clicking on its Toggle button.

	![Opening ODBC port 10000](images/opening-odbc-port-10000.png?raw=true "Opening ODBC Port 10000")

	_Opening ODBC port 10000_

1. Return to your Account page and select the **Downloads** icon from the **Manage your account** section.

	![Selecting the Downloads Option](images/selecting-the-downloads-option.png?raw=true "Selecting the Downloads Option")

	_Selecting the Downloads Option_

1. Select the appropriate .msi file to install the Hive ODBC drivers and Excel Hive Add-In.

	![Downloading the Hive ODBC driver](images/downloading-hiveodbc.png?raw=true "Downloading the Hive ODBC driver")

	_Downloading the Hive ODBC driver and Add-in for Excel_

1. Select the **Run anyway** option from the **SmartScreen Filter** window that pops up.
  
	![Selecting the Run anyway option](images/selecting-the-runanyway-option.png?raw=true "Selecting the Run anyway option")

	_Selecting the Run anyway option_

1. Open Excel after the installation completes. Under data menu click on the **Hive Pane**.

	![ExcelDataMenuHivePane](images/exceldatamenuhivepanel.png?raw=true "Excel Data Menu Hive Pane")

	_The Hive Pane_

1. Press the **Enter Cluster Details** button on the **Hive Query** pane.

	![Hive query builder](images/hive-query-builder.png?raw=true "Hive query builder")

	_Hive query builder_

1. Provide a **Description** for your cluster, enter the domain of your cluster for the **Host** value (you obtain this value from the Account Page) and enter **10000** for the port number. Enter the administrator credentials for your HDInsight on Azure cluster in the **Username/Password** option in the **Authentication** section (the administrator credentials are same you provided on the Getting Started section of this HOL). Then click **OK**.

	![Obtaining cluster url](images/obtaining-cluster-url.png?raw=true "Obtaining cluster url")

	_Obtaining cluster url_

	![Entering cluster details](images/entering-cluster-details.png?raw=true "Entering cluster details")

	_Entering cluster details_

<a name="Ex4Task2" />
#### Task 2 - Querying Data using the Hive Panel ####

In this task, you will connect query data using the Hive panel from Excel.

1. From the **Hive Query** panel, select crime_results from **Select the Hive Object to Query** menu. Then check city and maxviolentcrime in the **Columns** section.  
Click on the HiveQL to reveal the query: 

	`select city, maxviolentcrime from crime_results limit 200`

1. Click on **Execute Query** button.

	![Building and executing the Hive query](images/building-and-executing-the-hive-query.png?raw=true "Building and executing the Hive query")

	_Building and executing the Hive query_

1. This should display the cities with the most violent crime. From the **Insert** menu, select the Bar option to insert a bar chart on to the page to obtain a visualization of the data.

	![ExcelHiveQueryResults](images/inserting-a-bar-graph-to-view-results.png?raw=true "Excel Hive Query Results")

	_Inserting a bar graph to view results_

---

<a name="Summary" />
## Summary ##

In this hands-on lab, you have seen two ways to run MapReduce jobs using the HDInsight on Azure portal. One used the **Create Job** UI to run a Java MapReduce program using a jar file. The other used the **Interactive Console** to run a MapReduce job using a .js script within a Pig query.
You have also seen how to upload this data into HDInsight on Azure and query it using Hive scripts from the **Interactive Console**. 
Finally, you have seen how to use Excel via the Hive ODBC driver to access and view data that is stored in the HDFS cluster.

