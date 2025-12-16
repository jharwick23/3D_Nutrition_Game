using UnityEngine;
using System.Collections;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    // --- SETUP REFERENCES --- \\
    public GameObject OrangeBulletPrefab;
    public GameObject TomatoBulletPrefab;
    public Transform BulletSpawnPoint;
    public Camera PlayerCamera;
    public UIHandler _uiHandler;
    public PlayerControllerV2 PlayerController;
    public float MaxDistance = 100f;
    public LayerMask raycastMask;

    // --- AMMO/Type AND COOLDOWNS --- \\
    public enum BulletType { Tomato, Orange }
    public BulletType CurrentBulletType = BulletType.Orange;
    public int maxAmmo;
    public int currentAmmo;
    public float timeBetweenShooting;
    public float reloadTime = 1f;

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
        maxAmmo = OrangeBulletPrefab.GetComponent<OrangeBullet>().maxAmmo;
        currentAmmo = maxAmmo;
        timeBetweenShooting = OrangeBulletPrefab.GetComponent<OrangeBullet>().timeBetweenShooting;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");
    }

    public void Shoot()
    {
        if (!canShoot || currentAmmo <= 0 || isReloading || 
            !PlayerController.GetHatEquipped() || PlayerController.GetBlocking()
            || PlayerController.GetMeleeAttacking())
            return;

        // Start cooldown and reduce ammo
        StartCoroutine(ShootCooldown());
        ShootSound();
        currentAmmo--;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");

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

        // Check bullet type and instantiate accordingly
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

        // OLD
        //bullet.GetComponent<Rigidbody>().AddForce(shootDirection.normalized * ShootForce, ForceMode.Impulse);
        // Destroy(bullet, bullet.GetComponent<StandardBullet>().LifeTime);
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
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmo;
        canShoot = true;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");
    }

    public void SwitchBulletType()
    {
        if (CurrentBulletType == BulletType.Tomato)
        {
            CurrentBulletType = BulletType.Orange;
            _uiHandler.UpdateBulletTypeUI("Orange");
            timeBetweenShooting = OrangeBulletPrefab.GetComponent<OrangeBullet>().timeBetweenShooting;
        }
        else
        {
            CurrentBulletType = BulletType.Tomato;
            _uiHandler.UpdateBulletTypeUI("Tomato");
            timeBetweenShooting = TomatoBulletPrefab.GetComponent<TomatoBullet>().timeBetweenShooting;
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
