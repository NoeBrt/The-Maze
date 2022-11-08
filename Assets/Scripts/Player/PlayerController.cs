using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class PlayerController : MonoBehaviour
{
    #region var definitions

    [Header("Ui")]
    public UIPlayerManager PlayerUi{get; set;}
    private WinScreenController EndScreen;


    [Header("Player Character")]
    private CharacterController playerController;
    [SerializeField] private GameObject lightTorch;

    [Header("Input")]
    private float HorizontalInput;
    private float VerticalInput;

    [Header("Crouching")]
    private float normalPlayerHeight;
    [SerializeField] private float crouchPlayerHeight;

    [Header("Movement and Physics")]
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float walkSpeed = 10f;
    private float currentSpeed;
    [SerializeField] private float gravityForce = -(9.81f * 3); //gravity constant *3
    private Vector3 velocity;

    [Header("Jumping")]
    private bool isOnGround;
    [SerializeField] private Transform Feet;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 3;

    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }

    // [SerializeField] private Camera playerCamera;


    #endregion
    void Start()
    {
        gameObject.SetActive(true);
        playerController = GetComponent<CharacterController>();
        uiManager = GameObject.Find("PlayerCanvas").GetComponent<UIManager>();
        currentSpeed = walkSpeed;
        groundMask = LayerMask.GetMask("Ground");
        EndScreen = GameObject.Find("Canvas").GetComponent<WinScreenController>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        crouch();
        Gravity();
        OnGroundVelocity();
        toggleLight();
        Jump();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            normalPlayerHeight = playerController.height;
            playerController.height = crouchPlayerHeight;
            Feet.localPosition += new Vector3(0, (normalPlayerHeight - crouchPlayerHeight) / 2, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerController.height = normalPlayerHeight;
            Feet.localPosition -= new Vector3(0, (normalPlayerHeight - crouchPlayerHeight) / 2, 0);
        }
    }

    void Movement()
    {
        // Debug.Log(speed);
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        //transform.right and transform.forward to move the object in the local space;
        Vector3 movement = (HorizontalInput * transform.right + transform.forward * VerticalInput);
        sprint();
        playerController.Move(movement * Time.deltaTime * currentSpeed);
    }
    private void sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
    void Gravity()
    {
        velocity.y += gravityForce * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }
    void OnGroundVelocity()
    {
        isOnGround = Physics.CheckSphere(Feet.position, groundDistance, groundMask);
        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -1f;
        }

    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            //√[2×g×h] 
            velocity.y = Mathf.Sqrt(-3f * gravityForce * jumpHeight);
        }
    }


    void toggleLight()
    {
        if (Input.GetKeyDown(KeyCode.F))
            lightTorch.SetActive(!lightTorch.activeSelf);
    }



}
