using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClickToPoint : MonoBehaviour
{
    // Component
    //public CharacterController characterController; // 캐릭터 컨트롤러
    public Camera mainCamera; // 메인 카메라
    Animator anim;

    // Fuction
    [SerializeField] private float speed = 3.0f;      // 캐릭터 움직임 스피드
    [SerializeField] private Vector3 movePoint; // 이동 위치 저장
    [SerializeField] private bool moveBool;
    private static readonly int MoveParam = Animator.StringToHash("isMove");


    void Start()
    {
        // mainCamera = Camera.main;
        //characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveClick();
    }

    void MoveClick()
    {
        if (null == mainCamera)
            mainCamera = Camera.main;
        // 좌클릭 이벤트가 들어왔다면
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var layer = 1 << LayerMask.NameToLayer("Ground");
            if (!Physics.Raycast(ray, out var hitInfo, 100.0f, layer))
                return;

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                // 맞은 위치를 목적지로 저장
                movePoint = raycastHit.point;
                moveBool = transform;
                anim.SetBool(MoveParam, true);
                Debug.Log("movePoint : " + movePoint.ToString());
                Debug.Log("맞은 객체 : " + raycastHit.transform.name);

            }
        }

        if (moveBool)
        {
            Move();
            PlayerTurn();
        }
    }

    // 이동 함수
    void Move()
    {
        Vector3 direction = movePoint - transform.position;
        direction.y = 0f;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;

        // 이동이 거의 완료되었는지 체크
        if (Vector3.Distance(transform.position, movePoint) < 0.1f)
        {
            moveBool = false;
            anim.SetBool(MoveParam, false);
        }
    }
    // 플레이어 시점
    void PlayerTurn()
    {
        // 이동 방향 카메라 시점
        transform.LookAt(transform.position + movePoint);
    }
}
