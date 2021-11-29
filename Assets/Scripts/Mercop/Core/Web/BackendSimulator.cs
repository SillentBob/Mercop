using System;
using System.Collections;
using Mercop.Core.Web.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Mercop.Core.Web
{
    public static class BackendSimulator
    {
        public static IEnumerator GetData(UnityWebRequest request, Action<string> onFinish)
        {
            yield return new WaitForSeconds(2); //simulate network delay

            if (request.url.Contains(RequestParameters.GET_PLAYER_AUTH))
            {
                onFinish.Invoke(GetAuthRequestData(request.url));
            }
            else if (request.url.Contains(RequestParameters.GET_LEADERBOARDS))
            {
                onFinish.Invoke(GetLeaderboardsRequestData(request.url));
            }
        }

        private static string GetAuthRequestData(string request)
        {
            PlayerAuthData data = new PlayerAuthData()
            {
                idToken = "IdToken1",
                refreshToken = "RefreshToken1",
                user = new PlayerAuthUserData()
                {
                    id = "1"
                }
            };
            return JsonUtility.ToJson(data);
        }

        private static string GetLeaderboardsRequestData(string request)
        {
            string timeFrom = DateFormatter.FormatDatetimeToString(new DateTime(2021, 11, 10, 0, 0, 20));
            string timeTo = DateFormatter.FormatDatetimeToString(new DateTime(2060, 2, 19, 0, 0, 20));

            LeaderboardsData data = new LeaderboardsData()
            {
                @group = new LeaderboardsGroup
                {
                    week = 6,
                    start = timeFrom,
                    end = timeTo,
                    players = GetSavedLeaderboardsPlayers()
                }
            };
            return JsonUtility.ToJson(data);
        }

        private static LeaderboardsPlayerData[] GetSavedLeaderboardsPlayers()
        {
            //TODO try load from local save, if not found return newly created
            LeaderboardsPlayerData[] players = new LeaderboardsPlayerData[100];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new LeaderboardsPlayerData
                {
                    uid = $"uid{i}",
                    name = $"Player{i}",
                    scores = new LeaderboardsPlayerScores
                    {
                        current = i * 10,
                        past = i * 10
                    }
                };
            }

            return players;
        }
    }
}