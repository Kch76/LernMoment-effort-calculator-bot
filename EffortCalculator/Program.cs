using Octokit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator
{
    class Program
    {
        static ConcurrentBag<Issue> potentialIssues = new ConcurrentBag<Issue>();

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
                State = ItemState.Open,
                In = new[] { IssueInQualifier.Comment },
            };

            SearchIssuesResult openRelevantIssues = client.Search.SearchIssues(relevantIssuesRequest).GetAwaiter().GetResult();

            Parallel.ForEach<Issue>(openRelevantIssues.Items, i =>
            {
                potentialIssues.Add(GetCompleteIssue(client.Issue, i));
            });

            relevantIssuesRequest.State = ItemState.Closed;
            SearchIssuesResult closedRelevantIssues = client.Search.SearchIssues(relevantIssuesRequest).GetAwaiter().GetResult();
            Parallel.ForEach<Issue>(closedRelevantIssues.Items, i =>
            {
                potentialIssues.Add(GetCompleteIssue(client.Issue, i));
            });

            foreach (var issue in potentialIssues)
            {
                Console.WriteLine(issue.Repository.Name + " - #" + issue.Number + " - " + issue.State + " - " + issue.Title);
            }
            Console.WriteLine("Drücke 'Enter' um die Anwendung zu beenden!");
            Console.ReadLine();
        }

        private static Issue GetCompleteIssue(IIssuesClient client, Issue incompleteIssue)
        {
            string[] repoUrlSegments = incompleteIssue.Url.Segments;
            string repoOwnerName = repoUrlSegments[2].TrimEnd('/');
            string repoName = repoUrlSegments[3].TrimEnd('/');

            Issue completeIssue = null;
            completeIssue = client.Get(repoOwnerName, repoName, incompleteIssue.Number).GetAwaiter().GetResult();

            return completeIssue;
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
