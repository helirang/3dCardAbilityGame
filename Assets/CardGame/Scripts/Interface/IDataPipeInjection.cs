using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameStart,Playing,End
/// </summary>
public interface IDataPipeInjection
{
    /// <summary>
    /// 현재 프로젝트에서는 Start 이전에 주입되게 통일
    /// </summary>
    /// <param name="dataPipe"></param>
    public void SetDatapipe(SOStageDataPipe dataPipe);

    /// <summary>
    /// 반드시 사용한 DataPipe 이벤트는 할당 해제할 것.
    /// ex) datapipe.GameClearEvent -= GameClear;
    /// </summary>
    public void RemoveDatapipe();
}
