using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    private const string _url = "http://vrhackathon.azurewebsites.net/api/values";

    public Text uiText;

    private void Start()
    {
        UpdateScores();
    }

    public void PostScore(TimeSpan score, WebScript.PostCompletedEventHandler callback)
    {
        WebScript.Current.Post(_url, score.TotalMilliseconds.ToString(), callback);
    }

    public void UpdateScores()
    {
        WebScript.Current.Get(_url, (www, data) =>
        {
            var times = data.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => TimeSpan.FromMilliseconds(int.Parse(s)))
                .Select(t => string.Format("{0}:{1:00}:{2:000}", t.Minutes, t.Seconds, t.Milliseconds))
                .ToArray();

            uiText.text = "<b>Top Times</b>\n\n" + string.Join("\n", times);
        });
    }
}
