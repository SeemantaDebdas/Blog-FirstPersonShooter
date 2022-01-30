using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Camera cam;
    CharacterController controller;

    [SerializeField] float gravity = 20f;
    [SerializeField] float jumpHeight = 3f;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;

    [SerializeField] float mouseSensitivity;
    [SerializeField] bool isCursorLocked;

    [SerializeField] float velocitySmoothTime = 0.3f;
    [SerializeField] float mouseSmoothTime = 0.03f;

    Vector3 currentVelocity;
    Vector3 currentVelocityRef;

    Vector3 currentMouseDelta;
    Vector3 currentMouseDeltaRef;

    float speed;
    float verticalRotation = 0;

    float velocityY = 0;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (isCursorLocked)
            Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        MouseLook();
    }

    void MouseLook()
    { 
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;
        currentMouseDelta = Vector3.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaRef, mouseSmoothTime);

        verticalRotation -= currentMouseDelta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * currentMouseDelta.x);
    }

    void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");


        HandleGravityAndJump();
        
        speed = (Input.GetKey(KeyCode.LeftShift)) ? sprintSpeed : moveSpeed;

        Vector3 targetVelocity = (transform.right * inputX + transform.forward * inputY).normalized * speed + Vector3.up * velocityY;
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref currentVelocityRef, velocitySmoothTime);

        controller.Move(currentVelocity * Time.deltaTime);
    }

    void HandleGravityAndJump()
    {
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -0.1f;
        }
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }
        velocityY -= gravity * Time.deltaTime;
    }
}
