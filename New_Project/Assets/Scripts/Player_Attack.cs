using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    Animator anima;

    [SerializeField]
    private Vector2 point;//point 識情
    [SerializeField]
    private Vector2 size;//size 識情
    [SerializeField]
    private LayerMask layer;//layer 識情

    [SerializeField]
    int maxJumpAttack = 2;
    int currentJumpAttack;

    private PlayerController playerController;
    private Animator playerAnimator;

    private bool isJumpAttack;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = playerController.GetComponent<Animator>();
    }

    private void Start()
    {
        anima = GetComponent<Animator>();
        isJumpAttack = false;
    }
    private void Update()
    {
        if (!playerController.isCrouch)
        {
            Attack_1();
            Attack_2();
        }
        if (playerController.isCrouch)
        {
            Attack_3();
        }
    }

    public void Attack_1()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.isAttack && Check_Floor())
        {
            GameManager.instance.isAttack = true;
            anima.SetBool("Attack", true);  
        }
    } 
    public void Attack_2()
    {
        if (Input.GetMouseButtonDown(0) && !isJumpAttack && !Check_Floor())
        {
            isJumpAttack = true;
            anima.SetBool("JumpAttack", true);
            StartCoroutine("Wait");
        }
    }

    public void Attack_3()
    {
        if(Input.GetMouseButtonDown(0) && !GameManager.instance.isAttack && Check_Floor())
        {
            print(3);
            GameManager.instance.isAttack = true;
            anima.SetBool("CrouchSlash", true);
        }
    }

    public void AttackStop()
    {
        anima.SetBool("Attack", false);
        GameManager.instance.isAttack = false;
        ++currentJumpAttack;

        if (currentJumpAttack == 1)
        {
            StartCoroutine(Wait());
        }
    }

    private Collider2D Check_Floor()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + point, size, 0, layer);
    }


    private IEnumerator Wait()
    {
        yield return new WaitUntil(() => Check_Floor());
        

        print(11);
        anima.SetBool("JumpAttack", false);
        GameManager.instance.isAttack = false;
        isJumpAttack = false;
    }

   

    public void EndJumpAttack()
    {
        anima.SetBool("JumpAttack", false); 
    }

    public void EndCrouchSlash()
    {
        anima.SetBool("CrouchSlash", false);
        playerController.isCrouch = false;
        playerAnimator.SetBool("Crouch", false);
        playerController.playerSpeed = 5;
        GameManager.instance.isAttack = false;
    }
}
