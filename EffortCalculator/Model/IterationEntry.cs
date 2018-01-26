using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator.Model
{
    class IterationEntry
    {
        private Issue ghIssue;

        public IterationEntry(Issue githubIssue)
        {
            ghIssue = githubIssue;
        }

        public string Title
        {
            get { return ghIssue.Title; }
        }

        public Uri LinkToDetailedDescription
        {
            get { return new Uri(ghIssue.HtmlUrl); }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
