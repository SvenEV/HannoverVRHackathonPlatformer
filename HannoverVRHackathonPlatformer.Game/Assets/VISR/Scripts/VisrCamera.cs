using System;
using UnityEngine;

namespace VisrSdk
{
    [Serializable]
    public class VisrDevice
    {
        public int TagId;
        public string Name;
        public float K1 = 0.0f;
        public float K2 = 0.0f;
        public float K3 = 0.0f;
        public float K4 = 0.0f;

        public float DistortionScale = 1.0f;

        //Inter Pupilary Distance
        public float IPD = 60.0f;

        //Tray To Lens Distance
        public float TTL = 40.0f;

        public float ILD = 60.0f;
    }

    [Serializable]
    public class VisrDeviceLibrary
    {
        public string VersionString = "1.0.0";
        public VisrDevice[] Devices;
    }

    [SelectionBase]
    [ExecuteInEditMode]
    public class VisrCamera : MonoBehaviour
    {
        public Shader DistortionShader = null;

        public VisrDevice[] PresetDevices;
        public string CurrentDevice;

        public float K1 = 0.0f;
        public float K2 = 0.0f;
        public float K3 = 0.0f;
        public float K4 = 0.0f;

        public float DistortionScale = 1.0f;

        //Inter Pupilary Distance
        public float IPD = 60.0f;

        //Tray To Lens Distance
        public float TTL = 40.0f;

        public float ILD = 60.0f;

        public bool DisableStereoInEditor = false;

        public string TrackingNodeName = "Head";

        void Start()
        {
            // ensure we can never have stereo disabled on the target device
#if !UNITY_EDITOR
            DisableStereoInEditor = true;
#endif
        }

#if UNITY_EDITOR
        private void HandleStereoOption()
        {
            if (DisableStereoInEditor)
            {
                transform.Find("L").gameObject.SetActive(false);
                transform.Find("R").gameObject.SetActive(false);
                transform.Find("Preview").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("L").gameObject.SetActive(true);
                transform.Find("R").gameObject.SetActive(true);
                transform.Find("Preview").gameObject.SetActive(false);
            }
        }
#endif

        private void LateUpdate()
        {
#if UNITY_EDITOR
            HandleStereoOption();
#endif
            if(TrackingNodeName != null && Application.isPlaying)
            {
                TrackingSystem.Instance.SyncToTrackingNode(TrackingNodeName, transform);
            }
        }
    }
}