using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TraitDropdown : MonoBehaviour
{
    public static TraitDropdown Instance;

    public TextMeshProUGUI selectedTraitsText;
    public GameObject clickOutButton;
    public Transform cardContainer;

    public GameObject dropdown;

    public GameObject traitCardPrefab;

    List<Trait> selectedTraits = new List<Trait>();
    int enumLength = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        enumLength = Trait.GetNames(typeof(Trait)).Length;
        PopulateCards();
    }

    void PopulateCards()
    {
        for (int i = 0; i < enumLength; i++)
        {
            TraitCard card = Instantiate(traitCardPrefab, cardContainer).GetComponent<TraitCard>();
            card.SetTrait((Trait)i);
        }
    }

    string GetStringFromList()
    {
        string traitsText = "";
        for (int i = 0; i < selectedTraits.Count; i++)
        {
            if (i < selectedTraits.Count - 1)
            {
                traitsText += selectedTraits[i].ToString().Replace('_', '-') + ",";
            }
            else
            {
                traitsText += selectedTraits[i].ToString().Replace('_', '-');
            }
        }
        if (traitsText.Equals(""))
        {
            traitsText = "None";
        }
        return traitsText;
    }

    public void DeselectAllTraits()
    {
        for(int i = 0; i < cardContainer.childCount; i++)
        {
            cardContainer.GetChild(i).GetComponent<TraitCard>().Deselect();
        }
    }

    public void OnSelectTrait(Trait trait)
    {
        selectedTraits.Add(trait);
        //selectedTraitsText.text = GetStringFromList();
    }

    public void OnDeselectTrait(Trait trait)
    {
        selectedTraits.Remove(trait);
        //selectedTraitsText.text = GetStringFromList();
    }

    public void OnClickedArrow()
    {
        if (!dropdown.activeInHierarchy)
        {
            dropdown.SetActive(true);
            clickOutButton.SetActive(true);
        } else
        {
            dropdown.SetActive(false);
            clickOutButton.SetActive(false);
        }     
    }

    public void OnClickOut()
    {
        clickOutButton.SetActive(false);
        dropdown.SetActive(false);
    }
}
