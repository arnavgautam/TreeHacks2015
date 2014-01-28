<a name="Title" />
#Running Hadoop Jobs on Windows Azure#

---
<a name="Overview" />
## Overview ##

This hands-on lab shows two ways to run MapReduce programs in a Hadoop on Azure cluster and how to analyze data imported into the cluster from Excel using Hive-based connectivity. 

The first way to run a MapReduce program is with a Hadoop jar file using the **Create Job** UI. The second way is with a query using the fluent API layered on Pig that is provided by the **Interactive Console**. The first approach uses a MapReduce program written in Java; the second uses a script written in JavaScript. This lab also shows how to upload files to the HDFS cluster that are needed as input for a MapReduce program and how to read the MapReduce output files from the HDFS cluster to examine the results of an analysis.

The Windows Azure Marketplace collects data, imagery, and real-time web services from leading commercial data providers and authoritative public data sources. It simplifies the purchase and consumption of a wide variety of data including demographic, environment, financial, retail and sports. This lab shows how to upload this data into a Hadoop on Azure cluster and query it using Hive scripts.

A key feature of Microsoft’s Big Data Solution is the integration of Hadoop with Microsoft Business Intelligence (BI) components. A good example of this is the ability for Excel to connect to the Hive data warehouse framework in the Hadoop cluster. This lab shows how to use Excel via the Hive ODBC driver to access and view data in the cluster. 

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

* Run a basic Java MapReduce program using a Hadoop jar file

* Upload input files to the HDFS cluster and read output files from the HDFS cluster

* Run a JavaScript MapReduce script with a query using the fluent API on Pig that is provided by the Interactive JavaScript Console

* Import data from the Windows Azure Marketplace into a Hadoop on Azure cluster using the Interactive Hive Console

* Use Excel to query data stored in a Hadoop on Azure cluster

<a name="Prerequisites" />
### Prerequisites ###

