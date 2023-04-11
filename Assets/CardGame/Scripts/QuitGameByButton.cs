using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameByButton : MonoBehaviour
{
    [SerializeField] Button quitBtn;

    private void Awake()
    {
        quitBtn.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
