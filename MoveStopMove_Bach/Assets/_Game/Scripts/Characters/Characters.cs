using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : GameUnit
{
    protected float speed;
    protected float rotationSpeed;

    public float attackRange;
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public Animator anim;
    public Transform Target;

    private bool isMoving;
    private bool isAttack;
    private string currentAnim;

    public float size = 1;
    public int score;
    public int Score => score;

    public event Action OnDeath;

    public bool IsDead { get; set; }

    public void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    private void Update()
    {
        this.CharactersUpdate();
    }

    public Vector3 TargetDirection()
    {
        Vector3 direction = Target.position - this.transform.position;
        direction.y = 0f;
        return direction;
    }

    public void ChangeRotation()
    {
        Quaternion rotation = Quaternion.LookRotation(TargetDirection());
        this.transform.rotation = rotation;
    }

    public override void OnInit()
    {
        IsMoving = false;
        IsAttack = false;

        speed = 7f;
        this.rotationSpeed = 100f;
        this.attackRange = 5f;
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    public override void OnDespawn()
    {
        Destroy(gameObject);
    }

    protected virtual void CharactersUpdate()
    {

    }

    public virtual void AddScore(int value)
    {
        SetScore(score + value);
        UpSize();
    }

    public virtual void SetScore(int score)
    {
        this.score = score;

        //scoreText.text = score.ToString();

        SetSize(1 + this.score * 0.1f);
    }

    protected virtual void SetSize(float size)
    {
        size = Mathf.Clamp(size, 1f, 5f);
        this.size = size;
        transform.localScale = size * Vector3.one;
    }

    protected virtual void UpSize()
    {
        SetSize(1 + this.score * 0.1f);
    }
}