You must have an account to access Hadoop on Azure. You may request for an invitation [here](http://www.hadooponazure.com). To create a Hadoop cluster, follow the instructions outlined in the _Getting Started_ section of this lab.  You will also need Microsoft Excel to install the Hive Add-in.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

- [Getting Started: Requesting a new cluster](#GettingStarted)
- [Executing a basic Java MapReduce program using a Hadoop jar file](#Exercise1)
- [Executing a JavaScript MapReduce script using the Interactive Console](#Exercise2)
- [Importing data from the Windows Azure Marketplace using the Hive Interactive Console](#Exercise3)
- [Connecting to and querying Hive data in a cluster using Excel](#Exercise4)


<a name="GettingStarted" />
### Getting Started: Requesting a new cluster ###

In this exercise, you will request a new Hadoop cluster through the Hadoop on Windows Azure  portal.

1. Go to the [Hadoop on Windows  Azure](http://www.hadooponazure.com) portal and login using your credentials.

	>**Note:** Currently, the Developer Preview is available only by invitation.  You may request access in the portal.

1.  If you don't already have a working cluster, you will be asked to request a new one.  Fill the DNS name, choose a small sized cluster, create login credentials and select **Request cluster**.

	![Requesting a new cluster](images/requesting-a-new-cluster.png?raw=true "Requesting a new cluster")

	_Requesting a new cluster_


1. The process will take a few minutes.  While your cluster is requested, you will see the **Allocation in progress** message.

	![Cluster allocation in progress](images/cluster-allocation-in-progress.png?raw=true "Cluster allocation in progress")

	_Cluster allocation in progress_

<a name="Exercise1" />
### Exercise 1: Executing a basic Java MapReduce program using a Hadoop jar file ###

In this exercise, you will learn how to execute a simple Java MapReduce program using a Hadoop JAR file.

<a name="Ex1Task1" />
#### Task 1 - Deploying and Executing the Pi Estimator Sample  ####

In this task, you will deploy and execute the Pi Estimator sample, available in the Hadoop on Windows Azure portal.

1. From your **Account** page, click on the **Create Job** icon in the **Your Tasks** section.

1. This brings up the **Create Job** UI.

	![CreateJobUI](images/createjobui.png?raw=true "The Create Job UI")

	_The Create Job UI_

	>**Note:** To run a MapReduce program you need to specify the Job Name and the JAR File to use. Parameters are added to specify the name of the MapReduce program to run, the location of input and code files, and an output directory.

1. To see a simple example of how this interface is used to run the MapReduce job, let's look at the Pi Estimator sample. Return to your **Account** page. Scroll down to the **Samples** icon in the **Manage your account** section and click on it. 

1. From your **Account** page, scroll down to the **Samples** icon in the **Manage your account** section and click on it.   

	![Selecting Samples](images/account-page-samples.png?raw=true "Selecting Samples")

	_Selecting Samples_

1. Click on the **Pi Estimator** sample icon in the Hadoop Sample Gallery.

	![SelectingThePiEstimatorSample](images/selecting-the-piestimator-sample.png?raw=true "Pi Estimator Create Job UI")

	_Selecting the Pi Estimator sample_

	> **Note:** On the **Pi Estimator** page, information is provided about the application and downloads are make available for Java MapReduce programs and the jar file that contains the files needed by Hadoop on Azure to deploy the application.

1. Click on the **Deploy to your cluster** button on the right side to deploy the files to the cluster.

	![Deploying the Pi Estimator sample](images/deploying-the-piestimator-sample.png?raw=true "Deploying the Pi Estimator sample")
	
	_Deploying the Pi Estimator sample_

1. Once the **Create Job** page opens, check that the fields are already populated.	
	![PiEstimatorCreateJob](images/PiEstimatorCreateJob.PNG?raw=true "Pi Estimator Create Job UI")

	_The Pi Estimator Create Job page_


	>**Note:** The fields on the **Create Job** page are populated for you in this example. The first parameter value defaults to "pi 16 10000000". The first number indicates how many maps to create (default is 16) and the second number indicates how many samples are generated per map (10 million by default). So this program uses 160 million random points to make its estimate of Pi. The **Final Command** is automatically constructed for you from the specified parameters and jar file.

1. To run the program on the Hadoop cluster, simply click on the blue **Execute job** button on the right side of the page. The status of the job is displayed on the page and will change to  _Completed Successfully_ when it is done. The result is displayed at the bottom of the Output (stdout) section. For the default parameters, the result is Pi = 3.14159155000000000000 which is accurate to 8 decimal place, when rounded.

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
### Exercise 3: Importing data from the Windows Azure Marketplace using the Hive Interactive Console ###

In this exercise, you will learn how to subscribe and import data from the Windows Azure Marketplace.  You will also learn how to query data using the Hive Interactive Console.

<a name="Ex3Task1" />
#### Task 1 -  Registering a new Account ####

In this task, you will register a new account in the Windows Azure Marketplace.  You will then use this account to subscribe and import data.

1. Open the [Windows Azure Marketplace](https://datamarket.azure.com/ "DataMarket") page in a browser and sign in with a valid Windows Live ID.

	![The Windows Azure Marketplace home page](images/the-windows-azure-marketplace-home-page.png?raw=true "The Windows Azure Marketplace home page")

	_The Windows Azure Marketplace home page_

1. Click on the **MyAccount** tab and complete the Registration form to open a subscription account.

	![Registering a new account](images/registering-a-new-account.png?raw=true "Registering a new account")	

	_Registering a new account_
	
	
	![Obtaining the account keys](images/obtaining-the-account-keys.png?raw=true "Obtaining the account keys")

	_Obtaining the account keys_

	> **Note:** Account keys are used by applications to access your Windows Azure Marketplace dataset subscriptions.

<a name="Ex3Task2" />
#### Task 2 -  Subscribing to Crime Data and Building a Query ####

In this task, you will subscribe to crime data in the Windows Azure Marketplace, and you will build a query that will be used in the following tasks for importing data.

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
#### Task 3 - Importing Data from the Windows Azure Marketplace ####

In this task, you will use the query you built in the previous task to import data to your cluster.

1. Return to your Hadoop on Azure Account page, scroll down to the **Your Cluster** section and click on **Manage Cluster** icon.

	![Selecting the Manage Cluster option](images/selecting-the-manage-cluster-option.png?raw=true "Selecting the Manage Cluster option")

	_Selecting the Manage Cluster option_

1. Select the **DataMarket** icon option for importing data from Windows Azure Marketplace.

	![Selecting the DataMarket option](images/selecting-the-data-market-option.png?raw=true "Selecting the DataMarket option")

	_Selecting the DataMarket option_
	
1. Enter the subscription **User name** and **passkey**, **Query** and **Hive table name** obtained from your Windows Azure Marketplace account. Your user name is the email used for you Live ID. The value for the passkey is the account key default value assigned to you when you opened your Marketplace account. It can also be found as the Primary Account Key value on you Marketplace Account Details page.  Use the query you obtained earlier in Marketplace, but remove the trailing **top** parameter.  The query should look like this:

	````
    https://api.datamarket.azure.com/Data.ashx/data.gov/Crimes/CityCrime?
	````

1. After the parameters are entered, press the **Import Data** button

	![Importing data from the Windows Azure Marketplace](images/importing-data-from-the-marketplace.png?raw=true "Importing data from the Windows Azure Marketplace")

	_Importing data from the Windows Azure Marketplace_

1. The progress made importing is reported on the Data Market Import page that appears.  Wait a few minutes until the process finishes.

	![Importing-data-progress](images/importing-data-progress.png?raw=true "Importing data progress")

	_Importing data progress_


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

1. Provide a **Description** for your cluster, enter the domain of your cluster for the **Host** value (you obtain this value from the Account Page) and enter **10000** for the port number. Enter the administrator credentials for your Hadoop on Azure cluster in the **Username/Password** option in the **Authentication** section (the administrator credentials are same you provided on the Getting Started section of this HOL). Then click **OK**.

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

In this hands-on lab, you have seen two ways to run MapReduce jobs using the Hadoop on Azure portal. One used the **Create Job** UI to run a Java MapReduce program using a jar file. The other used the **Interactive Console** to run a MapReduce job using a .js script within a Pig query.
You have also seen how to upload this data into Hadoop on Azure and query it using Hive scripts from the **Interactive Console**. 
Finally, you have seen how to use Excel via the Hive ODBC driver to access and view data that is stored in the HDFS cluster.

