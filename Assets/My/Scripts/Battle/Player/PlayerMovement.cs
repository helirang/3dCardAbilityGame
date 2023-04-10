using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 속도")]
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float rotateSpeed = 10f;

    [Header("회전할 객체(몸통)")]
    [Tooltip("회전할 객체(캐릭터의 메쉬를 가진 부모 객체)의 Transform")]
    [SerializeField] Transform bodyTransform;
    [SerializeField] Rigidbody myRigidbody;
    GameInput inputActions;
    Vector3 moveDir = new Vector3();

    public void SetInputSystem(GameInput inputActions)
    {
        this.inputActions = inputActions;
    }

    public void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        if (inputActions == null) 
        { 
            inputActions = new GameInput();
            inputActions.Enable();
            CustomDebugger.Debug(LogType.LogWarning,"Not Set Movement InputSystem");
        }
    }

    void Update()
    {
        Vector2 inputVector = GetMovementVectorNormalized();
        moveDir.x = inputVector.x;
        moveDir.y = 0f;
        moveDir.z = inputVector.y;
        moveDir = IsoVectorConvert(moveDir);
        myRigidbody.velocity = Vector3.zero;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        if (moveDir != Vector3.zero)
            bodyTransform.forward = Vector3.Slerp(bodyTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    /// <summary>
    /// 이동 방향을 45도 회전
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>입력한 vector 값을 Euler(0,45f,0)해서 return</returns>
    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, 45f, 0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }
}
