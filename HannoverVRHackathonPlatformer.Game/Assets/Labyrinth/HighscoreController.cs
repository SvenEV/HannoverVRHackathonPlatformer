using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager
{

    private static string _url = "http://vrhackathon.azurewebsites.net/api/values";

    public static void UploadScore(TimeSpan score)
    {
        WebScript.Current.Post(_url, score.TotalMilliseconds.ToString(), PostCompleted);
    }

    private static void PostCompleted(WWW www)
    {

    }

    public static void UpdateScores()
    {
        WebScript.Current.Get(_url, GetCompleted);
    }

    private static void GetCompleted(WWW www, JSONNode json)
    {
        ///do something with json
    }
}
