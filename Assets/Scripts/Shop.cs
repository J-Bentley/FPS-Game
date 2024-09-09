using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour {

    private Camera cam;
    [SerializeField] LayerMask shopLayers;
    [SerializeField] float interactRange;
    [SerializeField] TextMeshProUGUI shopText;
    [SerializeField] GameObject shopMenu;
    public static bool isShopping = false;
    [SerializeField] GameObject pistolPrefab;
    [SerializeField] GameObject riflePrefab;
    [SerializeField] GameObject sniperPrefab;
    [SerializeField] GameObject shotgunPrefab;
    [SerializeField] GameObject healthpackPrefab;
    [SerializeField] int pistolCost;
    [SerializeField] int rifleCost;
    [SerializeField] int sniperCost;
    [SerializeField] int shotgunCost;
    [SerializeField] int healthpackCost;
    [SerializeField] GameObject dropPoint;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if(isShopping) { 
            Time.timeScale = 0f; // pause time when shopping
        } else {
            if(!GameManager.gamePaused) {
                Time.timeScale = 1f;
            }
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, interactRange, shopLayers)) { // search for shop layers
            shopText.text = "[E] Shop";
            shopText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) {
                OpenShop();
            }
        } else {
            shopText.enabled = false;
        }
    }

    void OpenShop() {
        shopMenu.SetActive(true);
        isShopping = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ExitShop() {
        shopMenu.SetActive(false);
        isShopping = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pistol() {
        if (Player.wallet >= pistolCost){ 
            Player.instance.SpendMoney(pistolCost);
            Instantiate(pistolPrefab, dropPoint.transform.position, Quaternion.identity);
            ExitShop();
        }
    }

    public void Rifle() {
        if (Player.wallet >= rifleCost){ 
            Player.instance.SpendMoney(rifleCost);
            Instantiate(riflePrefab, dropPoint.transform.position, Quaternion.identity);
            ExitShop();
        }
    }

    public void Sniper() {
        if (Player.wallet >= sniperCost){ 
            Player.instance.SpendMoney(sniperCost);
            Instantiate(sniperPrefab, dropPoint.transform.position, Quaternion.identity);
            ExitShop();
        }
    }

    public void Shotgun() {
        if (Player.wallet >= shotgunCost){ 
            Player.instance.SpendMoney(shotgunCost);
            Instantiate(shotgunPrefab, dropPoint.transform.position, Quaternion.identity);
            ExitShop();
        }
    }

    public void Healthpack() {
        if (Player.wallet >= healthpackCost){ 
            Player.instance.SpendMoney(healthpackCost);
            Instantiate(healthpackPrefab, dropPoint.transform.position, Quaternion.identity);
            ExitShop();
        }
    }
}
