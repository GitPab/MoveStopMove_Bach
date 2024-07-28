using UnityEngine;

public class Player : Characters
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform rightHand;
    [SerializeField] private SkinnedMeshRenderer pants;
    [SerializeField] private Transform hair;
    [SerializeField] private Transform leftHand;

    private PlayerRange playerRange;

    public StateMachine<Player> currentState;
    public int weaponID;
    public int Coins { get => coins; set => coins = value; }

    private int skinHairID;
    private int skinShieldID;
    private int skinPantID;
    private int coins;
    private float gravity;
    private float horizontal;
    private float vertical;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        this.OnInit();
    }

    public override void OnInit()
    {
        base.OnInit();

        this.SetInitalEquip();
        currentState = new StateMachine<Player>();
        currentState.SetOwner(this);
        this.gravity = 20f;
        currentState.ChangeState(new IdleState());

    }

    private void SetInitalEquip()
    {
        this.ChangeWeapon(Pref.CurWeaponId);
        this.ChangeHair(Pref.CurHairId);
        this.ChangePant(Pref.CurPantId);
        this.ChangeShield(Pref.CurShieldId);
    }

    //====Update======

    protected override void CharactersUpdate()
    {
        currentState.UpdateState(this);

        SetGravity();

        base.CharactersUpdate();

        IsMove();
    }

    //====Setup Joystick =======

    public void SetupJoystick(FloatingJoystick joystick)
    {
        this.joystick = joystick;
    }


    //=====SetGravity======
    private void SetGravity()
    {
        characterController.Move(Vector3.down * gravity * Time.deltaTime);
    }

    //===PlayerMovement=====

    private void GetInput()
    {
        if (joystick == null) return;
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
    }
    public bool IsMove()
    {
        GetInput();

        if (Mathf.Abs(this.horizontal) > 0.1f || Mathf.Abs(this.vertical) > 0.1f)
        {
            this.IsMoving= true;

            return true;
        }
        return false;
    }
    public void Moving()
    {
        GetInput();

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            characterController.Move(direction * speed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


    public void UpdateCoin(int coinChange, bool isUp)
    {
        if (isUp) this.Coins += coinChange;
        else this.Coins -= coinChange;
        Pref.Coins = this.Coins;
    }

    public void SetSkinPantID(int id)
    {
        this.skinPantID = id;
    }

    public void SetSkinHairID(int id)
    {
        this.skinHairID = id;
    }

    public void SetSkinShieldID(int id)
    {
        this.skinShieldID = id;
    }


    public void ChangeWeapon(int id)
    {
        this.weaponID= id;

        WeaponSpawner.Instance.ChangeModelWeaponPlayer(rightHand, id);
    }

    public void ChangePant(int id)
    {
        this.skinPantID = id;

        ChangeSkinPlayer.Ins.ChangePant(pants, skinPantID);
    }

    public void ChangeHair(int id)
    {

        this.skinHairID = id;
        ChangeSkinPlayer.Ins.ChangeModelHair(hair, skinHairID);
    }

    public void ChangeShield(int id)
    {
        this.weaponID = id;

        ChangeSkinPlayer.Ins.ChangeModelShield(leftHand, skinShieldID);
    }

    public void Reset()
    {
        // Reset player position
        transform.position = Vector3.zero;

        // Reset player coins
        Coins = 0;

        // Reset player equipment
        ChangeWeapon(0);
        ChangePant(0);
        ChangeHair(0);
        ChangeShield(0);

        // Reset any other player state variables
    }
}
