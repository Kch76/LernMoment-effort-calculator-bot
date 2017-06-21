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

                HourlyEffortGroup eIssue = new HourlyEffortGroup(issue);
                foreach (var item in comments)
                {
                    HourlyEffortEntry eComment = new HourlyEffortEntry(item);
                    eIssue.AddEffortComment(eComment);
                }

                if (eIssue.EffortInHours > 0.0f)
                {
                    sheet.AddEffortEntry(eIssue);
                }
                // Anzeige, dass dem Anwender klar ist, dass noch etwas passiert.
                Console.Write(".");
            }

            foreach (var issue in issueRepository.GetAllEffortRelatedIssues("Iteration: ", "suchja"))
            {
                IReadOnlyCollection<IssueComment> comments = issueRepository.GetAllEffortRelatedComments("Iteration: ", issue);

                foreach (var item in comments)
                {
                    if (item.Body.StartsWith("Iteration: ") && item.Reactions.Hooray == 0)
                    {
                        // extract owner and repository name
                        string[] repoUrlSegments = issue.Url.Segments;
                        string repoName = repoUrlSegments[3].TrimEnd('/');

                        // Namen extrahieren
                        string itName;
                        int itNameEndPosition = item.Body.IndexOf(Environment.NewLine);
                        itName = item.Body.Remove(itNameEndPosition, item.Body.Length - itNameEndPosition);
                        itName += " (" + repoName + ")";

                        var it = sheet.GetIteration(itName);
                        if (it == null)
                        {
                            it = new Iteration(itName);
                            it.AddIterationEntry(new IterationEntry(issue));
                            sheet.AddIteration(it);
                        }
                        else
                        {
                            it.AddIterationEntry(new IterationEntry(issue));
                        }

                        // wir brauchen nur einen Kommentar um zu bestätigen, dass dieses Issue
                        // tatsächlich zu einer Iteration gehört.
                        break;
                    }
                }

                // Anzeige, dass dem Anwender klar ist, dass noch etwas passiert.
                Console.Write(".");
            }
            return sheet;
        }
    }
}
