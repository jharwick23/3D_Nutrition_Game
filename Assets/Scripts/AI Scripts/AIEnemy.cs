using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIEnemy : MonoBehaviour
{

    public int enemyHealth = 100;
    public Slider healthSlider;
    public bool isCube;
    private DropService _dropService;

    void Awake()
    {
        _dropService = GameObject.FindWithTag("DropService").GetComponent<DropService>();
    }

    //Checks if game object has no health so it can destroy itself
    public void DoDeath()
    {
        if (enemyHealth <= 0)
        {
            _dropService.DropCoin(new Vector3(transform.position.x, 1, transform.position.z), 10); // Drop Coin
            Destroy(gameObject);
        }
    }
    
    //Updates UI when for health tracking
    public void UpdateUI()
    {
        if (isCube)
        {
            healthSlider.value = enemyHealth;
        }
    }
}
