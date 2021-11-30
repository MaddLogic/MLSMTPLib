using System.Collections.Generic;
using System.Linq;

namespace MLSMTPLib
{
    public class MLSMTPRecipient
    {
        public IList<string> To { get; set; } = new List<string>();
        public IList<string> Cc { get; set; } = new List<string>();
        public IList<string> Bcc { get; set; } = new List<string>();
        public string SubjectHeading { get; set; }

        public bool HasRecipient
        {
            get
            {
                return To.Any(t => !string.IsNullOrEmpty(t)) || Bcc.Any(t => !string.IsNullOrEmpty(t));
            }
        }
    }
}