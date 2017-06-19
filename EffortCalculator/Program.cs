using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator
{
    class Program
    {
        static List<EffortIssue> effortIssues = new List<EffortIssue>();
        static List<EffortComment> effortComments = new List<EffortComment>();

        static void Main(string[] args)
        {
            Console.WriteLine(" # # #   Effort Calculator   # # # ");
            string accessToken = RequestAccessTokenFromUser();

            var client = new GitHubClient(new ProductHeaderValue("Issue-Test-Client"));
            var authToken = new Credentials(accessToken);

            client.Credentials = authToken;

            var relevantIssuesRequest = new SearchIssuesRequest("Aufwand: ")
            {
                Involves = "suchja",
                Type = IssueTypeQualifier.Issue,
                In = new[] { IssueInQualifier.Comment },
            };

            SearchIssuesResult potentialEffortIssues = client.Search.SearchIssues(relevantIssuesRequest).GetAwaiter().GetResult();

            foreach (var issue in potentialEffortIssues.Items)
            {
                IReadOnlyCollection<IssueComment> comments = GetPotentialComments(client.Issue.Comment, issue);

                bool isIssueAdded = false;
                foreach (var item in comments)
                {
                    if (item.Body.StartsWith("Aufwand: ") && item.Reactions.Hooray == 0)
                    {
                        EffortIssue eIssue = new EffortIssue(issue);
                        EffortComment eComment = new EffortComment(item);

                        Console.WriteLine();
                        Console.WriteLine(eIssue);
                        Console.WriteLine(eComment);
                        Console.Write("Ist dies ein gültiger Kommentar für die Aufwandsabschätzung (j/n)? ");
                        ConsoleKeyInfo selection = Console.ReadKey();
                        Console.WriteLine();

                        if (selection.Key == ConsoleKey.J)
                        {
                            if (!isIssueAdded)
                            {
                                effortIssues.Add(eIssue);
                                isIssueAdded = true;
                            }

                            effortComments.Add(eComment);
                        }
                    }
                }
            }

            Console.WriteLine("Hier nochmals die ausgewählten Kommentare: ");
            foreach (var item in effortComments)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Drücke 'Enter' um die Anwendung zu beenden!");
            Console.ReadLine();
        }

        private static void OutputIssue(Issue issue)
        {
            string[] repoUrlSegments = issue.Url.Segments;
            string repoName = repoUrlSegments[3].TrimEnd('/');

            Console.WriteLine(repoName + " - #" + issue.Number + " - " + issue.State + " - " + issue.Title);
        }

        private static IReadOnlyCollection<IssueComment> GetPotentialComments(IIssueCommentsClient client, Issue issue)
        {
            string[] repoUrlSegments = issue.Url.Segments;
            string ownerName = repoUrlSegments[2].TrimEnd('/');
            string repoName = repoUrlSegments[3].TrimEnd('/');

            return client.GetAllForIssue(ownerName, repoName, issue.Number).GetAwaiter().GetResult();
        }

        private static string RequestAccessTokenFromUser()
        {
            string token = Properties.Settings.Default.AccessToken;

            Console.WriteLine("*** Personal Access Token Eingabe ***");
            Console.Write("Soll der gespeichert Token: {0} verwendet werden? (ja/nein): ", HidePartsOfToken(token));
            string retrieveNewKey = Console.ReadLine();

            if (retrieveNewKey.ToLower().Equals("nein"))
            {
                Console.Write("Bitte gib einen neuen Token ein: ");
                token = Console.ReadLine();

                // neuen Wert in den Settings speichern
                Properties.Settings.Default.AccessToken = token;
                Properties.Settings.Default.Save();
            }

            return token;
        }

        private static string HidePartsOfToken(string token)
        {
            StringBuilder sbToken = new StringBuilder(token);

            for (int i = 4; i < token.Length - 4; i++)
            {
                sbToken[i] = '*';
            }

            return sbToken.ToString();
        }
    }
}
