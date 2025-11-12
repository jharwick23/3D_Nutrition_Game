using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    // Bullet
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public Camera PlayerCamera;
    public float MaxDistance = 100f;
    // public float ShootForce = 150f;
    
    public void Shoot()
    {
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
        Destroy(bullet, bullet.GetComponent<StandardBullet>().LifeTime);
        bullet.GetComponent<StandardBullet>().Init(shootDirection);
    }
}
