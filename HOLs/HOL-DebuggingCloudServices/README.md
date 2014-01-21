# Debugging Applications in Windows Azure #

## Hands-On Lab ##

### Introduction ###

Hands-on labs are sets of step-by-step guides that are designed to help you learn how to use key Windows Azure services and features.  Each Lab provides instructions to guide you through the process of developing a complete application.

This Hands-On Lab is a step-by-step guide that is designed to help you debug a simple application by configuring a special trace listener that can write its output directly into a table in Windows Azure storage emulator.  To produce diagnostic data, you instrument the application to write its trace information using standard methods in the System.Diagnostics namespace. Finally, you create a simple log viewer application that can retrieve and display the contents of the diagnostics table.

> **Note:** You can download the latest build of the Windows Azure Training Kit which includes a tested version of this HOL from here: http://bit.ly/WindowsAzureTK.

Visit our [GitHub Homepage](http://windowsazure-trainingkit.github.com/) for more information about the **Windows Azure Training Kit**.

### Repository Structure ###

In the **root** folder of this repository you will find the lab document, **HOL.md**. Before beginning with the lab exercises, make sure you have followed all the required steps indicated at the setup section of the lab document. 

In the **Source** folder you will find the source code of each of the exercises, as well as the setup scripts. Throughout the lab you will be instructed to open and explore the different solutions from the source folder. It is typically comprised of the following subfolders:

- **Assets:** This folder contains files that are used throughout the exercises.
- **_Exercise Name_:** Exercise that requires a programming solution has its own code folder.
  - **Begin:** The begin solution is the initial incomplete solution that you will finish by following the steps of the corresponding exercise.
	- **End:** The end solution is the final result you will achieve at the end of an exercise.
- **Setup:** This folder contains the dependency files and the setup scripts necessary to initialize specific configurations of the lab, being its execution is required in the majority of the lab.


### Get Started ###

In order to run the solutions of the exercises provided by this lab you will first need configure your environment and install any necessary prerequisites such as runtimes, components, or libraries. For your ease, you can download and run the dependency checker [here] (http://contentinstaller.blob.core.windows.net/dependency-checker/DC.exe) to automatically check and install all the requirements.  Each lab also includes setup instructions for getting started.

### Contributing to the Repository ###

If you find any issues or opportunties for improving this hands-on lab, fix them! Feel free to contribute to this project by [forking](http://help.github.com/fork-a-repo/) this repository and make changes to the content. Once you've made your changes, share them back with the community by sending a pull request. Please see GitHub section [How to send pull requests](http://help.github.com/send-pull-requests/) and the [Windows Azure Contribution Guidelines](http://windowsazure.github.com/guidelines.html) for more information about contributing to projects.

### Reporting Issues ###

If you find any issues with this lab that you can't fix, feel free to report them in the [issues](https://github.com/WindowsAzure-TrainingKit/HOL-DebuggingCloudServices-VS2012/issues) section of this repository.