using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EffortCalculator.Model;

namespace EffortCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" # # #   Effort Calculator   # # # ");

            GitHubClient client = InitializeClient();
            GitHubIssueRepository issueRepository = new GitHubIssueRepository(client.Search, client.Issue);
            EffortSheetFactory sheetFactory = new EffortSheetFactory(issueRepository);

            EffortSheet sheet = sheetFactory.CreateNew("Aufwandsübersicht Juni 2017");

            Console.WriteLine();
            Console.WriteLine(sheet);
            Console.WriteLine();

            Console.Write("Soll der Kunde informiert werden (j/n)? ");
            ConsoleKeyInfo informUserKey = Console.ReadKey();
            if (informUserKey.Key == ConsoleKey.J)
            {
                var cis = new CustomerInformationService();
                NewIssue issue = cis.CreateEffortOverviewForCustomer(sheet);
                issueRepository.AddIssue(issue, "aufwand-test", "suchja");
            }

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
