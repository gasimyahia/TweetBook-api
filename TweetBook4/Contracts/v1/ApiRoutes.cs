

namespace TweetBook4.Contracts.vi
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";

        public const string Base = Root + "/" + Version;
        public static class posts
        {
            public const  string getAll = Base+"/posts";
            public const string Create = Base + "/posts";
            public const string Get = Base + "/posts/{postid}";
            public const string Update = Base + "/posts/{postid}";
            public const string Delete = Base + "/posts/{postid}";
            public const string PostsWitTag = Base + "/posts/tags";
            public const string Search = Base + "/posts/search";
        }

        public static class tags
        {
            public const string getAll = Base + "/tags";
            public const string Create = Base + "/tags";
            public const string Get = Base + "/tags/{tagId}";
            public const string Update = Base + "/tags/{tagId}";
            public const string Delete = Base + "/tags/{tagId}";
            public const string Search = Base + "/tags/search";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
