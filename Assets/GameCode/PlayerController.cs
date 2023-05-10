using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Funtion
    /// </summary>
    private float HorizentalAxis;
    private float VerticalAxis;

    private bool playerWalk = false;                            // 플레이어 걷기 bool 값
    private bool playerJump = false;                            // 플레이어 점프 bool 값
    private bool playerDash = false;                            // 플레이어 회피 bool 값
    [SerializeField] float force = 5f;

    private bool isJump = false;                                // 플레이어 점프 제어 bool 값
    private bool isDash = false;                                // 플레이어 회피 제어 bool 값
    [SerializeField] private bool isRagdoll = true; // Ragdoll 활성화 여부
    private bool isMove = true;

    /// <summary>
    /// Component
    /// </summary>
    public GameObject ragdoll; // Ragdoll 오브젝트
    Vector3 moveVec;
    Vector3 dashVec;                                  // 회피시 방향이 전환되지 않도록 제한
    Rigidbody playerRigidbody;
    Collider playerCollider;
    Animator animator;


    [Header("PlayerState")]
    [SerializeField] private float Speed = 10f;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
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

        //       playerWalk = Input.GetButton("Walk");                       // Left Ctrl
        playerJump = Input.GetButtonDown("Jump");                   // Space Bar
                                                                    //playerDash = Input.GetButtonDown("Dash");                   // Left Shift

        // Shift 키를 누르면 Ragdoll 활성화
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isRagdoll == false)
            {
                isMove = false;
                SetRagdoll(true);
            }
            else
            {
                isMove = true;
                ResetRagdoll(true);

            }
        }
    }

    #region Player 이동 관련
    // 플레이어 이동 함수
    public void PlayerMove()
    {
        if(isMove == true) 
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
        else
        {
            return;
        }

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
        // if (playerJump && isJump == false && moveVec == Vector3.zero && isDash == false)
        if (playerJump && isJump == false && isDash == false)
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

        // dashVec 값을 초기화함
        dashVec = Vector3.zero;
    }
    #endregion
    // Ragdoll 비활성화
    private void SetRagdoll(bool active)
    {
        isRagdoll = active;
        animator.enabled = !active; // 애니메이션 비활성화

        // Ragdoll 오브젝트 활성화/비활성화
        ragdoll.SetActive(active);

        // Rigidbody, Collider 활성화/비활성화
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = !active;
            rb.useGravity = active;
        }

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = active;
        }

        // 플레이어 캐릭터의 Rigidbody, Collider 활성화/비활성화
        playerRigidbody.isKinematic = active;
        GetComponent<CapsuleCollider>().enabled = !active;
    }
    // 레그돌 정상화
    private void ResetRagdoll(bool active)
    {
        isRagdoll = !active;
        animator.enabled = active; // 애니메이션 활성화

        // Ragdoll 오브젝트 활성화/비활성화
        ragdoll.SetActive(active);

        // Rigidbody, Collider 활성화/비활성화
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = active;
            rb.useGravity = active;
        }

        // 부모에서 콜라이더를 켰으니 이걸 꺼줘야함
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = !active;
        }

        // 플레이어 캐릭터의 Rigidbody, Collider 활성화/비활성화
        playerRigidbody.isKinematic = !active;
        GetComponent<CapsuleCollider>().enabled = active;
    }

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

        if (collision.gameObject.tag == "jumpZone")
        {
            playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            animator.SetTrigger("doJump");
            isJump = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            // 플레이어의 이동을 멈춤
            playerRigidbody.velocity = Vector3.zero;
        }

        if (collision.gameObject.tag == "vane")
        {
            //Vector3 direction = (collision.transform.position - transform.position).normalized; // 충돌 대상과의 벡터를 구합니다.
            playerRigidbody.GetComponent<Rigidbody>().AddForce(Vector3.back * force, ForceMode.Impulse); // 밀어냅니다.
        }
    }
}
