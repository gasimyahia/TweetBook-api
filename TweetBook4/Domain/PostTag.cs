using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Domain
{
    public class PostTag
    {
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual Post post { get; set; }
        [ForeignKey(nameof(TagId))]
        public Tag tag { get; set; }
    }
}
