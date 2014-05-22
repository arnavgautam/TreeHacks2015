- Add the NuGet
 - For each of your Xamarin projects, you will need to delete the following references:
   - System.IO
   - System.Runtime
   - System.Threading.Tasks
 - iOS only: Fix your json.net reference. Instead of portable-net45+wp80+win8 you need portable-net40+sl4+wp7+win8
 - Call CurrentPlatform.Init() from your Xamarin projects. It doesn't matter where  you put the code. I usually stick it in main.
 - For your xamarin projects, you need this app.config:

 <?xml version="1.0" encoding="utf-8"?>
 <configuration>
   <runtime>
     <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
       <dependentAssembly>
         <assemblyIdentity name="System.Net.Http" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
         <bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="2.5.0.0" />
       </dependentAssembly>
     </assemblyBinding>
   </runtime>
 </configuration>
