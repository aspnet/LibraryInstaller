﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Web.LibraryManager.Tools.Commands;

namespace Microsoft.Web.LibraryManager.Tools.Test
{
    [TestClass]
    public class LibmanInstallTest : CommandTestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public void TestInstall_CleanDirectory()
        {
            var command = new InstallCommand(HostEnvironment);
            command.Configure(null);

            int result = command.Execute("jquery@3.2.1", "--provider", "cdnjs", "--destination", "wwwroot");

            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "libman.json")));

            string text = File.ReadAllText(Path.Combine(WorkingDir, "libman.json"));
            string expectedText = @"{
  ""version"": ""1.0"",
  ""libraries"": [
    {
      ""provider"": ""cdnjs"",
      ""library"": ""jquery@3.2.1"",
      ""destination"": ""wwwroot""
    }
  ]
}";
            Assert.AreEqual(expectedText, text);
        }


        [TestMethod]
        public void TestInstall_WithExistingLibmanJson()
        {
            var command = new InstallCommand(HostEnvironment);
            command.Configure(null);

            string initialContent = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
  ]
}";

            File.WriteAllText(Path.Combine(WorkingDir, "libman.json"), initialContent);

            int result = command.Execute("jquery@3.2.1");

            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "libman.json")));

            string actualText = File.ReadAllText(Path.Combine(WorkingDir, "libman.json"));
            string expectedText = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@3.2.1""
    }
  ]
}";
            Assert.AreEqual(expectedText, actualText);
        }

        [TestMethod]
        public void TestInstall_Duplicate()
        {
            var command = new InstallCommand(HostEnvironment);
            command.Configure(null);

            string initialContent = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@3.2.1""
    }
  ]
}";

            File.WriteAllText(Path.Combine(WorkingDir, "libman.json"), initialContent);

            int result = command.Execute("jquery@3.2.1", "--provider", "cdnjs");

            string actualText = File.ReadAllText(Path.Combine(WorkingDir, "libman.json"));
            Assert.AreEqual(initialContent, actualText);

            var logger = HostEnvironment.Logger as TestLogger;

            Assert.AreEqual("Failed to install library.", logger.Messages[0].Value);
            Assert.AreEqual("[LIB010]: The \"jquery@3.2.1\" library is already installed by the \"cdnjs\" provider", logger.Messages[1].Value);
        }

        [TestMethod]
        public void TestInstall_WithExistingLibmanJson_SpecificFiles()
        {
            var command = new InstallCommand(HostEnvironment);
            command.Configure(null);

            string initialContent = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
  ]
}";

            File.WriteAllText(Path.Combine(WorkingDir, "libman.json"), initialContent);

            int result = command.Execute("jquery@3.2.1", "--files", "jquery.min.js");

            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "libman.json")));

            string actualText = File.ReadAllText(Path.Combine(WorkingDir, "libman.json"));
            string expectedText = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@3.2.1"",
      ""files"": [
        ""jquery.min.js""
      ]
    }
  ]
}";
            Assert.AreEqual(expectedText, actualText);
        }
    }
}
