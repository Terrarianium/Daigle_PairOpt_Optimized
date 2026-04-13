using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject camHolder;
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float maxForce;
    private Vector2 move, look;
    private float lookRotation;

    [SerializeField] float jumpForce;

    [SerializeField] private float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    public bool grounded;
    [SerializeField] private float groundDrag;

    private float yRotation;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }*/

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    public void Move()
    {
        //Target Velocity
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;

        //Align direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcuate forces
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        //Limit force
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void Look()
    {
        // Mouse input
        float mouseX = look.x * sensitivity;
        float mouseY = look.y * sensitivity;

        // Yaw (player rotates left/right)
        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // Pitch (camera up/down)
        lookRotation -= mouseY;
        lookRotation = Mathf.Clamp(lookRotation, -90f, 90f);

        camHolder.transform.localRotation = Quaternion.Euler(lookRotation, 0f, 0f);
    }

    public void Jump()
    {
        Vector3 jumpForces = Vector3.zero;

        if (grounded)
        {
            jumpForces = Vector3.up * jumpForce;
        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded) 
        { 
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }
}