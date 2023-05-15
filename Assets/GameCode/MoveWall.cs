using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public float delta = 2.0f; // 좌(우)로 이동 가능한 최대값
    public float speed = 3.0f; // 이동 속도

    private Vector3 startPosition;
    private Rigidbody rigid;

    void Start()
    {
        startPosition = transform.position;
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true; // Rigidbody를 Kinematic 모드로 설정하여 물리 효과 비활성화
    }

    void Update()
    {
        Vector3 newPosition = startPosition;
        newPosition.z += delta * Mathf.Sin(Time.time * speed);
        transform.position = newPosition;
    }
}
