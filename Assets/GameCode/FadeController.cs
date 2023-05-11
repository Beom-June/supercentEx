using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : UIController
{
    public Button BtnStart;
    public Image ImgStart;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    [SerializeField] AnimationCurve fadeCurve;

    [SerializeField] private float fadeInTime = 3f;
    [SerializeField] private float fadeOutTime = 3f;
    [SerializeField] private bool startBtn = false;
    private bool isFadeFinished = false;

    public bool startBoolCall
    {
        get { return startBtn; }
        set { startBtn = value; }
    }
    void Start()
    {
        StartCall();
        StartCoroutine(StartBtnCor(fadeInTime));
    }


    void StartCall()
    {
        BtnStart.onClick.AddListener(() =>
        {
            StartCoroutine(Fade(1, 0, fadeInTime));

            // 버튼 오브젝트 꺼버림
            BtnStart.gameObject.SetActive(false);
        });
        StartCoroutine(WaitForFadeFinished());
    }
    IEnumerator Fade(float start, float end, float time)
    {
        float currentTime = 0.0f;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float percent = currentTime / time;

            Color color = ImgStart.color;                  // 알파 값 변경
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));

            ImgStart.color = color;
            yield return null;
        }

        isFadeFinished = true;

    }
    IEnumerator StartBtnCor(float time)
    {
        // true가 되면 waitUntil() 실행
        yield return new WaitUntil(() => isFadeFinished);
        yield return new WaitForSeconds(time);
        startBtn = true;


    }
    IEnumerator WaitForFadeFinished()
    {
        // true가 되면 waitUntil() 실행
        yield return new WaitUntil(() => isFadeFinished);

        // Fade가 끝난 후 startBtn 변수 값이 true로 변경되었으므로 isTimeBool 값을 true로 설정
        FindObjectOfType<UIController>().isTimeBoolCall = true;
    }
}
