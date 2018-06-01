﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.LibraryManager.Contracts;

namespace Microsoft.Web.LibraryManager.Vsix.Contracts
{
    internal class PerProjectLogger : ILogger
    {
        private string _configFileName;
        private string _projectName;

        private string ProjectName
        {
            get
            {
                if (string.IsNullOrEmpty(_projectName))
                {
                    string projectName = VsHelpers.GetDTEProjectFromConfig(_configFileName)?.Name;
                    _projectName = string.IsNullOrEmpty(projectName) ? string.Empty : $" ({projectName})";
                }

                return _projectName;
            }
        }

        public PerProjectLogger(string configFileName)
        {
            _configFileName = configFileName;
        }

        public void Log(string message, LogLevel level)
        {
            Logger.LogEvent($"{message}{ProjectName}", level);
        }


        public void LogOperationSummary(IEnumerable<ILibraryInstallationResult> results, OperationType operation, TimeSpan elapsedTime)
        {
            int totalResultsCounts = results.Count();
            IEnumerable<ILibraryInstallationResult> successfulRestores = results.Where(r => r.Success);
            IEnumerable<ILibraryInstallationResult> failedRestores = results.Where(r => !r.Success);
            IEnumerable<ILibraryInstallationResult> cancelledRestores = results.Where(r => r.Cancelled);
            IEnumerable<ILibraryInstallationResult> upToDateRestores = results.Where(r => r.UpToDate);

            bool allSuccess = successfulRestores.Count() == totalResultsCounts;
            bool allFailed = failedRestores.Count() == totalResultsCounts;
            bool allCancelled = cancelledRestores.Count() == totalResultsCounts;
            bool allUpToDate = upToDateRestores.Count() == totalResultsCounts;
            bool partialSuccess = successfulRestores.Count() < totalResultsCounts;

            Logger.LogEvent(Resources.Text.RestoreLibrariesCompleted, LogLevel.Status);

            if (allUpToDate)
            {
                Logger.LogEvent(LibraryManager.Resources.Text.LibraryRestoredNoChange + Environment.NewLine, LogLevel.Operation);
            }
            else if (allSuccess)
            {
                string successText = string.Format(LibraryManager.Resources.Text.LibrariesRestored, totalResultsCounts, Math.Round(elapsedTime.TotalSeconds, 2));
                Logger.LogEvent(successText + Environment.NewLine, LogLevel.Operation);
            }
            else if (allCancelled)
            {
                string canceledText = string.Format(LibraryManager.Resources.Text.LibraryRestorationCancelled, totalResultsCounts, Math.Round(elapsedTime.TotalSeconds, 2));
                Logger.LogEvent(canceledText + Environment.NewLine, LogLevel.Operation);
            }
            else if (allFailed)
            {
                string failedText = string.Format(LibraryManager.Resources.Text.LibraryRestorationFailed, totalResultsCounts, Math.Round(elapsedTime.TotalSeconds, 2));
                Logger.LogEvent(failedText + Environment.NewLine, LogLevel.Operation);
            }
            else
            {
                var summarySuccessText = string.Format(Resources.Text.LibrariesRestoredSuccessfullySummary, successfulRestores.Count());
                Logger.LogEvent(summarySuccessText + Environment.NewLine, LogLevel.Operation);
                foreach (var result in successfulRestores)
                {
                    var successText = string.Format(Resources.Text.LibraryRestoredSuccessMessage, result.InstallationState.LibraryId);
                    Logger.LogEvent(successText, LogLevel.Operation);
                }

                if (failedRestores.Any())
                {
                    var summaryErrorText = string.Format(Resources.Text.LibrariesRestoreFailedSummary, failedRestores.Count());
                    Logger.LogEvent(Environment.NewLine + summaryErrorText + Environment.NewLine, LogLevel.Operation);
                    foreach (var result in failedRestores)
                    {
                        var errorText = string.Format(Resources.Text.LibraryRestoreFailedMessage, result.InstallationState.LibraryId);
                        Logger.LogEvent(errorText, LogLevel.Operation);
                    }
                }

                if (cancelledRestores.Any())
                {
                    var summaryCancellationText = string.Format(Resources.Text.LibrariesRestoreCancelledSummary, cancelledRestores.Count());
                    Logger.LogEvent(Environment.NewLine + summaryCancellationText + Environment.NewLine, LogLevel.Operation);
                    foreach (var result in cancelledRestores)
                    {
                        var cancellationText = string.Format(Resources.Text.LibraryRestoreCancelledMessage, result.InstallationState.LibraryId);
                        Logger.LogEvent(cancellationText, LogLevel.Operation);
                    }
                }
            }

            Logger.LogEvent(string.Format(Resources.Text.ElapsedTimeMessage, elapsedTime), LogLevel.Operation);
            Logger.LogEvent(Resources.Text.OperationFinishedMessage + Environment.NewLine, LogLevel.Operation);
        }
    }
}
