﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.LibraryManager.Contracts;

namespace Microsoft.Web.LibraryManager
{
    /// <summary>
    /// Finds conflicts between different libraries, based on files brought in by each library.
    /// </summary>
    internal static class LibrariesValidator
    {
        /// <summary>
        /// Returns a collection of <see cref="ILibraryOperationResult"/> that represents the status for validation of each 
        ///  library 
        /// </summary>
        /// <param name="libraries">Set of libraries to be validated</param>
        /// <param name="dependencies"><see cref="IDependencies"/>used to validate the libraries</param>
        /// <param name="defaultDestination">Default destination used to validate the libraries</param>
        /// <param name="defaultProvider">DefaultProvider used to validate the libraries</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ILibraryOperationResult>> GetLibrariesErrorsAsync(
            IEnumerable<ILibraryInstallationState> libraries,
            IDependencies dependencies,
            string defaultDestination,
            string defaultProvider,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Check for duplicate libraries 
            IEnumerable<string> duplicates = GetDuplicateLibraries(libraries, dependencies, defaultProvider, cancellationToken);
            if (duplicates != null && duplicates.Any())
            {
                return new[] { LibraryOperationResult.FromError(PredefinedErrors.DuplicateLibrariesInManifest(duplicates)) };
            }

            // Check for valid libraries
            IEnumerable<ILibraryOperationResult> validateLibraries = await ValidatePropertiesAsync(libraries, cancellationToken);

            if (!validateLibraries.All(t => t.Success))
            {
                return validateLibraries;
            }

            // Check for files conflicts
            IEnumerable<ILibraryOperationResult> expandLibraries = await ExpandLibrariesAsync(libraries, dependencies, defaultDestination, defaultProvider, cancellationToken);
            if (!expandLibraries.All(t => t.Success))
            {
                return expandLibraries;
            }

            libraries = expandLibraries.Select(l => l.InstallationState);
            IEnumerable<FileConflict> fileConflicts = GetFilesConflicts(libraries, cancellationToken);
            ILibraryOperationResult conflictErrors = GetConflictErrors(fileConflicts);

            return new [] { conflictErrors };
        }


        /// <summary>
        /// Returns a collection of <see cref="ILibraryOperationResult"/> that represents the status for validation of the Manifest and its libraries
        /// </summary>
        /// <param name="manifest">The <see cref="Manifest"/> to be validated</param>
        /// <param name="dependencies"><see cref="IDependencies"/>used to validate the libraries</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ILibraryOperationResult>> GetManifestErrorsAsync(
            Manifest manifest,
            IDependencies dependencies,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (manifest == null)
            {
                return new ILibraryOperationResult[] { LibraryOperationResult.FromError(PredefinedErrors.ManifestMalformed()) };
            }

            if (!IsValidManifestVersion(manifest.Version))
            {
                return new ILibraryOperationResult[] { LibraryOperationResult.FromError(PredefinedErrors.VersionIsNotSupported(manifest.Version)) };
            }

