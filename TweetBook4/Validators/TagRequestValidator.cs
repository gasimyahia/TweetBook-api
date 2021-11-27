using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Tags.Request;

namespace TweetBook4.Validators
{
    public class TagRequestValidator: AbstractValidator<TagRequest>
    {
        public TagRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z ]*$");
        }
    }
}
