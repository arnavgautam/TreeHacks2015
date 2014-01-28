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

using Autofac;
using Autofac.Builder;
using Microsoft.Samples.SocialGames.Common.Storage;
using Microsoft.WindowsAzure.StorageClient;

namespace Microsoft.Samples.SocialGames.Extensions
{
    public static class AutofacStorageExtensions
    {

        public static IRegistrationBuilder<AzureQueue<TMessage>, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterQueue<TMessage>(this ContainerBuilder builder, string name) where TMessage : AzureQueueMessage
        {
            return builder.RegisterType<AzureQueue<TMessage>>().WithParameter("queueName", name);
        }

        public static IRegistrationBuilder<AzureBlobContainer<TEntity>, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterBlob<TEntity>(this ContainerBuilder builder, string name)
        {
            return builder.RegisterType<AzureBlobContainer<TEntity>>().WithParameter("containerName", name);
        }

        public static IRegistrationBuilder<AzureBlobContainer<TEntity>, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterBlob<TEntity>(this ContainerBuilder builder, string name, bool jsonpSupport)
        {
            return builder.RegisterType<AzureBlobContainer<TEntity>>().WithParameter("containerName", name).WithParameter("jsonpSupport", jsonpSupport);
        }

        public static IRegistrationBuilder<AzureTable<TEntity>, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterTable<TEntity>(this ContainerBuilder builder, string name, bool jsonpSupport) where TEntity : TableServiceEntity, new()
        {
            return builder.RegisterType<AzureTable<TEntity>>().WithParameter("containerName", name).WithParameter("jsonpSupport", jsonpSupport);
        }
    }
}
