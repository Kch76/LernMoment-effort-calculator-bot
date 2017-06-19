using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator
{
    class GitHubIssueRepository
    {
        private ISearchClient searchClient;
        private IIssuesClient issuesClient;

        public GitHubIssueRepository(ISearchClient searchClient, IIssuesClient issuesClient)
        {
            this.searchClient = searchClient;
            this.issuesClient = issuesClient;
        }

        public IReadOnlyList<Issue> GetAllEffortRelatedIssues(string searchTerm, string involvedPerson)
        {
            var relevantIssuesRequest = new SearchIssuesRequest(searchTerm)
            {
                Involves = involvedPerson,
                Type = IssueTypeQualifier.Issue,
                In = new[] { IssueInQualifier.Comment },
            };

            SearchIssuesResult potentialEffortIssues = searchClient.SearchIssues(relevantIssuesRequest).GetAwaiter().GetResult();

            return potentialEffortIssues.Items;
        }
    }
}
