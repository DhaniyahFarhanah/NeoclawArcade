using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInputs controls;

    GameObject ClawAttachment;

    [SerializeField] Vector2 moveLeftJoy;
    [SerializeField] float clawSpeed;
    Vector3 clawMovement;

    private void Awake()
    {
        controls = new PlayerInputs();

        controls.Claw.MoveClaw.performed += ctx => moveLeftJoy = ctx.ReadValue<Vector2>();
        controls.Claw.MoveClaw.canceled += ctx => moveLeftJoy = Vector2.zero;
    }

    private void Start()
    {
        ClawAttachment = GameSingleton.Instance.ClawAttachment;
    }

    // Update is called once per frame
    void Update()
    {
        //update claw movement
        clawMovement = new Vector3(moveLeftJoy.x, 0, moveLeftJoy.y);
        ClawAttachment.transform.position += clawMovement * clawSpeed * Time.deltaTime;
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
