using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSetting : MonoBehaviour
{
    public Transform center;                                                    // 타겟 -> 태양
    private Vector3 distVec;                                                     // 각 타겟과의 거리 저장
    [SerializeField] private float planetSpeed;                                 // 행성 공전 속도
    [SerializeField] private float saveRotate;                                  // 자전 저장
    [SerializeField] private float rotateSpeed;                                 // 자전 속도
    [SerializeField] private float planetInclin;                                // 행성 기울기
    [SerializeField] private bool isMoon = false;                               // 달 구분을 위한 bool 값

    void Start()
    {
        distVec = transform.position - center.position;                          // 시작시 포지션 고정
    }
    void Update()
    {
        PlanetOrbit();
        PlanetRoate();
    }

    // 행성 공전 함수
    void PlanetOrbit()
    {
            transform.position = center.position + distVec;                                          // 센터 위치 + 행성이 가진 위치 더함
            transform.RotateAround(center.position, Vector3.up, planetSpeed * Time.deltaTime);       // 행성 회전
            distVec = transform.position - center.position;                                         // 로테이션 후의 위치를 가지고 목표와의 거리 유지

        if(isMoon == true)
        {
            // 지구 주위를 공전하면서 자전. 
            // transform.right : center 주위를 도는 것이기 떄문.
            // forward사용 하면 center를 바라보면서 회전해서
            transform.RotateAround(center.position, transform.right, planetSpeed * Time.deltaTime);
        }

    }

    // 행성 자전 속도 함수
    void PlanetRoate()
    {

            saveRotate += rotateSpeed * Time.deltaTime;                         // 저장용 변수 += 속도 * Time.deltaTime(1프레임을 동일하게 하기 위해);
            transform.rotation = Quaternion.Euler(saveRotate, 0, planetInclin);                                     // 물체 회전을 위해 사용
    }
    // 외부에서 호출하여 회전 속도를 변경하는 함수
    public void SetRotateSpeed(float newSpeed)
    {
        rotateSpeed = newSpeed;
    }
}
