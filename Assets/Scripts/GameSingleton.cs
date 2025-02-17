using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSingleton : MonoBehaviour
{
    public static GameSingleton Instance { get; private set; }

    [Header("GAMEOBJECTS")]
    public Rigidbody ClawMotorRb;
    public GameObject ClawAttachment;
    public GameObject ClawPiece;
    public GameObject Claw1;
    public GameObject Claw2;
    public GameObject Claw3;
    public ClawMovement ClawScript;
    public GameObject Joystick1;
    public GameObject Joystick2;
    public GameObject UpButton;
    public GameObject DownButton;
    public GameObject NormButton;
    public GameObject BallSpawn;
    public GameObject Confefe;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps it alive across scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
    }
}

