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
    public GameObject clickBlocker;

    bool isSelected = false;
    string name;
    Trait[] traits;
    int cost;
    int indexInSelectionList = 0;
    int indexInDeck = 0;
    public void OnClickCard()
    {
        GameManager.Instance.OnCardClicked(indexInDeck);
    }

    public string GetName()
    {
        return name;
    }

    public void SetIndexInSelectionList(int index)
    {
        indexInSelectionList = index;
    }

    public void SetIndexInDeck(int index)
    {
        indexInDeck = index;
    }

    public void SetName(string name)
    {
        this.name = name;
        cardNameText.text = name;
    }

    public void SetImage(Sprite image)
    {
        championImage.sprite = image;
    }

    public void SetCost(int cost)
    {
        this.cost = cost;
        if (cardFrame == null)
            return;
        cardFrame.sprite = UIStorage.GetCardFrames()[cost - 1];
    }

    public void OnSelect()
    {
        isSelected = true;
        GameManager.Instance.OnSelectedChampion(indexInSelectionList);
    }

    public void OnDeselect()
    {
        isSelected = false;
        GameManager.Instance.OnDeselectedChampion(indexInSelectionList);
    }

    public void DisableSelection()
    {
        if (!isSelected)
        {
            clickBlocker.gameObject.SetActive(true);
        }
    }

    public void EnableSelection()
    {
        if (!isSelected)
        {
            clickBlocker.gameObject.SetActive(false);
        }
    }

    public void SetTraits(Trait[] traits)
    {
        this.traits = traits;
        if (traitsText.Length == 0 || traitsText == null || traitImages.Length == 0 || traitImages == null)
            return;

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
