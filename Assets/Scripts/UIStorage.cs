using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStorage : MonoBehaviour
{
    public Sprite[] cardFrames;
    public Sprite[] traitIcons;
    public Color[] frameColors;

    static Sprite[] CARD_FRAMES;
    static Sprite[] TRAIT_ICONS;
    static Color[] FRAME_COLORS;

    private void Awake()
    {
        CARD_FRAMES = cardFrames;
        TRAIT_ICONS = traitIcons;
        FRAME_COLORS = frameColors;
    }

    public static Sprite[] GetCardFrames()
    {
        return CARD_FRAMES;
    }

    public static Color GetFrameColor(int index)
    {
        return FRAME_COLORS[index];
    }

    public static Sprite[] GetTraitIcons()
    {
        return TRAIT_ICONS;
    }
}
