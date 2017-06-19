using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator
{
    class EffortIssue
    {
        private Issue ghIssue;
        private List<EffortComment> comments = new List<EffortComment>();

        public EffortIssue(Issue githubIssue)
        {
            ghIssue = githubIssue;
        }

        public void AddEffortComment(EffortComment comment)
        {
            // TODO: Überlegen wie ich mit Kommentaren umgehe die bereits in der Liste sind.
            comments.Add(comment);
        }

        public override string ToString()
        {
            string result;
            string[] repoUrlSegments = ghIssue.Url.Segments;
            string repoName = repoUrlSegments[3].TrimEnd('/');

            // Issue information
            result = ghIssue.Title;
            result += " (" + repoName + ")";
            result += ", " + ghIssue.Number;

            // Comment information
            foreach (var comment in comments)
            {
                result += Environment.NewLine;
                result += "    " + comment.ToString();
            }
            return result;
        }
    }
}
