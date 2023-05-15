using UnityEngine;

public class Controller : MonoBehaviour
{
	private	Movement3D movement3D;

	public Camera mainCamera; // 메인 카메라
    Animator anim;

    // Fuction
    [SerializeField] private float speed = 3.0f;      // 캐릭터 움직임 스피드
    [SerializeField] private Vector3 movePoint; // 이동 위치 저장
    [SerializeField] private bool moveBool;
    private static readonly int MoveParam = Animator.StringToHash("isMove");

	private void Awake()
	{
		movement3D = GetComponent<Movement3D>();
	}

	private void Update()
	{
		// 마우스 왼쪽 버튼을 눌렀을 때
		if ( Input.GetMouseButtonDown(0) )
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if ( Physics.Raycast(ray, out hit, Mathf.Infinity) )
			{
				movement3D.MoveTo(hit.point);
			}
		}
		// if(moveBool)
		// {
		// 	Move();
		// 	PlayerTurn();
		// }
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
            //anim.SetBool(MoveParam, false);
        }
    }
    // 플레이어 시점
    void PlayerTurn()
    {
        // 이동 방향 카메라 시점
        transform.LookAt(transform.position + movePoint);
    }
}

