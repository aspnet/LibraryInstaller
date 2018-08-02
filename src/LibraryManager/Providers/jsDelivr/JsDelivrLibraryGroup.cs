﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.LibraryManager.Contracts;
using Microsoft.Web.LibraryManager.LibraryNaming;

namespace Microsoft.Web.LibraryManager.Providers.jsDelivr
{
    internal class JsDelivrLibraryGroup : ILibraryGroup
    {
        public JsDelivrLibraryGroup(string displayName, string description = null)
        {
            DisplayName = displayName;
            Description = description;
        }
        public string DisplayName { get; }

        public string Description { get; }

        public async Task<IEnumerable<string>> GetLibraryIdsAsync(CancellationToken cancellationToken)
        {

            if (JsDelivrCatalog.IsGitHub(DisplayName).Result)
            {

            }
            else
            {
                Microsoft.Web.LibraryManager.Providers.Unpkg.NpmPackageInfo npmPackageInfo = await Microsoft.Web.LibraryManager.Providers.Unpkg.NpmPackageInfoCache.GetPackageInfoAsync(DisplayName, CancellationToken.None);

                if (npmPackageInfo != null)
                {
                    return npmPackageInfo.Versions
                        .OrderByDescending(v => v)
                        .Select(semanticVersion => LibraryIdToNameAndVersionConverter.Instance.GetLibraryId(DisplayName, semanticVersion.ToString(), JsDelivrProvider.IdText))
                        .ToList();
                }
            }

            return Enumerable.Empty<string>();
        }
    }
}
