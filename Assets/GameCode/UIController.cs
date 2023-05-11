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

        // FadeController Ŭ������ �ν��Ͻ��� ã���ϴ�.
        FadeController fadeController = FindObjectOfType<FadeController>();

        // FadeController Ŭ�������� startBoolCall ������Ƽ�� ���� �����Ͽ� isTimeBool ���� �����մϴ�.
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
