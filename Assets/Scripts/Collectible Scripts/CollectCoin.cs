using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public float bobbleHeight = 0.25f;
    public float bobbleSpeed = 2f;
    public int coinAmount = 1;
    private Vector3 startPos;

    private void Start()
    {
        // Save starting position
        startPos = transform.position;
    }

    private void Update()
    {
        // sine wave offset to y
        float newY = startPos.y + Mathf.Sin(Time.time * bobbleSpeed) * bobbleHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // rotate
        transform.Rotate(Vector3.forward * 50f * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.GetComponent<PlayerControllerV2>();
            if (playerController != null)
            {
                CollectSound();
                playerController.AddCoins(coinAmount);
            }
            Destroy(gameObject);
        }
    }

    private void CollectSound()
    {
        if (!SFXManager.Instance)
        {
            Debug.LogError("SFXManager not found in scene");
            return;
        }

        SFXManager.Instance.Play(SFXManager.SFXType.CollectCoin);
    }
}
