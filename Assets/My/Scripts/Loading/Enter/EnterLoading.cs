using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EScene
{
    Title,
    Lobby,
    Loading,
    Battle,
    Math,
    Run
}

public class EnterLoading : MonoBehaviour
{
    public EScene nextSene;

    public void Enter()
    {
        NextSceneNum.num = (int)nextSene;
        SceneManager.LoadScene((int)EScene.Loading);
    }
}
