using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    GameObject confefe;

    public Transform confefeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        confefe = GameSingleton.Instance.Confefe;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        Instantiate(confefe, confefeSpawn);
    }
}
