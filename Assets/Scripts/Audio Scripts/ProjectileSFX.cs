using UnityEngine;

public class ProjectileSFX : MonoBehaviour
{
    public AudioClip splatSound;

    // Uses different SFX script than other SFX
    // Because the object get's destroyed the audio must create
    // a game object of it's own play the audio then destroy itself
    public void Play()
    {
        if (!splatSound) return;

        GameObject projectileAudio = new GameObject("ProjectileImpactSound");
        projectileAudio.transform.position = transform.position;

        AudioSource source = projectileAudio.AddComponent<AudioSource>();
        source.clip = splatSound;
        source.spatialBlend = 1f;
        source.dopplerLevel = 0f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.volume = 1f;
        source.Play();

        Destroy(projectileAudio, splatSound.length);
    }
}
