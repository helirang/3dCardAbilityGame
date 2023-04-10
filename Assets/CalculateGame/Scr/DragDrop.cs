using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GameCard))]
public class DragDrop : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    //참고 : https://www.youtube.com/watch?v=BGr-7GZJNXg

    [Header("위치 셋팅")]
    Canvas originCanvas;
    Transform parent;

    [Header("이동 셋팅")]
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    int siblingIndex = 0;

    [Header("적용 체크 및 객체 관리 셋팅")]
    GameCard gameCard;
    IObjectPool<GameCard> cardPool;

    public event System.Action ReleaseEvent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        this.gameCard = this.GetComponent<GameCard>();
    }

    public void Setting(Canvas originCanvas, Transform parent, IObjectPool<GameCard> pool,
        int aliveCardNum)
    {
        this.originCanvas = originCanvas;
        this.parent = parent;
        this.cardPool = pool;
        this.transform.SetSiblingIndex(aliveCardNum);
    }

    /// <summary>
    /// <para>마나가 카드 코스트보다 많으면 드래그 작동</para>
    /// 카드 코스트가 크면 드래그 취소
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameCard.CostCheck())
        {
            siblingIndex = this.transform.GetSiblingIndex();
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
            this.transform.SetParent(originCanvas.transform);
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / originCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isApply = false;

        foreach (var gm in eventData.hovered)
        {
            if (gm.GetComponent <Ability_DropSlot>() != null)
            {
                ETeamNum activeTeam = gameCard.GetTargetTeam();
                isApply = gm.GetComponent<Ability_DropSlot>().TeamCheck(activeTeam);
                break;
            }
        }

        canvasGroup.alpha = 1f;
        this.transform.SetParent(parent);
        canvasGroup.blocksRaycasts = true;

        if (isApply)
        {
            Debug.Log("Release");
            cardPool.Release(this.gameCard);
        }
        else
        {
            this.transform.SetSiblingIndex(siblingIndex);
        }
    }
}
