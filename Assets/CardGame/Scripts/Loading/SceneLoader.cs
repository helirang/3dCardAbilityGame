using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI progressText;
    float progress;
    bool isComplete = true;
    int count = 0;
    private void Start()
    {
        LoadLevel(NextSceneNum.num);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadLevel(int sceneIndex)
    {
        switch (sceneIndex)
        {
            //배틀씬이면 어빌리티(AddressableAssets)를 로드한다.
            case (int)EScene.Battle:
                LoadAbility();
                break;
            default:
                UnLoadAbility();
                StartCoroutine(LoadAsynchronously(sceneIndex));
                break;
        }
    }

    IEnumerator LoadAsynchronously(int sceneIndex,float startProgress=0f)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            progress = Mathf.Clamp(operation.progress / .9f,startProgress,1f);
            loadingBar.value = progress;
            progressText.text = progress * 100f + "%";

            if (progress >= .9f)
            {
                if(isComplete)
                yield return new WaitForSeconds(0.2f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// @todo 씬로더와 Ability_Origin과의 연계 해제 고려하기
    /// </summary>
    void LoadAbility()
    {
        List<SOCard> cards =  CardStorage.GetAllCardDatas();
        count = 0;
        foreach (var card in cards)
        {
            Addressables.LoadAssetAsync<Ability_Origin>(card.abilityAsset).Completed +=
                (handle) =>
                {
                    card.AbilityComplete(handle.Result,handle);
                    AbilityLoadCheck(cards.Count);
                };
        }
    }

    void UnLoadAbility()
    {
        List<SOCard> cards = CardStorage.GetAllCardDatas();
        foreach (var card in cards)
        {
            card.UnLoadAbility();
        }
    }

    void AbilityLoadCheck(int max)
    {
        float progressValue = 0.1f;

        count++;

        progress = Mathf.Clamp(count/max, 0f, progressValue);
        loadingBar.value = progress;
        progressText.text = progress * 100f + "%";

        if (count == max)
        {
            CustomDebugger.Debug(LogType.Log,"AbilityLoadComplete");
            StartCoroutine(LoadAsynchronously(NextSceneNum.num, progressValue));
        }
    }
}
