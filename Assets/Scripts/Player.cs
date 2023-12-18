using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera fpsCam;
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
    private AudioSource[] audioSources; //order: footsteps, out of breath, jump, take damage, heal, background music
    public Animator animator = null;

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
                audioSources[5].Play(); //loops background music
        }

        healthBar.value = currentHealth;
        staminaBar.value = currentStamina;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            //resets velocity when grounded, -2 for reasons
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    
        if (Input.GetKey("left shift") && controller.velocity.magnitude > 0.1f) { //Prevents using stamina if standing still.
            if (currentStamina > 0f) {
                speed = sprintSpeed;
                currentStamina -= Time.deltaTime;
                fpsCam.fieldOfView = sprintFov;

                if (gunEquipped) {
                    animator.SetTrigger("onSprint");
                }    
            }

            if (currentStamina <= 0f) {
                speed = originalSpeed;
                fpsCam.fieldOfView = defaultFov;
                currentStamina = 0;
                if (!audioSources[1].isPlaying) {
                    audioSources[1].Play(); //no stamina sound
                }
            }

        } else if (currentStamina < maxStamina) {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        if (!Input.GetKey("left shift")) {
            fpsCam.fieldOfView = defaultFov;
            speed = originalSpeed;
        }

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioSources[2].Play(); //jump sound
        }

        if (controller.velocity.magnitude > 0.1f) {
            if (isGrounded && !audioSources[0].isPlaying) {
                audioSources[0].Play(); //play footsteps when moving and grounded
            }
        } else {
            audioSources[0].Stop(); //stop footsteps if not grounded or not moving
        }
        
        if (!isGrounded && audioSources[0].isPlaying) {
            audioSources[0].Stop(); //stop footsteps when not grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ReceiveHealing (float heals) {
        currentHealth += heals;
        if (currentHealth > maxHealth){ 
            currentHealth = maxHealth;
        }
    }

    public void ReceiveStamina (float stamina) {
        currentStamina += stamina;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
    }

    public void TakeDamage (float damage) {
        currentHealth -= damage;
        if(!audioSources[3].isPlaying) { //stops audio from being triggered multiple times and not play
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
        SceneManager.LoadScene(2); //loads gameOver scene
    }
}
