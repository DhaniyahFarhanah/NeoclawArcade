using System;
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

    GameObject joystick1;
    GameObject joystick2;
    GameObject upButton;
    GameObject downButton;
    GameObject normButton;

    Transform ballSpawn;

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
    public float openDuration = 2f;

    [Header("Clamping")]
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY, maxY;
    [SerializeField] float minZ, maxZ;

    [Header("Balls")]
    [SerializeField] int numOfSpheres;
    [SerializeField] float spawnTime;
    [SerializeField] GameObject[] ranCapsule;

    [Header("Animations")]
    [SerializeField] float ButtonPress;
    float startpos;

    [Header("Camera")]
    public int CurrentCam = 0;
    public Animator CamAnim;

    bool inTransition;

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
        joystick1 = GameSingleton.Instance.Joystick1;
        joystick2 = GameSingleton.Instance.Joystick2;
        upButton = GameSingleton.Instance.UpButton;
        downButton = GameSingleton.Instance.DownButton;
        normButton = GameSingleton.Instance.NormButton;

        ballSpawn = GameSingleton.Instance.BallSpawn.transform;

        startpos = joystick1.transform.localEulerAngles.x;

        SpawnCapsules(spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        //update claw movement
        clawMovement = new Vector3(moveLeftJoy.x, upDownInput * upDownSpeed, moveLeftJoy.y);
        //clawAttachment.transform.position += clawMovement * clawSpeed * Time.deltaTime;
        MoveMotor();

        //OpenClaws();
        RotateJoysticks();
        MoveButtons();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void MoveMotor()
    {
        Vector3 newPosition = clawAttachment.transform.position + clawMovement * clawSpeed * Time.deltaTime;

        // Clamp position to keep it within the defined range
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);


        motorRigidbody.MovePosition(newPosition);
    }
    private void ToggleClaw()
    {
        if (!inTransition)
        {
            isClawOpen = !isClawOpen;
            if (isClawOpen)
                StartCoroutine(MoveClaw(openDuration, maxClawRotation, minClawRotation));
            else
                StartCoroutine(MoveClaw(openDuration, minClawRotation, maxClawRotation));
        }
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

    IEnumerator MoveClaw(float transitionDur, float startAngle, float endAngle)
    {
        inTransition = true;
        float timer = 0;
        while (timer < transitionDur)
        {
            timer += Time.deltaTime;

            float newAngle1 = Mathf.LerpAngle(startAngle, endAngle, timer / transitionDur);
            claw1.transform.localRotation = Quaternion.Euler(claw1.transform.localRotation.eulerAngles.x, claw1.transform.localRotation.eulerAngles.y, newAngle1);

            float newAngle2 = Mathf.LerpAngle(startAngle, endAngle, timer / transitionDur);
            claw2.transform.localRotation = Quaternion.Euler(claw2.transform.localRotation.eulerAngles.x, claw2.transform.localRotation.eulerAngles.y, newAngle2);

            float newAngle3 = Mathf.LerpAngle(startAngle, endAngle, timer / transitionDur);
            claw3.transform.localRotation = Quaternion.Euler(claw3.transform.localRotation.eulerAngles.x, claw3.transform.localRotation.eulerAngles.y, newAngle3);

            yield return null;
        }
        inTransition = false;
        yield break;
    }

    void RotateJoysticks()
    {
        // Define rotation angles based on input
        float tiltAmountX = -moveLeftJoy.y * 20f;  // Right Left tilt
        float tiltAmountZ = moveLeftJoy.x * 20f; // Up Down tilt

        Quaternion targetRotationZ = Quaternion.Euler(startpos, 0f, tiltAmountZ);
        Quaternion targetRotationX = Quaternion.Euler(tiltAmountX + startpos, 0f, 0f);

        // Apply local rotation (smooth transition)
        joystick1.transform.localRotation = Quaternion.Lerp(joystick1.transform.localRotation, targetRotationZ, Time.deltaTime * 5f);
        joystick2.transform.localRotation = Quaternion.Lerp(joystick2.transform.localRotation, targetRotationX, Time.deltaTime * 5f);
    }

    void MoveButtons()
    {
        // Define button target positions
        Vector3 Uppressed = new Vector3(upButton.transform.localPosition.x, ButtonPress, upButton.transform.localPosition.z);
        Vector3 UpnotPressed = new Vector3(upButton.transform.localPosition.x, 0f, upButton.transform.localPosition.z);

        Vector3 DownPressed = new Vector3(downButton.transform.localPosition.x, ButtonPress, downButton.transform.localPosition.z);
        Vector3 DownnotPressed = new Vector3(downButton.transform.localPosition.x, 0f, downButton.transform.localPosition.z);

        Vector3 ButtonPressed = new Vector3(normButton.transform.localPosition.x, ButtonPress, normButton.transform.localPosition.z);
        Vector3 ButtonnotPressed = new Vector3(normButton.transform.localPosition.x, 0f, normButton.transform.localPosition.z);

        if (inTransition)
        {
            normButton.transform.localPosition = Vector3.Lerp(normButton.transform.localPosition, ButtonPressed, Time.deltaTime * 1f);
        }
        else
        {
            normButton.transform.localPosition = Vector3.Lerp(normButton.transform.localPosition, ButtonnotPressed, Time.deltaTime * 1f);
        }


        // Smoothly move buttons based on input
        if (upDownInput > 0) // Move up
        {
            upButton.transform.localPosition = Vector3.Lerp(upButton.transform.localPosition, Uppressed, Time.deltaTime * 5f);
            downButton.transform.localPosition = Vector3.Lerp(downButton.transform.localPosition, DownnotPressed, Time.deltaTime * 5f);
        }
        else if (upDownInput < 0) // Move down
        {
            upButton.transform.localPosition = Vector3.Lerp(upButton.transform.localPosition, UpnotPressed, Time.deltaTime * 5f);
            downButton.transform.localPosition = Vector3.Lerp(downButton.transform.localPosition, DownPressed, Time.deltaTime * 5f);
        }
        else // Return to normal
        {
            upButton.transform.localPosition = Vector3.Lerp(upButton.transform.localPosition, UpnotPressed, Time.deltaTime * 5f);
            downButton.transform.localPosition = Vector3.Lerp(downButton.transform.localPosition, DownnotPressed, Time.deltaTime * 5f);
        }
    }

    IEnumerator SpawnBalls(float transitionDur)
    {
        for (int i = 0; i <= numOfSpheres; i++)
        {
            // Define spawn area limits
            float rangeX = 5f; // Adjust as needed
            float rangeY = 0f; // If you want vertical variation, change this
            float rangeZ = 5f; // Adjust as needed

            // Generate a random offset within the defined range
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-rangeX, rangeX),
                UnityEngine.Random.Range(-rangeY, rangeY),
                UnityEngine.Random.Range(-rangeZ, rangeZ)
            );

            GameObject capsule = ranCapsule[UnityEngine.Random.Range(0, ranCapsule.Length)];
            Instantiate(capsule, ballSpawn.position + randomOffset, Quaternion.identity);
            yield return new WaitForSeconds(transitionDur / numOfSpheres);
        }
    }

    public void SpawnCapsules(float tranTime)
    {
        StartCoroutine(SpawnBalls(tranTime));
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
