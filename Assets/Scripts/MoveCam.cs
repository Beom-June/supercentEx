using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] private float speed_move = 2.0f;
    //[SerializeField] private float speed_rot = 2.0f;

    public Transform targetSun;                                         // 타겟 -> 태양

    void Update()
    {
        moveCamera();
    }

    void moveCamera()
    {
        // 입력 값 받아옴
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        // 속도 계산
        Horizontal = Horizontal * speed_move * Time.deltaTime;

        
        Vertical = Vertical * speed_move * Time.deltaTime;
        
        // ransform.RotateAround(targetSun.position, Vector3.up, speed_rot * Time.deltaTime);       // 행성 회전

        // 이동
        transform.Translate(Vector3.right * Horizontal);
        transform.Translate(Vector3.forward * Vertical);

    }
}
