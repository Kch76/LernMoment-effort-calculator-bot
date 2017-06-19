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
        static void Main(string[] args)
        {
            Console.WriteLine(" # # #   Effort Calculator   # # # ");

            EffortSheet sheet = new EffortSheet("Aufwandsübersicht Juni 2017");
            GitHubClient client = InitializeClient();
            GitHubIssueRepository issueRepository = new GitHubIssueRepository(client.Search, client.Issue);

            foreach (var issue in issueRepository.GetAllEffortRelatedIssues("Aufwand: ", "suchja"))
            {
                IReadOnlyCollection<IssueComment> comments = issueRepository.GetAllEffortRelatedComments("Aufwand: ", issue);

                EffortIssue eIssue = new EffortIssue(issue);
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

            Console.WriteLine();
            Console.WriteLine(sheet);
            Console.WriteLine();

            Console.WriteLine("Drücke 'Enter' um die Anwendung zu beenden!");
            Console.ReadLine();
        }

        private static GitHubClient InitializeClient()
        {
            string accessToken = RequestAccessTokenFromUser();

            var client = new GitHubClient(new ProductHeaderValue("Issue-Test-Client"));
            var authToken = new Credentials(accessToken);

            client.Credentials = authToken;
            return client;
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
