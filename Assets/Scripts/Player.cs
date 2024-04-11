using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour {
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintFov;
    [SerializeField] private float staminaRegen;
    private Vector3 velocity;
    public bool isGrounded;
    public float maxHealth = 100f;
    public float currentStamina;
    public float maxStamina = 10f;
    public float currentHealth;
    public Slider staminaBar;
    public Slider healthBar;
    private float originalSpeed;
    private AudioSource[] audioSources;
    public static float wallet = 10f;
    [SerializeField] private TextMeshProUGUI moneyText;
    public GameManager gameManagerScript;

    void Start () {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        audioSources = GetComponents<AudioSource>();
        originalSpeed = speed;
    }

    void Update() {
        
        staminaBar.value = currentStamina;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    
        if (!GameManager.gamePaused && Input.GetKey("left shift") && controller.velocity.magnitude > 0.1f) {
            if (currentStamina > 0f) {
                speed = sprintSpeed;
                currentStamina -= Time.deltaTime;   
            }

            if (currentStamina <= 0f) {
                speed = originalSpeed;
                currentStamina = 0;
                if (!audioSources[1].isPlaying) {
                    audioSources[1].Play(); //no stamina sound
                }
            }
        } else if (currentStamina < maxStamina) {
            currentStamina += staminaRegen * Time.deltaTime;            
        }
        
        if (!Input.GetKey("left shift")) {
            speed = originalSpeed;
        }

        if (!GameManager.gamePaused && Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioSources[2].pitch = Random.Range(0.8f, 1.2f); audioSources[2].Play(); //jump sound
        }

        if (controller.velocity.magnitude > 0.1f) {
            if (isGrounded && !audioSources[0].isPlaying) {
                audioSources[0].pitch = Random.Range(0.9f, 1.1f); audioSources[0].Play(); //footsteps sound
            }
        } else {
            audioSources[0].Stop(); 
        }

        if (!isGrounded && audioSources[0].isPlaying) { 
            audioSources[0].Stop();
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ReceiveHealing (float heals) {
        currentHealth += heals;
        if (currentHealth > maxHealth) { 
            currentHealth = maxHealth;
        }
        healthBar.value = currentHealth;
    }

    public void ReceiveStamina (float stamina) {
        currentStamina += stamina;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
    }

    public void ReceiveMoney(float money) {
        wallet += money;
        moneyText.text = "$" + wallet;
    }

    public void TakeDamage (float damage) {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if(!audioSources[3].isPlaying) { 
            audioSources[3].pitch = Random.Range(0.8f, 1.2f);
            audioSources[3].Play(); //take damage sound
        }
        if (currentHealth <= 0) {
            Death();
        }
    }

    public void Jumpad(float jumpadHeight) {
        velocity.y = jumpadHeight;
    }

    void Death() {
        SceneManager.LoadScene(2);
    }
}
