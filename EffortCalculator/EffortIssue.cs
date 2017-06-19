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

        public EffortIssue(Issue githubIssue)
        {
            ghIssue = githubIssue;
        }

        public override string ToString()
        {
            string result;
            string[] repoUrlSegments = ghIssue.Url.Segments;
            string repoName = repoUrlSegments[3].TrimEnd('/');

            result = ghIssue.Title;
            result += " (" + repoName + ")";
            result += ", " + ghIssue.Number;

            return result;
        }
    }
}
