using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    //All Gameobjects
    PlayerInputs controls;

    GameObject clawAttachment;  
    GameObject clawPiece;
    GameObject claw1;
    GameObject claw2;
    GameObject claw3;
    Rigidbody motorRigidbody;

    [Header("Motor Movement")]
    Vector2 moveLeftJoy;                        //Input for joystick movement
    Vector3 clawMovement;                       //Conversion of Input
    [SerializeField] float clawSpeed;           //Speed multiplier for claw movement

    [Header("Claw Lowering")]
    [SerializeField] float upDownInput;         //Input of the button up down
    [SerializeField] float upDownSpeed;         //Speed of moving up and down
    [SerializeField] float clawLowerSpeed;      //Speed multiplier for claw move up or down

    [Header("Claw Grabbing")]
    [SerializeField] float maxClawRotation;     //Each individual claw rotation when open
    [SerializeField] float minClawRotation;     //Each individual claw rotation when close 
    [SerializeField] float clawRotationSpeed;   //Speed of the claw movement
    bool isClawOpen;                            //Bool to toggle claw
    float targetAngle;                          //Current angle needed for claw movement

    private void Awake()
    {
        controls = new PlayerInputs();

        controls.Claw.MoveClaw.performed += ctx => moveLeftJoy = ctx.ReadValue<Vector2>();
        controls.Claw.MoveClaw.canceled += ctx => moveLeftJoy = Vector2.zero;

        controls.Claw.LowerClaw.performed += ctx => upDownInput = ctx.ReadValue<float>();
        controls.Claw.LowerClaw.canceled += ctx => upDownInput = 0f;

        controls.Claw.CloseClaw.performed += ctx => ToggleClaw();
    }

    private void Start()
    {
        clawAttachment = GameSingleton.Instance.ClawAttachment;
        clawPiece = GameSingleton.Instance.ClawPiece;
        claw1 = GameSingleton.Instance.Claw1;
        claw2 = GameSingleton.Instance.Claw2;
        claw3 = GameSingleton.Instance.Claw3;
        motorRigidbody = GameSingleton.Instance.ClawMotorRb;
    }

    // Update is called once per frame
    void Update()
    {
        //update claw movement
        clawMovement = new Vector3(moveLeftJoy.x, upDownInput * upDownSpeed, moveLeftJoy.y);
        //clawAttachment.transform.position += clawMovement * clawSpeed * Time.deltaTime;
        MoveMotor();

        OpenClaws();
    }

    private void MoveMotor()
    {
        Vector3 newPosition = clawAttachment.transform.position + clawMovement * clawSpeed * Time.deltaTime;
        motorRigidbody.MovePosition(newPosition);
    }

    private void ToggleClaw()
    {
        isClawOpen = !isClawOpen;

        targetAngle = isClawOpen ? maxClawRotation : minClawRotation;
        
    }

    private void MoveMotorUp()
    {
        //Vector3 newPosition = clawAttachment.transform.position +
    }

    private void OpenClaws()
    {
        float currentAngle1 = claw1.transform.localEulerAngles.z;
        float newAngle1 = Mathf.LerpAngle(currentAngle1, targetAngle, Time.deltaTime * clawRotationSpeed);
        claw1.transform.localEulerAngles = new Vector3(claw1.transform.eulerAngles.x, claw1.transform.eulerAngles.y, newAngle1);

        float currentAngle2 = claw1.transform.localEulerAngles.z;
        float newAngle2 = Mathf.LerpAngle(currentAngle2, targetAngle, Time.deltaTime * clawRotationSpeed);
        claw2.transform.localEulerAngles = new Vector3(claw2.transform.eulerAngles.x, claw2.transform.eulerAngles.y, newAngle2);

        float currentAngle3 = claw1.transform.localEulerAngles.z;
        float newAngle3 = Mathf.LerpAngle(currentAngle3, targetAngle, Time.deltaTime * clawRotationSpeed);
        claw3.transform.localEulerAngles = new Vector3(claw3.transform.eulerAngles.x, claw3.transform.eulerAngles.y, newAngle3);
    }

    private void OnEnable()
    {
        controls.Claw.Enable();
    }
    private void OnDisable()
    {
        controls.Claw.Disable();
    }
}
