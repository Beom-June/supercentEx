using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{

    // 로켓 오브젝트
    public Transform rocket;
    // 행성 위쪽 포인트 리스트
    public List<Transform> finishPoint;

    // 시작 시간
    [SerializeField] private float startTime;

    // 체공 시간
    [SerializeField] private float journeyTime = 2.0f;

    public bool isMoving = false;

    private void Update()
    {
        moveKey();
    }

    // 키코드 받는 함수
    void moveKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            planetPoint(0);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            planetPoint(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            planetPoint(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            planetPoint(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            planetPoint(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            planetPoint(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            planetPoint(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            planetPoint(7);
        }
    }

    // 로켓 이동 함수
    void planetPoint(int number)
    {
        // 로켓이 이미 이동 중이면 무시
        if (isMoving)
            return;

        // 로켓과 도착지점 사이의 거리 계산
        Vector3 distance = finishPoint[number].position - rocket.position;

        // 로켓의 이동 속도 계산
        float speed = distance.magnitude / journeyTime;

        // 로켓 이동 방향 계산
        Vector3 direction = distance.normalized;

        // 로켓 이동
        isMoving = true;
        startTime = Time.time;

        StartCoroutine(MoveRocketCoroutine(direction, speed, finishPoint[number]));
    }
    IEnumerator MoveRocketCoroutine(Vector3 direction, float speed, Transform destination)
    {

        Vector3 tailDirection = transform.up;
        Quaternion startRotation = transform.rotation;

        while (isMoving)
        {
            float distance = speed * Time.deltaTime;
            float elapsedTime = Time.time - startTime;          // Time.time 시작된 이후 경과 시간을 초 단위로 반환
            float completeTime = elapsedTime / journeyTime;


            // 비행기 회전 처리.  completeTime 값이 1이 되면, 로켓은 도착지점에 도달한 것.
            if (completeTime <= 1f)
            {
                Quaternion rotation = Quaternion.FromToRotation(tailDirection, direction);
                transform.rotation = Quaternion.Slerp(startRotation, startRotation * rotation, completeTime * 5f);
            }
            // 로켓 회전 도착후 정방향
            else
            {
                Vector3 upVector = rocket.position - direction;
                rocket.rotation = Quaternion.LookRotation(Vector3.zero, upVector);

                // 로켓이 도착지점에 도착하면 종료
                isMoving = false;
                rocket.SetParent(destination); // 로켓의 부모를 도착지점으로 변경
            }

            // 로켓 위치 계산 및 이동
            Vector3 newPosition = rocket.position + direction * distance;
            rocket.position = newPosition;

            yield return null;
        }
    }
}
