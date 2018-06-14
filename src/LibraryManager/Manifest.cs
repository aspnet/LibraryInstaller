﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.LibraryManager.Contracts;
using Newtonsoft.Json;

namespace Microsoft.Web.LibraryManager
{
    /// <summary>
    /// Represents the manifest JSON file and orchestrates the interaction
    /// with the various <see cref="IProvider"/> instances.
    /// </summary>
    public class Manifest
    {
        /// <summary>
        /// Supported versions of Library Manager
        /// </summary>
        public static readonly Version[] SupportedVersions = { new Version("1.0") };
        private IHostInteraction _hostInteraction;
        private readonly List<ILibraryInstallationState> _libraries;
        private IDependencies _dependencies;
        private LibrariesValidator _librariesValidator;

        /// <summary>
        /// Creates a new instance of <see cref="Manifest"/>.
        /// </summary>
        /// <param name="dependencies">The host provided dependencies.</param>
        public Manifest(IDependencies dependencies)
        {
            _libraries = new List<ILibraryInstallationState>();
            _dependencies = dependencies;
            _hostInteraction = dependencies?.GetHostInteractions();
        }

        /// <summary>
        /// The version of the <see cref="Manifest"/> document format.
        /// </summary>
        [JsonProperty(ManifestConstants.Version)]
        public string Version { get; set; }

        /// <summary>
        /// The default <see cref="Manifest"/> library provider.
        /// </summary>
        [JsonProperty(ManifestConstants.DefaultProvider)]
        public string DefaultProvider { get; set; }

        /// <summary>
        /// The default destination path for libraries.
        /// </summary>
        [JsonProperty(ManifestConstants.DefaultDestination)]
        public string DefaultDestination { get; set; }

        /// <summary>
        /// A list of libraries contained in the <see cref="Manifest"/>.
        /// </summary>
        [JsonProperty(ManifestConstants.Libraries)]
        [JsonConverter(typeof(LibraryStateTypeConverter))]
        public IEnumerable<ILibraryInstallationState> Libraries => _libraries;

