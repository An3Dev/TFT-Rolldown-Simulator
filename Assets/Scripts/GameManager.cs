using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI levelText;
    public Card[] cardSlotArray; // assign these in the inspector. These are the cards that are in the slots
    public List<Card> championSelectionCardsList = new List<Card>();
    public GameObject championSelectionButtonPrefab;
    public Transform championSelectionButtonParent;

    public ChampionCard[] allChampions;
    List<ChampionCard> oneCostChamps, twoCostChamps, threeCostChamps, fourCostChamps, fiveCostChamps;

    public int level = 1;

    int maxSelectedCards = 10;

    List<Card> selectedCards = new List<Card>();

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
        if( Instance != null)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }

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

        // create champion selection buttons and populate them
        for(int i = 0; i < allChampions.Length; i++)
        {
            Card card = Instantiate(championSelectionButtonPrefab, championSelectionButtonParent).GetComponent<Card>();
            card.SetCost(allChampions[i].cost);
            card.SetName(allChampions[i].name);
            card.SetTraits(allChampions[i].traits);
            card.SetImage(allChampions[i].image);
            card.SetIndexInSelectionList(i);
            championSelectionCardsList.Add(card);
        }
    }

    public void OnSelectedChampion(int index)
    {

        Card card = championSelectionCardsList[index];
        print("Selected " + card.GetName());

        // add this card to selected cards
        selectedCards.Add(card);

        if (selectedCards.Count >= maxSelectedCards)
        {
            // disable selection
            DisableCardSelection(true);
        }
        // populate and enable card

    }

    void DisableCardSelection(bool disable)
    {
        if (disable)
        {
            for (int i = 0; i < championSelectionCardsList.Count; i++)
            {
                championSelectionCardsList[i].DisableSelection();
            }
        } else
        {
            for (int i = 0; i < championSelectionCardsList.Count; i++)
            {
                championSelectionCardsList[i].EnableSelection();
            }
        }

    }


    public void OnDeselectedChampion(int index)
    {
        Card card = championSelectionCardsList[index];
        // add this card to selected cards
        selectedCards.Remove(card);

        // if cards list was previously at max capacity
        if (selectedCards.Count + 1 >= maxSelectedCards)
        {
            // enable selection
            DisableCardSelection(false);
        }

        // disable the UI card

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RefreshCardSlots();
        }
    }

    public void OnCardClicked(int indexInDeck)
    {
        print(cardSlotArray[indexInDeck].GetName());

        // do some logic to calculate whether this was a correct click, incorrect click, or missed click

        cardSlotArray[indexInDeck].gameObject.SetActive(false);
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
        for (int i = 0; i < cardSlotArray.Length; i++)
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

            SetCardUI(cardSlotArray[i], champions[randomChamp], i);         
        }
    }


    void SetCardUI(Card currCard, ChampionCard champ, int index)
    {
        currCard.gameObject.SetActive(true);

        currCard.SetImage(champ.image);
        currCard.SetName(champ.championName);
        currCard.SetTraits(champ.traits);
        currCard.SetCost(champ.cost);
        currCard.SetIndexInDeck(index);
    }
}
