using UnityEngine;

public class LockCameraPosition : MonoBehaviour 
{
    private Vector3 originalPosition;

    void Start() {
        originalPosition = transform.localPosition;
    }

    void LateUpdate() {
        // Constantly reset to original position
        transform.localPosition = originalPosition;
    }
}
