using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private float fovSpeed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float gravity = -35f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float sprintSpeed = 16f;
    [SerializeField] private float sprintFov = 100f;
    [SerializeField] private float staminaRegen = 1f;
    private Vector3 velocity;
    private bool isGrounded;
    private float defaultFov;
    public float maxHealth = 100f;
    public float currentStamina;
    public float maxStamina = 10f;
    public float currentHealth;
    public Slider staminaBar;
    public Slider healthBar;
    public bool gunEquipped = false;
    private float originalSpeed;
    private AudioSource[] audioSources;
    public static float totalMoney = 0f;
    [SerializeField] private TextMeshProUGUI moneyText;
    public GameManager gameManagerScript;

    void Start () {
        defaultFov = fpsCam.fieldOfView;
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
        if (!audioSources[5].isPlaying) {
            audioSources[5].Play(); //loop background music
        }
        
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
                fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, sprintFov, fovSpeed * Time.deltaTime);
                currentStamina -= Time.deltaTime;
                   
            }

            if (currentStamina <= 0f) {
                speed = originalSpeed;
                fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, defaultFov, fovSpeed * Time.deltaTime);
                currentStamina = 0;
                if (!audioSources[1].isPlaying) {
                    audioSources[1].Play(); //no stamina sound
                }
            }
        } else if (currentStamina < maxStamina) {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        if (!Input.GetKey("left shift")) {
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, defaultFov, fovSpeed * Time.deltaTime);
            speed = originalSpeed;
        }

        if (!GameManager.gamePaused && Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioSources[2].Play(); //jump sound
        }

        if (controller.velocity.magnitude > 0.1f) {
            if (isGrounded && !audioSources[0].isPlaying) {
                audioSources[0].Play(); //footsteps sound
            }
        } else {
            audioSources[0].Stop(); 
        }
        
        if (!isGrounded && audioSources[0].isPlaying) audioSources[0].Stop();

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
        totalMoney += money;
        moneyText.text = "$" + totalMoney;
    }

    public void TakeDamage (float damage) {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if(!audioSources[3].isPlaying) { 
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
