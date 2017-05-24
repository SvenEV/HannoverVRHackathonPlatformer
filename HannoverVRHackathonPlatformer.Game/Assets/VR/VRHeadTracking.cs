using UnityEngine;

public class VRHeadTracking : MonoBehaviour
{
    private Vector3 lastMouse;

    private void Start()
    {
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 1.0f / 100.0f;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(1))
                lastMouse = Input.mousePosition;

            if (!Input.GetMouseButton(1))
                return;

            var deltaMouse = Input.mousePosition - lastMouse;
            lastMouse = Input.mousePosition;

            transform.rotation = transform.rotation
                * Quaternion.AngleAxis(-deltaMouse.y * .2f, Vector3.right)
                * Quaternion.AngleAxis(deltaMouse.x * .2f, Vector3.up);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        else
        {
#if UNITY_WSA
            var screenRotation = new Vector3(-90, 0, 90);

            var rotationRate = Input.gyro.rotationRate * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(-rotationRate.x, -rotationRate.y, rotationRate.z);

            var acceleration = Input.acceleration;

            //perform orientation transform for landscape left
            acceleration = new Vector3(-acceleration.x, acceleration.y, acceleration.z);

            var pitchVector = Vector3.ProjectOnPlane(acceleration, Vector3.right).normalized;
            var rollVector = Vector3.ProjectOnPlane(acceleration, Vector3.forward).normalized;

            var pitch = Mathf.Atan2(-pitchVector.y, pitchVector.z) * Mathf.Rad2Deg;
            var roll = Mathf.Atan2(rollVector.y, rollVector.x) * Mathf.Rad2Deg;

            var rotation = Quaternion.Euler(new Vector3(pitch, transform.eulerAngles.y, roll) + screenRotation);

            //This represents a basic complimentary filter, it's not good long term we should use a Kalman filter here [ld]
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
#endif
        }
    }
}
