using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Text;
using System.Collections.Generic;

public class WebScript : MonoBehaviour
{
    private static WebScript _instance;

    /// <summary>
    /// Gets an instance of the <see cref="WebScript"/> class.
    /// </summary>
    public static WebScript Current
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject("WebScript").AddComponent<WebScript>();

            return _instance;
        }
    }

    public delegate void PostCompletedEventHandler(WWW www);
    public delegate void GetCompletedEventHandler(WWW www, JSONNode json);

    /// <summary>
    /// Retrieves JSON data from the specified URL.
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="callback">
    /// The method that is called to process the returned data
    /// when the GET operation is completed.
    /// </param>
    public void Get(string url, GetCompletedEventHandler callback)
    {
        var www = new WWW(url);
        StartCoroutine(GetAsync(www, callback));
    }
    
    /// <summary>
    /// Submits JSON data to the specified URL.
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="json">The JSON data</param>
    /// <param name="callback">
    /// The method that is called when the POST operation is
    /// completed. Can be used to check for errors etc.
    /// </param>
    public void Post(string url, string data, PostCompletedEventHandler callback)
    {
        var bytes = Encoding.UTF8.GetBytes(data.ToString());
        var headers = new Dictionary<string, string>();
        var www = new WWW(url, bytes, headers);
        StartCoroutine(PostAsync(www, callback));
    }

    private IEnumerator GetAsync(WWW www, GetCompletedEventHandler callback)
    {
        yield return www;

        var json = JSON.Parse(www.text);

        if (callback != null)
            callback(www, json);
    }

    private IEnumerator PostAsync(WWW www, PostCompletedEventHandler callback)
    {
        yield return www;

        if (callback != null)
            callback(www);
    }
}
