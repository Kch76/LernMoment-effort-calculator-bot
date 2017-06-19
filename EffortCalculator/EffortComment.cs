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
            return CreatedAt + " - " + Body;
        }
    }
}
