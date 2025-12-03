using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIEnemy : MonoBehaviour
{

    public int enemyHealth;
    public Slider healthSlider;
    public bool isCube;

    
    //Checks if game object has no health so it can destroy itself
    public void DoDeath()
    {
        if (enemyHealth <= 0)
        {
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
