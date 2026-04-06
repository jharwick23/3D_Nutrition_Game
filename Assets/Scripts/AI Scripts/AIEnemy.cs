using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AIEnemy : MonoBehaviour
{

    public int enemyHealth = 100;
    public Slider healthSlider;
    public bool isCube = true;
    private DropService _dropService;
    [SerializeField] private bool dropItems = true;
    private bool canDamage = true;

    void Awake()
    {
        if(!_dropService)
        {
            _dropService = GameObject.FindWithTag("DropService").GetComponent<DropService>();
        }
    }

    //Checks if game object has no health so it can destroy itself
    public void DoDeath()
    {
        if (enemyHealth <= 0)
        {
            if (dropItems)
            {
                OnDeathEvent();
                //Debug.Log(_dropService);
                //Debug.Log(transform);
                if (SceneManager.GetActiveScene().name == "SideQuestOne")
                {
                    _dropService.DropCoin(new Vector3(transform.position.x, transform.position.y, transform.position.z), 15); // Drop Coin
                }
                else if(SceneManager.GetActiveScene().name == "FirstLevel")
                {
                    _dropService.DropCoin(new Vector3(transform.position.x, transform.position.y, transform.position.z), 10); // Drop Coin
                }
                else if(SceneManager.GetActiveScene().name == "SecondLevel")
                {
                    _dropService.DropCoin(new Vector3(transform.position.x, transform.position.y, transform.position.z), 15); // Drop Coin
                }
                else if(SceneManager.GetActiveScene().name == "ThirdLevel")
                {
                    _dropService.DropCoin(new Vector3(transform.position.x, transform.position.y, transform.position.z), 18); // Drop Coin
                }
                else if(SceneManager.GetActiveScene().name == "FourthLevel")
                {
                    _dropService.DropCoin(new Vector3(transform.position.x, transform.position.y, transform.position.z), 20); // Drop Coin
                }
            
                int val = Random.Range(0, 2);
            
                if (val == 0)
                {
                    _dropService.DropHeal(new Vector3(transform.position.x, transform.position.y, transform.position.z), 15);          
                }

            }
            
            Destroy(gameObject);
        }
    }
    
    //Damage function to decoup function and allow added behaviors
    public void DoDamage(int damage)
    {
        if (canDamage)
        {
            enemyHealth -= damage;
        }
        
    }


    //set bool if enemy can be damaged if immunity is needed
    protected void SetImmunity(bool immunity)
    {
        canDamage = immunity;
    }
 
    //Updates UI when for health tracking
    public void UpdateUI()
    {
        if (isCube)
        {
            healthSlider.value = enemyHealth;
        }
    }

    //New protected event for enemies outside to be able to inherit and do extra functions: inheritance
    protected virtual void OnDeathEvent()
    {

    }

    //Handles memory leak with nav mesh agents
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
