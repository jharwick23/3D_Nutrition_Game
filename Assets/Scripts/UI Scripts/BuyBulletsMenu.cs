using UnityEngine;
using TMPro;

public class BuyBulletsMenu : MonoBehaviour
{
    private ProjectileGun _projectileGun;
    [SerializeField] private GameObject buyBulletPanel;
    private PlayerControllerV2 _playerController;
    private UIHandler _uiHandler;
    public TextMeshProUGUI OrangeButtonText;
    public TextMeshProUGUI TomatoButtonText;
    public TextMeshProUGUI BananaButtonText;
    public TextMeshProUGUI LemonButtonText;
    public TextMeshProUGUI CarrotButtonText;
    public TextMeshProUGUI DescriptionText;

    private void Start()
    {
        _projectileGun ??= FindFirstObjectByType<ProjectileGun>();
        _playerController ??= FindFirstObjectByType<PlayerControllerV2>();
        _uiHandler ??= FindFirstObjectByType<UIHandler>();
        DescriptionText.text = "Select a bullet to see its description.";
        UpdateUI();
    }

    public void EnableUpgradeMenu()
    {
        buyBulletPanel.SetActive(true);

        // Disable Crosshair
        UIHandler _uiHandler;
        _uiHandler = FindFirstObjectByType<UIHandler>();
        if (_uiHandler)
        {
            _uiHandler.ToggleCrosshair(false);
        }
        else
        {
            Debug.Log("UI Handler was not set!");
        }

        // Disable Inputs
        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.DisableInputsForVendors();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableUpgradeMenu()
    {
        buyBulletPanel.SetActive(false);

        // Enable Crosshair
        UIHandler _uiHandler;
        _uiHandler = FindFirstObjectByType<UIHandler>();
        if (_uiHandler)
        {
            _uiHandler.ToggleCrosshair(true);
        }
        else
        {
            Debug.Log("UI Handler was not set!");
        }

        // Enable Inputs
        InputHandlerV2 inputHandler = FindFirstObjectByType<InputHandlerV2>();
        if (inputHandler)
        {
            inputHandler.EnableInputs();
        }
        else
        {
            Debug.Log("Inputhandler not found!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool isUpgradeMenuActive()
    {
        return buyBulletPanel.activeSelf;
    }

    public void UpdateUI()
    {
        PlayerPrefs.Save();
        OrangeButtonText.text = "Owned";
        if (PlayerPrefs.GetInt("Bullet_Tomato", 0) == 1)
        {
            TomatoButtonText.text = "Owned";
        }
        else
        {
            TomatoButtonText.text = "Buy";
        }

        if (PlayerPrefs.GetInt("Bullet_Banana", 0) == 1)
        {
            BananaButtonText.text = "Owned";
        }
        else
        {
            BananaButtonText.text = "Buy";
        }

        if (PlayerPrefs.GetInt("Bullet_Lemon", 0) == 1)
        {
            LemonButtonText.text = "Owned";
        }
        else
        {
            LemonButtonText.text = "Buy";
        }

        if (PlayerPrefs.GetInt("Bullet_Carrot", 0) == 1)
        {
            CarrotButtonText.text = "Owned";
        }
        else
        {
            CarrotButtonText.text = "Buy";
        }
        
    }

    public void OnBuyBulletTomatoButton()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 300 || PlayerPrefs.GetInt("Bullet_Tomato", 0) == 1)
        {
            Debug.Log("Not enough coins to buy Tomato Bullets or you already own this bullet!");
            _uiHandler.SetAlertPrompt("Not enough coins to buy Tomato Bullets or you already own this bullet!", 2f, Color.red);
            return;
        }
        else
        {
            PlayerPrefs.SetInt("Bullet_Tomato", 1);
            _projectileGun.LoadOwnedBullets();
            _playerController.AddCoins(-300);
            UpdateUI();
            _uiHandler.SetAlertPrompt("Tomato Bullets Purchased!", 2f, Color.green);
        }
    }

    public void OnBuyBulletBananaButton()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 300 || PlayerPrefs.GetInt("Bullet_Banana", 0) == 1)
        {
            Debug.Log("Not enough coins to buy Banana Bullets or you already own this bullet!");
            _uiHandler.SetAlertPrompt("Not enough coins to buy Banana Bullets or you already own this bullet!", 2f, Color.red);
            return;
        }
        else
        {
            PlayerPrefs.SetInt("Bullet_Banana", 1);
            _projectileGun.LoadOwnedBullets();
            _playerController.AddCoins(-300);
            UpdateUI();
            _uiHandler.SetAlertPrompt("Banana Bullets Purchased!", 2f, Color.green);
        }
    }

    public void OnBuyBulletLemonButton()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 300 || PlayerPrefs.GetInt("Bullet_Lemon", 0) == 1)
        {
            Debug.Log("Not enough coins to buy Lemon Bullets or you already own this bullet!");
            _uiHandler.SetAlertPrompt("Not enough coins to buy Lemon Bullets or you already own this bullet!", 2f, Color.red);
            return;
        }
        else
        {
            PlayerPrefs.SetInt("Bullet_Lemon", 1);
            _projectileGun.LoadOwnedBullets();
            _playerController.AddCoins(-300);
            UpdateUI();
            _uiHandler.SetAlertPrompt("Lemon Bullets Purchased!", 2f, Color.green);
        }
    }

    public void OnBuyBulletCarrotButton()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 300 || PlayerPrefs.GetInt("Bullet_Carrot", 0) == 1)
        {
            Debug.Log("Not enough coins to buy Carrot Bullets or you already own this bullet!");
            _uiHandler.SetAlertPrompt("Not enough coins to buy Carrot Bullets or you already own this bullet!", 2f, Color.red);
            return;
        }
        else
        {
            PlayerPrefs.SetInt("Bullet_Carrot", 1);
            _projectileGun.LoadOwnedBullets();
            _playerController.AddCoins(-300);
            UpdateUI();
            _uiHandler.SetAlertPrompt("Carrot Bullets Purchased!", 2f, Color.green);
        }
    }

    public void OnWipeBulletsButton()
    {
        PlayerPrefs.DeleteKey("Bullet_Tomato");
        PlayerPrefs.DeleteKey("Bullet_Banana");
        PlayerPrefs.DeleteKey("Bullet_Lemon");
        PlayerPrefs.DeleteKey("Bullet_Carrot");
        _projectileGun.LoadOwnedBullets();
        UpdateUI();
        _uiHandler.SetAlertPrompt("Bullets Wiped!", 2f, Color.green);
    }

    public void SelectBulletDescription(string bulletType)
    {
        // Build string
        switch (bulletType)
        {
            case "Orange":
                DescriptionText.text = "Orange:\n\n- Low damage\n\n- Short cooldown\n\n- High speed\n\n- Short range";
                break;
            case "Tomato":
                DescriptionText.text = "Tomato:\n\n- Moderate damage\n\n- Long cooldown\n\n- Moderate speed\n\n- Moderate range";
                break;
            case "Banana":
                DescriptionText.text = "Banana:\n\n- Low damage\n\n- Short cooldown\n\n- High speed\n\n- Short range";
                break;
            case "Lemon":
                DescriptionText.text = "Lemon:\n\n- High damage\n\n- Short cooldown\n\n- Low speed\n\n- Long range";
                break;
            case "Carrot":
                DescriptionText.text = "Carrot:\n\n- Moderate damage\n\n- Short cooldown\n\n- Moderate speed\n\n- Long range";
                break;
            default:
                DescriptionText.text = "";
                break;
        }
    }
}
