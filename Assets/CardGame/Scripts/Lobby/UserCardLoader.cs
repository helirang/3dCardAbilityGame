using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

public class UserCardLoader : MonoBehaviour
{
    [SerializeField] List<SOCard> basicCardList;

    private void Awake()
    {
        //@todo 테스트용 동작 / 차후 변경하기
        foreach (var data in basicCardList)
        {
            CardStorage.AddCard(data);
        }
    }

    // @todo 모든 SOCard를 호출하는 기능 구현하기 or 모든 SOCard를 캐싱하기
    // SOCard를 어드레서블 에셋으로 변경하기
    // AssetReference.Asset을 활용해 참조가 없는 SOCard만 로드하기
    void LoadUserCardData()
    {

    }
}
