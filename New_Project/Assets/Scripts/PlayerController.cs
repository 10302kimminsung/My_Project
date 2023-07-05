using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public float playerSpeed = 5;//플레이어 스피드 지정
    Rigidbody2D rgBody;
    [SerializeField] [Range(0, 100)]
    int jumpPower = 10;//점프 파워 지정

    [SerializeField]
    int maxJumpCount;
    int currentJumpCount;
    

    [SerializeField]
    private Vector2 point;//point 선언
    [SerializeField]
    private Vector2 size;//size 선언
    [SerializeField]
    private LayerMask layer;//layer 선언
    public bool isCrouch;


    void Start()
    {
        animator = GetComponent<Animator>();//애니메이터 불러오기
        rgBody = GetComponent<Rigidbody2D>();//리지드바디 불러오기
        isCrouch = false;
    }

    void Update()
    {
        if(!GameManager.instance.isAttack)//GameManager에있는 isAttack이 true면
        {
            Player_Crouch();
            PlayerMove();//플레이어 스크립트 재생
            if (currentJumpCount < maxJumpCount)//currentJumpCount가 maxJumpCount보다 작으면
            Player_Jump();//플레이어 점프 스크립트 재생
        }
    }

    public void PlayerMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 vec = new Vector3(x, 0);
        transform.position += vec * playerSpeed * Time.deltaTime;
        if (x != 0)
        {
            animator.SetBool("Run", true);
            transform.localScale = new Vector3(x, 1, 1);
        }

        else
        {
            animator.SetBool("Run", false);
        }

    }

    private void Player_Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
            rgBody.velocity = Vector2.up * jumpPower;
            ++currentJumpCount;

            if (currentJumpCount == 1)
            {
                StartCoroutine(Wait());
            }
        }
    }

    public void Player_Crouch()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouch && CheckFloor())
        {
            isCrouch = true;
            animator.SetBool("Crouch", true);
            animator.SetBool("Run", false);
            playerSpeed = 0;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isCrouch = false;
            animator.SetBool("Crouch", false);
            playerSpeed = 5;
        }
    }
    

    private Collider2D CheckFloor()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + point, size, 0, layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + point, size);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => CheckFloor());
        currentJumpCount = 0;
        animator.SetBool("Jump", false);
    }
}
