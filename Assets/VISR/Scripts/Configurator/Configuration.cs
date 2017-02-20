using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Configuration : MonoBehaviour {

    public static bool HasDisplayed = false;
    public static string TargetLevelName = null;

    public string DefaultLevelName;

    public GameObject UICanvas;

    public static void CheckConfiguration()
    {
        if(!HasDisplayed)
        {
            TargetLevelName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("DeviceConfig");
            HasDisplayed = true;
        }
        else
        {
            if(Screen.orientation != ScreenOrientation.LandscapeLeft)
                Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

	void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
	}

    public void LaunchApp()
    {
        if (UICanvas != null)
            UICanvas.SetActive(false);

        HasDisplayed = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        if (TargetLevelName == null)
            TargetLevelName = DefaultLevelName;

        SceneManager.LoadScene(TargetLevelName);
    }

    public void BuyDevice()
    {
        Application.OpenURL("http://visr-vr.com/shop");
    }
}
