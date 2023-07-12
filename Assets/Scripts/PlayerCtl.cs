using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtl : MonoBehaviour
{
    //�ӵ� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;
    //����
    [SerializeField]
    private float jumpForce;
    //����
    private bool isRun;
    private bool isGround = true;
    private bool isCrouch;
    //�ɱ�
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    private Rigidbody myRigid;
    //ȭ�� ����
    [SerializeField]
    private float lookSensitivity;
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    [SerializeField]
    private Camera theCamera;

    private Animator ani;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
        ani = GetComponent<Animator>();
    }
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }
    //�̵�
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        ani.SetBool("Walk", _velocity.magnitude > 0);
        ani.SetBool("Run", isRun);
        Debug.Log(ani.GetBool("Walk") + "�ȱ�");
        Debug.Log(ani.GetBool("Run") + "�ٱ�");
    }
    //ī�޶� ����
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    //ĳ���� ī�޶� �¿�
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            Running();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    private void Running()
    {
        isCrouch = true;
        Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
        {
            Crouch();
        }
        myRigid.velocity = transform.up * jumpForce;
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    public void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGround == true)
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    IEnumerator CrouchCoroutine()
    {
        float duration = 0.2f; // �ɱ�/�Ͼ�� �ð�
        float elapsedTime = 0f;
        Vector3 initialCameraPosition = theCamera.transform.localPosition;
        Vector3 targetCameraPosition = new Vector3(0f, applyCrouchPosY, 0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // �ε巯�� �ɱ� ����� ���� ��� ����Ͽ� ����
            float curveValue = Mathf.Sin(t * Mathf.PI * 0.5f); // ���۰� �� �κ��� �� ������ �����ǵ��� Sin � ���
            Vector3 newCameraPosition = Vector3.Lerp(initialCameraPosition, targetCameraPosition, curveValue);
            theCamera.transform.localPosition = newCameraPosition;

            yield return null;
        }
        theCamera.transform.localPosition = targetCameraPosition;
    }
}