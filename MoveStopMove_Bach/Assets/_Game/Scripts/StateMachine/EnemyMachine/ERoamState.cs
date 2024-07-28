using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ERoamState : IState<Enemy>
{
    public void OnEnter(Enemy owner)
    {
        owner.agent.speed = 2.0f;
        owner.agent.stoppingDistance = 2.5f;
    }

    public void OnExecute(Enemy owner)
    {
        if (owner.IsGotoTarget())
        {
            owner.StopMove();
            owner.currentState.ChangeState(new ESleepState());
            return;
        }
        
        if (Random.value < 0.1f)
        {
            owner.finalPosition = new Vector3(
                Random.Range(-10, 10),
                0,
                Random.Range(-10, 10)
            ) + owner.transform.position;

            owner.GotoTarget();
        }
    }

    public void OnExit(Enemy t)
    {

    }
}