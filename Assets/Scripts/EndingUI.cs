using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    public GameObject[] Endings;

    public void GetEnding(int endingNum)
    {
        if (endingNum > Endings.Length)
        {
            Endings[0].SetActive(true);
        }

        Endings[endingNum].SetActive(true);
    }

    public void RestarPuzzle()
    {
        SceneManager.LoadScene("MainScene");
    }
}
