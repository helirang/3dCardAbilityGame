using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameStart,Playing,End
/// </summary>
public interface IDataPipeInjection
{
    /// <summary>
    /// ���� ������Ʈ������ Start ������ ���Եǰ� ����
    /// </summary>
    /// <param name="dataPipe"></param>
    public void SetDatapipe(SOStageDataPipe dataPipe);

    /// <summary>
    /// �ݵ�� ����� DataPipe �̺�Ʈ�� �Ҵ� ������ ��.
    /// ex) datapipe.GameClearEvent -= GameClear;
    /// </summary>
    public void RemoveDatapipe();
}
