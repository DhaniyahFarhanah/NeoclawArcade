using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInputs controls;

    GameObject clawAttachment;
    GameObject clawPiece;
    GameObject claw1;
    GameObject claw2;
    GameObject claw3;

    [SerializeField] Vector2 moveLeftJoy;
    [SerializeField] quaternion rotation;
    [SerializeField] float clawSpeed;

    [SerializeField] float maxClawRotation;
    [SerializeField] float minClawRotation;
    [SerializeField] float clawRotationSpeed;
    [SerializeField] bool isClawOpen;
    [SerializeField] float targetAngle;
    Vector3 clawMovement;

    private void Awake()
    {
        controls = new PlayerInputs();

        controls.Claw.MoveClaw.performed += ctx => moveLeftJoy = ctx.ReadValue<Vector2>();
        controls.Claw.MoveClaw.canceled += ctx => moveLeftJoy = Vector2.zero;

        controls.Claw.CloseClaw.performed += ctx => ToggleClaw();
    }

    private void Start()
    {
        clawAttachment = GameSingleton.Instance.ClawAttachment;
        clawPiece = GameSingleton.Instance.ClawPiece;
        claw1 = GameSingleton.Instance.Claw1;
        claw2 = GameSingleton.Instance.Claw2;
        claw3 = GameSingleton.Instance.Claw3;
    }

    // Update is called once per frame
    void Update()
    {
        //update claw movement
        clawMovement = new Vector3(moveLeftJoy.x, 0, moveLeftJoy.y);
        clawAttachment.transform.position += clawMovement * clawSpeed * Time.deltaTime;

        MoveClaws();
    }

    private void ToggleClaw()
    {
        isClawOpen = !isClawOpen;

        targetAngle = isClawOpen ? maxClawRotation : minClawRotation;
        
    }

    private void MoveClaws()
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