        /// <summary>
        /// Validates libraries in this manifest
        /// </summary>
        /// <param name="libraries"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ILibraryOperationResult>> ValidateLibrariesAsync(IEnumerable<ILibraryInstallationState> libraries, CancellationToken cancellationToken)
        {
            return await _librariesValidator.ValidateLibrariesAsync(libraries, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Manifest"/> class from a file on disk.
        /// </summary>
        /// <remarks>
        /// The <paramref name="fileName"/> doesn't have to exist on disk. It will be created when
        /// <see cref="SaveAsync"/> is invoked.
        /// </remarks>
        /// <param name="fileName">The absolute file path to the manifest JSON file.</param>
        /// <param name="dependencies">The host provided dependencies.</param>
        /// <param name="cancellationToken">A token that allows for cancellation of the operation.</param>
        /// <returns>An instance of the <see cref="Manifest"/> class.</returns>
        public static async Task<Manifest> FromFileAsync(string fileName, IDependencies dependencies, CancellationToken cancellationToken)
        {
            if (File.Exists(fileName))
            {
                string json = await FileHelpers.ReadFileAsTextAsync(fileName, cancellationToken).ConfigureAwait(false);
                return FromJson(json, dependencies);
            }

            return FromJson("{}", dependencies);
        }

        /// <summary>
        /// Creates an instance of the <see cref="Manifest"/> class based on
        /// the provided JSON string.
        /// </summary>
        /// <param name="json">A string of JSON in the correct format.</param>
        /// <param name="dependencies">The host provided dependencies.</param>
        /// <returns></returns>
        public static Manifest FromJson(string json, IDependencies dependencies)
        {
            try
            {
                Manifest manifest = JsonConvert.DeserializeObject<Manifest>(json);
                manifest._dependencies = dependencies;
                manifest._hostInteraction = dependencies.GetHostInteractions();
                manifest._librariesValidator = new LibrariesValidator(dependencies, manifest.DefaultDestination, manifest.DefaultProvider);

                return manifest;
            }
            catch (Exception)
            {
                dependencies.GetHostInteractions().Logger.Log(PredefinedErrors.ManifestMalformed().Message, LogLevel.Task);
                return null;
            }
        }

        internal void AddLibraryValidator(LibrariesValidator librariesValidator)
        {
            _librariesValidator = librariesValidator;
        }

        /// <summary>
        /// Removes the library from the <see cref="Libraries"/>
        /// </summary>
        /// <param name="libraryToUpdate"></param>
        /// <param name="newlibraryId"></param>
        public void ReplaceLibraryId(ILibraryInstallationState libraryToUpdate, string newlibraryId)
        {
            if (libraryToUpdate != null && libraryToUpdate is LibraryInstallationState state)
            {
                state.LibraryId = newlibraryId;
            }
        }

        //private static void UpdateLibraryProviderAndDestination(Manifest manifest)
        //{
        //    foreach (LibraryInstallationState state in manifest.Libraries.Cast<LibraryInstallationState>())
        //    {
        //        UpdateLibraryProviderAndDestination(state, manifest.DefaultProvider, manifest.DefaultDestination);
        //    }
        //}

        //private static void UpdateLibraryProviderAndDestination(ILibraryInstallationState state, string defaultProvider, string defaultDestination)
        //{
        //    LibraryInstallationState libraryState = state as LibraryInstallationState;

        //    if (libraryState == null)
        //    {
        //        return;
        //    }

        //    if (state.ProviderId == null)
        //    {
 
        //    }

        //    if (libraryState.DestinationPath == null)
        //    {
        //        libraryState.DestinationPath = defaultDestination;
        //        libraryState.IsUsingDefaultDestination = true;
        //    }
        //}

        private static bool IsValidManifestVersion(string version)
        {
            try
            {
                return SupportedVersions.Contains(new Version(version));
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a deep copy of the manifest.
        /// </summary>
        /// <returns></returns>
        public Manifest Clone()
        {
            var manifest = new Manifest(_dependencies)
            {
                Version = Version,
                DefaultDestination = DefaultDestination,
                DefaultProvider = DefaultProvider
            };

            foreach (LibraryInstallationState lib in _libraries.Cast<LibraryInstallationState>())
            {
                var newState = new LibraryInstallationState()
                {
                    LibraryId = lib.LibraryId,
                    DestinationPath = lib.DestinationPath,
                    Files = lib.Files == null ? null : new List<string>(lib.Files),
                    ProviderId = lib.ProviderId,
                    //IsUsingDefaultDestination = lib.IsUsingDefaultDestination,
                    //IsUsingDefaultProvider = lib.IsUsingDefaultProvider
                };

                manifest._libraries.Add(newState);
            }

            manifest._librariesValidator = new LibrariesValidator(_dependencies, DefaultDestination, DefaultProvider);

            return manifest;
        }

        /// <summary>
        /// Validates a single library 
        /// </summary>
        /// <param name="libraryId"></param>
        /// <param name="providerId"></param>
        /// <param name="files"></param>
        /// <param name="destination"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ILibraryOperationResult> ValidateLibraryAsync(string libraryId, string providerId, IReadOnlyList<string> files, string destination, CancellationToken cancellationToken)
        {

            var desiredState = new LibraryInstallationState()
            {
                LibraryId = libraryId,
                Files = files,
                ProviderId = providerId,
                DestinationPath = destination
            };

            return await _librariesValidator.ValidateLibraryAsync(desiredState, cancellationToken).ConfigureAwait(false);   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="libraryId"></param>
        /// <param name="providerId"></param>
        /// <param name="files"></param>
        /// <param name="destination"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ILibraryOperationResult>> InstallLibraryAsync(string libraryId, string providerId, IReadOnlyList<string> files, string destination, CancellationToken cancellationToken)
        {
            var libraryState = new LibraryInstallationState()
            {
                LibraryId = libraryId,
                Files = files,
                ProviderId = providerId,
                DestinationPath = destination
            };

            var librariesToValidate = new List<ILibraryInstallationState>(_libraries);
            librariesToValidate.Add(libraryState);

            IEnumerable<ILibraryOperationResult> validationResults = await _librariesValidator.ValidateLibrariesAsync(librariesToValidate, cancellationToken).ConfigureAwait(false);
            if (!validationResults.All(r => r.Success))
            {
                return validationResults;
            }

            ILibraryOperationResult result = await _librariesValidator.GetExpandedLibraryAsync(libraryState, cancellationToken);
            if (!result.Success)
            {
                return new[] { result };
            }

            ILibraryInstallationState desiredLibraryState = result.InstallationState;
            IProvider provider = _dependencies.GetProvider(desiredLibraryState.ProviderId);
            if (provider == null)
            {
                return new[] { new LibraryOperationResult(libraryState, new IError[] { PredefinedErrors.ProviderUnknown(libraryState.ProviderId) }) };
            }

            ILibraryOperationResult installResult = await provider.InstallAsync(result.InstallationState, cancellationToken).ConfigureAwait(false);

            if (installResult.Success)
            {
                _libraries.Add(libraryState);
            }

            return new[] { LibraryOperationResult.FromSuccess(libraryState) };
        }

        /// <summary>
        /// Adds a library to the <see cref="Libraries"/> collection.
        /// </summary>
        /// <param name="state">An instance of <see cref="ILibraryInstallationState"/> representing the library to add.</param>
        internal void AddLibrary(ILibraryInstallationState state)
        {
            ILibraryInstallationState existing = _libraries.Find(p => p.LibraryId == state.LibraryId && p.ProviderId == state.ProviderId);

            if (existing != null)
                _libraries.Remove(existing);

            _libraries.Add(state);
        }

        /// <summary>
        /// Adds a version to the manifest
        /// </summary>
        /// <param name="version"></param>
        public void AddVersion(string version)
        {
            Version = version;
        }

        /// <summary>
        /// Restores all libraries in the <see cref="Libraries"/> collection.
        /// </summary>
        /// <param name="cancellationToken">A token that allows for cancellation of the operation.</param>
        public async Task<IEnumerable<ILibraryOperationResult>> RestoreAsync(CancellationToken cancellationToken)
        {
            //TODO: This should have an "undo scope"
            var results = new List<ILibraryOperationResult>();

            if (!IsValidManifestVersion(Version))
            {
                return new ILibraryOperationResult[] { LibraryOperationResult.FromError(PredefinedErrors.VersionIsNotSupported(Version)) };
            }

            IEnumerable<ILibraryOperationResult> validationResults = await _librariesValidator.ValidateLibrariesAsync(_libraries, cancellationToken);
            if (!validationResults.All(r => r.Success))
            {
                return validationResults;
            }

            foreach (ILibraryInstallationState state in Libraries)
            {
                results.Add(await RestoreLibraryAsync(state, cancellationToken).ConfigureAwait(false));
            }

            return results;
        }

        private async Task<ILibraryOperationResult> RestoreLibraryAsync(ILibraryInstallationState libraryState, CancellationToken cancellationToken)
        {
            _hostInteraction.Logger.Log(string.Format(Resources.Text.Restore_RestoreOfLibraryStarted, libraryState.LibraryId, libraryState.DestinationPath), LogLevel.Operation);

            if (cancellationToken.IsCancellationRequested)
            {
                return LibraryOperationResult.FromCancelled(libraryState);
            }

            ILibraryOperationResult result = await _librariesValidator.GetExpandedLibraryAsync(libraryState, cancellationToken);
            if (!result.Success)
            {
                return result;
            }

            ILibraryInstallationState desiredLibraryState = result.InstallationState;
            IProvider provider = _dependencies.GetProvider(desiredLibraryState.ProviderId);
            if (provider == null)
            {
                return new LibraryOperationResult(libraryState, new IError[] { PredefinedErrors.ProviderUnknown(libraryState.ProviderId) });
            }

            try
            {
                return await provider.InstallAsync(desiredLibraryState, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return LibraryOperationResult.FromCancelled(libraryState);
            }
        }

        /// <summary>
        /// Uninstalls the specified library and removes it from the <see cref="Libraries"/> collection.
        /// </summary>
        /// <param name="libraryId">The library identifier.</param>
        /// <param name="deleteFilesFunction"></param>
        /// <param name="cancellationToken"></param>
        public async Task<ILibraryOperationResult> UninstallAsync(string libraryId, Func<IEnumerable<string>, Task<bool>> deleteFilesFunction, CancellationToken cancellationToken)
        {
            ILibraryInstallationState library = Libraries.FirstOrDefault(l => l.LibraryId == libraryId);

            if (cancellationToken.IsCancellationRequested)
            {
                return LibraryOperationResult.FromCancelled(library);
            }


            if (library != null)
            {
                try
                {
                    ILibraryOperationResult result = await DeleteLibraryFilesAsync(library, deleteFilesFunction, cancellationToken).ConfigureAwait(false);

                    if (result.Success)
                    {
                        _libraries.Remove(library);

                        return result;
                    }
                }
                catch (OperationCanceledException)
                {
                    return LibraryOperationResult.FromCancelled(library);
                }
            }

            return LibraryOperationResult.FromError(PredefinedErrors.CouldNotDeleteLibrary(library.LibraryId)); ;
        }

        /// <summary>
        /// Saves the manifest file to disk.
        /// </summary>
        /// <param name="fileName">The absolute file path to save the <see cref="Manifest"/> to.</param>
        /// <param name="cancellationToken">A token that allows for cancellation of the operation.</param>
        /// <returns></returns>
        public async Task SaveAsync(string fileName, CancellationToken cancellationToken)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            //foreach (ILibraryInstallationState library in _libraries)
            //{
            //    if (library is LibraryInstallationState state)
            //    {
            //        if (state.IsUsingDefaultDestination)
            //        {
            //            state.DestinationPath = null;
            //        }
            //        if (state.IsUsingDefaultProvider)
            //        {
            //            state.ProviderId = null;
            //        }
            //    }
            //}

            string json = JsonConvert.SerializeObject(this, settings);

            //UpdateLibraryProviderAndDestination(this);

            byte[] buffer = Encoding.UTF8.GetBytes(json);

            using (FileStream writer = File.Create(fileName, 4096, FileOptions.Asynchronous))
            {
                await writer.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///  Deletes all library output files from disk.
        /// </summary>
        /// <remarks>
        /// The host calling this method provides the <paramref name="deleteFileAction"/>
        /// that deletes the files from the project.
        /// </remarks>
        /// <param name="deleteFileAction">>An action to delete the files.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ILibraryOperationResult>> CleanAsync(Func<IEnumerable<string>, Task<bool>> deleteFileAction, CancellationToken cancellationToken)
        {
            var results = new List<ILibraryOperationResult>();

            foreach (ILibraryInstallationState state in Libraries)
            {
                results.Add(await DeleteLibraryFilesAsync(state, deleteFileAction, cancellationToken).ConfigureAwait(false));
            }

            return results;
        }

        /// <summary>
        /// Removes unwanted library files
        /// </summary>
        /// <param name="newManifest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUnwantedFilesAsync(Manifest newManifest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (newManifest != null)
            {
                IEnumerable<FileIdentifier> existingFiles = await GetAllManifestFilesWithVersionsAsync(Libraries, cancellationToken).ConfigureAwait(false);
                IEnumerable<FileIdentifier> newFiles = await GetAllManifestFilesWithVersionsAsync(newManifest.Libraries, cancellationToken).ConfigureAwait(false);
                IEnumerable<string> filesToRemove = existingFiles.Except(newFiles).Select(f => f.Path);

                if (filesToRemove.Any())
                {
                    IHostInteraction hostInteraction = _dependencies.GetHostInteractions();
                    return await hostInteraction.DeleteFilesAsync(filesToRemove, cancellationToken).ConfigureAwait(false);
                }
            }

            return true;
        }

        private async Task<IEnumerable<FileIdentifier>> GetAllManifestFilesWithVersionsAsync(IEnumerable<ILibraryInstallationState> libraries, CancellationToken cancellationToken)
        {
            var files = new HashSet<FileIdentifier>();

            IEnumerable<ILibraryOperationResult> results = await _librariesValidator.GetExpandedLibrariesAsync(libraries, cancellationToken);
            if (results.All(r => r.Success))
            {
                IEnumerable<ILibraryInstallationState> expandedLibraries = results.Select(r => r.InstallationState);
                foreach (ILibraryInstallationState expandedLibrary in expandedLibraries)
                {
                    IProvider provider = _dependencies.GetProvider(expandedLibrary.ProviderId);
                    HashSet<FileIdentifier> libraryFiles = GetLibraryFilesWithVersions(expandedLibrary);

                    foreach (FileIdentifier fileIdentifier in libraryFiles)
                    {
                        files.Add(fileIdentifier);
                    }
                }
            }

            return files;
        }

        private HashSet<FileIdentifier> GetLibraryFilesWithVersions(ILibraryInstallationState expandedLibrary)
        {
            var filesWithVersions = new HashSet<FileIdentifier>();

            try
            {
                IProvider provider = _dependencies.GetProvider(expandedLibrary.ProviderId);
                ILibrary library = provider.GetLibraryFromIdentifier(expandedLibrary.LibraryId);

                if (expandedLibrary.Files != null)
                {
                    filesWithVersions = new HashSet<FileIdentifier>(expandedLibrary.Files.Select(f => new FileIdentifier(Path.Combine(expandedLibrary.DestinationPath, f), library.Version)));
                }
            }
            catch { }

            return filesWithVersions;
        }

        private async Task<ILibraryOperationResult> DeleteLibraryFilesAsync(ILibraryInstallationState libraryState,
                                                                            Func<IEnumerable<string>, Task<bool>> deleteFilesFunction,
                                                                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                ILibraryOperationResult result = await _librariesValidator.GetExpandedLibraryAsync(libraryState, cancellationToken);

                if (!result.Success)
                {
                    return result;
                }

                ILibraryInstallationState desiredLibraryState = result.InstallationState;
                IProvider provider = _dependencies.GetProvider(desiredLibraryState.ProviderId);

                var filesToDelete = new List<string>();
                foreach (string file in desiredLibraryState.Files)
                {
                    var url = new Uri(file, UriKind.RelativeOrAbsolute);

                    if (!url.IsAbsoluteUri)
                    {
                        string relativePath = Path.Combine(desiredLibraryState.DestinationPath, file).Replace('\\', '/');
                        filesToDelete.Add(relativePath);
                    }
                }

                bool success = true;
                if (deleteFilesFunction != null)
                {
                    success = await deleteFilesFunction.Invoke(filesToDelete).ConfigureAwait(false);
                }

                if (success)
                {
                    return LibraryOperationResult.FromSuccess(libraryState);
                }
                else
                {
                    return LibraryOperationResult.FromError(PredefinedErrors.CouldNotDeleteLibrary(libraryState.LibraryId));
                }
            }
            catch (OperationCanceledException)
            {
                return LibraryOperationResult.FromCancelled(libraryState);
            }
            catch (Exception)
            {
                return LibraryOperationResult.FromError(PredefinedErrors.CouldNotDeleteLibrary(libraryState.LibraryId));
            }
        }
    }
}
