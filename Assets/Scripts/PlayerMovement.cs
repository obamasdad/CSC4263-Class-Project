using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private Vector3 defaultPosition;
    private bool isGameOver = false;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text respawnText;
    [SerializeField] private Text healthText; // Reference to the Text UI element
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float endLevelX = 50f;
    [SerializeField] private float cameraSmoothTime = 0.15f; // Adjust for smoothness
    [SerializeField] private Vector3 cameraOffset;

    public bool canMove = true;

    private Vector3 cameraVelocity = Vector3.zero;

    private void Start()
    {
        defaultPosition = transform.position;
        currentHealth = maxHealth;
        UpdateHealthUI();

        gameOverUI.SetActive(false);
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Prevent physics jittering
    }

    private void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                RestartGame();
            }
            return; // 🚀 Ensure the player can still press "F" while game over
        }

        if (!canMove) return; // 🚀 Stop movement if "canMove" is false

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();

        // Check if player falls below the screen
        if (transform.position.y < -10f)
        {
            TakeDamage(currentHealth); // Set health to 0
        }
    }

    private void FixedUpdate()
    {
        if (!isGameOver && canMove)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

    private void LateUpdate()
    {
        SmoothCameraFollow();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        canMove = false;
        rb.linearVelocity = Vector2.zero; // 🚀 Stop momentum immediately
        rb.isKinematic = true;
        gameOverUI.SetActive(true);
        respawnText.text = "Press F to Respawn";
    }

    private void RestartGame()
    {
        isGameOver = false;
        canMove = true;
        rb.isKinematic = false;
        transform.position = defaultPosition;
        currentHealth = maxHealth;
        UpdateHealthUI();
        rb.linearVelocity = Vector2.zero;
        gameOverUI.SetActive(false);
    }

    private void SmoothCameraFollow()
    {
        if (cameraTransform != null)
        {
            float targetX = Mathf.Min(transform.position.x, endLevelX);
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, cameraTransform.position.z) + cameraOffset;

            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref cameraVelocity, cameraSmoothTime);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }
}
