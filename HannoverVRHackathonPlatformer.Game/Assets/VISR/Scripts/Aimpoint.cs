using UnityEngine;

public class Aimpoint : MonoBehaviour
{
    public float PushBack = 0.2f;
    public GameObject Reticle;

    private int _previousTouchCount = 0;

    private void Update()
    {
        RaycastHit rayHit;

        if (Physics.Raycast(new Ray(transform.position, transform.forward), out rayHit))
        {
            Reticle.SetActive(true);
            Reticle.transform.position = rayHit.point + -transform.forward * PushBack;
            Reticle.transform.rotation.SetLookRotation(-transform.forward);

            int touchCount = Input.touchCount;
            if (Input.GetMouseButtonDown(0))
            {
                touchCount = 1;
                _previousTouchCount = 0;
            }

            LookReciever reciever = rayHit.collider.GetComponentInParent<LookReciever>();
            if (reciever != null)
                reciever.LookRecieved(rayHit, touchCount > 0 && _previousTouchCount == 0);

            _previousTouchCount = touchCount;
        }
        else
        {
            Reticle.SetActive(false);
        }
    }
}
