using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public bool isGrounded = false; // Flag to check if grounded

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Get horizontal input (-1 for left, 1 for right)

        MovePlayer(moveInput);
        FlipSprite(moveInput);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Check for space key press and grounded state
        {
            Jump();
        }
    }

    void MovePlayer(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Set velocity based on input and maintain vertical velocity

        // Update animation parameter based on movement (Optional)
        animator.SetFloat("Speed", Mathf.Abs(moveInput)); // Set "Speed" parameter in Animator based on absolute movement value (0 for idle, positive for right, negative for left)
    if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) // Check for "A" or Left Arrow key release
    {
        if (moveInput < 0) // If moving left
        {
            moveInput = 0; // Set moveInput to 0 to stop movement
            animator.SetFloat("Speed", 0); // Set "Speed" parameter to 0 for idle animation (optional)
        }
    }

    if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) // Check for "D" or Right Arrow key release
    {
        if (moveInput > 0) // If moving right
        {
            moveInput = 0; // Set moveInput to 0 to stop movement
            animator.SetFloat("Speed", 0); // Set "Speed" parameter to 0 for idle animation (optional)
        }
    }    
    
    }

    void FlipSprite(float moveInput)
    {
        if (moveInput > 0) // Moving right
        {
            transform.localScale = new Vector3(3, 3, 1); // Set scale to positive (facing right)
        }
        else if (moveInput < 0) // Moving left
        {
            transform.localScale = new Vector3(-3, 3, 1); // Set scale to negative (facing left)
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply jump force
        isGrounded = false; // Set grounded flag to false
        animator.SetTrigger("Jump"); // Trigger "Jump" animation in Animator
    }

    void OnCollisionEnter2D(Collision2D collision) // Detect collision for grounding
    {
        if (collision.gameObject.tag == "Ground") // Check if colliding with ground
        {
            isGrounded = true;
        }
    }
}
