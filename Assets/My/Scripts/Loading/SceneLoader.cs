using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI progressText;
    float progress;

    private void Start()
    {
        LoadLevel(NextSceneNum.num);
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
            if (progress < .9f)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                operation.allowSceneActivation = true;
            }
        }
    }
}
