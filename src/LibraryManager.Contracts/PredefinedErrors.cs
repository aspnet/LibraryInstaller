﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.LibraryManager.Contracts.Resources;

namespace Microsoft.Web.LibraryManager.Contracts
{
    /// <summary>
    /// A list of predefined errors any <see cref="IProvider"/> can use.
    /// </summary>
    /// <remarks>
    /// It is recommended to use these errors where applicable to create a
    /// uniform experience across providers.
    /// </remarks>
    public static class PredefinedErrors
    {
        /// <summary>
        /// Represents an unhandled exception that occured in the provider.
        /// </summary>
        /// <remarks>
        /// An <see cref="IProvider.InstallAsync"/> should never throw and this error
        /// should be used as when catching generic exeptions.
        /// </remarks>
        /// <returns>The error code LIB000</returns>
        public static IError UnknownException()
            => new Error("LIB000", Text.ErrorUnknownException);

        /// <summary>
        /// The specified provider is unknown to the host.
        /// </summary>
        /// <param name="providerId">The unique ID of the <see cref="IProvider"/>.</param>
        /// <returns>The error code LIB001</returns>
        public static IError ProviderUnknown(string providerId)
            => new Error("LIB001", string.Format(Text.ErrorProviderUnknown, providerId));

        /// <summary>
        /// The <see cref="IProvider"/> is unable to resolve the source.
        /// </summary>
        /// <param name="libraryId">The ID of the library that could not be resolved.</param>
        /// <param name="providerId">The ID of the <see cref="IProvider"/> that could not resolve the resource.</param>
        /// <returns>The error code LIB002</returns>
        public static IError UnableToResolveSource(string libraryId, string providerId)
            => new Error("LIB002", string.Format(Text.ErrorUnableToResolveSource, libraryId, providerId));

        /// <summary>
        /// The <see cref="IProvider"/> failed to write a file in the <see cref="ILibraryInstallationState.Files"/> array.
        /// </summary>
        /// <param name="file">The file name that failed to be written to disk.</param>
        /// <returns>The error code LIB003</returns>
        public static IError CouldNotWriteFile(string file)
            => new Error("LIB003", string.Format(Text.ErrorCouldNotWriteFile, file));

        /// <summary>
        /// The manifest JSON file is malformed.
        /// </summary>
        /// <returns>The error code LIB004</returns>
        public static IError ManifestMalformed()
           => new Error("LIB004", string.Format(Text.ErrorManifestMalformed));

        /// <summary>
        /// The relative path is undefined.
        /// </summary>
        /// <returns>The error code LIB005</returns>
        public static IError PathIsUndefined()
           => new Error("LIB005", string.Format(Text.ErrorPathIsUndefined));

        /// <summary>
        /// The library id is undefined.
        /// </summary>
        /// <returns>The error code LIB006</returns>
        public static IError LibraryIdIsUndefined()
           => new Error("LIB006", string.Format(Text.ErrorLibraryIdIsUndefined));

        /// <summary>
        /// The provider is undefined.
        /// </summary>
        /// <returns>The error code LIB007</returns>
        public static IError ProviderIsUndefined()
           => new Error("LIB007", string.Format(Text.ErrorProviderIsUndefined));

        /// <summary>
        /// The "path" must be inside the working directory
        /// </summary>
        /// <returns>The error code LIB008</returns>
        public static IError PathOutsideWorkingDirectory()
           => new Error("LIB008", string.Format(Text.ErrorPathOutsideWorkingDirectory));

        /// <summary>
        /// The "version" is not supported
        /// </summary>
        /// <returns>The error code LIB009</returns>
        public static IError VersionIsNotSupported(string version)
           => new Error("LIB009", string.Format(Text.ErrorNotSupportedVersion, version));

        /// <summary>
        /// Library is already installed by the provider.
        /// </summary>
        /// <param name="libraryId"></param>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public static IError LibraryAlreadyInstalled(string libraryId, string providerId)
            => new Error("LIB010", string.Format(Text.ErrorLibraryAlreadyInstalled, libraryId, providerId));

        /// <summary>
        /// Library cannot be updated as updated version is already installed.
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        public static IError CouldNotUpdateDueToConflicts(string oldId, string newId)
            => new Error("LIB011", string.Format(Text.ErrorLibraryCannotUpdateDueToConflicts, oldId, newId));

        /// <summary>
        /// Library cannot be updated as new version does not have specified files.
        /// </summary>
        /// <param name="libraryId"></param>
        /// <param name="newId"></param>
        /// <param name="invalidFiles"></param>
        /// <returns></returns>
        public static IError CouldNotUpdateDueToFileConflicts(string libraryId, string newId, IReadOnlyList<string> invalidFiles)
            => new Error("LIB012", string.Format(Text.ErrorLibraryCannotUpdateDueToFileConflicts, libraryId, newId, string.Join(", ", invalidFiles)));

        /// <summary>
        /// Restore errors due to conflicting libraries.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="conflictingLibraryIds"></param>
        /// <returns></returns>
        public static IError ConflictingLibrariesInManifest(string file, IReadOnlyList<string> conflictingLibraryIds)
            => new Error("LIB013", string.Format(Text.ErrorConflictingLibraries, file, string.Join(", ", conflictingLibraryIds)));

        /// <summary>
        /// Library cannot be installed as conflicting libraries are installed.
        /// </summary>
        /// <param name="libraryId"></param>
        /// <param name="conflictingLibraries"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IError LibraryCannotBeInstalledDueToConflicts(string file, List<string> conflictingLibraries)
        => new Error("LIB014", string.Format(Text.ErrorLibraryCannotInstallDueToConflicts, file, string.Join(", ", conflictingLibraries)));
    }
}
