namespace Mercop.Core.Web.Data
{
    public class RequestParameters
    {
        public const string GET_PLAYER_AUTH = "v1/auth/register";
        
        public const string GET_LEADERBOARDS = "v1/leaderboards";
        public const string GET_LEADERBOARDS_HEADER = "idToken";
        
        public const string POST_LEADERBOARDS = "v1/leaderboards/submit";
        public const string POST_LEADERBOARDS_HEADER = "idToken";
    }
}