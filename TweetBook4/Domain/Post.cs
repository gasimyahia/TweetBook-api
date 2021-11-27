
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetBook4.Domain
{
    public class Post
    {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public List<Tag> Tags { get; set; }

    }

    
}
