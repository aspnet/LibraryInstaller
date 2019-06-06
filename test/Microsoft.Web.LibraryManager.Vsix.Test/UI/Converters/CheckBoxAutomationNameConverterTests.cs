﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Web.LibraryManager.Vsix.Resources;
using Microsoft.Web.LibraryManager.Vsix.UI.Converters;

namespace Microsoft.Web.LibraryManager.Vsix.Test.UI.Converters
{
    [TestClass]
    public class CheckBoxAutomationNameConverterTests
    {
        [TestMethod]
        public void Convert_WithNullValues_ReturnsNull()
        {
            CheckBoxAutomationNameConverter checkBoxAutomationNameConverter = new CheckBoxAutomationNameConverter();

            object result = checkBoxAutomationNameConverter.Convert(null, null, null, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void Convert_WithNulIsCheckedValue_ReturnsStringWithIndeterminateText()
        {
            CheckBoxAutomationNameConverter checkBoxAutomationNameConverter = new CheckBoxAutomationNameConverter();

            string fileName = "jquery.min.js";
            object[] values = new object[] { Text.File, fileName, null};
            object result = checkBoxAutomationNameConverter.Convert(values, null, null, null);

            string expected = string.Format(Text.Indeterminate, Text.File, fileName);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Convert_WithCheckedState_ReturnsStringWithCheckedText()
        {
            CheckBoxAutomationNameConverter checkBoxAutomationNameConverter = new CheckBoxAutomationNameConverter();

            string fileName = "jquery.min.js";
            object[] values = new object[] { Text.File, fileName, true };
            object result = checkBoxAutomationNameConverter.Convert(values, null, null, null);

            string expected = string.Format(Text.Checked, Text.File, fileName);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Convert_WithUnCheckedState_ReturnsStringWithUnCheckedText()
        {
            CheckBoxAutomationNameConverter checkBoxAutomationNameConverter = new CheckBoxAutomationNameConverter();

            string fileName = "jquery.min.js";
            object[] values = new object[] { Text.File, fileName, false };
            object result = checkBoxAutomationNameConverter.Convert(values, null, null, null);

            string expected = string.Format(Text.UnChecked, Text.File, fileName);
            Assert.AreEqual(expected, result);
        }
    }
}
