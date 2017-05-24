using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(HighscoreManager))]
public class GameController : MonoBehaviour
{
    private TimeSpan time;
    private bool isGameRunning = true;
    private HighscoreManager highscoreManager;

    // reference to the UI text component
    public Text timeText;
    public AudioClip coinCollectSound;
    public AudioClip finishSound;

    private void Start()
    {
        highscoreManager = GetComponent<HighscoreManager>();
    }

    private void Update()
    {
        if (isGameRunning)
        {
            time += TimeSpan.FromSeconds(Time.deltaTime);
        }

        timeText.text = string.Format("{0}:{1:00}:{2:000}", time.Minutes, time.Seconds, time.Milliseconds);
    }

    // This is called when the player reaches 'finishArea'
    public void OnFinish()
    {
        isGameRunning = false;
        GetComponent<AudioSource>().PlayOneShot(finishSound);
        timeText.text += "\r\nsaving...";
        highscoreManager.PostScore(time, OnScorePosted);
    }

    private void OnScorePosted(WWW www)
    {
        SceneManager.LoadScene(0);
    }

    public void OnCoinCollected(Collider coin, Collider player)
    {
        time -= TimeSpan.FromSeconds(5);

        if (time < TimeSpan.Zero)
            time = TimeSpan.Zero;

        GetComponent<AudioSource>().PlayOneShot(coinCollectSound);

        Destroy(coin.gameObject);
    }
}