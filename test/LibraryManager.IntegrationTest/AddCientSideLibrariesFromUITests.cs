﻿using System.IO;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Web.LibraryManager.IntegrationTest.Services;

namespace Microsoft.Web.LibraryManager.IntegrationTest
{
    [TestClass]
    public class AddCientSideLibrariesFromUITests : VisualStudioLibmanHostTest
    {
        private ProjectTestExtension _webProject;
        const string _projectName = @"TestProjectCore20";
        const string _libman = "libman.json";
        private string _initialLibmanFileContent;
        private string _pathToLibmanFile;

        protected override void DoHostTestInitialize()
        {
            base.DoHostTestInitialize();

            _webProject = Solution[_projectName];
            ProjectItemTestExtension libmanConfig = _webProject[_libman];
            _pathToLibmanFile = Path.Combine(SolutionRootPath, _projectName, _libman);
            _initialLibmanFileContent = File.ReadAllText(_pathToLibmanFile);

            string libmanConfigFullPath = libmanConfig.FullPath;

            if (File.Exists(libmanConfigFullPath))
            {
                string projectPath = Path.Combine(SolutionRootPath, _projectName);
                libmanConfig.Delete();
                Helpers.FileIO.WaitForDeletedFile(projectPath, libmanConfigFullPath, caseInsensitive: false);
            }
        }

        protected override void DoHostTestCleanup()
        {
            ProjectItemTestExtension libManConfig = _webProject[_libman];

            if (libManConfig != null)
            {
                libManConfig.Open();

                Editor.Selection.SelectAll();
                Editor.KeyboardCommands.Delete();
                Editor.Edit.InsertTextInBuffer(_initialLibmanFileContent);

                libManConfig.Save();
            }

            base.DoHostTestCleanup();
        }

        [TestMethod]
        public void InstallClientSideLibraries_FromProjectRoot_SmokeTest()
        {
            SetLibraryAndClickInstall(_projectName, "jquery-validate@1.17.0");

            string pathToLibrary = Path.Combine(SolutionRootPath, _projectName, "wwwroot", "lib", "jquery-validate");
            string[] expectedFiles = new[]
            {
                Path.Combine(pathToLibrary, "jquery.validate.js"),
                Path.Combine(pathToLibrary, "localization", "messages_ar.js"),
            };

            string manifestContents = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""libraries"": [
    {
      ""library"": ""jquery-validate@1.17.0"",
      ""destination"": ""wwwroot/lib/jquery-validate/""
    }
  ]
}";
            Helpers.FileIO.WaitForRestoredFiles(pathToLibrary, expectedFiles, caseInsensitive: true, timeout: 20000);
            Assert.AreEqual(manifestContents, File.ReadAllText(_pathToLibmanFile));
        }

        [TestMethod]
        public void InstallClientSideLibraries_FromFolder_SmokeTest()
        {
            SetLibraryAndClickInstall("wwwroot", "jquery-validate@1.17.0");

            string pathToLibrary = Path.Combine(SolutionRootPath, _projectName, "wwwroot", "jquery-validate");
            string[] expectedFiles = new[]
            {
                Path.Combine(pathToLibrary, "jquery.validate.js"),
                Path.Combine(pathToLibrary, "localization", "messages_ar.js"),
            };

            string manifestContents = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""libraries"": [
    {
      ""library"": ""jquery-validate@1.17.0"",
      ""destination"": ""wwwroot/jquery-validate/""
    }
  ]
}";
            Helpers.FileIO.WaitForRestoredFiles(pathToLibrary, expectedFiles, caseInsensitive: true, timeout: 20000);
            Assert.AreEqual(manifestContents, File.ReadAllText(_pathToLibmanFile));
        }

        private void SetLibraryAndClickInstall(string nodeName, string library)
        {
            SolutionExplorer.Select(nodeName);
            InstallDialogTestService installDialogTestService = VisualStudio.Get<InstallDialogTestService>();
            InstallDialogTestExtension installDialogTestExtenstion = installDialogTestService.OpenDialog();

            installDialogTestExtenstion.SetLibrary(library);
            installDialogTestExtenstion.WaitForFileSelections();
            installDialogTestExtenstion.ClickInstall();
        }
    }
}