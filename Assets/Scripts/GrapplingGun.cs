using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrapple;

    private bool canGapple;

    public Transform gunTip;
    public Transform cam;
    public Transform player;

    private float maxDistance = 100f;
    private SpringJoint joint;

    public void OnGrappleStart(InputAction.CallbackContext context)
    {
        StartGrapple();
    }

    public void OnGrappleStop(InputAction.CallbackContext context)
    {
        StopGrapple();
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        StopGrapple();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, whatIsGrapple))
        {
            canGapple = true;
        }
        else
        {
            canGapple = false;
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        if (joint != null) return;

        if (canGapple)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance))
            {
                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                //Distance grapple will try to keep from grapple point.
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.1f;

                joint.spring = 2f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
            }
        }
    }

        void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}