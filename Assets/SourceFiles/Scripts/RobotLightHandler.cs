using UnityEngine;

public class RobotLightHandler : MonoBehaviour
{
    [Header("ASSIGN THESE")]
    public Transform handPoint; 
    public float grabDistance = 2f; 

    private GameObject pickedObject = null;
    private Vector3 originalCameraPosition; 

    void Start()
    {
        originalCameraPosition = Camera.main.transform.localPosition;
    }

    void Update()
    {
        Debug.DrawRay(handPoint.position, handPoint.forward * grabDistance, Color.green);

        if (Input.GetKeyDown(KeyCode.E) && pickedObject == null)
        {
            TryGrabObject();
        }

        if (pickedObject != null && Input.GetKeyDown(KeyCode.R))
        {
            DropObject();
        }

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

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        obj.transform.SetParent(handPoint, true);
        obj.transform.localPosition = Vector3.zero;
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
        if (Camera.main.transform.localPosition != originalCameraPosition)
        {
            Camera.main.transform.localPosition = originalCameraPosition;
        }
    }
}
