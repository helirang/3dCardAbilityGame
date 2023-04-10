using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("�̵� �ӵ�")]
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float rotateSpeed = 10f;

    [Header("ȸ���� ��ü(����)")]
    [Tooltip("ȸ���� ��ü(ĳ������ �޽��� ���� �θ� ��ü)�� Transform")]
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
    /// �̵� ������ 45�� ȸ��
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>�Է��� vector ���� Euler(0,45f,0)�ؼ� return</returns>
    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, 45f, 0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }
}
