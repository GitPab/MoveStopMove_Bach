using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackState : IState<Player>
{
    private float attackDelayTime;
    
    public void OnEnter(Player t)
    {
        t.IsAttack = true;
        t.IsMoving = false;

        attackDelayTime = 1f;

        t.StartCoroutine(DelayedAttack(t));
    }

    private IEnumerator DelayedAttack(Player t)
    {
        yield return new WaitForSeconds(0.4f);

        t.ChangeAnim("Attack");
        WeaponSpawner.Instance.SpawnPlayerWeapon(t.weaponID, t.TargetDirection(), t.transform.position + Vector3.up + t.transform.forward, Quaternion.identity, t.attackRange);
    }

    public void OnExecute(Player t)
    {
        if (t.IsMoving)
        {
            t.IsAttack = false;
            t.currentState.ChangeState(new IdleState());

            return;
        }
        else
        {
            attackDelayTime -= Time.deltaTime;
            if (attackDelayTime > 0f) return;

            t.IsAttack = false;
            t.currentState.ChangeState(new IdleState());

            return;
        }
        
        

    }

    public void OnExit(Player t)
    {

    }

}
