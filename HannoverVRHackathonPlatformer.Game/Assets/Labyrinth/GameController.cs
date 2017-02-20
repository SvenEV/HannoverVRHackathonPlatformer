using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private TimeSpan time;
    private bool isGameRunning = true;

    // reference to the UI text component
    public Text timeText;

    public AudioClip CoinCollectSound;
    public AudioClip FinishSound;

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
        GetComponent<AudioSource>().PlayOneShot(FinishSound);
        HighscoreManager.UploadScore(time);
    }

    public void OnCoinCollected(Collider coin, Collider player)
    {
        time -= TimeSpan.FromSeconds(5);

        if (time < TimeSpan.Zero)
            time = TimeSpan.Zero;

        GetComponent<AudioSource>().PlayOneShot(CoinCollectSound);

        Destroy(coin.gameObject);
    }
}