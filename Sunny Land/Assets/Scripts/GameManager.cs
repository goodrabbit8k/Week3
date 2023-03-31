using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool finish;

    void Start()
    {
        finish = false;
    }

    void Update()
    {
        Finish();
    }

    void Finish()
    {
        if (finish)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
