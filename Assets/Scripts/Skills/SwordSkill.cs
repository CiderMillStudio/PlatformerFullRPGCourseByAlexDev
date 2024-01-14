using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce = new Vector2(24f,24f); //was previously launchDirection
    [SerializeField] private float swordGravity;
    [SerializeField] private float returnSpeed = 8f;

    private Vector2 finalDirection;

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
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, 
            transform.rotation);

            player.AssignNewSword(newSword);

/*        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
        newSwordScript.SetUpSword(launchDirection, swordGravity);*/ 
        //^^this is Alex's solution, but I want to try my own first)

        newSword.GetComponent<SwordSkillController>().SetUpSword(finalDirection, 
            swordGravity, player, returnSpeed); //<--my solution, which is shorter.

        DotsActive(false);

    }
    
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2 (AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
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
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++) 
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }

    }

    private Vector2 DotsPosition(float t)  //PHYSICS!!! 
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}
