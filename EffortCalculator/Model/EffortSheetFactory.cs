using System;
using System.Collections.Generic;
using Octokit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator.Model
{
    class EffortSheetFactory
    {
        private GitHubIssueRepository issueRepository;

        public EffortSheetFactory(GitHubIssueRepository issueRepository)
        {
            this.issueRepository = issueRepository;
        }

        /// <summary>
        /// Erstellt eine neue Aufwandsübersicht mit Einträgen von GitHub
        /// </summary>
        /// <remarks>
        /// Holt alle relevanten, bezogen auf den Aufwand, Issues und deren Kommentare von GitHub
        /// und erstellt daraus eine entsprechende Übersicht.
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        public EffortSheet CreateNew(string name)
        {
            EffortSheet sheet = new EffortSheet(name);
            foreach (var issue in issueRepository.GetAllEffortRelatedIssues("Aufwand: ", "suchja"))
            {
                IReadOnlyCollection<IssueComment> comments = issueRepository.GetAllEffortRelatedComments("Aufwand: ", issue);

                EffortEntry eIssue = new EffortEntry(issue);
                foreach (var item in comments)
                {
                    EffortComment eComment = new EffortComment(item);
                    eIssue.AddEffortComment(eComment);
                }

                if (eIssue.EffortInHours > 0.0f)
                {
                    sheet.AddEffortEntry(eIssue);
                }
                // Anzeige, dass dem Anwender klar ist, dass noch etwas passiert.
                Console.Write(".");
            }

            return sheet;
        }
    }
}
