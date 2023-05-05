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

    private void Start()
    {
    }

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

        StartCoroutine(MoveRocketCoroutine(direction, speed));
    }
    IEnumerator MoveRocketCoroutine(Vector3 direction, float speed)
    {
        while (isMoving)
        {
            float distance = speed * Time.deltaTime;
            float elapsedTime = Time.time - startTime;
            float completeTime = elapsedTime / journeyTime;

            // 로켓 회전 처리
            if (completeTime >= 1f)
            {
                Vector3 upVector = rocket.position - direction;
                rocket.rotation = Quaternion.LookRotation(Vector3.forward, upVector);
            }

            // 로켓 위치 계산 및 이동
            Vector3 newPosition = rocket.position + direction * distance;
            rocket.position = newPosition;

            // 로켓이 도착지점에 도착하면 종료
            if (completeTime >= 1f)
            {
                isMoving = false;
            }

            yield return null;
        }
    }
}
