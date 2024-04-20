using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; 
    public float jumpForce = 5f; 
    public bool isGrounded = false; 

    private Rigidbody2D rb; 
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        MovePlayer(moveInput);
        FlipSprite(moveInput);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) 
        {
            Jump();
        }
    }

    void MovePlayer(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); 


        animator.SetFloat("Speed", Mathf.Abs(moveInput)); 
    if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) 
    {
        if (moveInput < 0) 
        {
            moveInput = 0; 
            animator.SetFloat("Speed", 0); 
        }
    }

    if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) 
    {
        if (moveInput > 0)
        {
            moveInput = 0; 
            animator.SetFloat("Speed", 0);
        }
    }    
    
    }

    void FlipSprite(float moveInput)
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(3, 3, 1); 
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-3, 3, 1); 
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false; 
        animator.SetTrigger("Jump"); 
    }
    [SerializeField] private string Fight;
    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Ground") 
        {
            isGrounded = true;
        }
        
        if (collision.gameObject.tag == "Enemy") 
        {
            SceneManager.LoadScene(Fight);
        }
    }

}
