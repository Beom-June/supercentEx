using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> Music; // 사용할 BGM
    public Text soundText;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            RandomPlay();
            soundText.text = audioSource.clip.name; // 재생 중인 음악 정보를 Text UI에 표시
        }
    }

    void RandomPlay()
    {
        audioSource.clip = Music[Random.Range(0, Music.Count)];
        audioSource.Play();
    }
}
