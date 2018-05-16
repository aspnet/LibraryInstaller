﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Web.LibraryManager.Contracts;

namespace Microsoft.Web.LibraryManager.Tools.Commands
{
    /// <summary>
    /// Defines a command to allow cleaning the libman cache.
    /// </summary>
    internal class CacheCleanCommand : BaseCommand
    {
        public CacheCleanCommand(IHostEnvironment environment, bool throwOnUnexpectedArg = true)
            : base(throwOnUnexpectedArg, "clean", Resources.CacheCleanCommandDesc, environment)
        {
        }

        /// <summary>
        /// Command argument that specifies the provider for which the cache should be cleaned.
        /// </summary>
        public CommandArgument Provider { get; private set; }

        public override BaseCommand Configure(CommandLineApplication parent = null)
        {
            base.Configure(parent);

            Provider = Argument("provider", Resources.CacheCleanProviderArgumentDesc);

            return this;
        }

        protected override Task<int> ExecuteInternalAsync()
        {
            if (string.IsNullOrWhiteSpace(Provider.Value))
            {
                try
                {
                    if (Directory.Exists(HostInteractions.CacheDirectory))
                    {
                        Directory.Delete(HostInteractions.CacheDirectory, true);
                    }

                    Logger.Log(Resources.CacheCleanedMessage, LogLevel.Operation);
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(Resources.CacheCleanFailed, ex.Message), LogLevel.Error);
                }
            }
            else if (Directory.Exists(Path.Combine(HostInteractions.CacheDirectory, Provider.Value)))
            {
                try
                {
                    Directory.Delete(Path.Combine(HostInteractions.CacheDirectory, Provider.Value), true);

                    Logger.Log(Resources.CacheForProviderCleanedMessage, LogLevel.Operation);
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(Resources.CacheCleanFailed, ex.Message), LogLevel.Error);
                }
            }

            return Task.FromResult(0);
        }
    }
}
