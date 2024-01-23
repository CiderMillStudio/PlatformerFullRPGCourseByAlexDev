using System;
using UnityEngine;

public enum SwordType //enums: basically a list of elements, and each type has an associated index number, bout how does this differ from lists? Notice how we're making the enum OUTSIDE of the class?
{
    Regular, //index 0
    Bounce, //index 1, etc...
    Pierce,
    Spin
}

public class SwordSkill : Skill
{

    public SwordType swordType = SwordType.Regular;


    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce = new Vector2(24f, 24f); //was previously launchDirection
    private float swordGravity = 3;
    [SerializeField] private float regularSwordGravity = 3f;
    [SerializeField] private float returnSpeed = 8f;
    [SerializeField] private float freezeTimeDuration = 0.9f;

    private Vector2 finalDirection;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceSwordGravity = 6f;
    [SerializeField] private float bounceSpeed = 20f;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceSwordGravity;

    [Header("Spin Info")]
    [SerializeField] private float hitCoolown = 0.35f;
    [SerializeField] private float maxTravelDistance = 7f;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = 1f;
    
    



    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [Tooltip("Keep this value around 0.1f, not 1.0, due to large launchForce vector values")]
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] Transform dotParent;


    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Regular)
        {
            swordGravity = regularSwordGravity;
        }
        else if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceSwordGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceSwordGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    public void CreateSword()
    {
        SetupGravity(); //TALK ABOut THIS LATER!

        GameObject newSword = Instantiate(swordPrefab, player.transform.position,
            transform.rotation);

        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetUpBounce(true, bounceAmount, bounceSpeed);
        //{ swordGravity = bounceSwordGravity; }
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCoolown);


        player.AssignNewSword(newSword);

 

        newSwordScript.SetUpSword(finalDirection, swordGravity, player, returnSpeed, freezeTimeDuration); //Turns out Alex was right!

        DotsActive(false);

    }

   

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    #region AimRegion
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        SetupGravity();

        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }

    }

    private Vector2 DotsPosition(float t)  //PHYSICS!!! 
    {
        SetupGravity();


        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
