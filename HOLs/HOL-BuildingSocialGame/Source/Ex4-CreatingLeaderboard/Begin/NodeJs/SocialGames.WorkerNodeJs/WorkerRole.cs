// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.SocialGames.WorkerNodeJs
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Security.Permissions;
    using System.Threading;
    using Autofac;
    using Microsoft.Samples.SocialGames;
    using Microsoft.Samples.SocialGames.Common.JobEngine;
    using Microsoft.Samples.SocialGames.Common.Storage;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.Samples.SocialGames.Extensions;
    using Microsoft.Samples.SocialGames.Repositories;
    using Microsoft.Samples.SocialGames.Worker.Commands;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.Diagnostics.Management;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.Net.Sockets;

    public class WorkerRole : RoleEntryPoint
    {
        private static string wadConnectionString = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";
        private Process proc;

        public static IEnumerable<string> ConfiguredCounters
        {
            get
            {
                yield return @"\Processor(_Total)\% Processor Time";
                yield return @"\Memory\Available MBytes";
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool IsDevelopmentEnvironment()
        {
            return !RoleEnvironment.IsAvailable ||
            (RoleEnvironment.IsAvailable && RoleEnvironment.DeploymentId.StartsWith("deployment", StringComparison.OrdinalIgnoreCase));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public override void Run()
        {
            this.LaunchNode();

            // Setup AutoFac
            var builder = new ContainerBuilder();
            this.DependencySetup(builder);
            var container = builder.Build();

            // Call Initializers
            var initializers = container.Resolve<IEnumerable<IStorageInitializer>>();
            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            var account = container.Resolve<CloudStorageAccount>();
            var userRepository = container.Resolve<IUserRepository>();
            var gameRepository = container.Resolve<IGameRepository>();
            var workerContext = container.Resolve<IWorkerContext>();

            // TaskBuilder callback for logging errors
            Action<ICommand, IDictionary<string, object>, Exception> logException = (cmd, context, ex) =>
            {
                Trace.TraceError(ex.ToString());
            };

            // Process for Invite messages
            Task.TriggeredBy(Message.OfType<InviteMessage>(account, ConfigurationConstants.InvitesQueue))
                .SetupContext((message, context) =>
                {
                    context.Add("userId", message.UserId);
                    context.Add("invitedUserId", message.InvitedUserId);
                    context.Add("gameQueueId", message.GameQueueId);
                    context.Add("timestamp", message.Timestamp);
                    context.Add("message", message.Message);
                    context.Add("url", message.Url);
                })
                .Do(container.Resolve<InviteCommand>())
                .OnError(logException)
                .Start();

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public override bool OnStart()
        {
            Trace.TraceInformation("Microsoft.Samples.SocialGames.Worker.OnStart");
            ServicePointManager.DefaultConnectionLimit = 12;
            RoleEnvironment.Changing += this.RoleEnvironmentChanging;
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                string configuration = RoleEnvironment.IsAvailable ?
                    RoleEnvironment.GetConfigurationSettingValue(configName) :
                    ConfigurationManager.AppSettings[configName];

                configSetter(configuration);
            });

            if (!IsDevelopmentEnvironment())
            {
                ConfigureDiagnosticMonitor();
            }

            return base.OnStart();
        }

        protected void DependencySetup(ContainerBuilder builder)
        {
            // Cloud Storage Account
            builder.RegisterInstance<CloudStorageAccount>(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"));

            // Queues
            builder.RegisterQueue<InviteMessage>(ConfigurationConstants.InvitesQueue)
                .AsImplementedInterfaces();

            // Blobs
            builder.RegisterBlob<UserProfile>(ConfigurationConstants.UsersContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<UserSession>(ConfigurationConstants.UserSessionsContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<Friends>(ConfigurationConstants.FriendsContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<NotificationStatus>(ConfigurationConstants.NotificationsContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<Game>(ConfigurationConstants.GamesContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<GameQueue>(ConfigurationConstants.GamesQueuesContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<UserProfile>(ConfigurationConstants.GamesContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();

            // Tables
            builder.RegisterTable<UserStats>(ConfigurationConstants.UserStatsTableName, true /* jsonpSupport */)
                .AsImplementedInterfaces();

            // Repositories
            builder.RegisterType<GameRepository>().AsImplementedInterfaces();
            builder.RegisterType<IdentityProviderRepository>().AsImplementedInterfaces();
            builder.RegisterType<NotificationRepository>().AsImplementedInterfaces();
            builder.RegisterType<UserRepository>().AsImplementedInterfaces();

            // Commands
            builder.RegisterType<InviteCommand>();

            // Misc
            builder.RegisterType<InMemoryWorkerContext>().AsImplementedInterfaces();
        }

        private static void ConfigureDiagnosticMonitor()
        {
            var storageAccount = CloudStorageAccount.FromConfigurationSetting(wadConnectionString);
            var roleInstanceDiagnosticManager = storageAccount.CreateRoleInstanceDiagnosticManager(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);
            var diagnosticMonitorConfiguration = roleInstanceDiagnosticManager.GetCurrentConfiguration();

            // Performance Counters
            ConfiguredCounters.ToList().ForEach(
                counter =>
                {
                    var counterConfiguration = new PerformanceCounterConfiguration
                    {
                        CounterSpecifier = counter,
                        SampleRate = TimeSpan.FromSeconds(30)
                    };

                    diagnosticMonitorConfiguration.PerformanceCounters.DataSources.Add(counterConfiguration);
                });

            diagnosticMonitorConfiguration.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

            roleInstanceDiagnosticManager.SetCurrentConfiguration(diagnosticMonitorConfiguration);
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private bool NodeIsOk()
        {
            Trace.TraceInformation("Testing Node.Js server");

            try
            {
                TcpClient node;

                node = new TcpClient("127.0.0.1", 8124);

                Thread.Sleep(500);

                node.Close();

                return true;
            }
            catch
            {
                Trace.TraceError("Testing Failed");
                return false;
            }
        }

        private Process LaunchNode()
        {
            Trace.TraceInformation("Launching Node.Js server");
            this.proc = new Process()
            {
                StartInfo = new ProcessStartInfo(
                    Environment.ExpandEnvironmentVariables(@"%RoleRoot%\approot\node.exe"),
                    @".\server.js")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.ExpandEnvironmentVariables(@"%RoleRoot%\approot\"),
                }
            };
            this.proc.Start();

            return this.proc;
        }
    }
}