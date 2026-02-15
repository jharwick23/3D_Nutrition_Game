using UnityEngine;

public class MinionLink : MonoBehaviour
{
    public BossController boss;

    //Extra script to do damage when enemy dies
    void OnDestroy()
    {
        if (boss != null && boss.currentState == BossController.BossState.PhaseOne)
        {
            boss.MinionDied();
        }
    }
}

