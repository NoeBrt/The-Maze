using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    #region var definitions
    [Header("Player Character")]
    private CharacterController playerController;
    [Header("Crouching")]
    private float normalPlayerHeight;
    [SerializeField] private float crouchPlayerHeight;

    [Header("Movement and Physics")]
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float walkSpeed = 10f;
    public float speedGain { get; set; } = 0f;

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
    public Vector2 stepSoundVolume { get; set; } = new Vector2(0.45f, 0.65f);
    public Vector3 Velocity { get => velocity; set => velocity = value; }

    private UIPlayerManager playerUi;
    private float magnitude;


    // [SerializeField] private Camera playerCamera;


    #endregion
    void Start()
    {
        playerUi = GameObject.Find("Canvas").GetComponentInChildren<UIPlayerManager>(true);
        animator = GetComponent<Animator>();
        gameObject.SetActive(true);
        playerController = GetComponent<CharacterController>();
        FootAudioSource = transform.Find("Feet").GetComponent<AudioSource>();
        currentSpeed = walkSpeed;
        groundMask = LayerMask.GetMask("Ground");
        fieldOfView = 60f;
        distanceWalked = 0f;
        SettingManager.Instance.MusicSource = Camera.main.GetComponent<AudioSource>();
        SettingManager.Instance.SfxSounds.Add(transform.Find("Main Camera").Find("Torch").GetComponent<AudioSource>());
        SettingManager.Instance.SfxSounds.Add(transform.Find("Feet").GetComponent<AudioSource>());
        SettingManager.Instance.initSettingValue();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        crouch();
        Gravity();
        OnGroundVelocity();
        Jump();
        DisplayPauseUi();
    }
    void DisplayPauseUi()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerUi.PauseUi.SetActive(!playerUi.PauseUi.activeSelf);
            playerUi.setVisible(!playerUi.PauseUi.activeSelf);
            GameObject.Find("Canvas").transform.Find("OptionUi").gameObject.SetActive(false);
            Cursor.lockState = playerUi.PauseUi.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = playerUi.PauseUi.activeSelf ? 0 : 1;
        }
    }

    void crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            normalPlayerHeight = playerController.height;
            playerController.height = crouchPlayerHeight;
            Feet.localPosition += new Vector3(0, (normalPlayerHeight - crouchPlayerHeight) / 3.5f, 0);
            speedGain -= 3;
            stepSoundVolume -= Vector2.one * 0.3f;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            playerController.height = normalPlayerHeight;
            Feet.localPosition -= new Vector3(0, (normalPlayerHeight - crouchPlayerHeight) / 3.5f, 0);
            speedGain += 3;
            stepSoundVolume += Vector2.one * 0.3f;

        }
    }

    void Movement()
    {
        // Debug.Log(speed);
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        //transform.right and transform.forward to move the object in the local space;
        Vector3 movement = (HorizontalInput * transform.right + transform.forward * VerticalInput).normalized * (currentSpeed + speedGain);
        velocity = movement + Vector3.up * velocity.y;
        sprint();
        playerController.Move(velocity * Time.deltaTime);
        magnitude = new Vector2(velocity.x, velocity.z).magnitude;
        distanceWalked += Mathf.Clamp(new Vector3(velocity.x, velocity.z).magnitude, 0, 23f) * Time.deltaTime;
        if (distanceWalked > FootstepLength && isOnGround)
        {
            distanceWalked = distanceWalked % FootstepLength;
            PlayFootstep();
        }
    }

    void PlayFootstep()
    {
        int n = Random.Range(1, FootstepSounds.Length);
        FootAudioSource.pitch = Random.Range(0.7f, 1f);
        FootAudioSource.PlayOneShot(FootstepSounds[n], Random.Range(stepSoundVolume.x, stepSoundVolume.y));
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
            else if (currentSpeed >= walkSpeed && movingVelocity.magnitude >= 0 && (movingVelocity.magnitude < sprintSpeed || !Input.GetKey(KeyCode.LeftShift)))
            {

                currentSpeed -= walkSpeed * Time.deltaTime * 2;
            }

            float parameter = Mathf.InverseLerp(walkSpeed, sprintSpeed, currentSpeed + speedGain);
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
