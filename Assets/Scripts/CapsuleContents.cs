using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CapsuleType
{
    Tech,
    Character,
    Games,
    Snacks
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public class CapsuleContents : MonoBehaviour
{
    public CapsuleType type;
    public Rarity rarity;

    public GameObject sparkle;

    // Start is called before the first frame update
    void Start()
    {
        WhatsInside();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WhatsInside()
    {
        int percentage = UnityEngine.Random.Range(0, 100);

        if(percentage < 50)
        {
            rarity = Rarity.Common;
        }
        else if(percentage >= 50 && percentage < 80)
        {
            rarity = Rarity.Uncommon;
        }

        else if(percentage >= 80 && percentage <= 95)
        {
            rarity = Rarity.Rare;
        }

        else
        {
            rarity = Rarity.Legendary;
            sparkle.SetActive(true);
        }
    }
}
