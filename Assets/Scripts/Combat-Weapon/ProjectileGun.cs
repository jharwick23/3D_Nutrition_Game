using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    // --- SETUP REFERENCES --- \\
    public GameObject OrangeBulletPrefab;
    public GameObject TomatoBulletPrefab;
    public GameObject BananaBulletPrefab;
    public GameObject LemonBulletPrefab;
    public GameObject CarrotBulletPrefab;
    public Transform BulletSpawnPoint;
    public Camera PlayerCamera;
    public UIHandler _uiHandler;
    public PlayerControllerV2 PlayerController;
    public float MaxDistance = 100f;
    public LayerMask raycastMask;

    // --- AMMO/Type AND COOLDOWNS --- \\
    public enum BulletType { Tomato, Orange, Banana, Lemon, Carrot }
    private List<BulletType> _ownedBullets = new List<BulletType>();
    private int _currentBulletIndex = 0;
    public BulletType CurrentBulletType = BulletType.Orange;
    public int maxAmmo;
    public int currentAmmo;
    public float timeBetweenShooting;
    private float _reloadTime = 2f;

    // --- STATE TRACKING --- \\
    private bool canShoot = true;
    private bool isReloading = false;
    // public float ShootForce = 150f; // OLD

    void Awake()
    {
        if (PlayerCamera == null)
        {
            PlayerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        if (_uiHandler == null)
        {
            _uiHandler = FindFirstObjectByType<UIHandler>();
        }
        
        if (PlayerController == null)
        {
            PlayerController = FindFirstObjectByType<PlayerControllerV2>();
        }
    }
    
    private void Start()
    {
        LoadOwnedBullets();
        maxAmmo = OrangeBulletPrefab.GetComponent<OrangeBullet>().maxAmmo;
        currentAmmo = maxAmmo;
        timeBetweenShooting = OrangeBulletPrefab.GetComponent<OrangeBullet>().timeBetweenShooting;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");
    }

    public void Shoot()
    {
        // -- Shooting Preconditions -- \\
        if (!canShoot || isReloading || 
            !PlayerController.GetHatEquipped() || PlayerController.GetBlocking()
            || PlayerController.GetMeleeAttacking())
            return;

        if (currentAmmo <= 0)
        {
            StartReloading();
            return;
        }

        // -- Shoot Cooldown and Ammo Management -- \\
        StartCoroutine(ShootCooldown());
        ShootSound();
        currentAmmo--;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");

        // -- Shooting Logic -- \\
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxDistance, raycastMask))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * MaxDistance;
        }

        Vector3 shootDirection = (targetPoint - BulletSpawnPoint.position).normalized;

        // -- Instantiate Current Bullet -- \\
        if (CurrentBulletType == BulletType.Orange)
        {
            GameObject bullet = Instantiate(OrangeBulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<OrangeBullet>().Init(shootDirection);
        }
        else if (CurrentBulletType == BulletType.Tomato)
        {
            GameObject bullet =  Instantiate(TomatoBulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<TomatoBullet>().Init(shootDirection);
        }
        else if (CurrentBulletType == BulletType.Banana)
        {
            GameObject bullet =  Instantiate(BananaBulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<BananaBullet>().Init(shootDirection);
        }
        else if (CurrentBulletType == BulletType.Lemon)
        {
            GameObject bullet =  Instantiate(LemonBulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<LemonBullet>().Init(shootDirection);
        }
        else if (CurrentBulletType == BulletType.Carrot)
        {
            GameObject bullet =  Instantiate(CarrotBulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<CarrotBullet>().Init(shootDirection);
        }
    }

    public void LoadOwnedBullets()
    {
        _ownedBullets.Clear();

        if (PlayerPrefs.GetInt("Bullet_Orange", 1) == 1)
        {
            _ownedBullets.Add(BulletType.Orange);
        }
        if (PlayerPrefs.GetInt("Bullet_Tomato", 0) == 1)
        {
            _ownedBullets.Add(BulletType.Tomato);
        }
        if (PlayerPrefs.GetInt("Bullet_Banana", 0) == 1)
        {
            _ownedBullets.Add(BulletType.Banana);
        }
        if (PlayerPrefs.GetInt("Bullet_Lemon", 0) == 1)
        {
            _ownedBullets.Add(BulletType.Lemon);
        }
        if (PlayerPrefs.GetInt("Bullet_Carrot", 0) == 1)
        {
            _ownedBullets.Add(BulletType.Carrot);
        }

        _currentBulletIndex = 0;
        CurrentBulletType = _ownedBullets[0];
        UpdateBulletStats();
    }

    public void StartReloading()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            PlayerController.SetHatEquipped(true);
            PlayerController._hatHandler.SetOnGun();
            PlayerController.SetLastShootingAttackTime();
            StartCoroutine(Reload());
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShooting);
        canShoot = true;
    }

    private IEnumerator Reload()
    {  
        canShoot = false;
        isReloading = true;
        _uiHandler.UpdateAmmoUI("Reloading...");

        float finalReloadTime = _reloadTime - PlayerPrefs.GetInt("ReloadSpeedStat", 0) * 0.1f; // 0.1 seconds faster per stat level
        if (finalReloadTime < 0.2f)
        {
            finalReloadTime = 0.2f; // Minimum reload time
        }
        yield return new WaitForSeconds(finalReloadTime);

        isReloading = false;
        currentAmmo = maxAmmo;
        canShoot = true;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");
    }

    public void SwitchBulletType()
    {
        if (_ownedBullets.Count <= 1)
        {
            return;
        }

        _currentBulletIndex++;

        if (_currentBulletIndex >= _ownedBullets.Count)
        {
            _currentBulletIndex = 0;
        }

        CurrentBulletType = _ownedBullets[_currentBulletIndex];
        UpdateBulletStats();
    }

    private void UpdateBulletStats()
    {
        switch (CurrentBulletType)
        {
            case BulletType.Orange:
                maxAmmo = OrangeBulletPrefab.GetComponent<OrangeBullet>().maxAmmo;
                timeBetweenShooting = OrangeBulletPrefab.GetComponent<OrangeBullet>().timeBetweenShooting;
                _uiHandler.UpdateBulletTypeUI("Orange");
                break;
            case BulletType.Tomato:
                maxAmmo = TomatoBulletPrefab.GetComponent<TomatoBullet>().maxAmmo;
                timeBetweenShooting = TomatoBulletPrefab.GetComponent<TomatoBullet>().timeBetweenShooting;
                _uiHandler.UpdateBulletTypeUI("Tomato");
                break;
            case BulletType.Banana:
                maxAmmo = BananaBulletPrefab.GetComponent<BananaBullet>().maxAmmo;
                timeBetweenShooting = BananaBulletPrefab.GetComponent<BananaBullet>().timeBetweenShooting;
                _uiHandler.UpdateBulletTypeUI("Banana");
                break;
            case BulletType.Lemon:
                maxAmmo = LemonBulletPrefab.GetComponent<LemonBullet>().maxAmmo;
                timeBetweenShooting = LemonBulletPrefab.GetComponent<LemonBullet>().timeBetweenShooting;
                _uiHandler.UpdateBulletTypeUI("Lemon");
                break;
            case BulletType.Carrot:
                maxAmmo = CarrotBulletPrefab.GetComponent<CarrotBullet>().maxAmmo;
                timeBetweenShooting = CarrotBulletPrefab.GetComponent<CarrotBullet>().timeBetweenShooting;
                _uiHandler.UpdateBulletTypeUI("Carrot");
                break;
        }
    }   

    private void ShootSound()
    {
        if (!SFXManager.Instance)
        {
            Debug.LogError("SFXManager not found in scene");
            return;
        }

        SFXManager.Instance.Play(SFXManager.SFXType.Shoot);
    }
}
