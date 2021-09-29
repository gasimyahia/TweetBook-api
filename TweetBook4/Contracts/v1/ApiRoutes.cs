

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
        }
    }
}
