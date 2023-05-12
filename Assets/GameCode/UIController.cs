using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerController player;
    public FadeController fadeControll;
    public GameObject endPopUp;

    public Button JumpButton, ragdollButton, endButton, restartButton;
    public Text txtTime;
    public Text txtDurTime;
    public AudioSource playerSfx;
    public AudioClip crowdSfx;
    [SerializeField] bool isRag = false;
    [SerializeField] bool isTimeBool = false;
    [SerializeField] private float time = 0.0f;
    [SerializeField] private float endTime = 0.0f;

    public bool isTimeBoolCall
    {
        get { return isTimeBool; }
        set { isTimeBool = value; }
    }

    void Update()
    {
        if (isTimeBool == true)
        {
            TimeFuc();
            DurTime();
        }
    }
    void Start()
    {
        JumpCall();
        RagdollCall();
        EndCall(false);
        RestartCall(true);


        // FadeController Ŭ������ �ν��Ͻ��� ã���ϴ�.
        FadeController fadeController = FindObjectOfType<FadeController>();

        isTimeBool = fadeController.startBoolCall;

        // 시간 기록
        time = Time.time;
    }

    void JumpCall()
    {
        JumpButton.onClick.AddListener(() =>
        {
            player.playerRigidCall.AddForce(Vector3.up * 5, ForceMode.Impulse);
            player.playerAnimCall.SetTrigger("doJump");

            player.playerSfxCall.PlayOneShot(player.jumpSfxCall);
        });
    }
    void RagdollCall()
    {

        ragdollButton.onClick.AddListener(() =>
        {
            player.SetRagdoll(true);
        });
    }

    void EndCall(bool flag)
    {
        endButton.onClick.AddListener(() =>
        {
            EditorApplication.isPlaying = flag;

            // 사운드 재생
            playerSfx.PlayOneShot(crowdSfx);
        });
    }
    void RestartCall(bool flag)
    {
        restartButton.onClick.AddListener(() =>
        {
            EditorApplication.isPlaying = flag;
            player.transform.position = player.spawnPoint.transform.position;

            // 팝업 끄기위함
            endPopUp.SetActive(false);

            // 사운드 재생
            playerSfx.PlayOneShot(crowdSfx);
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

    // PopUp UI 에서 걸린 시간 출력
    void DurTime()
    {
        if (fadeControll.startBoolCall)
        {
            // 현재 시간과 시작 시간을 빼서 경과 시간을 계산
            endTime = Time.time - time;
            txtDurTime.text = $"{endTime:N2}";
        }
    }

}
