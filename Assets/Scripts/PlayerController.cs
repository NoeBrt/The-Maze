using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region var definitions

    // private Rigidbody playerBody;
    [Header("Ui")]
    private UIManager uiManager;

    [Header("Player Character")]
    private CharacterController playerController;

    [Header("Input")]
    private float HorizontalInput;
    private float VerticalInput;

    [Header("Crouching")]
    private float normalPlayerHeight;
    [SerializeField]
    private float crouchPlayerHeight;

    [Header("Movement and Physics")]
    [SerializeField]
    private float sprintSpeed = 20f;
    [SerializeField]
    private float walkSpeed = 10f;
    private float speed;
    [SerializeField]
    private float gravityForce = -(9.81f * 3); //gravity constant *3
    // Start is called before the first frame update
    private Vector3 velocity;

    [Header("Jumping")]
    private bool isOnGround;
    [SerializeField]
    private Transform Feet;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float jumpHeight = 3;

    [Header("Bonus")]
    [SerializeField]
    private float bonusEffectDuration = 5f;
    [SerializeField]
    private float timeToRemove = 10f;
    [SerializeField]
    private float speedBonusGain = 10f;


    #endregion

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        speed = walkSpeed;
        groundMask=LayerMask.GetMask("Ground");
        //  playerBody=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        crouch();
        Gravity();
        OnGroundVelocity();
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            Jump();
            // JumpRigibody();
        }
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
        Debug.Log(speed);
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        //transform.right and transform.forward to move the object in the local space;
        Vector3 movement = (HorizontalInput * transform.right + transform.forward * VerticalInput);
        sprint();
        playerController.Move(movement * Time.deltaTime * speed);
    }
    private void sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
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
        //√[2×g×h] 
        velocity.y = Mathf.Sqrt(-3f * gravityForce * jumpHeight);
    }
    /*
    void JumpRigibody(){
    Physics.gravity*=1.5f;
    playerBody.AddForce(Vector3.up*20f,ForceMode.Impulse);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBonus"))
        {
            Destroy(other.gameObject);
            StartCoroutine(speedBonusEffect(bonusEffectDuration));
            uiManager.updateBonusText("Speed Bonus", bonusEffectDuration);
        }
        else if (other.CompareTag("TimeBonus"))
        {
            Destroy(other.gameObject);
            uiManager.removeTime(timeToRemove);
            uiManager.updateBonusText("Time Bonus", bonusEffectDuration);
        }


    }
    IEnumerator speedBonusEffect(float time)
    {
        uiManager.updateBonusText("Speed ++");
        walkSpeed += speedBonusGain;
        sprintSpeed += speedBonusGain;

        yield return new WaitForSeconds(time);
        walkSpeed -= speedBonusGain;
        sprintSpeed -= speedBonusGain;
        uiManager.updateBonusText("");
    }

}
