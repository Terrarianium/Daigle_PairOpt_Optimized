using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrapple;

    public Transform gunTip;
    public Transform cam;
    public Transform player;

    private float maxDistance = 100f;
    private SpringJoint joint;

    public void OnGrappleStart(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        StartGrapple();
    }

    public void OnGrappleStop(InputAction.CallbackContext context)
    {
        if (!context.canceled) return;
        StopGrapple();
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        StopGrapple();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        if (joint != null)
        {
            StopGrapple(); // remove old joint first
        }

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, whatIsGrapple))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //Distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.1f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
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