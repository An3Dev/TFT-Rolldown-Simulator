using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStorage : MonoBehaviour
{
    public Sprite[] cardFrames;
    public Sprite[] traitIcons;

    static Sprite[] CARD_FRAMES;
    static Sprite[] TRAIT_ICONS;

    private void Awake()
    {
        CARD_FRAMES = cardFrames;
        TRAIT_ICONS = traitIcons;
    }

    public static Sprite[] GetCardFrames()
    {
        return CARD_FRAMES;
    }

    public static Sprite[] GetTraitIcons()
    {
        return TRAIT_ICONS;
    }
}
