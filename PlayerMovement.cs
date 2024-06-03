using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float normalSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 8f;
    public float gravity = 20f;
    public float lookSpeed = 1f;
    public float lookXLimit = 80f;

 
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    public bool canMove = true;
    public bool isJumping = false;
  
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
        #region Player Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        // Andar ou correr pressionando shift
        bool isRuning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX;
        float curSpeedY;

        if (canMove)
        {
            if (isRuning)
            {
               curSpeedX = runSpeed * Input.GetAxis("Vertical");
               curSpeedY = runSpeed * Input.GetAxis("Horizontal");
            }
            else
            {
                curSpeedX = normalSpeed * Input.GetAxis("Vertical");
                curSpeedY = normalSpeed * Input.GetAxis("Horizontal");
            }
        }
        else
        {
          curSpeedX = 0;
          curSpeedY = 0;  
        }

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        #endregion
 
        #region Jump

        bool jump = Input.GetButton("Jump");

        if (jump && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        #endregion
 
        #region Rotation Camera
        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
 
        #endregion
    }
}
