using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class PlayerController : MonoBehaviour
{
    #region var definitions
    public UIPlayerManager PlayerUi { get; set; }
    [Header("Player Character")]
    private CharacterController playerController;
    [Header("Crouching")]
    private float normalPlayerHeight;
    [SerializeField] private float crouchPlayerHeight;

    [Header("Movement and Physics")]
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float walkSpeed = 10f;
    private float currentSpeed;
    [SerializeField] private float gravityForce = -(9.81f * 3); //gravity constant *3
    [Header("FootStep")]
    private Vector3 velocity;
    [SerializeField] private AudioSource FootAudioSource;
    [SerializeField] private AudioClip[] FootstepSounds;
    private float distanceWalked;
    [SerializeField] private float FootstepLength;

    [Header("Jumping")]
    private bool isOnGround;
    [SerializeField] private Transform Feet;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 3;
    float fieldOfView;
    Animator animator;
    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }

    // [SerializeField] private Camera playerCamera;


    #endregion
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(true);
        playerController = GetComponent<CharacterController>();
        PlayerUi = GameObject.Find("PlayerCanvas").GetComponent<UIPlayerManager>();
        FootAudioSource = transform.Find("Feet").GetComponent<AudioSource>();
        currentSpeed = walkSpeed;
        groundMask = LayerMask.GetMask("Ground");
        fieldOfView = 60f;
        distanceWalked = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        crouch();
        Gravity();
        OnGroundVelocity();
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
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        //transform.right and transform.forward to move the object in the local space;
        Vector3 movement = (HorizontalInput * transform.right + transform.forward * VerticalInput).normalized * currentSpeed;
        velocity = movement + Vector3.up * velocity.y;
        sprint();
        playerController.Move(velocity * Time.deltaTime);

        distanceWalked += new Vector3(velocity.x, velocity.z).magnitude * Time.deltaTime;
        if (distanceWalked > FootstepLength && isOnGround)
        {
            distanceWalked = distanceWalked % FootstepLength;
            PlayFootstep();
        }
    }

    void PlayFootstep()
    {
        int n = Random.Range(1, FootstepSounds.Length);
        //FootAudioSource.clip = FootstepSounds[n];
        FootAudioSource.pitch = Random.Range(0.7f, 1f);
        FootAudioSource.PlayOneShot(FootstepSounds[n], Random.Range(0.5f, 0.8f));
        // FootstepSounds[n] = FootstepSounds[0];
        //FootstepSounds[0] = FootAudioSource.clip;
    }






    private void sprint()
    {
        Vector2 movingVelocity = new Vector2(velocity.x, velocity.z);
        if (isOnGround)
        {
            if (Input.GetKey(KeyCode.LeftShift) && currentSpeed < sprintSpeed && movingVelocity.magnitude > 0)
            {
                currentSpeed += sprintSpeed * Time.deltaTime * 2;

            }
            else if (currentSpeed >= walkSpeed && movingVelocity.magnitude >= 0 && (movingVelocity.magnitude < sprintSpeed||!Input.GetKey(KeyCode.LeftShift)))
            {

                currentSpeed -= walkSpeed * Time.deltaTime * 2;
            }

            float parameter = Mathf.InverseLerp(walkSpeed, sprintSpeed, currentSpeed);
            fieldOfView = Mathf.Lerp(60, 70, parameter);
        }
        Camera.main.fieldOfView = fieldOfView;
    }


    IEnumerator addfieldOfView(float add, Camera camera)
    {
        for (float i = 0.1f; Mathf.Abs(i) <= Mathf.Abs(add); i += add / 10f)
        {
            camera.fieldOfView -= i;
            yield return null;
        }
    }
    private void setFieldOfView(float fielOfView)
    {
        if (fielOfView == Camera.main.fieldOfView)
        {
            return;
        }
        StartCoroutine(addfieldOfView(Camera.main.fieldOfView - fielOfView, Camera.main));
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





}
