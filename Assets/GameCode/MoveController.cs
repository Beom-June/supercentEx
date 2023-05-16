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

    [SerializeField] private float minMoveDistance = 0.001f; // �ִϸ��̼��� �۵��ϱ� ���� �ּ� �̵� �Ÿ�

    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (null == mainCamera)
            mainCamera = Camera.main;

        // ��Ŭ�� �̺�Ʈ�� ���Դٸ�
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1 << groundLayer))
            {
                MoveTo(hit.point);
            }
        }

        // �̵� ������ üũ�Ͽ� �ִϸ��̼� ����
        UpdateAnimation();
    }

    private void MoveTo(Vector3 destination)
    {
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(destination, out navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            navMeshAgent.enabled = true;
            navMeshAgent.autoTraverseOffMeshLink = true; // Off-Mesh Link �ڵ� �ǳʶٱ� Ȱ��ȭ
            navMeshAgent.autoBraking = false; // �������� �����ϸ� ������ ����
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

        // �̵� ������ üũ�Ͽ� ������ �ִϸ��̼� ����
        if (isMoving)
        {
            if (!wasMoving)
            {
                anim.SetBool("isMove", true); // ������ �ִϸ��̼� �ѱ�
            }
        }
        else
        {
            anim.SetBool("isMove", false); // ������ �ִϸ��̼� ����
        }

        // ���� ������ üũ�Ͽ� ���� �ִϸ��̼� ����
        if (navMeshAgent.isOnOffMeshLink && !isJumping && isMoving)
        {
            OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;

            if (data.linkType == OffMeshLinkType.LinkTypeJumpAcross)
            {
                StartCoroutine(JumpCoroutine());
            }
        }

        // Jump �ִϸ��̼� ����
        bool isJumpingState = isJumping && !isMoving;
        anim.SetBool("isJump", isJumpingState);

        // Jump �ִϸ��̼��� ����Ǹ� isJumping�� false�� �����Ͽ� Exit ���·� ���̽�ŵ�ϴ�.
        if (!isJumping && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anim.SetBool("isJump", false);
        }
    }

    IEnumerator JumpCoroutine()
    {
        isJumping = true;

        // ���� �ִϸ��̼��� Ȱ��ȭ�մϴ�.
        anim.SetBool("isJump", true);

        // ���� �ð� ��� �� ���� �ִϸ��̼��� ��Ȱ��ȭ�մϴ�.
        yield return new WaitForSeconds(1f); // ���� �ִϸ��̼��� ��� �ð��� ����Ͽ� ������ ��� �ð��� �����մϴ�.
        anim.SetBool("isJump", false);

        isJumping = false;

        // Off mesh link�� �ǳʶٵ��� NavMeshAgent�� ����մϴ�.
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
