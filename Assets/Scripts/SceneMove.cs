using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    int index = 0;
    private void Update()
    {
        MoveScenePlus(index);
        MoveSceneMinus(index);
    }

    public void MoveScenePlus(int sceneId)
    {
        sceneId = SceneManager.GetActiveScene().buildIndex + 1;
        if (Input.GetMouseButtonDown(0))
        {
            if (sceneId < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(sceneId);
            }
            else
            {
                Debug.Log("마지막 씬입니다.");
            }

        }
    }

    public void MoveSceneMinus(int sceneId)
    {
            sceneId = SceneManager.GetActiveScene().buildIndex - 1;
        if (Input.GetMouseButtonDown(1))
        {
            if (sceneId >= 0)
            {
                SceneManager.LoadScene(sceneId);
            }
            else
            {
                Debug.Log("첫 번째 씬입니다.");
            }

        }
    }
}
