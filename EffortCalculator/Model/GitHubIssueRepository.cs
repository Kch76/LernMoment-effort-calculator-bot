using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator.Model
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

        public IReadOnlyList<IssueComment> GetAllEffortRelatedComments(string searchTerm, Issue owningIssue)
        {
            // extract owner and repository name
            string[] repoUrlSegments = new Uri(owningIssue.Url).Segments;
            string ownerName = repoUrlSegments[2].TrimEnd('/');
            string repoName = repoUrlSegments[3].TrimEnd('/');

            IReadOnlyList<IssueComment> allComments = issuesClient.Comment.GetAllForIssue(ownerName, repoName, owningIssue.Number).GetAwaiter().GetResult();
            List<IssueComment> effortRelatedComments = new List<IssueComment>();

            foreach (var item in allComments)
            {
                if (item.Body.StartsWith(searchTerm) && item.Reactions.Hooray == 0)
                {
                    effortRelatedComments.Add(item);
                }
            }

            return effortRelatedComments.AsReadOnly();
        }

        public Issue GetIssueWithName(string repositorName, string ownerName, string issueName)
        {
            IReadOnlyList<Issue> issues = issuesClient.GetAllForRepository(ownerName, repositorName).GetAwaiter().GetResult();

            return issues.FirstOrDefault(i =>
            {
                if (i.Title.Equals(issueName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public void AddIssue(NewIssue issue, string repositorName, string ownerName)
        {
            issuesClient.Create(ownerName, repositorName, issue).GetAwaiter().GetResult();
        }

        public void AddCommentToIssue(string repositorName, string ownerName, Issue parent, string comment)
        {
            issuesClient.Comment.Create(ownerName, repositorName, parent.Number, comment).GetAwaiter().GetResult();
        }
    }
}
