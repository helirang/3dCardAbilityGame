using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterLoading_AnykeyPress : EnterLoading
{
    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Enter();
        }
    }
}
