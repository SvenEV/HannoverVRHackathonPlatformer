using System;
using UnityEngine;

public class VRCamera : MonoBehaviour
{
    private Vector3 _mousePosition;

    public Shader DistortionShader;
    public VRDeviceSettings VRDeviceSettings;

    private void Start()
    {
        SetupCameras();
        gameObject.AddComponent<VRHeadTracking>();
    }

    private void SetupCameras()
    {
#if UNITY_WSA
        GetComponent<Camera>().enabled = false;

        var left = new GameObject("Left Eye").transform;
        var right = new GameObject("Right Eye").transform;

        left.SetParent(transform);
        right.SetParent(transform);

        left.localPosition = new Vector3(VRDeviceSettings.Ipd / -200f, 0, 0);
        right.localPosition = new Vector3(VRDeviceSettings.Ipd / 200f, 0, 0);

        var leftCam = left.gameObject.AddComponent<Camera>();
        leftCam.rect = new Rect(0, 0, .5f, 1);
        leftCam.fieldOfView = 95;

        var rightCam = right.gameObject.AddComponent<Camera>();
        rightCam.rect = new Rect(.5f, 0, .5f, 1);
        rightCam.fieldOfView = 95;

        left.gameObject.AddComponent<VREye>().Configure(VRDeviceSettings, DistortionShader);
        right.gameObject.AddComponent<VREye>().Configure(VRDeviceSettings, DistortionShader);
#endif
    }
}

public class VREye : MonoBehaviour
{
    private Material _material;
    private VRDeviceSettings _settings;

    public void Configure(VRDeviceSettings settings, Shader distortionShader)
    {
        _settings = settings;
        _material = new Material(distortionShader) { hideFlags = HideFlags.DontSave };
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float xOffset = _settings.Ild - 60;

        _material.SetFloat("_XOffset", xOffset);
        _material.SetFloat("_K1", _settings.K1 * _settings.DistortionScale);
        _material.SetFloat("_K2", _settings.K2 * _settings.DistortionScale);
        _material.SetFloat("_K3", _settings.K3 * _settings.DistortionScale);
        _material.SetFloat("_K4", _settings.K4 * _settings.DistortionScale);

        Graphics.Blit(source, destination, _material);
    }

}

[Serializable]
public class VRDeviceSettings
{
    public float K1 = .39f;
    public float K2 = .15f;
    public float K3 = .1f;
    public float K4 = .03f;
    public float DistortionScale = 1;
    public float Ipd = 60;
    public float Ttl = 0;
    public float Ild = 60;
}