using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 5;
    private bool isGrounded = true;
    private Animator animator;
    Rigidbody rb;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

        if (jumpAction == null)
        {
            Debug.LogError("Jump action not found!");
            return;
        }

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        jumpAction.performed += HandleJump;
    }

    private void Update()
    {
        MovePlayer();
        
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
        transform.position += movement;

        if (movement.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void HandleJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            animator.SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        animator.SetBool("isJumping", false);
    }
}