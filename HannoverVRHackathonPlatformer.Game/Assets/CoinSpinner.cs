using UnityEngine;

public class CoinSpinner : MonoBehaviour
{
    private Vector3 startPosition;

    // rotation in degrees per second
    public float RotationSpeed = 180;

    // controls how fast the coin moves up and down
    public float WobbleSpeed = 4;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
        transform.position = startPosition + .5f * Vector3.up * Mathf.Sin(WobbleSpeed * Time.time);
    }
}
