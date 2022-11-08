using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    #region var definitions

    [Header("Input")]
    private float xMouseInput;
    private float yMouseInput;
    [SerializeField]
    private float mouseSensitivity = 100f;
    private float xAxisRotation = 0f;

    [Header("Player")]
    [SerializeField]
    private Transform playerBody;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        xMouseInput = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yMouseInput = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xAxisRotation -= yMouseInput;
        xAxisRotation = Mathf.Clamp(xAxisRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xAxisRotation, 0f, 0f);
    }
    void Update()
    {
        playerBody.Rotate(Vector3.up * xMouseInput);

    }
}
