using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Easteregg : MonoBehaviour
{
    [Header("Easter Egg Counters")]
    private int clickCount = 0;
    private int clickcount2 = 0;

    [Header("Easter Egg UI Elements")]
    public GameObject EasterImage;
    public GameObject EasterImage2;

    [Header("Character Buttons")]
    public Button characterButton;
    public Button characterButton2;
    void Start()
    {
        EasterImage.transform.SetAsLastSibling();
        EasterImage.SetActive(false);
        characterButton.onClick.AddListener(OnCharacterClick);
        EasterImage2.transform.SetAsLastSibling();
        EasterImage2.SetActive(false);
        characterButton2.onClick.AddListener(OnClick);
    }
    void OnCharacterClick()
    {
        clickCount++;
        if (clickCount >= 3)
        {
           clickEasterEgg();
            clickCount = 0;
        }
    }
    void clickEasterEgg()
    {
        EasterImage.SetActive(true);
        StartCoroutine(Hide());
    }
    void OnClick()
    {
        clickcount2++;
       if (clickcount2 >= 3)
       {
         ClickrEasterEgg();
       }
    }
    void ClickrEasterEgg()
    {
        EasterImage2.SetActive(true);
        StartCoroutine(Hide2());
    }
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(2f);
        EasterImage.SetActive(false);
    }
   IEnumerator Hide2()
   {
        yield return new WaitForSeconds(2f);
        EasterImage2.SetActive(false);
    }
}

   
