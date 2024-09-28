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

        // Suscríbete al evento de salto aquí en lugar de en OnEnable
        jumpAction.performed += HandleJump;
    }

    private void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
    }
    void HandleJump(InputAction.CallbackContext context)
    {
        // Solo saltar si estamos en el suelo
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si tocamos el suelo nuevamente
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}