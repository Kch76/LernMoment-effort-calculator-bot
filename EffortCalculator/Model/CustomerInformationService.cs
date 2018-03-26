using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator.Model
{
    class CustomerInformationService
    {
        public NewIssue CreateEffortOverviewForCustomer(EffortSheet sheet)
        {
            string sheetOverviewInMarkdown = CreateSheetOverviewInMarkdown(sheet);

            NewIssue result = new NewIssue(sheet.Name)
            {
                Body = sheetOverviewInMarkdown
            };

            return result;
        }

        public string GetEffortOverviewForUpdate(EffortSheet sheet)
        {
            string result = CreateSheetOverviewInMarkdown(sheet);

            result += Environment.NewLine;
            result += Environment.NewLine;
            result += "Update vom: " + DateTime.Now;

            return result;
        }

        private static string CreateSheetOverviewInMarkdown(EffortSheet sheet)
        {
            string result = CreateSummary(sheet);
            result += Environment.NewLine;

            result += CreateHourlyEffortTable(sheet);

            result += CreateIterationListing(sheet);

            result += Environment.NewLine;
            return result;
        }

        private static string CreateIterationListing(EffortSheet sheet)
        {
            string result = Environment.NewLine;
            result += Environment.NewLine;
            result += "Iterationen: " + Environment.NewLine;

            IReadOnlyList<Iteration> iterations = sheet.GetAllIterations();
            foreach (var it in iterations)
            {
                result += Environment.NewLine;
                result += " - " + it.ProjectName + " - " + it.Name;

                IReadOnlyList<IterationEntry> itEntries = it.GetAllEntries();
                foreach (var entry in itEntries)
                {
                    result += Environment.NewLine;
                    result += "   - [" + entry.Title + "](" + entry.LinkToDetailedDescription + ")";
                }
            }

            return result;
        }

        private static string CreateHourlyEffortTable(EffortSheet sheet)
        {
            string result = Environment.NewLine;
            result += Environment.NewLine;
            result += "| Aktivität | Aufwand | Von | Bis |" + Environment.NewLine;
            result += "| --- | ---: | --- | --- |";

            IReadOnlyList<HourlyEffortGroup> effortGroups = sheet.GetAllEffortGroups();
            foreach (var group in effortGroups)
            {
                result += Environment.NewLine;
                result += $"| [{group.Name}]({group.LinkToDetailedDescription.AbsolutePath}) | {group.EffortInHours}h |";
                result += $" {group.FirstDate.ToShortDateString()} | {group.LastDate.ToShortDateString()} |";
            }

            result += Environment.NewLine;
            result += "| Summe | " + sheet.SumHourlyEffort() + "h |";

            return result;
        }

        private static string CreateSummary(EffortSheet sheet)
        {
            string result = $"Der noch abzurechnende stündliche Aufwand beträgt: **{sheet.SumHourlyEffort()}h**";
            result += Environment.NewLine;
            result += $"Die noch abzurechnende Anzahl an Iterationen beträgt: **{sheet.SumIterations()} Iterationen**";
            return result;
        }
    }
}
