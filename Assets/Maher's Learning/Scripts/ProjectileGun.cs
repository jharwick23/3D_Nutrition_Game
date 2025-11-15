using UnityEngine;
using System.Collections;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    // SETUP REFERENCES
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public Camera PlayerCamera;
    public TextMeshProUGUI AmmoDisplay;
    public float MaxDistance = 100f;

    // AMMO AND COOLDOWNS
    public int maxAmmo;
    public int currentAmmo;
    public float timeBetweenShooting = 0.5f;
    public float reloadTime = 1f;

    // STATE TRACKING
    private bool canShoot = true;
    private bool isReloading = false;
    // public float ShootForce = 150f; // OLD
    
    private void Awake()
    {
        maxAmmo = BulletPrefab.GetComponent<StandardBullet>().maxAmmo;
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    public void Shoot()
    {
        if (!canShoot || currentAmmo <= 0 || isReloading)
            return;

        // Start cooldown and reduce ammo
        StartCoroutine(ShootCooldown());
        currentAmmo--;
        UpdateAmmoDisplay();


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
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.LookRotation(shootDirection));
        //bullet.GetComponent<Rigidbody>().AddForce(shootDirection.normalized * ShootForce, ForceMode.Impulse);
        // Destroy(bullet, bullet.GetComponent<StandardBullet>().LifeTime);
        bullet.GetComponent<StandardBullet>().Init(shootDirection);
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
        UpdateAmmoDisplay("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmo;
        canShoot = true;
        UpdateAmmoDisplay();
    }

    public void UpdateAmmoDisplay(string ammoText = "")
    {
        if (AmmoDisplay != null)
        if (ammoText != "")
        {
            AmmoDisplay.text = ammoText;
        }
        else
        {
            AmmoDisplay.text = $"{currentAmmo} / Inf";
        }
    }
}
