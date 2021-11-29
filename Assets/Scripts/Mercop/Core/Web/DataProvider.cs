using System;
using System.Collections;
using Mercop.Core.Web;
using Mercop.Core.Web.Data;
using UnityEngine;
using UnityEngine.Networking;

public class DataProvider : MonoBehaviour
{
    private PlayerAuthData playerAuthData;

    public void GetPlayerAuthData(Action<PlayerAuthData> onFinish)
    {
        var request = BuildPlayerAuthDataRequest(); 
        StartCoroutine(GetData(request,onFinish));
    }
    
    public void GetLeaderboardsData(string country, string playerIdToken, Action<LeaderboardsData> onFinish)
    {
        var request = BuildLeaderboardsDataRequest(country, playerIdToken); 
        StartCoroutine(GetData(request,onFinish));
    }

    private IEnumerator GetData<T>(UnityWebRequest request, Action<T> processDataAction)
    {
        T loadedData = default(T);
        yield return StartCoroutine(BackendSimulator.GetData(request,
            (jsonData) => loadedData = OnDataLoadFinish<T>(jsonData)));
        processDataAction.Invoke(loadedData);
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
    
    private T OnDataLoadFinish<T>(string jsonData)
    {
        T data = JsonUtility.FromJson<T>(jsonData);
        return data;
    }

}