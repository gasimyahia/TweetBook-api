using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetBook4.Domain
{
    public class Tag
    {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        [ForeignKey(nameof(createdBy))]
        public IdentityUser user { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
