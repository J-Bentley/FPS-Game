using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour {

    private Camera cam;
    [SerializeField] private LayerMask shopLayers;
    [SerializeField] private float interactRange;
    [SerializeField] private TextMeshProUGUI equipUI;
    [SerializeField] private GameObject shopMenu;
    public static bool isShopping = false;
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject riflePrefab;
    [SerializeField] private GameObject sniperPrefab;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private GameObject healthpackPrefab;
    [SerializeField] private float pistolCost;
    [SerializeField] private float rifleCost;
    [SerializeField] private float sniperCost;
    [SerializeField] private float shotgunCost;
    [SerializeField] private float healthpackCost;
    [SerializeField] private GameObject dropPoint;
    private AudioSource[] audioSources;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Start() {
        cam = Camera.main;
        audioSources = GetComponents<AudioSource>();
    }

    void Update() {
        if(isShopping) {
            Time.timeScale = 0.2f;
        } else {
            if(!GameManager.gamePaused) {
                Time.timeScale = 1f;
            }
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, interactRange, shopLayers)) {
            equipUI.enabled = true;
            equipUI.text = "[E] Shop";
            if (Input.GetKeyDown(KeyCode.E)) {
                shopMenu.SetActive(true);
                isShopping = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ExitShop() {
        shopMenu.SetActive(false);
        isShopping = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pistol() {
        if (Player.wallet >= pistolCost){ 
            Player.wallet -= pistolCost;
            moneyText.text = "$" + Player.wallet;
            Instantiate(pistolPrefab, dropPoint.transform.position, Quaternion.identity);
            audioSources[8].Play();
            ExitShop();
        } else {
            audioSources[9].Play();
        }
    }

    public void Rifle() {
        if (Player.wallet >= rifleCost){ 
            Player.wallet -= rifleCost;
            moneyText.text = "$" + Player.wallet;
            Instantiate(riflePrefab, dropPoint.transform.position, Quaternion.identity);
            audioSources[8].Play();
            ExitShop();
        } else {
            audioSources[9].Play();
        }
    }

    public void Sniper() {
        if (Player.wallet >= sniperCost){ 
            Player.wallet -= sniperCost;
            moneyText.text = "$" + Player.wallet;
            Instantiate(sniperPrefab, dropPoint.transform.position, Quaternion.identity);
            audioSources[8].Play();
            ExitShop();
        } else {
            audioSources[9].Play();
        }
    }

    public void Shotgun() {
        if (Player.wallet >= shotgunCost){ 
            Player.wallet -= shotgunCost;
            moneyText.text = "$" + Player.wallet;
            Instantiate(shotgunPrefab, dropPoint.transform.position, Quaternion.identity);
            audioSources[8].Play();
            ExitShop();
        } else {
            audioSources[9].Play();
        }
    }

    public void Healthpack() {
        if (Player.wallet >= healthpackCost){ 
            Player.wallet -= healthpackCost;
            moneyText.text = "$" + Player.wallet;
            Instantiate(healthpackPrefab, dropPoint.transform.position, Quaternion.identity);
            audioSources[8].Play();
            ExitShop();
        } else {
            audioSources[9].Play();
        }
    }
}
