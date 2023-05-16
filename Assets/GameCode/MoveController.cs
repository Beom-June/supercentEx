using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveController : MonoBehaviour
{
    public Camera mainCamera;
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private bool isMoving;
    private bool isJumping = false;

    [SerializeField] private float minMoveDistance = 0.001f; // 애니메이션을 작동하기 위한 최소 이동 거리

    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (null == mainCamera)
            mainCamera = Camera.main;

        // 좌클릭 이벤트가 들어왔다면
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1 << groundLayer))
            {
                MoveTo(hit.point);
            }
        }

        // 이동 중인지 체크하여 애니메이션 갱신
        UpdateAnimation();
    }

    private void MoveTo(Vector3 destination)
    {
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(destination, out navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            navMeshAgent.enabled = true;
            navMeshAgent.autoTraverseOffMeshLink = true; // Off-Mesh Link 자동 건너뛰기 활성화
            navMeshAgent.autoBraking = false; // 목적지에 도착하면 멈추지 않음
            navMeshAgent.SetDestination(navMeshHit.position);
        }
        else
        {
            Debug.LogWarning("Failed to find valid NavMesh position for destination: " + destination);
        }
    }

    private void UpdateAnimation()

    {
        bool wasMoving = isMoving;
        isMoving = navMeshAgent.velocity.magnitude > minMoveDistance;

        // 이동 중인지 체크하여 움직임 애니메이션 갱신
        if (isMoving)
        {
            if (!wasMoving)
            {
                anim.SetBool("isMove", true); // 움직임 애니메이션 켜기
            }
        }
        else
        {
            anim.SetBool("isMove", false); // 움직임 애니메이션 끄기
        }

        // 점프 중인지 체크하여 점프 애니메이션 갱신
        if (navMeshAgent.isOnOffMeshLink && !isJumping && isMoving)
        {
            OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;

            if (data.linkType == OffMeshLinkType.LinkTypeJumpAcross)
            {
                StartCoroutine(JumpCoroutine());
            }
        }

        // Jump 애니메이션 제어
        bool isJumpingState = isJumping && !isMoving;
        anim.SetBool("isJump", isJumpingState);

        // Jump 애니메이션이 종료되면 isJumping을 false로 설정하여 Exit 상태로 전이시킵니다.
        if (!isJumping && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anim.SetBool("isJump", false);
        }
    }

    IEnumerator JumpCoroutine()
    {
        isJumping = true;

        // 점프 애니메이션을 활성화합니다.
        anim.SetBool("isJump", true);

        // 점프 시간 대기 후 점프 애니메이션을 비활성화합니다.
        yield return new WaitForSeconds(1f); // 점프 애니메이션의 재생 시간을 고려하여 적절한 대기 시간을 설정합니다.
        anim.SetBool("isJump", false);

        isJumping = false;

        // Off mesh link를 건너뛰도록 NavMeshAgent에 명령합니다.
        navMeshAgent.CompleteOffMeshLink();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TimeLine"))
        {
            navMeshAgent.enabled = false;
        }
        if (other.gameObject.CompareTag("FinishPoint"))
        {
            navMeshAgent.enabled = true;
        }
    }
}
