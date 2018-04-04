﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Microsoft.Web.LibraryManager.Vsix
{
    internal static class VsHelpers
    {
        private static IComponentModel _compositionService;

        public static DTE2 DTE { get; } = GetService<DTE, DTE2>();

        public static TReturnType GetService<TServiceType, TReturnType>()
        {
            return (TReturnType)ServiceProvider.GlobalProvider.GetService(typeof(TServiceType));
        }

        public static string GetFileInVsix(string relativePath)
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(folder, relativePath);
        }

        public static bool IsConfigFile(this ProjectItem item)
        {
            return item.Name.Equals(Constants.ConfigFileName, StringComparison.OrdinalIgnoreCase);
        }

        public static void CheckFileOutOfSourceControl(string file)
        {
            if (!File.Exists(file) || DTE.Solution.FindProjectItem(file) == null)
                return;

            if (DTE.SourceControl.IsItemUnderSCC(file) && !DTE.SourceControl.IsItemCheckedOut(file))
                DTE.SourceControl.CheckOutItem(file);

            var info = new FileInfo(file)
            {
                IsReadOnly = false
            };
        }

        public static void AddFileToProject(this Project project, string file, string itemType = null)
        {
            if (project.IsKind(ProjectTypes.ASPNET_5, ProjectTypes.DOTNET_Core, ProjectTypes.SSDT))
                return;

            try
            {
                if (DTE.Solution.FindProjectItem(file) == null)
                {
                    ProjectItem item = project.ProjectItems.AddFromFile(file);

                    if (string.IsNullOrEmpty(itemType)
                        || project.IsKind(ProjectTypes.WEBSITE_PROJECT)
                        || project.IsKind(ProjectTypes.UNIVERSAL_APP))
                    {
                        return;
                    }

                    item.Properties.Item("ItemType").Value = "None";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                // TODO: Implement logging
            }
        }

        public static void AddFilesToProject(this Project project, IEnumerable<string> files)
        {
            if (project == null || project.IsKind(ProjectTypes.ASPNET_5, ProjectTypes.DOTNET_Core, ProjectTypes.SSDT))
                return;

            if (project.IsKind(ProjectTypes.WEBSITE_PROJECT))
            {
                Command command = DTE.Commands.Item("SolutionExplorer.Refresh");

                if (command.IsAvailable)
                    DTE.ExecuteCommand(command.Name);

                return;
            }

            var solutionService = Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;

            IVsHierarchy hierarchy = null;
            solutionService?.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

            if (hierarchy == null)
                return;

            var ip = (IVsProject)hierarchy;
            VSADDRESULT[] result = new VSADDRESULT[files.Count()];

            ip.AddItem(VSConstants.VSITEMID_ROOT,
                       VSADDITEMOPERATION.VSADDITEMOP_LINKTOFILE,
                       string.Empty,
                       (uint)files.Count(),
                       files.ToArray(),
                       IntPtr.Zero,
                       result);
        }

        public static string GetRootFolder(this Project project)
        {
            if (project == null)
                return null;

            if (project.IsKind(ProjectKinds.vsProjectKindSolutionFolder))
                return Path.GetDirectoryName(DTE.Solution.FullName);

            if (string.IsNullOrEmpty(project.FullName))
                return null;

            string fullPath;

            try
            {
                fullPath = project.Properties.Item("FullPath").Value as string;
            }
            catch (ArgumentException)
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    fullPath = project.Properties.Item("ProjectDirectory").Value as string;
                }
                catch (ArgumentException)
                {
                    // Installer projects have a ProjectPath.
                    fullPath = project.Properties.Item("ProjectPath").Value as string;
                }
            }

            if (string.IsNullOrEmpty(fullPath))
                return File.Exists(project.FullName) ? Path.GetDirectoryName(project.FullName) : null;

            if (Directory.Exists(fullPath))
                return fullPath;

            if (File.Exists(fullPath))
                return Path.GetDirectoryName(fullPath);

            return null;
        }

        public static bool IsKind(this Project project, params string[] kindGuids)
        {
            foreach (string guid in kindGuids)
            {
                if (project.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static void SatisfyImportsOnce(this object o)
        {
            _compositionService = _compositionService ?? GetService<SComponentModel, IComponentModel>();

            if (_compositionService != null)
            {
                _compositionService.DefaultCompositionService.SatisfyImportsOnce(o);
            }
        }

        public static bool ProjectContainsManifestFile(Project project)
        {
            string configFilePath = Path.Combine(GetRootFolder(project), Constants.ConfigFileName);

            if (File.Exists(configFilePath))
            {
                return true;
            }

            return false;
        }

        public static bool SolutionContainsManifestFile(IVsSolution solution)
        {
            IEnumerable<IVsHierarchy> hierarchies = GetProjectsInSolution(solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);

            foreach (IVsHierarchy hierarchy in hierarchies)
            {
                Project project = GetDTEProject(hierarchy);

                if (project != null && ProjectContainsManifestFile(project))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution, __VSENUMPROJFLAGS flags)
        {
            if (solution == null)
                yield break;

            Guid guid = Guid.Empty;
            if (ErrorHandler.Failed(solution.GetProjectEnum((uint)flags, ref guid, out IEnumHierarchies enumHierarchies)) || enumHierarchies == null)
            {
                yield break;
            }

            IVsHierarchy[] hierarchy = new IVsHierarchy[1];
            while (enumHierarchies.Next(1, hierarchy, out uint fetched) == VSConstants.S_OK && fetched == 1)
            {
                if (hierarchy.Length > 0 && hierarchy[0] != null)
                    yield return hierarchy[0];
            }
        }

        public static Project GetDTEProject(IVsHierarchy hierarchy)
        {
            if (ErrorHandler.Succeeded(hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out object obj)))
            {
                return obj as Project;
            }

            return null;
        }

        public static string GetProjectName(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                return null;
            }

            string projectName = null;
            IVsHierarchy hierarchy = null;
            uint vsItemID = (uint)VSConstants.VSITEMID.Nil;

            if (TryGetHierarchy(filePath, out hierarchy, out vsItemID))
            {
                object propertyObject;

                int hr = hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out propertyObject);

                if (hr == VSConstants.S_OK)
                {
                    projectName = propertyObject as string;
                }
            }

            return projectName;
        }

        public static bool TryGetHierarchy(string filePath, out IVsHierarchy vsHierarchy, out uint vsItemId)
        {
            bool result = true;

            vsHierarchy = null;
            vsItemId = (uint)VSConstants.VSITEMID.Nil;

            IVsUIShellOpenDocument vsUIShellOpenDocument = ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;

            IOleServiceProvider serviceProviderUnused = null;
            int docInProject = 0;
            IVsUIHierarchy uiHier = null;

            int hr = vsUIShellOpenDocument.IsDocumentInAProject(filePath, out uiHier, out vsItemId, out serviceProviderUnused, out docInProject);
            if (ErrorHandler.Succeeded(hr) && uiHier != null)
            {
                vsHierarchy = uiHier;
            }
            else
            {
                vsHierarchy = null;
                vsItemId = (uint)VSConstants.VSITEMID.Nil;
                result = false;
            }

            return result;
        }
    }

    public static class ProjectTypes
    {
        public const string ASPNET_5 = "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}";
        public const string DOTNET_Core = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
        public const string MISC = "{66A2671D-8FB5-11D2-AA7E-00C04F688DDE}";
        public const string NODE_JS = "{9092AA53-FB77-4645-B42D-1CCCA6BD08BD}";
        public const string SOLUTION_FOLDER = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
        public const string SSDT = "{00d1a9c2-b5f0-4af3-8072-f6c62b433612}";
        public const string UNIVERSAL_APP = "{262852C6-CD72-467D-83FE-5EEB1973A190}";
        public const string WAP = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
        public const string WEBSITE_PROJECT = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
    }
}
