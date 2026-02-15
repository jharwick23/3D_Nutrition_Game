using UnityEngine;
using UnityEngine.UI;

public class BulletTypeUIHandler : MonoBehaviour
{
    private Image _bulletTypeDisplay;
    public Sprite tomatoSprite;
    public Sprite orangeSprite;
    public Sprite carrotSprite;
    public Sprite bananaSprite;
    public Sprite lemonSprite;

    void Awake()
    {
        _bulletTypeDisplay = GetComponent<Image>();
        if (_bulletTypeDisplay == null)
        {
            Debug.LogError("No Image component found");
        }
    }

    public void UpdateBulletType(string bulletType)
    {
        if (_bulletTypeDisplay == null)
        {
            Debug.LogError("Bullet Type Display image isn't assigned.");
            return;
        }

        switch (bulletType)
        {
            case "Orange":
            _bulletTypeDisplay.sprite = orangeSprite;
            break;
            case "Tomato":
            _bulletTypeDisplay.sprite = tomatoSprite;
            break;
            case "Carrot":
            _bulletTypeDisplay.sprite = carrotSprite;
            break;
            case "Banana":
            _bulletTypeDisplay.sprite = bananaSprite;
            break;
            case "Lemon":
            _bulletTypeDisplay.sprite = lemonSprite;
            break;
            default:
            Debug.LogWarning($"Unknown bullet type: {bulletType}. Sprite wasn't updated!");
            break;
        }
        
    }
}
