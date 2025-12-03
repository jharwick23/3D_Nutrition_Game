using UnityEngine;
using System.Collections;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    // --- SETUP REFERENCES --- \\
    public GameObject StandardBulletPrefab;
    public GameObject TomatoBulletPrefab;
    public Transform BulletSpawnPoint;
    public Camera PlayerCamera;
    public UIHandler _uiHandler;
    public PlayerControllerV2 PlayerController;
    public float MaxDistance = 100f;

    // --- AMMO/Type AND COOLDOWNS --- \\
    public enum BulletType { Standard, Tomato }
    public BulletType CurrentBulletType = BulletType.Standard;
    public int maxAmmo;
    public int currentAmmo;
    public float timeBetweenShooting = 0.5f;
    public float reloadTime = 1f;

    // --- STATE TRACKING --- \\
    private bool canShoot = true;
    private bool isReloading = false;
    // public float ShootForce = 150f; // OLD
    
    private void Start()
    {
        maxAmmo = StandardBulletPrefab.GetComponent<StandardBullet>().maxAmmo;
        currentAmmo = maxAmmo;
        if (PlayerCamera == null)
        {
            PlayerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        if (_uiHandler == null)
        {
            _uiHandler = FindFirstObjectByType<UIHandler>();
        }
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");
        
        if (PlayerController == null)
        {
            PlayerController = FindFirstObjectByType<PlayerControllerV2>();
        }
    }

    public void Shoot()
    {
        if (!canShoot || currentAmmo <= 0 || isReloading || 
            !PlayerController.GetEquipped() || PlayerController.GetBlocking()
            || PlayerController.GetMeleeAttacking())
            return;

        // Start cooldown and reduce ammo
        StartCoroutine(ShootCooldown());
        currentAmmo--;
        _uiHandler.UpdateAmmoUI(currentAmmo.ToString() + " / Inf");


        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxDistance))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * MaxDistance;
        }

        Vector3 shootDirection = (targetPoint - BulletSpawnPoint.position).normalized;

        // Check bullet type and instantiate accordingly
        if (CurrentBulletType == BulletType.Standard)
        {
            GameObject bullet = Instantiate(StandardBulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<StandardBullet>().Init(shootDirection);
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
        if (CurrentBulletType == BulletType.Standard)
        {
            CurrentBulletType = BulletType.Tomato;
        }
        else
        {
            CurrentBulletType = BulletType.Standard;
        }
    }   
}
