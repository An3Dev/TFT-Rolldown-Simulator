using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI levelText;
    public Card[] cards;

    public ChampionCard[] allChampions;
    List<ChampionCard> oneCostChamps, twoCostChamps, threeCostChamps, fourCostChamps, fiveCostChamps;

    public int level = 1;

    int[,] probabilities = new int[,] 
        { 
            { 100, 0, 0, 0, 0 }, // level 1
            { 100, 0, 0, 0, 0 }, 
            { 75, 25, 0, 0, 0 },
            { 55, 30, 15, 0, 0 }, 
            { 45, 33, 20, 2, 0 }, 
            { 35, 35, 25, 5, 0 }, 
            { 22, 35, 30, 12, 1 },
            { 15, 25, 35, 20, 5 }, 
            { 10, 15, 30, 30, 15 } // level 9
        };

    private void Awake()
    {
        oneCostChamps = new List<ChampionCard>();
        twoCostChamps = new List<ChampionCard>();
        threeCostChamps = new List<ChampionCard>();
        fourCostChamps = new List<ChampionCard>();
        fiveCostChamps = new List<ChampionCard>();

        // populate the cost arrays
        for (int i = 0; i < allChampions.Length; i++)
        {
            if (allChampions[i].cost == 1)
            {
                oneCostChamps.Add(allChampions[i]);
            } else if (allChampions[i].cost == 2)
            {
                twoCostChamps.Add(allChampions[i]);    
            }
            else if (allChampions[i].cost == 3)
            {
                threeCostChamps.Add(allChampions[i]);
            }
            else if (allChampions[i].cost == 4)
            {
                fourCostChamps.Add(allChampions[i]);
            } else
            {
                fiveCostChamps.Add(allChampions[i]);
            }

            if (allChampions[i].image.name.Equals("Square"))
            {
                Debug.Log("Missing Image: " + allChampions[i].championName);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RefreshCardSlots();
        }
    }
    public void OnClickStart() {
        print("Start button clicked");

        // spawn the cards.
        RefreshCardSlots();
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void RefreshCardSlots()
    {
        // loops through each card slot
        for (int i = 0; i < cards.Length; i++)
        {
            int cost = 0;

            int random = Random.Range(1, 100);
            int lastTotal = 0;

             // get length 1 will return the number of columns
            for(int x = 0; x < probabilities.GetLength(1); x++)
            {
                int probability = probabilities[level - 1, x];
                if (random > lastTotal && random <= lastTotal + probability)
                {
                    cost = x + 1;
                    break;
                }
                lastTotal += probability;
            }

            // this decides what cost card we're going to use
            //cost = Random.Range(1, 6);
            ChampionCard[] champions;
            // right here we're choosing which cards to use. It's like having 5 different decks of cards, each having a different cost.
            if (cost == 1)
            {
                champions = oneCostChamps.ToArray();
            }
            else if (cost == 2)
            {
                champions = twoCostChamps.ToArray();
            }
            else if (cost == 3)
            {
                champions = threeCostChamps.ToArray();
            }
            else if (cost == 4)
            {
                champions = fourCostChamps.ToArray();
            }
            else
            {
                champions = fiveCostChamps.ToArray();
            }

            // selects random champion from the specific "deck" in which all cards have the same cost
            int randomChamp = Random.Range(0, champions.Length);

            SetCardUI(cards[i], champions[randomChamp]);         
        }
    }


    void SetCardUI(Card currCard, ChampionCard champ)
    {
        currCard.gameObject.SetActive(true);

        currCard.SetImage(champ.image);
        currCard.SetName(champ.championName);
        currCard.SetTraits(champ.traits);
        currCard.SetCost(champ.cost);
    }
}
