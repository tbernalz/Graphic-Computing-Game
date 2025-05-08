// good 2 ( better than "good")

using UnityEngine;

public class RobotLightHandler : MonoBehaviour
{
    [Header("ASSIGN THESE")]
    public Transform handPoint; // Drag the robot's HAND bone/empty object here
    public float grabDistance = 2f; // Max distance to grab objects

    private GameObject pickedObject = null;
    private Vector3 originalCameraPosition; // To prevent camera bugs

    void Start()
    {
        // Store original camera position to prevent bugs
        originalCameraPosition = Camera.main.transform.localPosition;
    }

    void Update()
    {
        // Always show debug ray from hand
        Debug.DrawRay(handPoint.position, handPoint.forward * grabDistance, Color.green);

        // Grab with E (raycast-based, works in third person)
        if (Input.GetKeyDown(KeyCode.E) && pickedObject == null)
        {
            TryGrabObject();
        }

        // Drop with R
        if (pickedObject != null && Input.GetKeyDown(KeyCode.R))
        {
            DropObject();
        }

        // Prevent camera from moving to hand
        FixCameraPosition();
    }

    void TryGrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(handPoint.position, handPoint.forward, out hit, grabDistance))
        {
            if (hit.collider.CompareTag("WallLight"))
            {
                GrabObject(hit.collider.gameObject);
            }
        }
    }

    void GrabObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Prepare object for grabbing
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Parent to hand WITHOUT affecting scale
        obj.transform.SetParent(handPoint, true);
        obj.transform.localPosition = Vector3.zero; // Center on hand
        obj.transform.localRotation = Quaternion.identity;

        pickedObject = obj;
    }

    void DropObject()
    {
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        
        pickedObject.transform.SetParent(null);
        pickedObject = null;
    }

    void FixCameraPosition()
    {
        // Prevent camera from following the hand
        if (Camera.main.transform.localPosition != originalCameraPosition)
        {
            Camera.main.transform.localPosition = originalCameraPosition;
        }
    }
}
