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
    public GameObject filterPanel;

    bool isSelected = false;
    string name;
    Trait[] traits;
    int cost;
    int indexInSelectionList = 0;
    int indexInDeck = 0;
    bool isFiltered = false;
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
        if (!cardNameText)
            return;
        cardNameText.text = name;
    }

    public void SetImage(Sprite image)
    {
        championImage.sprite = image;
    }

    public Sprite GetImage()
    {
        return championImage.sprite;
    }

    public void SetCost(int cost)
    {
        this.cost = cost;
        if (cardFrame == null)
            return;
        //cardFrame.sprite = UIStorage.GetCardFrames()[cost - 1];
        SetFrameColorUI();
    }

    void SetFrameColorUI()
    {
        cardFrame.color = UIStorage.GetFrameColor(cost - 1);
    }

    public int GetCost()
    {
        return cost;
    }


    public void OnSelect()
    {
        isSelected = true;
        filterPanel.SetActive(false);
        GameManager.Instance.OnSelectedChampion(indexInSelectionList);
    }

    public void OnDeselect()
    {
        isSelected = false;
        if (isFiltered)
        {
            filterPanel.SetActive(true);
        }
        GameManager.Instance.OnDeselectedChampion(indexInSelectionList);
    }


    public void Filter()
    {
        isFiltered = true;
        if (!isSelected)
        {
            filterPanel.SetActive(true);
        }
    }

    public void Unfilter()
    {
        isFiltered = false;
        filterPanel.SetActive(false);
    }
    public void DisableSelection()
    {
        if (!isSelected)
        {
            filterPanel.gameObject.SetActive(true);
        }
    }

    public void EnableSelection()
    {
        if (!isSelected)
        {
            filterPanel.gameObject.SetActive(false);
        }
    }

    public void SetTraits(Trait[] traits)
    {
        this.traits = traits;

        // if don't have text or trait images
        if ((traitsText.Length == 0 || traitsText == null) && (traitImages.Length == 0 || traitImages == null))
            return;

        // iterate through each trait that this card has
        for (int i = 0; i < traits.Length; i++)
        {
            if (i == 2)
            {
                Transform parent = traitImages[2].transform.parent.parent;
                for (int x = 0; x < parent.childCount; x++)
                {
                    parent.GetChild(x).gameObject.SetActive(true);
                }
            }

            // set trait text
            if(traitsText.Length > i)
                traitsText[i].text = traits[i].ToString();

            if (traitImages.Length > i)
            {
                int indexOfTrait = (int)traits[i];
                Sprite traitIcon = UIStorage.GetTraitIcons()[indexOfTrait];
                // set trait icon
                traitImages[i].sprite = traitIcon;
            }          
        }

        // disable pictures and text of this ui because we don't want to diplay it
        if (traits.Length < 3)
        {
            Transform parent = traitImages[2].transform.parent.parent;
            for (int x = 0; x < parent.childCount; x++)
            {
                parent.GetChild(x).gameObject.SetActive(false);
            }
        }
    }

    public Trait[] GetTraits()
    {
        return traits;
    }

}
