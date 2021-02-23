using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Champion Card", menuName = "Champion Card")]
public class ChampionCard : ScriptableObject
{
    public string championName;
    public string description;
    public Sprite image;
    public Trait[] traits;
    public int cost;
    public Tier tier;
}
