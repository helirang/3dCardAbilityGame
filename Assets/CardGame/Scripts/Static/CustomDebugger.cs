
public enum LogType 
{
    Log,
    LogWarning,
    LogError
}

public static class CustomDebugger
{
    public static void Debug(LogType type,string msg)
    {
#if UNITY_EDITOR
        switch (type)
        {
            case LogType.Log:
                UnityEngine.Debug.Log(msg);
                break;
            case LogType.LogWarning:
                UnityEngine.Debug.LogWarning(msg);
                break;
            case LogType.LogError:
                UnityEngine.Debug.LogError(msg);
                break;
        }
#endif
    }
}
