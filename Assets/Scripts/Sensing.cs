using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public bool isPlaying;
    GameObject confefe;
    [SerializeField] GameObject EndScreen;

    [Header("UI Elements")]
    [SerializeField] Image capImage;
    [SerializeField] TMP_Text typeText;
    [SerializeField] Image itemGot;

    [Header("Capsule")]
    [SerializeField] Sprite SnackSprite;
    [SerializeField] Sprite TechSprite;
    [SerializeField] Sprite GamesSprite;
    [SerializeField] Sprite CharacterSprite;

    [Header("Tech")]
    [SerializeField]
    string[] techNames;
    [SerializeField]
    Sprite[] techSprites;

    [Header("Snack")]
    [SerializeField]
    string[] snackNames;
    [SerializeField]
    Sprite[] snackSprites;

    [Header("Games")]
    [SerializeField]
    string[] gamesNames;
    [SerializeField]
    Sprite[] gamesSprites;

    [Header("Character")]
    [SerializeField]
    string[] charNames;
    [SerializeField]
    Sprite[] charSprites;

    public Transform confefeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        confefe = GameSingleton.Instance.Confefe;
        StartCoroutine(Playing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaying)
        {
            Instantiate(confefe, confefeSpawn);

            if(other.gameObject.GetComponent<CapsuleContents>() != null)
            {
                CapsuleContents script = other.gameObject.GetComponent<CapsuleContents>();
                StartCoroutine(StartScreen(script.type, script.rarity));
            }
            
        }
        
    }

    IEnumerator Playing()
    {
        yield return new WaitForSeconds(10f);
        isPlaying = true;
    }

    IEnumerator StartScreen(CapsuleType capType, Rarity capRare)
    {
        FillScript(capType, capRare);
        yield return new WaitForSeconds(0.5f);
        EndScreen.SetActive(true);
        yield return new WaitForSeconds(5.5f);
        EndScreen.SetActive(false);
    }

    public void FillScript(CapsuleType capType, Rarity capRare)
    {
        switch (capType)
        {
            case CapsuleType.Tech:
                capImage.sprite = TechSprite;

                switch (capRare)
                {
                    case Rarity.Common:
                        typeText.text = techNames[0];
                        itemGot.sprite = techSprites[0];
                        break;
                    case Rarity.Uncommon:
                        typeText.text = techNames[1];
                        itemGot.sprite = techSprites[1];
                        break;
                    case Rarity.Rare:
                        typeText.text = techNames[2];
                        itemGot.sprite = techSprites[2];
                        break;
                    case Rarity.Legendary:
                        typeText.text = techNames[3];
                        itemGot.sprite = techSprites[3];
                        break;
                }

                break;
            case CapsuleType.Character:
                capImage.sprite = CharacterSprite;
                switch (capRare)
                {
                    case Rarity.Common:
                        typeText.text = charNames[0];
                        itemGot.sprite = charSprites[0];
                        break;
                    case Rarity.Uncommon:
                        typeText.text = charNames[1];
                        itemGot.sprite = charSprites[1];
                        break;
                    case Rarity.Rare:
                        typeText.text = charNames[2];
                        itemGot.sprite = charSprites[2];
                        break;
                    case Rarity.Legendary:
                        typeText.text = charNames[3];
                        itemGot.sprite = charSprites[3];
                        break;
                }

                break;
            case CapsuleType.Games:
                capImage.sprite = GamesSprite;
                switch (capRare)
                {
                    case Rarity.Common:
                        typeText.text = gamesNames[0];
                        itemGot.sprite = gamesSprites[0];
                        break;
                    case Rarity.Uncommon:
                        typeText.text = gamesNames[1];
                        itemGot.sprite = gamesSprites[1];
                        break;
                    case Rarity.Rare:
                        typeText.text = gamesNames[2];
                        itemGot.sprite = gamesSprites[2];
                        break;
                    case Rarity.Legendary:
                        typeText.text = gamesNames[3];
                        itemGot.sprite = gamesSprites[3];
                        break;
                }

                break;
            case CapsuleType.Snacks:
                capImage.sprite = SnackSprite;
                switch (capRare)
                {
                    case Rarity.Common:
                        typeText.text = snackNames[0];
                        itemGot.sprite = snackSprites[0];
                        break;
                    case Rarity.Uncommon:
                        typeText.text = snackNames[1];
                        itemGot.sprite = snackSprites[1];
                        break;
                    case Rarity.Rare:
                        typeText.text = snackNames[2];
                        itemGot.sprite = snackSprites[2];
                        break;
                    case Rarity.Legendary:
                        typeText.text = snackNames[3];
                        itemGot.sprite = snackSprites[3];
                        break;
                }

                break;
        }
    }

}
