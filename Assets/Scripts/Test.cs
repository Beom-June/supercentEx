using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Test : MonoBehaviour
{


    public int SceneNumber;
        void Start()
    {
    }

    public void SceneMove(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    void Update()
    {
        Test123();
    }

    public void Test123()
    {
     if(Input.GetMouseButtonDown(0))
     {

     }
    }
}
