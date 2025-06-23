using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public bool followX = true;
    public bool followY = true;
    public bool followRotation = false;

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        if (followX) newPosition.x = cameraTransform.position.x;
        if (followY) newPosition.y = cameraTransform.position.y;
        transform.position = newPosition;

        if (followRotation)
        {
            transform.rotation = cameraTransform.rotation;
        }
    }
}