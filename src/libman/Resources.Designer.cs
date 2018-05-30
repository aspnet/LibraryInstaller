﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Web.LibraryManager.Tools {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Web.LibraryManager.Tools.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Available files are : {0}.
        /// </summary>
        internal static string AvailableFilesForLibrary {
            get {
                return ResourceManager.GetString("AvailableFilesForLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete all files from the local machine&apos;s LibMan cache..
        /// </summary>
        internal static string CacheCleanCommandDesc {
            get {
                return ResourceManager.GetString("CacheCleanCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cache cleaned..
        /// </summary>
        internal static string CacheCleanedMessage {
            get {
                return ResourceManager.GetString("CacheCleanedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to clean cache: {0}.
        /// </summary>
        internal static string CacheCleanFailed {
            get {
                return ResourceManager.GetString("CacheCleanFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider for which the cache files should be cleaned..
        /// </summary>
        internal static string CacheCleanProviderArgumentDesc {
            get {
                return ResourceManager.GetString("CacheCleanProviderArgumentDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to List or clean libman cache contents.
        /// </summary>
        internal static string CacheCommandDesc {
            get {
                return ResourceManager.GetString("CacheCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cache contents:.
        /// </summary>
        internal static string CacheContentMessage {
            get {
                return ResourceManager.GetString("CacheContentMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (empty).
        /// </summary>
        internal static string CacheEmptyMessage {
            get {
                return ResourceManager.GetString("CacheEmptyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} cache cleaned..
        /// </summary>
        internal static string CacheForProviderCleanedMessage {
            get {
                return ResourceManager.GetString("CacheForProviderCleanedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Display a list of all libraries that are stored in the local machine’s LibMan cache..
        /// </summary>
        internal static string CacheListCommandDesc {
            get {
                return ResourceManager.GetString("CacheListCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to List files that are cached for each library.
        /// </summary>
        internal static string CacheListFilesOptionDesc {
            get {
                return ResourceManager.GetString("CacheListFilesOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lists the libraries cached for each provider..
        /// </summary>
        internal static string CacheListLibrariesOptionDesc {
            get {
                return ResourceManager.GetString("CacheListLibrariesOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select an option:.
        /// </summary>
        internal static string ChooseAnOption {
            get {
                return ResourceManager.GetString("ChooseAnOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deletes all library files defined in libman.json from the project.
        /// </summary>
        internal static string CleanCommandDesc {
            get {
                return ResourceManager.GetString("CleanCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     Deletes any folders that become empty after this operation. .
        /// </summary>
        internal static string CleanCommandRemarks {
            get {
                return ResourceManager.GetString("CleanCommandRemarks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to clean some files..
        /// </summary>
        internal static string CleanFailed {
            get {
                return ResourceManager.GetString("CleanFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path, relative to the current directory, where library files should be installed if no destination is defined for a given library..
        /// </summary>
        internal static string DefaultDestinationOptionDesc {
            get {
                return ResourceManager.GetString("DefaultDestinationOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provider to use if no provider is defined for a given library. (eg. “cdnjs”, “filesystem”).
        /// </summary>
        internal static string DefaultProviderOptionDesc {
            get {
                return ResourceManager.GetString("DefaultProviderOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Location to install the library (if not specified, the default destination location will be used).
        /// </summary>
        internal static string DestinationOptionDesc {
            get {
                return ResourceManager.GetString("DestinationOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default destination is not set. Please provide a destination using &apos;--destination&apos;.
        /// </summary>
        internal static string DestinationRequiredWhenNoDefaultIsPresent {
            get {
                return ResourceManager.GetString("DestinationRequiredWhenNoDefaultIsPresent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory not found {0}.
        /// </summary>
        internal static string DirectoryNotFoundMessage {
            get {
                return ResourceManager.GetString("DirectoryNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Examples:.
        /// </summary>
        internal static string ExamplesHeader {
            get {
                return ResourceManager.GetString("ExamplesHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to restore &quot;{0}&quot;.
        /// </summary>
        internal static string FailedToRestoreLibraryMessage {
            get {
                return ResourceManager.GetString("FailedToRestoreLibraryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} deleted.
        /// </summary>
        internal static string FileDeleted {
            get {
                return ResourceManager.GetString("FileDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to delete {0}.
        /// </summary>
        internal static string FileDeleteFail {
            get {
                return ResourceManager.GetString("FileDeleteFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The files from the specified library to install (if not specified, all files from the library will be installed).
        /// </summary>
        internal static string FilesOptionDesc {
            get {
                return ResourceManager.GetString("FilesOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} written to disk.
        /// </summary>
        internal static string FileWrittenToDisk {
            get {
                return ResourceManager.GetString("FileWrittenToDisk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please fix the libman.json file and try again.
        /// </summary>
        internal static string FixManifestFile {
            get {
                return ResourceManager.GetString("FixManifestFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create a new libman.json.
        /// </summary>
        internal static string InitCommandDesc {
            get {
                return ResourceManager.GetString("InitCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to init: A &apos;libman.json&apos; already exists.
        /// </summary>
        internal static string InitFailedLibmanJsonFileExists {
            get {
                return ResourceManager.GetString("InitFailedLibmanJsonFileExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add a library definition to the LibMan.json file, and download the library to the specified location.
        /// </summary>
        internal static string InstallCommandDesc {
            get {
                return ResourceManager.GetString("InstallCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     libman install jquery@3.2.1 
        ///    libman install jquery --provider cdnjs --destination wwwroot\scripts\jquery --files jquery.min.js 
        ///    libman install myCalendar --provider filesystem --files calendar.js --files calendar.css.
        /// </summary>
        internal static string InstallCommandExamples {
            get {
                return ResourceManager.GetString("InstallCommandExamples", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library to install.
        /// </summary>
        internal static string InstallCommandLibraryIdArgumentDesc {
            get {
                return ResourceManager.GetString("InstallCommandLibraryIdArgumentDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     CDNJS libraries have a library ID format of “&lt;libraryName&gt;@&lt;libraryVersion&gt;”
        ///    For CDNJS libraries, if no version is specified in the library ID, the highest version available is used..
        /// </summary>
        internal static string InstallCommandProviderSpecificLogic {
            get {
                return ResourceManager.GetString("InstallCommandProviderSpecificLogic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     Initializes a libman.json if one does not exist
        ///    If no default provider exists, --provider option is required
        ///    If no default destination exists, --destination option is required
        ///    If no files are specified, the entire library is included.
        /// </summary>
        internal static string InstallCommandRemarks {
            get {
                return ResourceManager.GetString("InstallCommandRemarks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Installed library &quot;{0}&quot; to &quot;{1}&quot;.
        /// </summary>
        internal static string InstalledLibrary {
            get {
                return ResourceManager.GetString("InstalledLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to install library &quot;{0}&quot;.
        /// </summary>
        internal static string InstallLibraryFailed {
            get {
                return ResourceManager.GetString("InstallLibraryFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid arguments for command: {0}.
        /// </summary>
        internal static string InvalidArgumentsMessage {
            get {
                return ResourceManager.GetString("InvalidArgumentsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library &quot;{0}&quot; does not contain the following files: {1}.
        /// </summary>
        internal static string InvalidFilesForLibrary {
            get {
                return ResourceManager.GetString("InvalidFilesForLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; is not a valid value for --to-version.
        /// </summary>
        internal static string InvalidToVersion {
            get {
                return ResourceManager.GetString("InvalidToVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library &quot;{0}&quot; is already up to date.
        /// </summary>
        internal static string LatestVersionAlreadyInstalled {
            get {
                return ResourceManager.GetString("LatestVersionAlreadyInstalled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library Manager.
        /// </summary>
        internal static string LibmanCommandDesc {
            get {
                return ResourceManager.GetString("LibmanCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to libman.json was not found:{0}.
        /// </summary>
        internal static string LibmanJsonNotFound {
            get {
                return ResourceManager.GetString("LibmanJsonNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library &quot;{0}&quot; cannot be installed. &quot;{1}&quot; is already installed at &quot;{2}&quot;.
        ///Please specify a different destination..
        /// </summary>
        internal static string LibraryCannotBeInstalledDueToConflictingLibraries {
            get {
                return ResourceManager.GetString("LibraryCannotBeInstalledDueToConflictingLibraries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LibraryId is required to install.
        /// </summary>
        internal static string LibraryIdRequiredForInstall {
            get {
                return ResourceManager.GetString("LibraryIdRequiredForInstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LibraryId is required to uninstall.
        /// </summary>
        internal static string LibraryIdRequiredForUnInstall {
            get {
                return ResourceManager.GetString("LibraryIdRequiredForUnInstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LibraryId is required for update.
        /// </summary>
        internal static string LibraryIdRequiredForUpdate {
            get {
                return ResourceManager.GetString("LibraryIdRequiredForUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one library found with id &quot;{0}&quot;.
        /// </summary>
        internal static string MoreThanOneLibraryFoundToUninstall {
            get {
                return ResourceManager.GetString("MoreThanOneLibraryFoundToUninstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one library found with id &quot;{0}&quot; for provider &quot;{1}&quot;. Please remove the manually from the libman.json file.
        /// </summary>
        internal static string MoreThanOneLibraryFoundToUninstallForProvider {
            get {
                return ResourceManager.GetString("MoreThanOneLibraryFoundToUninstallForProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one library found with id &quot;{0}&quot;.
        /// </summary>
        internal static string MoreThanOneLibraryFoundToUpdate {
            get {
                return ResourceManager.GetString("MoreThanOneLibraryFoundToUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No libraries to update. Please use install command to install libraries.
        /// </summary>
        internal static string NoLibrariesToUpdate {
            get {
                return ResourceManager.GetString("NoLibrariesToUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No library found with id &quot;{0}&quot; to update.
        /// </summary>
        internal static string NoLibraryFoundToUpdate {
            get {
                return ResourceManager.GetString("NoLibraryFoundToUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library &quot;{0}&quot; is not installed. Nothing to uninstall.
        /// </summary>
        internal static string NoLibraryToUninstall {
            get {
                return ResourceManager.GetString("NoLibraryToUninstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path to the project (Current directory is used as default).
        /// </summary>
        internal static string ProjectPathOptionDesc {
            get {
                return ResourceManager.GetString("ProjectPathOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider &quot;{0}&quot; is not installed.
        /// </summary>
        internal static string ProviderNotInstalled {
            get {
                return ResourceManager.GetString("ProviderNotInstalled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider to use (if not specified, the default provider will be used).
        /// </summary>
        internal static string ProviderOptionDesc {
            get {
                return ResourceManager.GetString("ProviderOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default Provider is not set. Please provide a provider using &apos;--provider&apos;.
        /// </summary>
        internal static string ProviderRequiredWhenNoDefaultIsPresent {
            get {
                return ResourceManager.GetString("ProviderRequiredWhenNoDefaultIsPresent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider-specific logic:.
        /// </summary>
        internal static string ProviderSpecificLogicHeader {
            get {
                return ResourceManager.GetString("ProviderSpecificLogicHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remarks:.
        /// </summary>
        internal static string RemarksHeader {
            get {
                return ResourceManager.GetString("RemarksHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Downloads all files from provider and saves them to specified destination.
        /// </summary>
        internal static string RestoreCommandDesc {
            get {
                return ResourceManager.GetString("RestoreCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     Error if no libman.json in current folder
        ///    If a library specifies a provider, it will override the defaultProvider
        ///    If a library specifies a destination, it will override the defaultDestination.
        /// </summary>
        internal static string RestoreCommandRemarks {
            get {
                return ResourceManager.GetString("RestoreCommandRemarks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please specify a different destination..
        /// </summary>
        internal static string SpecifyDifferentDestination {
            get {
                return ResourceManager.GetString("SpecifyDifferentDestination", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Similar libraries: .
        /// </summary>
        internal static string SuggestedIdsMessage {
            get {
                return ResourceManager.GetString("SuggestedIdsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deletes all files for the specified library from their specified destination, then removess the specified library definition from libman.json.
        /// </summary>
        internal static string UnInstallCommandDesc {
            get {
                return ResourceManager.GetString("UnInstallCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     libman uninstall jquery
        ///    libman uninstall jquery@3.3.1.
        /// </summary>
        internal static string UnInstallCommandExamples {
            get {
                return ResourceManager.GetString("UnInstallCommandExamples", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library to uninstall.
        /// </summary>
        internal static string UninstallCommandLibraryIdArgumentDesc {
            get {
                return ResourceManager.GetString("UninstallCommandLibraryIdArgumentDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provider of the library to be uninstalled.
        /// </summary>
        internal static string UninstallCommandProviderOptionDesc {
            get {
                return ResourceManager.GetString("UninstallCommandProviderOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     CDNJS libraries have a library ID format of “&lt;libraryName&gt;@&lt;libraryVersion&gt;”
        ///    If no version is specified in the library ID, this command will act on any library with libraryName matching the given libraryId.
        /// </summary>
        internal static string UnInstallCommandProviderSpecificLogic {
            get {
                return ResourceManager.GetString("UnInstallCommandProviderSpecificLogic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     Error if no libman.json in current folder
        ///    Error if specified library doesn&apos;t exist
        ///    If there&apos;s more than one library with the same libraryId, you&apos;ll be prompted to choose..
        /// </summary>
        internal static string UnInstallCommandRemarks {
            get {
                return ResourceManager.GetString("UnInstallCommandRemarks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uninstalled library &quot;{0}&quot;.
        /// </summary>
        internal static string UninstalledLibrary {
            get {
                return ResourceManager.GetString("UninstalledLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to uninstall &quot;{0}&quot;.
        /// </summary>
        internal static string UninstallFailed {
            get {
                return ResourceManager.GetString("UninstallFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Updates the specified library.
        /// </summary>
        internal static string UpdateCommandDesc {
            get {
                return ResourceManager.GetString("UpdateCommandDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     libman update jquery 
        ///    libman update jquery --to jquery@3.3.1
        ///    libman update jquery@2.2.0
        ///    libman update jquery -pre 
        ///.
        /// </summary>
        internal static string UpdateCommandExamples {
            get {
                return ResourceManager.GetString("UpdateCommandExamples", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Library to update.
        /// </summary>
        internal static string UpdateCommandLibraryArgumentDesc {
            get {
                return ResourceManager.GetString("UpdateCommandLibraryArgumentDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If specified, the latest pre-release version of the library will be downloaded (where applicable).
        /// </summary>
        internal static string UpdateCommandPreReleaseOptionDesc {
            get {
                return ResourceManager.GetString("UpdateCommandPreReleaseOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If specified, only libraries from the specified provider will be updated.
        /// </summary>
        internal static string UpdateCommandProviderOptionDesc {
            get {
                return ResourceManager.GetString("UpdateCommandProviderOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     Error if no libman.json in current folder
        ///    Error if specified library doesn&apos;t exist
        ///    If there&apos;s more than one library with the same libraryId, you&apos;ll be prompted to choose..
        /// </summary>
        internal static string UpdateCommandRemarks {
            get {
                return ResourceManager.GetString("UpdateCommandRemarks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The version to update the library to (needs complete libraryid for the provider).
        /// </summary>
        internal static string UpdateCommandToVersionOptionDesc {
            get {
                return ResourceManager.GetString("UpdateCommandToVersionOptionDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use --provider to disambiguate libraries of same name from different providers in the project.
        /// </summary>
        internal static string UseProviderToDisambiguateMessage {
            get {
                return ResourceManager.GetString("UseProviderToDisambiguateMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Set the verbosity of output (eg. &quot;normal&quot;, &quot;detailed&quot;, &quot;quiet&quot;).
        /// </summary>
        internal static string VerbosityOptionDesc {
            get {
                return ResourceManager.GetString("VerbosityOptionDesc", resourceCulture);
            }
        }
    }
}
