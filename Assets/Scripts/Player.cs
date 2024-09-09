using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour {
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float sprintSpeed;
    [SerializeField] float staminaRegen;
    [SerializeField] TextMeshProUGUI walletText;
    public static CharacterController controller;
    public static Player instance;
    public static int wallet;
    public bool isGrounded;
    public float maxHealth = 100f;
    public static float currentStamina;
    public float maxStamina = 10f;
    public float currentHealth;
    public Slider staminaBar;
    public Slider healthBar;
    Vector3 velocity;
    float originalSpeed;
    AudioSource[] audioSources;
    Vector3 platformVelocity;
    Transform currentPlatform;
    Vector3 lastPlatformPosition;

    void Awake() {
        if (instance == null){
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start () {
        controller = GetComponent<CharacterController>();
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

         if (currentPlatform != null) {
            platformVelocity = currentPlatform.position - lastPlatformPosition;
            controller.Move(platformVelocity); 
            lastPlatformPosition = currentPlatform.position;
        }
    
        if (!GameManager.gamePaused && Input.GetKey("left shift") && controller.velocity.magnitude > 0.1f) {
            if (currentStamina > 0f) {
                speed = sprintSpeed;
                currentStamina -= Time.deltaTime;   
            }

            if (currentStamina <= 0f) {
                speed = originalSpeed;
                currentStamina = 0;
                if (!audioSources[0].isPlaying) {
                    audioSources[0].Play(); //no stamina sound
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
            audioSources[1].pitch = Random.Range(0.8f, 1.2f);
            audioSources[1].Play(); //jump sound
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
        audioSources[4].Play(); //heals sound
    }

    public void ReceiveStamina (float stamina) {
        currentStamina += stamina;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
    }

    public void ReceiveMoney(int money) { 
        wallet += money;
        walletText.SetText("$" + wallet.ToString()); 
        Debug.Log("Received $" + money + ". Wallet is $" + wallet);
    }

    public void SpendMoney(int money) {
        wallet -= money;
        walletText.SetText("$" + wallet.ToString());
        audioSources[3].Play(); //cash register sound
        Debug.Log("Spent $" + money + ". Wallet is $" + wallet);
    }

    public void TakeDamage (float damage) {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if(!audioSources[2].isPlaying) { 
            audioSources[2].pitch = Random.Range(0.8f, 1.2f);
            audioSources[2].Play(); //take damage sound
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

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.tag == "MovingPlatform") {
            if (hit.transform != currentPlatform) {
                currentPlatform = hit.transform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "MovingPlatform") {
            currentPlatform = null;
            platformVelocity = Vector3.zero;
        }
    }
}
