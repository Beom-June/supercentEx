using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Funtion
    /// </summary>
    private float HorizentalAxis;
    private float VerticalAxis;

    private bool playerWalk = false;                                   // 플레이어 걷기 bool 값
    [SerializeField] private bool playerJump = false;                   // 플레이어 점프 bool 값
    [SerializeField] private bool playerDash = false;                      // 플레이어 회피 bool 값

    [SerializeField] private bool isJump = false;                    // 플레이어 점프 제어 bool 값

    private bool isDash = false;                                     // 플레이어 회피 제어 bool 값
    [SerializeField] private bool isRagdoll = true;                 // Ragdoll 활성화 여부
    [SerializeField] private bool isMove = true;
    [SerializeField] private bool isProgressBar;                // 스타트 지점 부터 프로그래스바 체크를 위한 bool 값
    [SerializeField] private float totalDistance;                                    // 전체 거리 (endPoint ~ startPoint)dddddddddd
    [SerializeField] private float currentDistance;                                  // 현재 위치(거리)
    [SerializeField]private Vector3 prevPosition;                                   // 이전 위치 저장


    /// <summary>
    /// Component
    /// </summary>
    public GameObject ragdoll; // Ragdoll 오브젝트
    private JoyStick joyStick;

    [Header("ProgressBar")]
    public Transform startPoint;
    public Transform endPoint;
    public Slider progressBar;

    Vector3 moveVec;
    Vector3 dashVec;                                  // 회피시 방향이 전환되지 않도록 제한
    Rigidbody playerRigidbody;
    Collider playerCollider;                        
    Animator animator;

    // 프로퍼티
    public Rigidbody playerRigidCall
    {
        get { return playerRigidbody; }
    }
    public Animator playerAnimCall
    {
        get { return animator; }
    }

    [Header("PlayerState")]
    [SerializeField] private float Speed = 10f;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }
    void Start()
    {
        joyStick = GameObject.Find("JoyStickBackGround").GetComponent<JoyStick>();

        // 출발 지점과 도착 지점 사이의 거리를 계산
        totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        prevPosition = transform.position;                                              // 이전 위치를 현재 위치로 저장
    }

    void Update()
    {
        PlayerInput();

        PlayerMove();
        PlayerTurn();
        PlayerJump();
        PlayerDash();

        ProgressBarControll();
    }

    void PlayerInput()
    {
        // 방향키 이동
        HorizentalAxis = Input.GetAxisRaw("Horizontal");
        VerticalAxis = Input.GetAxisRaw("Vertical");

        // 조이스틱 이동
        HorizentalAxis = joyStick.inputHorizontal();
        VerticalAxis = joyStick.inputVertical();

        //playerWalk = Input.GetButton("Walk");                       // Left Ctrl
        playerJump = Input.GetButtonDown("Jump");                   // Space Bar
                                                                    //playerDash = Input.GetButtonDown("Dash");                   // Left Shift


        // Shift 키를 누르면 Ragdoll 활성화
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 레그돌이 꺼지면
            if (isRagdoll == false)
            {
                isMove = false;
                SetRagdoll(true);
            }
            // 레그돌이 켜지면
            else
            {
                isMove = true;
                SetRagdoll(false);                  // 내가 원하는 값
                //ResetRagdoll(true);                 // 현재 가라친거

            }
        }
    }

    #region Player 이동 관련
    // 플레이어 이동 함수
    public void PlayerMove()
    {
        if (isMove == true)
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
    public void PlayerJump()
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

    // Ragdoll 활성화 세팅
    public void SetRagdoll(bool flag)
    {
        isRagdoll = flag;
        animator.enabled = !flag; // 애니메이션 비활성화

        // Ragdoll 오브젝트 활성화/비활성화
        if (flag == true)
        {
            ragdoll.SetActive(flag);
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = !flag;
                rb.useGravity = flag;
            }
        }
        else
        {
            ragdoll.SetActive(!flag);
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = flag;
                rb.useGravity = flag;
            }
        }
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = flag;
        }

        // 플레이어 캐릭터의 Rigidbody, Collider 활성화/비활성화
        playerRigidbody.isKinematic = flag;
        GetComponent<CapsuleCollider>().enabled = !flag;
    }

    void ProgressBarControll()
    {
        // 전체거리 : 출발 지점, 끝 지점 계산
        float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);

        // 현재 거리 : 움직인 거리 : 시작 지점
        float currentDistance = Vector3.Distance(transform.position, startPoint.position);

        // 진행 상황을 계산
        float progress = currentDistance / totalDistance;

        progressBar.value = progress;
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("obstacle"))
        {
            if (isRagdoll == true)
            {
                Debug.Log("test");
                //foreach (Collider col in GetComponentsInChildren<Collider>())
                //{
                //    col.enabled = false;
                //}
                ////foreach (Rigidbody playerRigid in GetComponentsInChildren<Rigidbody>())
                ////{
                ////    playerRigid.isKinematic = true;
                ////}
                //// rigidbody를 비활성화합니다.
                //GetComponent<Rigidbody>().isKinematic = true;

            }
            else
            {
                Debug.Log("testgood");
                //// 충돌한 방향과 반대 방향으로 force를 가해줍니다.
                //Vector3 collisionDirection = transform.position - collision.gameObject.transform.position;
                //collisionDirection.Normalize();
                //GetComponent<Rigidbody>().AddForce(collisionDirection * 10f, ForceMode.Impulse);

                //// 충돌 무시
                //Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }

        }
    }
}
