using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIEnemy : MonoBehaviour
{

    public int enemyHealth = 100;
    public Slider healthSlider;
    public bool isCube = true;
    private DropService _dropService;
    [SerializeField] private bool dropItems = true;

    void Awake()
    {
        _dropService = GameObject.FindWithTag("DropService").GetComponent<DropService>();
    }

    //Checks if game object has no health so it can destroy itself
    public void DoDeath()
    {
        if (enemyHealth <= 0)
        {
            if (dropItems)
            {
                _dropService.DropCoin(new Vector3(transform.position.x, transform.position.y, transform.position.z), 10); // Drop Coin
            
                int val = Random.Range(0, 1);
            
                if (val == 0)
            
                {
                    _dropService.DropHeal(new Vector3(transform.position.x, transform.position.y, transform.position.z), 20);          
                }

            }
            
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

    private void OnDestroy()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
    }
}
