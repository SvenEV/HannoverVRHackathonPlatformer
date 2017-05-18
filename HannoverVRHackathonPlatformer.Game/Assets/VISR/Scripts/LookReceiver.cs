using UnityEngine;

public enum LookState
{
    None,
    Gaze,
    Action,
    Count
}

public class LookReciever : MonoBehaviour
{
    public LookState CurrentState = LookState.None;

    private bool _lastButtonState = false;

    public void LookRecieved(RaycastHit lookRay, bool actionButton)
    {
        if (_lastButtonState != actionButton)
        {
            SendMessage("OnLookStateAction", lookRay, SendMessageOptions.DontRequireReceiver);
        }

        SendMessage("OnLookStateChanged", CurrentState, SendMessageOptions.DontRequireReceiver);
        _lastButtonState = actionButton;
    }
}
