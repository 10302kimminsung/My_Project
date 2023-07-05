using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public float playerSpeed = 5;//�÷��̾� ���ǵ� ����
    Rigidbody2D rgBody;
    [SerializeField] [Range(0, 100)]
    int jumpPower = 10;//���� �Ŀ� ����

    [SerializeField]
    int maxJumpCount;
    int currentJumpCount;
    

    [SerializeField]
    private Vector2 point;//point ����
    [SerializeField]
    private Vector2 size;//size ����
    [SerializeField]
    private LayerMask layer;//layer ����
    public bool isCrouch;


    void Start()
    {
        animator = GetComponent<Animator>();//�ִϸ����� �ҷ�����
        rgBody = GetComponent<Rigidbody2D>();//������ٵ� �ҷ�����
        isCrouch = false;
    }

    void Update()
    {
        if(!GameManager.instance.isAttack)//GameManager���ִ� isAttack�� true��
        {
            Player_Crouch();
            PlayerMove();//�÷��̾� ��ũ��Ʈ ���
            if (currentJumpCount < maxJumpCount)//currentJumpCount�� maxJumpCount���� ������
            Player_Jump();//�÷��̾� ���� ��ũ��Ʈ ���
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
