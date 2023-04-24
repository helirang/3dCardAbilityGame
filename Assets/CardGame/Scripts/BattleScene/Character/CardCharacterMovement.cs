using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] UnityEngine.AI.NavMeshAgent nav;

    [Header("WorldUI")]
    Transform canvasTransform;

    Vector3 basePosition;
    Quaternion baseRotation;

    /// <summary>
    /// 목표와 일정 간격 이내로 진입하면 호출되는 이벤트
    /// </summary>
    public event System.Action ArriveEvent;
    public void Setting(Transform uiTransform)
    {
        canvasTransform = uiTransform;

        //턴 종료시, 할당될 위치와 회전
        basePosition = this.transform.position;
        baseRotation = this.transform.rotation;

        CanvasRotation();
    }
    public void MoveBasePosition()
    {
        this.transform.position = basePosition;
        this.transform.rotation = baseRotation;

        CanvasRotation();
    }

    public void CanvasRotation()
    {
        //canvasTransform.rotation = Quaternion.identity;
        //canvasTransform.rotation = Quaternion.Euler(30f, 0f, 0f);
        canvasTransform.rotation = Quaternion.Euler(
            Camera.main.transform.rotation.eulerAngles.x, 0f, 0f);
    }

    public void MoveStart(Vector3 target)
    {
        StartCoroutine(Action(target));
    }

    IEnumerator Action(Vector3 target)
    {
        bool isArrive = false;

        // navmeshAgent는 Action에서만 관리한다. 액션 시작에 활성화, 액션 끝 부분에 비활성화
        nav.isStopped = false;
        nav.SetDestination(target);

        if (nav.pathPending)
            yield return null;

        while (!isArrive)
        {
            if (nav.pathPending)
                yield return null;

            //타겟과 일정 거리 안에 가까워지면 도착 이벤트를 호출한다.
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                if (ArriveEvent != null)
                {
                    ArriveEvent.Invoke();
                }
                else
                {
                    CustomDebugger.Debug(LogType.LogWarning, 
                        "ArriveEvent에 바인딩 된 함수가 없습니다.");
                }
                isArrive = true; //While문 해제
            }

            CanvasRotation();
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;

            yield return null;
        }

        nav.isStopped = true;
    }

    private void OnDestroy()
    {
        ArriveEvent = null;
    }
}
