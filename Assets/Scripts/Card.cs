using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Card : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public Image championImage, cardFrame;
    public TextMeshProUGUI[] traitsText;
    public Image[] traitImages;

    Trait[] traits;
    int cost;
    public void OnClickCard()
    {
        print("Clicked card");
        // trigger method in game manager to pick card
    }

    public void SetName(string name)
    {
        cardNameText.text = name;
    }

    public void SetImage(Sprite image)
    {
        championImage.sprite = image;
    }
    public void SetCost(int cost)
    {
        this.cost = cost;
        cardFrame.sprite = UIStorage.GetCardFrames()[cost - 1];
    }

    public void SetTraits(Trait[] traits)
    {
        this.traits = traits;
        for(int i = 0; i < traits.Length; i++)
        {
            // set trait text
            traitsText[i].text = traits[i].ToString();

            int indexOfTrait = (int)traits[i];
            Sprite traitIcon = UIStorage.GetTraitIcons()[indexOfTrait];
            // set trait icon
            traitImages[i].sprite = traitIcon;
        }
    }

    public Trait[] GetTraits()
    {
        return traits;
    }

}
