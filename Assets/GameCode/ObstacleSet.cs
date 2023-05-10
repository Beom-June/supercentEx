using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 장애물 
public enum Obstacle
{
    none,                   // 아무것도 아님
    scaffolding,            // 가운데 축고정 발판
    trampoline,             // 트램폴린, 점프대
    upDown,                 // 위 아래로 움직이는 오브젝트
    wallAttack,             // 벽에서 미는 물체가 나옴
    vane,                    // 축을 기점으로 360도 회전하는 오브젝트
    chu,
}
public class ObstacleSet : MonoBehaviour
{
    public Obstacle obstacle = Obstacle.none;

    private float timer;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float amplitude = 30f;
    [SerializeField] private float frequency = 2f;


    void Start()
    {
    }
    void Update()
    {
    }

    void FixedUpdate()
    {
        switch (obstacle)
        {
            case Obstacle.chu:
                obstacle_chu();
                break;

            case Obstacle.vane:
                obstacle_vane();
                break;

            case Obstacle.upDown:
                obstacle_upDown();
                break;

            case Obstacle.wallAttack:
                break;
        }

    }


    // vane : 360도 회전하는 장애물
    void obstacle_vane()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }

    void obstacle_chu()
    {
        timer += Time.deltaTime;
        float angle = Mathf.Sin(timer * frequency) * amplitude;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void obstacle_upDown()
    {

    }
    private void OnTriggerEnter(Collider coll)
    {

    }
}
