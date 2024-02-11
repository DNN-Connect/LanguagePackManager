using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Github;
using DotNetNuke.Instrumentation;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Scheduling;
using System;

namespace Connect.LanguagePackManager.Core
{
  public class CheckGithubTask : SchedulerClient
  {
    private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(CheckGithubTask));

    public CheckGithubTask(ScheduleHistoryItem history)
    {
      this.ScheduleHistoryItem = history;
    }

    public override void DoWork()
    {
      try
      {
        AddLogLine($"Cleaning up temp folder");
        Common.Globals.CleanupTempFolder();

        var links = PackageLinkRepository.Instance.GetPackageLinks();
        foreach (var link in links)
        {
          AddLogLine($"Checking {link.Name}");
          try
          {
            if (link.IsResourcesRepo)
            {
              GithubController.CheckResourcesRepo(link);
            }
            else
            {
              GithubController.CheckPackage(link);
            }
            AddLogLine($"Finished checking {link.Name}");
          }
          catch (Exception ex)
          {
            Logger.Error(ex);
            AddLogLine($"Error checking {link.Name}, check log for details - continuing with other packages");
          }
        }

        TextRepository.Instance.RefreshNrTexts();
        AddLogLine($"Refreshed Nr Texts");

        ScheduleHistoryItem.Succeeded = true;
      }
      catch (Exception ex)
      {
        ScheduleHistoryItem.Succeeded = false;
        ScheduleHistoryItem.AddLogNote($"Failed: {ex.Message} ({ex.StackTrace}) <br />");
        Logger.Error(ex);
        Errored(ref ex);
        Exceptions.LogException(ex);
      }
    }

    private void AddLogLine(string line)
    {
      ScheduleHistoryItem.AddLogNote(line + "<br />");
      Logger.Info(line);
    }
  }
}
