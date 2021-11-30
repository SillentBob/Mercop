using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mercop.Core.Web.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Mercop.Core.Web
{
    public static class BackendSimulator
    {
        private const string LeaderboardsFileName = "Leaderboards.json";

        public static IEnumerator GetData(UnityWebRequest request, Action<string> onFinish)
        {
            yield return new WaitForSeconds(2); //simulate network delay

            if (request.url.Contains(RequestParameters.GET_PLAYER_AUTH))
            {
                onFinish.Invoke(GetAuthRequestData(request.url));
            }
            else if (request.url.Contains(RequestParameters.GET_LEADERBOARDS))
            {
                onFinish.Invoke(LoadOrGenerateLeaderboards(request.url));
            }
        }

        public static IEnumerator PostData(UnityWebRequest request, string jsonTextData, Action<bool> onFinish)
        {
            yield return new WaitForSeconds(0.5f); //simulate network delay
            if (request.url.Contains(RequestParameters.POST_LEADERBOARDS))
            {
                var isPosted = DoPostLeaderboardsData(request.url, jsonTextData);
                onFinish.Invoke(isPosted);
            }
        }

        private static string GetAuthRequestData(string request)
        {
            PlayerAuthData data = new PlayerAuthData
            {
                idToken = "IdToken1",
                refreshToken = "RefreshToken1",
                user = new PlayerAuthUserData
                {
                    id = "1"
                }
            };
            return JsonUtility.ToJson(data);
        }

        private static string LoadOrGenerateLeaderboards(string request, bool forceGenerateNew = false)
        {
            LeaderboardsData lbData = LoadLocalLeaderboards();
            if (lbData == null || forceGenerateNew)
            {
                string timeFrom = DateFormatter.FormatDatetimeToString(new DateTime(2021, 11, 10, 0, 0, 20));
                string timeTo = DateFormatter.FormatDatetimeToString(new DateTime(2060, 2, 19, 0, 0, 20));

                LeaderboardsData data = new LeaderboardsData()
                {
                    group = new LeaderboardsGroup
                    {
                        week = 6,
                        start = timeFrom,
                        end = timeTo,
                        players = GenerateLeaderboardsPlayers()
                    }
                };
                lbData = data;
                SaveLeaderboardsToLocal(lbData);
            }

            return JsonUtility.ToJson(lbData);
        }

        private static LeaderboardsPlayerData[] GenerateLeaderboardsPlayers()
        {
            LeaderboardsPlayerData[] players = new LeaderboardsPlayerData[100];
            for (int i = 0; i < players.Length; i++)
            {
                var id = i + 1;
                players[i] = new LeaderboardsPlayerData
                {
                    uid = $"uid{id}",
                    name = $"Player{id}",
                    scores = new LeaderboardsPlayerScores
                    {
                        current = (players.Length - i) * 10,
                        past = (players.Length - i) * 10
                    }
                };
            }

            return players;
        }


        /// <returns>true if score is saved, false if score was too low</returns>
        private static bool DoPostLeaderboardsData(string requestUrl, string jsonTextData)
        {
            PostLeaderboardsData dataToSave = JsonUtility.FromJson<PostLeaderboardsData>(jsonTextData);
            LoadOrGenerateLeaderboards(null);
            LeaderboardsData lbData = LoadLocalLeaderboards();

            for (int i = 0; i < lbData.group.players.Length; i++)
            {
                var player = lbData.group.players[i];
                if (player.scores.current < dataToSave.score)
                {
                    for (int p = lbData.group.players.Length - 1; p > i; p--)
                    {
                        lbData.group.players[p] = lbData.group.players[p - 1];
                    }

                    lbData.group.players[i] = new LeaderboardsPlayerData
                    {
                        uid = "1", //TODO fix getting uid from PlayerAuthData
                        name = dataToSave.name,
                        scores = new LeaderboardsPlayerScores()
                        {
                            current = dataToSave.score,
                            past = dataToSave.score
                        }
                    };
                    SaveLeaderboardsToLocal(lbData);
                    return true;
                }
            }

            return false;
        }

        private static LeaderboardsData LoadLocalLeaderboards()
        {
            string savePath = Path.Combine(Application.persistentDataPath, LeaderboardsFileName);
            LeaderboardsData saveData = null;
            if (File.Exists(savePath))
            {
                FileStream dataStream = new FileStream(savePath, FileMode.Open);
                BinaryFormatter converter = new BinaryFormatter();
                try
                {
                    saveData = converter.Deserialize(dataStream) as LeaderboardsData;
                    dataStream.Close();
                }
                catch (Exception e)
                {
                    Debug.Log($"Load failed! " + e.Message);
                }
            }

            return saveData;
        }

        public static void SaveLeaderboardsToLocal(LeaderboardsData data)
        {
            string savePath = Path.Combine(Application.persistentDataPath, LeaderboardsFileName);
            FileStream dataStream = new FileStream(savePath, FileMode.Create);
            BinaryFormatter converter = new BinaryFormatter();
            converter.Serialize(dataStream, data);
            dataStream.Close();
        }

#if UNITY_EDITOR
        [MenuItem("Mercop/PrintLeaderboards")]
        private static void PrintLeaderboards()
        {
            LoadOrGenerateLeaderboards(null);
            LeaderboardsData data = LoadLocalLeaderboards();
            for (int i = 0; i < data.group.players.Length; i++)
            {
                Debug.Log($"{data.group.players[i].name},{data.group.players[i].scores.current}");
            }
        }
#endif

#if UNITY_EDITOR
        [MenuItem("Mercop/ResetLeaderboards")]
        private static void ResetLeaderboards()
        {
            LoadOrGenerateLeaderboards(null, true);
        }
#endif
    }
}