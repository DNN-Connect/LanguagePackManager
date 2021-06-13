using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Github;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Scheduling;
using System;
using System.Text;

namespace Connect.LanguagePackManager.Core
{
    public class CheckGithubTask : SchedulerClient
    {
        public CheckGithubTask(ScheduleHistoryItem history)
        {
            this.ScheduleHistoryItem = history;
        }

        public StringBuilder Log { get; set; } = new StringBuilder();
        public DateTime Start { get; private set; } = DateTime.Now;

        public override void DoWork()
        {
            try
            {
                var links = PackageLinkRepository.Instance.GetPackageLinks();
                foreach (var link in links)
                {
                    AddLogLine($"Checking {link.Name}");
                    GithubController.CheckPackage(link);
                    AddLogLine($"Finished checking {link.Name}");
                }
                ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote(Log.ToString().Replace(Environment.NewLine, "<br />"));
            }
            catch (Exception ex)
            {
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote($"Failed: {ex.Message} ({ex.StackTrace}) <br />{Log.ToString().Replace(Environment.NewLine, "<br />")}");
                Errored(ref ex);
                Exceptions.LogException(ex);
            }
        }

        private void AddLogLine(string line)
        {
            Log.AppendLine($"{Start.ToString("HH:mm:ss")} {line}");
        }
    }
}
