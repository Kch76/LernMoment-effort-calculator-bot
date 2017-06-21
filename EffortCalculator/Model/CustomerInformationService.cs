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

        private string CreateSheetOverviewInMarkdown(EffortSheet sheet)
        {
            string result = $"Der noch abzurechnende stündliche Aufwand beträgt: **{sheet.SumHourlyEffort()}h**";
            result += Environment.NewLine;
            result += $"Die noch abzurechnende Anzahl an Iterationen beträgt: **{sheet.SumIterations()} Iterationen**";

            return result;
        }
    }
}
