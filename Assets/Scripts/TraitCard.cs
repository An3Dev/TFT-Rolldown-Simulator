using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TraitCard : MonoBehaviour
{
    public TextMeshProUGUI traitText;
    public GameObject highlightedImage;
    private Trait trait;
    private bool isSelected;
    private int indexInTraitEnum;

    public void SetTrait(Trait trait)
    {
        this.trait = trait;
        if (trait.ToString().Contains("_"))
        {
            traitText.text = trait.ToString().Replace('_', '-');
        }
        else
        {
            traitText.text = trait.ToString();
        }
        indexInTraitEnum = (int)trait;
    }

    public Trait GetTrait()
    {
        return trait;
    }

    public void Deselect()
    {
        GameManager.Instance.DeselectCardsWithTrait(indexInTraitEnum);
        TraitDropdown.Instance.OnDeselectTrait(trait);
        isSelected = false;
        highlightedImage.SetActive(false);
    }

    public void OnClick()
    {
        if (!isSelected)
        {
            GameManager.Instance.SelectCardsWithTrait(indexInTraitEnum);
            TraitDropdown.Instance.OnSelectTrait(trait);

            highlightedImage.SetActive(true);
            isSelected = true;
        }
        else
        {
            GameManager.Instance.DeselectCardsWithTrait(indexInTraitEnum);
            TraitDropdown.Instance.OnDeselectTrait(trait);
            isSelected = false;
            highlightedImage.SetActive(false);
        }
        Tooltip.Instance.ResetTimer();
    }

    public void OnHover()
    {
        Tooltip.Instance.StartTooltipTimer(trait);
    }

    public void OnCursorExit()
    {
        Tooltip.Instance.ResetTimer();
        Tooltip.Instance.DisableTooltip();
    }
}
