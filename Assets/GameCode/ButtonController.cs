using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public PlayerController player;

    public Button JumpButton, ragdollButton, endButton;
    public Text txtTime;
    [SerializeField] bool isRag = false;
    [SerializeField] private float time = 0.0f;

    void Update()
    {
        TimeFuc();
    }
    void Start()
    {
        JumpCall();
        ragdollCall();
        endCall(false);
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
        // if (time > 0)
        time += Time.deltaTime;
        txtTime.text = $"{time:N2}";


        // txtTime.text = Mathf.Ceil(time).ToString();
    }
}
