using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace EffortCalculator
{
    class EffortComment
    {
        private IssueComment ghComment;

        public EffortComment(IssueComment githubComment)
        {
            ghComment = githubComment;
            EffortInHours = ExtractEffort(ghComment.Body);
        }

        private float effort;
        public float EffortInHours
        {
            get { return effort; }
            private set { effort = value; }
        }

        public DateTime CreatedAt
        {
            get { return ghComment.CreatedAt.LocalDateTime; }
        }

        public string Body
        {
            get { return ghComment.Body; }
        }

        public override string ToString()
        {
            string result;

            result = "[" + CreatedAt + "](" + ghComment.HtmlUrl.AbsoluteUri + ")";
            result += ", Aufwand: " + EffortInHours + "h";

            return  result;
        }

        private float ExtractEffort(string text)
        {
            // entfernt "Aufwand: " aus dem Text. Damit sollte die Aufwandszahl direkt am ANfang
            // der Zeichenkette stehen.
            string effortAtStart = text.Remove(0, 9);
            int effortEndPosition = effortAtStart.IndexOf('h');

            string effortAsString = effortAtStart.Substring(0, effortEndPosition);

            return float.Parse(effortAsString);
        }
    }
}
