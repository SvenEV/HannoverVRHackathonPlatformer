using System;
using System.Reflection;
using UnityEngine;

namespace VisrSdk
{
    [ExecuteInEditMode]
    public class LensCorrection : MonoBehaviour
    {
        private Material material;

        // Creates a private material used to the effect
        private void Awake()
        {
            if (!Application.isEditor)
                SetLandscapeOrientation();

            var visrCamera = GetComponentInParent<VisrCamera>();
            var shader = visrCamera.DistortionShader;

            if (material != null)
                DestroyImmediate(material);

            if (shader == null)
                Debug.LogError("Shader was not found");
            else
                material = new Material(shader);
        }

        private void Start()
        {
            if (!Application.isEditor)
                SetLandscapeOrientation();
            SetShaderParams();
        }

        private void SetLandscapeOrientation()
        {
            if (Screen.orientation != ScreenOrientation.LandscapeLeft)
                Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        private Vector2 GetViewSize()
        {
#if UNITY_EDITOR
            var type = Type.GetType("UnityEditor.GameView,UnityEditor");
            var method = type.GetMethod("GetSizeOfMainGameView", BindingFlags.NonPublic | BindingFlags.Static);
            return (Vector2)method.Invoke(null, null);
#else
            return new Vector2(Screen.width, Screen.height);
#endif
        }

        private void SetShaderParams()
        {
            var camera = GetComponent<Camera>();
            var config = GetComponentInParent<VisrCamera>();

            SetRectForTtl(camera, config);

            transform.localPosition = (transform.localPosition.x > 0)
                ? transform.localPosition = new Vector3(config.IPD * 0.0005f, 0, 0)
                : transform.localPosition = new Vector3(config.IPD * -0.0005f, 0, 0);

            var xOffset = config.ILD - 60;

            material.SetFloat("_XOffset", transform.localPosition.x > 0 ? -xOffset : xOffset);
            material.SetFloat("_K1", config.K1 * config.DistortionScale);
            material.SetFloat("_K2", config.K2 * config.DistortionScale);
            material.SetFloat("_K3", config.K3 * config.DistortionScale);
            material.SetFloat("_K4", config.K4 * config.DistortionScale);
        }

        private void SetRectForTtl(Camera camera, VisrCamera config)
        {
            var screenSizePixels = GetViewSize(); //we cache this cause it's actually kinda expensive
            var inchesToMm = 25.4f;
            var pixPerMm = Screen.dpi / inchesToMm;

            if (pixPerMm == 0)
                Debug.LogError("Screen DPI value not valid, skipping lens config");

            //the vertical position of the middle of the screen in mm
            var centerHeight = (screenSizePixels.y / 2) / pixPerMm;
            var heightOffset = centerHeight - config.TTL;
            var heightOffsetClipSpace = heightOffset / screenSizePixels.y;

            camera.rect = new Rect(camera.rect.x, -heightOffsetClipSpace, camera.rect.width, camera.rect.height);
        }

        // Postprocess the image
        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material == null)
                return;

            if (Application.isEditor)
                SetShaderParams();

            Graphics.Blit(source, destination, material);
        }
    }
}