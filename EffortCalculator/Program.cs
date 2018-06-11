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

            EffortSheet sheet = sheetFactory.CreateNew("Aufwandsübersicht Juni 2018");

            Console.WriteLine();
            Console.WriteLine(sheet);
            Console.WriteLine();

            Console.Write("Soll der Kunde informiert werden (j/n)? ");
            ConsoleKeyInfo informUserKey = Console.ReadKey();
            if (informUserKey.Key == ConsoleKey.J)
            {
                string targetRepositoryName = GetTargetRepositoryName();
                string customerName = GetCustomerName();
                var cis = new CustomerInformationService();
                var existingOverview = issueRepository.GetIssueWithName(targetRepositoryName, customerName, sheet.Name);

                if (existingOverview == null)
                {
                    NewIssue issue = cis.CreateEffortOverviewForCustomer(sheet);
                    issueRepository.AddIssue(issue, targetRepositoryName, customerName);
                }
                else
                {
                    string newOverview = cis.GetEffortOverviewForUpdate(sheet);
                    issueRepository.AddCommentToIssue(targetRepositoryName, customerName, existingOverview, newOverview);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Drücke 'Enter' um die Anwendung zu beenden!");
            Console.ReadLine();
        }

        private static string GetTargetRepositoryName()
        {
            string name = Properties.Settings.Default.DefaultTargetRepository;

            Console.WriteLine();
            Console.Write("Soll das gespeichert Repository: {0} verwendet werden? (j/n): ", name);
            ConsoleKeyInfo retrieveNewTargetRepository = Console.ReadKey();

            if (retrieveNewTargetRepository.Key == ConsoleKey.N)
            {
                Console.WriteLine();
                Console.Write("Bitte gib den neuen Repository-Namen ein: ");

                name = Console.ReadLine();

                // neuen Wert in den Settings speichern
                Properties.Settings.Default.DefaultTargetRepository = name;
                Properties.Settings.Default.Save();
            }

            return name;
        }

        private static string GetCustomerName()
        {
            string name = Properties.Settings.Default.DefaultCustomerName;

            Console.WriteLine();
            Console.Write("Soll der gespeicherte Kundenname: {0} verwendet werden? (j/n): ", name);
            ConsoleKeyInfo retrieveNewCustomer = Console.ReadKey();

            if (retrieveNewCustomer.Key == ConsoleKey.N)
            {
                Console.WriteLine();
                Console.Write("Bitte gib den neuen Kundennamen ein: ");

                name = Console.ReadLine();

                // neuen Wert in den Settings speichern
                Properties.Settings.Default.DefaultCustomerName = name;
                Properties.Settings.Default.Save();
            }

            return name;
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
            Console.Write("Soll der gespeichert Token: {0} verwendet werden? (j/n): ", HidePartsOfToken(token));
            ConsoleKeyInfo retrieveNewKey = Console.ReadKey();

            if (retrieveNewKey.Key == ConsoleKey.N)
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
