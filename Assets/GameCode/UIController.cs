using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerController player;
    public FadeController fadeControll;

    public Button JumpButton, ragdollButton, endButton;
    public Text txtTime;
    [SerializeField] bool isRag = false;
    [SerializeField] bool isTimeBool = false;
    [SerializeField] private float time = 0.0f;

    public bool isTimeBoolCall
    {
        get { return isTimeBool; }
        set { isTimeBool = value; }
    }

    void Update()
    {
        if (isTimeBool == true)
            TimeFuc();
    }
    void Start()
    {
        JumpCall();
        ragdollCall();
        endCall(false);

        // FadeController 클래스의 인스턴스를 찾습니다.
        FadeController fadeController = FindObjectOfType<FadeController>();

        // FadeController 클래스에서 startBoolCall 프로퍼티의 값을 참조하여 isTimeBool 값을 설정합니다.
        isTimeBool = fadeController.startBoolCall;
    }

    void JumpCall()
    {
        JumpButton.onClick.AddListener(() =>
        {
            player.playerRigidCall.AddForce(Vector3.up * 5, ForceMode.Impulse);
            player.playerAnimCall.SetTrigger("doJump");
        });
    }
    void ragdollCall()
    {

        ragdollButton.onClick.AddListener(() =>
        {
            player.SetRagdoll(true);
        });
    }

    void endCall(bool flag)
    {
        endButton.onClick.AddListener(() =>
        {
            EditorApplication.isPlaying = flag;
            Debug.Log("TT");
        });
    }
    void TimeFuc()
    {
        if (fadeControll.startBoolCall)
        {
            time += Time.deltaTime;
            txtTime.text = $"{time:N2}";
        }
    }

}
