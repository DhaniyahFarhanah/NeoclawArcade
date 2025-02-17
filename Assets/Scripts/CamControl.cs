using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public int CurrentCam;
    public Animator CamAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamAnim.SetInteger("Cam", CurrentCam);
    }

    
}