            return await GetLibrariesErrorsAsync(manifest.Libraries, dependencies, manifest.DefaultDestination, manifest.DefaultProvider, cancellationToken);
        }

        private static bool IsValidManifestVersion(string version)
        {
            Version parsedVersion;
            if (Version.TryParse(version, out parsedVersion))
            {
                return Manifest.SupportedVersions.Contains(parsedVersion);
            }

            return false;
        }

        /// <summary>
        /// Validates the values of each Library property and returns a collection of ILibraryOperationResult for each of them 
        /// </summary>
        /// <param name="libraries"></param>
        /// <param name="dependencies"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<IEnumerable<ILibraryOperationResult>> ValidatePropertiesAsync(IEnumerable<ILibraryInstallationState> libraries, IDependencies dependencies, CancellationToken cancellationToken)
        {
            List<ILibraryOperationResult> validationStatus = new List<ILibraryOperationResult>();

            foreach (ILibraryInstallationState library in libraries)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ILibraryOperationResult result = await library.IsValidAsync(dependencies).ConfigureAwait(false);
                if (!result.Success)
                {
                    validationStatus.Add(result);
                }
                else
                {
                    validationStatus.Add(LibraryOperationResult.FromSuccess(library));
                }
            }

            return validationStatus;
        }

        private static IEnumerable<string> GetDuplicateLibraries(
            IEnumerable<ILibraryInstallationState> libraries,
            IDependencies dependencies,
            string defaultProvider,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var duplicateLibraries = new List<string>();
                IEnumerable<IProvider> providers = GetProviders(libraries, dependencies, defaultProvider);

                foreach (IProvider provider in providers)
                {
                    IEnumerable<ILibraryInstallationState> providerLibraries = libraries.Where(l => l.ProviderId == provider.Id);
                    duplicateLibraries.AddRange(providerLibraries.GroupBy(l => l.Name).Where(g => g.Count() > 1).Select(g => g.Key));
                }

                return duplicateLibraries;
            }
            catch (InvalidLibraryException)
            {
                return null;
            }
        }

        private static IEnumerable<IProvider> GetProviders(
            IEnumerable<ILibraryInstallationState> libraries,
            IDependencies dependencies,
            string defaultProvider)
        {
            var providers = new List<IProvider>();

            foreach (ILibraryInstallationState library in libraries)
            {

                IProvider provider = GetProvider(library, dependencies, defaultProvider, out _);
                if (provider != null)
                {
                    providers.Add(provider);
                }
            }

            return providers;
        }

        private static IProvider GetProvider(
            ILibraryInstallationState library,
            IDependencies dependencies,
            string defaultProvider,
            out IError error)
        {
            error = null;

            if (string.IsNullOrEmpty(library.ProviderId) && string.IsNullOrEmpty(defaultProvider))
            {
                error = PredefinedErrors.ProviderIsUndefined();
                return null;
            }

            if (dependencies != null)
            {
                string providerId = library.ProviderId ?? defaultProvider;
                IProvider provider = dependencies.GetProvider(providerId);
                if (provider == null)
                {
                    error = PredefinedErrors.ProviderUnknown(library.ProviderId);
                    return null;
                }

                return provider;
            }

            return null;
        }

        /// <summary>
        /// Expands the files property for each library 
        /// </summary>
        /// <param name="libraries"></param>
        /// <param name="dependencies"></param>
        /// <param name="defaultDestination"></param>
        /// <param name="defaultProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<IEnumerable<ILibraryOperationResult>> ExpandLibrariesAsync(
            IEnumerable<ILibraryInstallationState> libraries,
            IDependencies dependencies,
            string defaultDestination,
            string defaultProvider,
            CancellationToken cancellationToken)
        {
            List<ILibraryOperationResult> expandedLibraries = new List<ILibraryOperationResult>();

            foreach (ILibraryInstallationState library in libraries)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string installDestination = string.IsNullOrEmpty(library.DestinationPath) ? defaultDestination : library.DestinationPath;
                string providerId = string.IsNullOrEmpty(library.ProviderId) ? defaultProvider : library.ProviderId;

                IProvider provider = dependencies.GetProvider(providerId);
                if (provider == null)
                {
                    return new [] { LibraryOperationResult.FromError(PredefinedErrors.ProviderIsUndefined()) };
                }

                ILibraryOperationResult desiredState = await provider.UpdateStateAsync(library, cancellationToken);
                if (!desiredState.Success)
                {
                    return new [] { desiredState };
                }

                expandedLibraries.Add(desiredState);
            }

            return expandedLibraries;
        }

        /// <summary>
        /// Detects files conflicts in between libraries in the given collection 
        /// </summary>
        /// <param name="libraries"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A collection of <see cref="FileConflict"/> for each library conflict</returns>
        private static IEnumerable<FileConflict> GetFilesConflicts(IEnumerable<ILibraryInstallationState> libraries, CancellationToken cancellationToken)
        {
            Dictionary<string, List<ILibraryInstallationState>> _fileToLibraryMap = new Dictionary<string, List<ILibraryInstallationState>>(RelativePathEqualityComparer.Instance);

            foreach (ILibraryInstallationState library in libraries)
            {
                string destinationPath = library.DestinationPath;

                IEnumerable<string> files = library.Files.Select(f => Path.Combine(destinationPath, f));

                foreach (string file in files)
                {
                    if (!_fileToLibraryMap.ContainsKey(file))
                    {
                        _fileToLibraryMap[file] = new List<ILibraryInstallationState>();
                    }

                    _fileToLibraryMap[file].Add(library);
                }
            }

            return _fileToLibraryMap
                    .Where(f => f.Value.Count > 1)
                    .Select(f => new FileConflict(f.Key, f.Value));

        }

        /// <summary>
        /// Generates a single ILibraryOperationResult with a collection of IErros based on the collection of FileConflict
        /// </summary>
        /// <param name="fileConflicts"></param>
        /// <returns></returns>
        private static ILibraryOperationResult GetConflictErrors(IEnumerable<FileConflict> fileConflicts)
        {
            if (fileConflicts.Any())
            {
                var errors = new List<IError>();
                foreach (FileConflict conflictingLibraryGroup in fileConflicts)
                {
                    errors.Add(PredefinedErrors.ConflictingLibrariesInManifest(conflictingLibraryGroup.File, conflictingLibraryGroup.Libraries.Select(l => l.LibraryId).ToList()));
                }

                return new LibraryOperationResult(errors.ToArray());
            }

            return LibraryOperationResult.FromSuccess(null);
        }
    }
}
