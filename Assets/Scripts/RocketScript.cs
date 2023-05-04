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
    [SerializeField] private float journeyTime = 1.0f;

    void Update()
    {
        moveKey();
    }

    // 키코드 받는 함수
    void moveKey()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            planetPoint(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            planetPoint(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            planetPoint(2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            planetPoint(3);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            planetPoint(4);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            planetPoint(5);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            planetPoint(6);
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            planetPoint(7);
        }
    }
    
    // 로켓 이동 함수
    void planetPoint(int number)
    {
                    // 중간 값 계산 -> (로켓 + 도착지점) / 0.5f;
            Vector3 center = (rocket.position + finishPoint[number].position) * 0.5f;
            center -= new Vector3(0, 1f, 0);

            Vector3 rocketRiseCenter = rocket.position - center;
            // 거리계산
            Vector3 finishSetCener = finishPoint[number].position - center;

            // 시간 계산. 도착 시간
            float CompleteTime = (Time.time - startTime) / journeyTime;
            // Slerp -> 현재 위치, 목표위치, 속도
            this.rocket.transform.position = Vector3.Slerp(rocketRiseCenter, finishSetCener, CompleteTime * Time.deltaTime);
            transform.position += center;
    }

    // // 코루틴후 오브젝트는 다시 위쪽으로
    // IEnumerator testCor(int num)
    // {
    //     yield return new WaitForSeconds(2f);

    //     planetPoint(num);
    // }
}
