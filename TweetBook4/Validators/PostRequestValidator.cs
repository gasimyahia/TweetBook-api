using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Posts;

namespace TweetBook4.Validators
{
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z ]*$");
        }
    }
}
