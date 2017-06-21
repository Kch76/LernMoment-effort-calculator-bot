using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator.Model
{
    class HourlyEffortGroup
    {
        private Issue ghIssue;
        private List<HourlyEffortEntry> comments = new List<HourlyEffortEntry>();

        public HourlyEffortGroup(Issue githubIssue)
        {
            ghIssue = githubIssue;
        }

        public void AddEffortComment(HourlyEffortEntry comment)
        {
            // TODO: Überlegen wie ich mit Kommentaren umgehe die bereits in der Liste sind.
            comments.Add(comment);
        }

        public float EffortInHours
        {
            get
            {
                float result = 0f;
                foreach (var comment in comments)
                {
                    result += comment.EffortInHours;
                }
                return result;
            }
        }

        public override string ToString()
        {
            string result;
            string[] repoUrlSegments = ghIssue.Url.Segments;
            string repoName = repoUrlSegments[3].TrimEnd('/');

            // Issue information
            result = ghIssue.Title;
            result += " (" + repoName + ")";
            result += ", [Issue: " + ghIssue.Number + "](" + ghIssue.HtmlUrl.AbsoluteUri + ")";
            result += ", " + EffortInHours + "h";

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
