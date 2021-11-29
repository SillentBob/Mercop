using System;

namespace Mercop.Core.Web.Data
{
    [Serializable]
    public class PlayerAuthData
    {
        public string idToken;
        public string refreshToken;
        public PlayerAuthUserData user;
    }

    [Serializable]
    public class PlayerAuthUserData
    {
        public string id;
    }
}