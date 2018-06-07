﻿using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Web.LibraryManager.IntegrationTest
{
    [TestClass]
    public class LibmanCompletionTests : VisualStudioLibmanHostTest
    {
        ProjectItemTestExtension _libManConfig;

        [TestInitialize()]
        public void initialize()
        {
            string projectName = "TestProjectCore20";

            ProjectTestExtension webProject = Solution.ProjectsRecursive[projectName];
            _libManConfig = webProject.Find(SolutionItemFind.FileName, "libman.json");
        }

        [TestMethod]
        public void LibmanCompletion_Destination()
        {
            _libManConfig.Open();
            string[] expectedCompletionEntries = new[] {
                "Properties/",
                "wwwroot/",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown(4);
            Editor.KeyboardCommands.Type("\"destination\":");

            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true);
        }

        [TestMethod]
        public void LibmanCompletion_Provider()
        {
            _libManConfig.Open();

            string[] expectedCompletionEntries = new[] {
                "cdnjs",
                "filesystem",
                "unpkg",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown(4);
            Editor.KeyboardCommands.Type("\"provider\":");

            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true);
        }

        [TestMethod]
        public void LibmanCompletion_DefaultProvider()
        {
            _libManConfig.Open();

            string[] expectedCompletionEntries = new[] {
                "cdnjs",
                "filesystem",
                "unpkg",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown();
            Editor.Caret.MoveToEndOfLine();
            Editor.KeyboardCommands.Enter();
            Editor.KeyboardCommands.Type("\"defaultProvider\":");

            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true);
        }

        [TestMethod]
        public void LibmanCompletion_DefaultDestination()
        {
            _libManConfig.Open();
            string[] expectedCompletionEntries = new[] {
                "Properties/",
                "wwwroot/",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown();
            Editor.Caret.MoveToEndOfLine();
            Editor.KeyboardCommands.Enter();
            Editor.KeyboardCommands.Type("\"defaultDestination\":");

            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true);
        }

        [TestMethod]
        public void LibmanCompletion_LibraryForCdnjs()
        {
            _libManConfig.Open();
            string[] expectedCompletionEntries = new[] {
                "bootstrap",
                "jquery",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown(4);
            Editor.KeyboardCommands.Type("\"provider\": \"cdnjs\",");
            Editor.KeyboardCommands.Enter();

            Editor.KeyboardCommands.Type("\"library\":");
            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true, timeout: 5000);

            Editor.KeyboardCommands.Type("j");
            LibmanTestsUtility.WaitForCompletionEntryNotPresent(Editor, "bootstrap", caseInsensitive: true);

            Editor.KeyboardCommands.Type("q");
            LibmanTestsUtility.WaitForCompletionEntry(Editor, "jquery", caseInsensitive: true);
        }

        [TestMethod]
        [Ignore("Ignored for bug 628945")]
        public void LibmanCompletion_LibraryVersionForCsnjs()
        {
            _libManConfig.Open();

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown(4);
            Editor.KeyboardCommands.Type("\"provider\": \"cdnjs\",");
            Editor.KeyboardCommands.Enter();

            Editor.KeyboardCommands.Type("\"library\":");
            LibmanTestsUtility.WaitForCompletionEntry(Editor, "jquery", caseInsensitive: true, timeout: 5000);

            Editor.KeyboardCommands.Type("jquery@");
            LibmanTestsUtility.WaitForCompletionEntries(Editor, new string[] { }, caseInsensitive: true);
        }

        [TestMethod]
        public void LibmanCompletion_LibraryForFilesystem()
        {
            _libManConfig.Open();
            string[] expectedCompletionEntries = new[] {
                "Properties/",
                "wwwroot/",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown(4);
            Editor.KeyboardCommands.Type("\"provider\": \"filesystem\",");
            Editor.KeyboardCommands.Enter();

            Editor.KeyboardCommands.Type("\"library\":");
            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true);

            Editor.KeyboardCommands.Type("w");
            LibmanTestsUtility.WaitForCompletionEntryNotPresent(Editor, "Properties/", caseInsensitive: true);

            Editor.KeyboardCommands.Type("w");
            LibmanTestsUtility.WaitForCompletionEntry(Editor, "wwwroot/", caseInsensitive: true);
        }

        [TestMethod]
        [Ignore("Ignored for bug 629065")]
        public void LibmanCompletion_LibraryForUnpkg()
        {
            _libManConfig.Open();
            string[] expectedCompletionEntries = new[] {
                "bootstrap",
                "jquery",
            };

            Editor.Caret.MoveToBeginningOfFile();
            Editor.Caret.MoveDown(4);
            Editor.KeyboardCommands.Type("\"provider\": \"unpkg\",");
            Editor.KeyboardCommands.Enter();

            Editor.KeyboardCommands.Type("\"library\":");
            LibmanTestsUtility.WaitForCompletionEntries(Editor, expectedCompletionEntries, caseInsensitive: true, timeout: 5000);

            Editor.KeyboardCommands.Type("j");
            LibmanTestsUtility.WaitForCompletionEntryNotPresent(Editor, "bootstrap", caseInsensitive: true);

            Editor.KeyboardCommands.Type("q");
            LibmanTestsUtility.WaitForCompletionEntry(Editor, "jquery", caseInsensitive: true);
        }
    }
}
