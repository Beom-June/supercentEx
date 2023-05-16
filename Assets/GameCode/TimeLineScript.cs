using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineScript : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject fadeUI;
    public GameObject mainCam;
    public GameObject timelineCam;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.Stop(); // 컴포넌트가 꺼져 있도록 초기에 멈춤
    }
    private void OnTriggerEnter(Collider Coll)
    {
        if (Coll.gameObject.CompareTag("Player"))
        {
            if (director != null)
            {
                director.Play();
                fadeUI.SetActive(true);

                //timelineCam.SetActive(true);
                //mainCam.SetActive(false);
            }
        }
        if (Coll.gameObject.CompareTag("FinishPoint"))
        {
            if (director != null)
            {
                director.Stop();

                mainCam.SetActive(true);
                timelineCam.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
        }
    }
}