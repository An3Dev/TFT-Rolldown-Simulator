using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI levelText;
    public Card[] cardSlotArray; // assign these in the inspector. These are the cards that are in the slots
    public List<Card> championSelectionCardsList = new List<Card>();
    public GameObject championSelectionButtonPrefab;
    public Transform championSelectionButtonParent;

    public GameObject championSelectionListParent;
    public GameObject startButton, restartButton;

    public ChampionCard[] allChampions;
    List<ChampionCard> oneCostChamps, twoCostChamps, threeCostChamps, fourCostChamps, fiveCostChamps;
    public TextMeshProUGUI[] probabilityText;

    public GameObject[] objectsToDisableOnPlay;

    public Card[] selectedCardsUIArray;

    public int level = 1;

    int maxSelectedCards = 10;

    List<Card> selectedCards = new List<Card>();

    [SerializeField]
    int score = 0;

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


        CreateChampionSelectionButtons();

        SetProbabilityText();
        SetLevelText();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RefreshCardSlots();
        }
    }

    public void CreateChampionSelectionButtons()
    {
        for (int i = 0; i < allChampions.Length; i++)
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

    public void OnClickStart()
    {
        championSelectionListParent.SetActive(false);
        // spawn the cards.
        RefreshCardSlots();
        DisablePreGameObjects(true);

        PopulateSelectedCardsUI();
    }

    void PopulateSelectedCardsUI()
    {
        //this array holds the card instances 
        for (int i = 0; i < selectedCardsUIArray.Length; i++)
        {
            // if the number of selected cards is less than the current card slot number
            if (selectedCards.Count == i)
            {
                //disable the rest of the cards
                for (int x = i; x < selectedCardsUIArray.Length; x++)
                {
                    selectedCardsUIArray[x].gameObject.SetActive(false);
                }
                return;
            }
            selectedCardsUIArray[i].gameObject.SetActive(true);
            selectedCardsUIArray[i].SetImage(selectedCards[i].GetImage());
            //selectedCardsUIArray[i].SetName(selectedCards[i].GetName());
        }
    }

    public void OnClickRestart()
    {
        championSelectionListParent.SetActive(true);
        DisablePreGameObjects(false);

        // disable cards
        for (int i = 0; i < cardSlotArray.Length; i++)
        {
            cardSlotArray[i].gameObject.SetActive(false);
        }
    }

    public void DisablePreGameObjects(bool disable)
    {
        for (int i = 0; i < objectsToDisableOnPlay.Length; i++)
        {
            objectsToDisableOnPlay[i].gameObject.SetActive(!disable);
        }
    }

    public void OnSelectedChampion(int index)
    {
        Card card = championSelectionCardsList[index];

        // add this card to selected cards
        selectedCards.Add(card);

        if (selectedCards.Count > 0)
        {
            startButton.SetActive(true);
        }

        if (selectedCards.Count >= maxSelectedCards)
        {
            // disable selection
            DisableCardSelection(true);
        }
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

        if (selectedCards.Count <= 0)
        {
            startButton.SetActive(false);
        }

        // if cards list was previously at max capacity
        if (selectedCards.Count + 1 >= maxSelectedCards)
        {
            // enable selection
            DisableCardSelection(false);
        }
    }

    public void OnChangeLevel(Single level)
    {
        this.level = (int)level;
        SetLevelText();
        SetProbabilityText();
    }

    void SetLevelText()
    {
        levelText.text = level.ToString();
    }

    void SetProbabilityText()
    {
        for(int i = 0; i < probabilityText.Length; i++)
        {
            probabilityText[i].text = $"• {probabilities[level-1, i]}%";
        }
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }


    public void OnCardClicked(int indexInDeck)
    {
        // do some logic to calculate whether this was a correct click, incorrect click, or missed click

        Card card = cardSlotArray[indexInDeck];
        Trait[] traits = card.GetTraits();

        // iterate through this card's traits
        for(int i = 0; i < traits.Length; i++)
        {
            // iterate through all selected cards
            for (int x = 0; x < selectedCards.Count; x++)
            {
                Card selectedCard = selectedCards[i];
                Trait[] selectedCardTraits = selectedCard.GetTraits();

                for (int z = 0; z < selectedCardTraits.Length; z++)
                {
                    if (selectedCardTraits[z].Equals(traits[i]))
                    {
                        score++;
                        print(selectedCardTraits[z] + " matches with " + traits[i]);
                        break;
                    }
                }
            }
        }

        cardSlotArray[indexInDeck].gameObject.SetActive(false);
    }


    public void OnClickRefresh()
    {
        RefreshCardSlots();

        // check for missed cards
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
