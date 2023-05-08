using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Funtion
    /// </summary>
    float HorizentalAxis;
    float VerticalAxis;

    bool playerWalk;                            // 플레이어 걷기 bool 값
    bool playerJump;                            // 플레이어 점프 bool 값
    bool playerDash;                            // 플레이어 회피 bool 값

    bool isJump;                                // 플레이어 점프 제어 bool 값
    bool isDash;                                // 플레이어 회피 제어 bool 값

    /// <summary>
    /// Component
    /// </summary>
    Vector3 moveVec;
    Vector3 dashVec;                                  // 회피시 방향이 전환되지 않도록 제한
    Rigidbody playerRigidbody;
    Animator animator;

    [Header("PlayerState")]
    [SerializeField]private float Speed = 10f;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
    }
    void Update()
    {
        PlayerInput();

        PlayerMove();
        PlayerTurn();
        PlayerJump();
        PlayerDash();
    }

    void PlayerInput()
    {
        HorizentalAxis = Input.GetAxisRaw("Horizontal");
        VerticalAxis = Input.GetAxisRaw("Vertical");

        playerWalk = Input.GetButton("Walk");                       // Left Ctrl
        playerJump = Input.GetButtonDown("Jump");                   // Space Bar
        playerDash = Input.GetButtonDown("Dash");                   // Left Shift

    }


    #region Player 이동 관련
    // 플레이어 이동 함수
    public void PlayerMove()
    {
        moveVec = new Vector3(HorizentalAxis, 0, VerticalAxis).normalized;

        if (isDash)
        {
            moveVec = dashVec;
        }
        if (playerWalk)
        {
            transform.position += moveVec * Speed * 0.3f * Time.deltaTime;
        }
        else
        {
            transform.position += moveVec * Speed * Time.deltaTime;
        }
        //transform.position += moveVec * Speed * (WalkDown ? 0.3f : 1f) * Time.deltaTime;
        animator.SetBool("isMove", moveVec != Vector3.zero);
        animator.SetBool("isWalk", playerWalk);

    }

    // 플레이어 시점
    void PlayerTurn()
    {
        // 이동 방향 카메라 시점
        transform.LookAt(transform.position + moveVec);
    }
    // 플레이어 점프 함수
    void PlayerJump()
    {
        if (playerJump && isJump == false && moveVec == Vector3.zero && isDash == false)
        {
            playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            animator.SetTrigger("doJump");
            isJump = true;
        }
    }
    // 플레이어 대쉬 함수
    void PlayerDash()
    {
        //if (playerJump && isJump == false && moveVec != Vector3.zero && isDash == false)
        if (playerDash && isJump == false && moveVec != Vector3.zero && isDash == false)
        {
            dashVec = moveVec;
            Speed *= 2;
            animator.SetTrigger("doDash");
            isDash = true;

            // 대쉬 빠져 나오는 속도
            Invoke("PlayerDashEnd", 0.2f);

        }
    }

    // PlayerDash에서 사용 중
    void PlayerDashEnd()
    {
        // 원래 속도로 돌아오게함
        Speed *= 0.5f;
        isDash = false;
    }
    #endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }

        if (collision.gameObject.tag == "FinishPoint")
        {
            SceneManager.LoadScene("SolarSystemScene");
        }
    }
}
