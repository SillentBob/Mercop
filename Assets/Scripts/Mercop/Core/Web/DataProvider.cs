using System;
using System.Collections;
using Mercop.Core.Web;
using Mercop.Core.Web.Data;
using UnityEngine;
using UnityEngine.Networking;

public class DataProvider : MonoBehaviour
{
    public void GetPlayerAuthData(Action<PlayerAuthData> onFinish)
    {
        var request = BuildPlayerAuthDataRequest();
        StartCoroutine(GetData(request, onFinish));
    }

    public void GetLeaderboardsData(string country, string playerIdToken, Action<LeaderboardsData> onFinish)
    {
        var request = BuildLeaderboardsDataRequest(country, playerIdToken);
       
        StartCoroutine(GetData(request, onFinish));
    }

    public void PostPlayerData(PlayerAuthData playerAuthData, string tournamentId, string playerName, int score,
        Action<bool> onPostResult)
    {
        var data = new PostLeaderboardsData
        {
            tournamentId = tournamentId,
            name = playerName,
            score = score
        };
        var request = BuildPostLeaderboardsData(playerAuthData.idToken, data);
        
        //data in POST body is encoded, so we create undecoded data copy for backend simulator for simplicity
        StartCoroutine(PostData(request, onPostResult, JsonUtility.ToJson(data)));
    }

    private IEnumerator GetData<T>(UnityWebRequest request, Action<T> processDataAction)
    {
        T loadedData = default(T);
        
        // Uncomment if real www request needed
        //
        // yield return request.SendWebRequest();
        // if (request.result != UnityWebRequest.Result.Success) {
        //     Debug.Log(request.error);
        // }
        // else
        // {
        //     // Show results as text
        //     Debug.Log(request.downloadHandler.text);
        //     // Or retrieve results as binary data
        //     byte[] results = request.downloadHandler.data;
        //     loadedData = OnDataLoadFinish<T>(request.downloadHandler.text)
        // }
        
        yield return StartCoroutine(BackendSimulator.GetData(request,
            (jsonData) => loadedData = OnDataLoadFinish<T>(jsonData)));
        if (processDataAction != null)
        {
            processDataAction.Invoke(loadedData);
        }
    }

    private IEnumerator PostData(UnityWebRequest request, Action<bool> onPostDataResult, string textJsonData)
    {
        bool success = false;
        yield return StartCoroutine(BackendSimulator.PostData(request,
            textJsonData, (isSuccess) => success = OnPostFinish(isSuccess)));
        if (onPostDataResult != null)
        {
            onPostDataResult.Invoke(success);
        }
    }

    private UnityWebRequest BuildPlayerAuthDataRequest()
    {
        var getRequest = $"{RequestParameters.GET_PLAYER_AUTH}";
        UnityWebRequest webRequest = UnityWebRequest.Get(getRequest);
        return webRequest;
    }

    private UnityWebRequest BuildLeaderboardsDataRequest(string country, string userIdToken)
    {
        var getRequest = $"{RequestParameters.GET_LEADERBOARDS}?{country}";
        UnityWebRequest webRequest = UnityWebRequest.Get(getRequest);
        webRequest.SetRequestHeader(RequestParameters.GET_LEADERBOARDS_HEADER, userIdToken);
        return webRequest;
    }

    private UnityWebRequest BuildPostLeaderboardsData(string userIdToken, PostLeaderboardsData data)
    {
        var postRequest = $"{RequestParameters.POST_LEADERBOARDS}";
        var jsonData = JsonUtility.ToJson(data);
        UnityWebRequest webRequest = UnityWebRequest.Post(postRequest, jsonData);
        webRequest.SetRequestHeader(RequestParameters.POST_LEADERBOARDS_HEADER, userIdToken);
        return webRequest;
    }

    private T OnDataLoadFinish<T>(string jsonData)
    {
        T data = JsonUtility.FromJson<T>(jsonData);
        return data;
    }

    private bool OnPostFinish(bool isSuccess)
    {
        return isSuccess;
    }
}