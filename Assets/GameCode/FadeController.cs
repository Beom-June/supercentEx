using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Button BtnStart;
    public Image ImgStart;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    [SerializeField] AnimationCurve fadeCurve;

    [SerializeField] private float fadeInTime = 5f;
    [SerializeField] private float fadeOutTime = 5f;
    void Start()
    {
        StartCall();
    }


    void StartCall()
    {
        BtnStart.onClick.AddListener(() =>
        {
            BtnStart.gameObject.SetActive(false);
            StartCoroutine(Fade(1, 0, fadeInTime));
        });
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
        this.gameObject.SetActive(false);
    }
}
