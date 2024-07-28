using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Characters
{
    public StateMachine<Enemy> currentState;
    public Vector3 finalPosition;
    public bool IsFoundCharacter { get => isFoundCharacter; set => isFoundCharacter = value; }
    public Collider[] hitColliders;
    public int enemyIDWeapon;


    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask layerCharacter;
    [SerializeField] private Collider enemyCollider;

    private bool isFoundCharacter;
    public EnemyRange enemyRange;

    public List<Transform> targets = new List<Transform>();

    public override void OnInit()
    {
        base.OnInit();
        currentState = new StateMachine<Enemy>();
        currentState.SetOwner(this);
        IsFoundCharacter = false;

        IsAttack = false;
        currentState.ChangeState(new ESleepState());
        currentState.AddState(new ERoamState());


        this.GetComponent<Characters>().OnDeath += IncreaseSize;

        this.enemyIDWeapon = RandomWeapon();
    }

    protected override void CharactersUpdate()
    {
        currentState.UpdateState(this);

        base.CharactersUpdate();
    }


    public void SeekForTarget()
    {
        Vector3 scaleBox = new Vector3(100, 5, 100);
        hitColliders = Physics.OverlapBox(this.transform.position, scaleBox / 2, Quaternion.identity, layerCharacter);

        if (hitColliders.Length < 2)
        {
            finalPosition = player.position;
            return;
        }

        float minDistance = float.MaxValue;
        Vector3 minPos = Vector3.zero;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == this.enemyCollider || hitColliders[i].transform == this.transform)
            {
                continue;
            }

            float distanceTmp = Vector3.Distance(hitColliders[i].transform.position, this.transform.position);

            if (distanceTmp < minDistance)
            {
                minDistance = distanceTmp;
                minPos = hitColliders[i].transform.position;
            }
        }

        if (minPos != Vector3.zero)
        {
            finalPosition = minPos;
            IsFoundCharacter = true;
            GotoTarget();
        }
        else
        {
            finalPosition = player.position;
        }
    }


    public void GotoTarget()
    {
        Debug.Log(finalPosition + gameObject.name);
        agent.SetDestination(finalPosition);

    }

    public void StopMove()
    {
        this.finalPosition = this.transform.position;
        this.GotoTarget();
    }

    public bool IsGotoTarget()
    {
        if (Vector3.Distance(this.transform.position, finalPosition) < 2.5f)
        {
            IsFoundCharacter = false;

            return true;
        }
        return false;
    }
    public void IncreaseSize()
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        enemyRange = GetComponent<EnemyRange>(); 
        enemyRange.ChangeAttackRange(transform.localScale.x); // increase attack range
    }
    public override void OnDespawn()
    {
        SimplePool.Despawn(this);
    }


    private int RandomWeapon()
    {
        int TmpRandom = Random.Range(1, SOManager.Ins.weaponS0.Count + 1);

        for (int i = 0; i < SOManager.Ins.weaponS0.Count; i++)
        {
            if (SOManager.Ins.weaponS0[i].IDWeapon == TmpRandom) return SOManager.Ins.weaponS0[i].IDWeapon;
        }
        return SOManager.Ins.weaponS0[0].IDWeapon;

    }
    public void Reset()
    {
        // Reset enemy position
        transform.position = Vector3.zero;

        // Reset enemy state
        IsFoundCharacter = false;
        IsAttack = false;
        currentState.ChangeState(new ESleepState());

        // Reset enemy navigation
        agent.SetDestination(Vector3.zero);

        // Reset final position to a random position within a certain range of the enemy's current position
        float range = 10f;
        Vector3 randomPosition = transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        finalPosition = randomPosition;
    }
}