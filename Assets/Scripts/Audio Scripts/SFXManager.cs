using UnityEngine;

public class SFXManager: MonoBehaviour
{
    public static SFXManager Instance;
    public AudioSource audioSource;
    public enum SFXType
    {
        KnifeSwing,
        DamageTaken,
        Death,
        Heal,
        Shoot,
        CollectCoin,
        SugarBullet,
        SodaBullet,
        CrashIntoWall,
        EnemyExploding
    }

    [Header("Player")]
    public AudioClip knifeSwing;
    public AudioClip damageTaken;
    public AudioClip death;
    public AudioClip heal;
    public AudioClip shoot;
    public AudioClip collectCoin;
    public AudioClip sugarBullet;
    public AudioClip sodaBullet;
    public AudioClip crashIntoWall;
    public AudioClip enemyExploding;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (!audioSource)
        {
            Debug.LogError("SFXManager is missing an AudioSource!");
        }
    }

    public void Play(SFXType type)
    {
        if (!audioSource) return;

        switch (type)
        {
            case SFXType.KnifeSwing:
                audioSource.PlayOneShot(knifeSwing);
                break;
            
            case SFXType.DamageTaken:
                audioSource.PlayOneShot(damageTaken);
                break;

            case SFXType.Death:
                audioSource.PlayOneShot(death);
                break;

            case SFXType.Heal:
                audioSource.PlayOneShot(heal, 0.75f);
                break;

            case SFXType.Shoot:
                audioSource.PlayOneShot(shoot);
                break;
            
            case SFXType.CollectCoin:
                audioSource.PlayOneShot(collectCoin);
                break;
            case SFXType.SugarBullet:
                audioSource.PlayOneShot(sugarBullet);
                break;
            case SFXType.SodaBullet:
                audioSource.PlayOneShot(sodaBullet);
                break;
            case SFXType.CrashIntoWall:
                audioSource.PlayOneShot(crashIntoWall);
                break;
            case SFXType.EnemyExploding:
                audioSource.PlayOneShot(enemyExploding, 10f);
                break;
        }
    }
}
