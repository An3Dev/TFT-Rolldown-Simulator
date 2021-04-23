using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI levelText, scoreText, correctCardNumText, incorrectCardNumText, missedCardsNumText, refreshesNumText;
    public Card[] cardSlotArray; /* assign these in the inspector. These are the cards that are in the slots */
    public List<Card> championSelectionCardsList = new List<Card>();
    public GameObject championSelectionButtonPrefab;
    public Transform championSelectionButtonParent;

    public TMP_Dropdown timerDropdown;

    public GameObject championSelectionListParent;
    public GameObject startButton, restartButton, disabledStartButton;

    public ChampionCard[] allChampions;
    List<ChampionCard> oneCostChamps, twoCostChamps, threeCostChamps, fourCostChamps, fiveCostChamps;
    public TextMeshProUGUI[] probabilityText;

    public GameObject[] objectsToDisableOnPlay;
    public GameObject[] objectsToEnableOnPlay;

    public GameObject enableGlowCheckMark, enableGlowEmptyBox;

    [Header("SELECTED CARDS COMPONENTS")]
    public Card[] selectedCardsUIArray;
    public GridLayoutGroup gridLayoutGroup;
    public float maxCardWidth = 200;
    public float minWidth = 100;
    float cardWidthToHeightRatio = 0.75f;
    bool useCardGlow = false;

    int level = 1;

    int maxSelectedCards = 58;

    List<Card> selectedCards = new List<Card>();

    int score = 0, correctCardsNum, incorrectCardsNum, missedCardsNum, refreshesNum;

    [Header("TIMER SETTINGS")]
    float timerAmountInSeconds = 30;

    float timer;
    public TextMeshProUGUI timerSetupText, timerUnitsText, timerCountdownText;

    bool startedGame = false;

    [Header("SEARCH BAR COMPONENTS")]
    public GameObject clearSearchBarTextButton;
    public TextMeshProUGUI searchBarInputFieldText;
    public TMP_InputField searchBarInputField;

    int[,] probabilities = new int[,] 
        { 
            { 100, 0, 0, 0, 0 }, /* level 1 */
            { 100, 0, 0, 0, 0 }, 
            { 75, 25, 0, 0, 0 },
            { 55, 30, 15, 0, 0 }, 
            { 45, 33, 20, 2, 0 }, 
            { 35, 35, 25, 5, 0 }, 
            { 22, 35, 30, 12, 1 },
            { 15, 25, 35, 20, 5 }, 
            { 10, 15, 30, 30, 15 } /* level 9  */
        };

    bool sortChampionsAlphabetically = false;
    bool isSearchBarSelected = false;
    Card[] costSortedCardsArray;

    public int[] secondsInDropdown;

    bool inSettings = false;
    private const string timerPreferenceKey = "TimerPreferenceKey";
    private const string enableCardGlowKey = "EnableCardGlow";

    List<string> cardsThatHaveBeenClickedOn = new List<string>();

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

        /* populate the cost arrays*/
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

        useCardGlow = PlayerPrefs.GetInt(enableCardGlowKey, 0) == 0 ? false : true;
        enableGlowCheckMark.SetActive(useCardGlow);
        enableGlowEmptyBox.SetActive(!useCardGlow);


        CreateChampionSelectionButtons();
        costSortedCardsArray = new Card[championSelectionCardsList.Count];
        PopulateCostSortedArray();
        OnSortByCostClicked();
        SetLevel(8);
        SetProbabilityText();
        SetLevelText();

        //print(PlayerPrefs.GetInt(timerPreferenceKey, 1));
        timerAmountInSeconds = PlayerPrefs.GetInt(timerPreferenceKey, 1);

        timerDropdown.SetValueWithoutNotify((int)timerAmountInSeconds);
    }



    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    public void OpenLink(string link)
    {
#if UNITY_EDITOR
        Application.OpenURL(link);
        return;
#endif
        OpenLinkJSPlugin(link);

    }
    public void OpenLinkJSPlugin(string link)
    {
#if !UNITY_EDITOR
		openWindow(link);
		

#endif
    }

    void PopulateCostSortedArray()
    {
        // fill it with cards
        for(int i = 0; i < championSelectionCardsList.Count; i++)
        {
            costSortedCardsArray[i] = championSelectionCardsList[i];
        }

        // sort it by cost
        int length = costSortedCardsArray.Length;
        for (int i = 0; i < length - 1; i++)
        {
            for (int x = 0; x < length - i - 1; x++)
            {
                Card card = costSortedCardsArray[x];
                int cost = card.GetCost();
                if (cost > costSortedCardsArray[x + 1].GetCost())
                {
                    Card temp = costSortedCardsArray[x];
                    costSortedCardsArray[x] = costSortedCardsArray[x + 1];
                    costSortedCardsArray[x + 1] = temp;              
                }
            }
        }
    }

    public void OpenedSettings(bool opened)
    {
        inSettings = opened;
    }

    public void EnableCardGlow(bool enable)
    {
        useCardGlow = enable;
        PlayerPrefs.SetInt(enableCardGlowKey, enable ? 1 : 0);
    }

    private void Update()
    {
        if (inSettings)
            return;


        if (Input.GetKeyDown(KeyBinds.GetKeyBind(KeyBinds.Action.Refresh)))
        {
            OnClickRefresh();
        }

        if (Input.GetKeyDown(KeyBinds.GetKeyBind(KeyBinds.Action.Restart)))
        {
            OnClickRestart();
        }

        if (!isSearchBarSelected)
        {
            if (Input.GetKeyDown(KeyBinds.GetKeyBind(KeyBinds.Action.StartGame)) && !startedGame)
            {
                OnClickStart();
            }
        }
        /* timer logic*/
        if (startedGame && timerAmountInSeconds != 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timerCountdownText.text = "0.0";

                /* stop the timer */
                startedGame = false;
                DisableCardsInSlots();
            } else
            {
                int seconds = (int)timer % 60;
                int minutes = (int)timer / 60 % 60;
                timerCountdownText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
            }

        } else
        {
            timerCountdownText.gameObject.SetActive(false);
        }
    }

    void DisableCardsInSlots()
    {
        for(int i = 0; i < cardSlotArray.Length; i++) 
        {
            cardSlotArray[i].gameObject.SetActive(false);
        }
    }

    public void OnDropdownChanged(int index)
    {
        if (index == 0)
        {
            timerAmountInSeconds = 0;
        } else
        {
            timerAmountInSeconds = secondsInDropdown[index];
        }
        //print("Changed: " + index);
        PlayerPrefs.SetInt(timerPreferenceKey, index);
    }

    public void OnChangeTimer(float seconds)
    {
        if (seconds <= 0)
        {
            timerSetupText.text = "Disabled";
            timerAmountInSeconds = 0;
        } else
        {
            timerSetupText.text = seconds.ToString() + " secs";
            timerAmountInSeconds = seconds;
            timer = timerAmountInSeconds;
        }
    }

    public void OnEndInputFieldEdit(string text)
    {
        text = text.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            OnChangeTimer(timerAmountInSeconds);
        } else
        {
            if (text.ToLower().Contains("sec"))
            {
                
            }
        }
    }

#region Search Bar Code

    public void OnSearchBarSelect()
    {
        isSearchBarSelected = true;
    }

    public void OnSearchBarDeselect()
    {
        isSearchBarSelected = false;
    }

    public void OnSortAlphabeticallyClicked()
    {
        print("Sort by alpha");
        sortChampionsAlphabetically = true;
        SortUIAlphabetically();
    }

    public void SortUIAlphabetically()
    {       
        // the champion selection cards list is already sorted alphabetically.
        for (int i = 0; i < championSelectionCardsList.Count; i++)
        {
            championSelectionCardsList[i].transform.SetSiblingIndex(i);
        }     
    }

    public void OnSortByCostClicked()
    {
        sortChampionsAlphabetically = false;
        SortUIByCost();
    }

    public void SortUIByCost()
    {
        for (int i = 0; i < costSortedCardsArray.Length; i++)
        {
            costSortedCardsArray[i].transform.SetSiblingIndex(i);
        }
    }

    void UnfilterAllCards()
    {
        for (int i = 0; i < championSelectionCardsList.Count; i++)
        {
            championSelectionCardsList[i].Unfilter();
        }
    }
    public void OnSearchInputFieldChanged(String input)
    {
        input = input.Trim();
        if (String.IsNullOrEmpty(input))
        {
            clearSearchBarTextButton.SetActive(false);
            // unfilter all cards
            UnfilterAllCards();
        } else // if search bar contains input
        {

            clearSearchBarTextButton.SetActive(true);

            int cost = 0;
            int.TryParse(input, out cost);
            
            // if input is a number
            if (cost != 0)
            {
                print(input + " is a number");

                // loops through all buttons and sets them to filtered if they don't match the input
                for (int i = 0; i < championSelectionCardsList.Count; i++)
                {
                    if (championSelectionCardsList[i].GetCost() == cost)
                    {
                        // this cost should be shown and unfiltered
                        championSelectionCardsList[i].Unfilter();
                    } else
                    {
                        championSelectionCardsList[i].Filter();
                    }
                }
                return;
            }

            string[] words = input.Split(new string[] { " ", ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
            
            List<Card> unsetCards = new List<Card>(championSelectionCardsList);
            string s = "";
            for(int i = 0; i < words.Length; i++)
            {
                s += words[i] + "\n";
            }

            for (int i = 0; i < unsetCards.Count; i++)
            {
                Card thisCard = unsetCards[i];
                foreach (string word in words)
                {
                    // if card name contains search input
                    if (thisCard.GetName().ToLower().Contains(word.ToLower()))
                    {
                        // this card will not be filtered
                        thisCard.Unfilter();
                        break;
                    }

                    Trait[] traits = thisCard.GetTraits();
                    bool unfiltered = false;
                    // if card traits contains search input
                    foreach (Trait trait in traits)
                    {
                        if (trait.ToString().ToLower().Contains(word.ToLower()))
                        {
                            thisCard.Unfilter();
                            unfiltered = true;
                            break;
                        }
                    }      
                    if (!unfiltered)
                    {
                        thisCard.Filter();
                    } else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void OnClearTextClicked()
    {
        print("Clear text");
        searchBarInputField.SetTextWithoutNotify("");
        clearSearchBarTextButton.SetActive(false);
        searchBarInputField.Select();
        UnfilterAllCards();
    }
#endregion


    public void CreateChampionSelectionButtons()
    {
        for (int i = 0; i < allChampions.Length; i++)
        {
            Card card = Instantiate(championSelectionButtonPrefab, championSelectionButtonParent).GetComponent<Card>();
            card.SetCost(allChampions[i].cost);
            card.SetName(allChampions[i].championName);
            card.SetTraits(allChampions[i].traits);
            card.SetImage(allChampions[i].image);

            card.SetIndexInSelectionList(i);
            championSelectionCardsList.Add(card);
            card.transform.name = allChampions[i].championName;
        }
    }

    public void OnClickStart()
    {

        if (selectedCards.Count > 0)
        {
            startButton.SetActive(true);
            disabledStartButton.SetActive(false);
        }
        else
        {
            disabledStartButton.SetActive(true);
            startButton.SetActive(false);
            return;
        }

        startedGame = true;
        championSelectionListParent.SetActive(false);

        /* reset the scores */
        score = 0;
        correctCardsNum = 0;
        incorrectCardsNum = 0;
        missedCardsNum = 0;
        refreshesNum = 0;
        timer = timerAmountInSeconds;
        SetScoreText();

        /* spawn the cards. */
        RefreshCardSlots();
        DisablePreStartObjects(true);
        EnablePostStartObjects(true);
        PopulateSelectedCardsUI();

        ResizeGridLayout();
    }

    void ResizeGridLayout()
    {
        if (selectedCards.Count <= 21)
        {
            gridLayoutGroup.cellSize = new Vector2(maxCardWidth, maxCardWidth * cardWidthToHeightRatio);
        }
        else if (selectedCards.Count <= 36)
        {
            gridLayoutGroup.cellSize = new Vector2(150, 150 * cardWidthToHeightRatio);
        } else if (selectedCards.Count <= 55)
        {
            gridLayoutGroup.cellSize = new Vector2(125, 125 * cardWidthToHeightRatio);
        } else
        {
            gridLayoutGroup.cellSize = new Vector2(114, 114 * cardWidthToHeightRatio);
        }
    }

    public void SelectCardsWithTrait(int indexInTrait)
    {
        Trait trait = (Trait)indexInTrait;
        for (int i = 0; i < championSelectionCardsList.Count; i++)
        {
            Trait[] traits = championSelectionCardsList[i].GetTraits();
            foreach(Trait thisTrait in traits)
            {
                if (thisTrait.Equals(trait))
                {
                    if (!championSelectionCardsList[i].IsSelected())
                    {
                        championSelectionCardsList[i].OnSelect();
                    }
                }
            }       
        }
    }

    public void DeselectCardsWithTrait(int indexInTrait)
    {
        Trait trait = (Trait)indexInTrait;
        for (int i = 0; i < championSelectionCardsList.Count; i++)
        {
            Trait[] traits = championSelectionCardsList[i].GetTraits();
            foreach (Trait thisTrait in traits)
            {
                if (thisTrait.Equals(trait))
                {
                    championSelectionCardsList[i].OnDeselect();
                }
            }
        }
    }

    void PopulateSelectedCardsUI()
    {
        /*this array holds the card instances */
        for (int i = 0; i < selectedCardsUIArray.Length; i++)
        {
            /* if the number of selected cards is less than the current card slot number */
            if (selectedCards.Count == i)
            {
                /* disable the rest of the cards */
                for (int x = i; x < selectedCardsUIArray.Length; x++)
                {
                    selectedCardsUIArray[x].gameObject.SetActive(false);
                }
                return;
            }
            selectedCardsUIArray[i].gameObject.SetActive(true);
            selectedCardsUIArray[i].SetImage(selectedCards[i].GetImage());
            selectedCardsUIArray[i].SetTraits(selectedCards[i].GetTraits());
            selectedCardsUIArray[i].SetName(selectedCards[i].GetName());
        }
    }

    public void OnClickRestart()
    {
        startedGame = false;
        championSelectionListParent.SetActive(true);
        DisablePreStartObjects(false);
        EnablePostStartObjects(false);
        cardsThatHaveBeenClickedOn.Clear();
        /* disable cards */
        for (int i = 0; i < cardSlotArray.Length; i++)
        {
            cardSlotArray[i].gameObject.SetActive(false);
        }

        /* disable selected cards */
        for(int i = 0; i < selectedCardsUIArray.Length; i++)
        {
            selectedCardsUIArray[i].gameObject.SetActive(false);
        }

        if (selectedCards.Count > 0)
        {
            startButton.SetActive(true);
            disabledStartButton.SetActive(false);
        }
        else
        {
            disabledStartButton.SetActive(true);
            startButton.SetActive(false);
        }
    }

    public void DisablePreStartObjects(bool disable)
    {
        for (int i = 0; i < objectsToDisableOnPlay.Length; i++)
        {
            objectsToDisableOnPlay[i].gameObject.SetActive(!disable);
        }
    }

    public void EnablePostStartObjects(bool enable)
    {
        for (int i = 0; i < objectsToEnableOnPlay.Length; i++)
        {
            objectsToEnableOnPlay[i].gameObject.SetActive(enable);
        }
    }

    public void OnSelectedChampion(int index)  
    {
        Card card = championSelectionCardsList[index];

        /*add this card to selected cards */
        selectedCards.Add(card);

        if (selectedCards.Count > 0)
        {
            startButton.SetActive(true);
            disabledStartButton.SetActive(false);
        }
        else
        {
            disabledStartButton.SetActive(true);
            startButton.SetActive(false);
        }

        if (selectedCards.Count >= maxSelectedCards)
        {
            /*disable selection */
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
        /* add this card to selected cards */
        selectedCards.Remove(card);

        if (selectedCards.Count > 0)
        {
            startButton.SetActive(true);
            disabledStartButton.SetActive(false);
        }
        else
        {
            disabledStartButton.SetActive(true);
            startButton.SetActive(false);
        }

        /* if cards list was previously at max capacity */
        if (selectedCards.Count + 1 >= maxSelectedCards)
        {
            /* enable selection */
            DisableCardSelection(false);
        }
    }

    public void DeselectAllSelectionButtons()
    {
        for(int i = 0; i < championSelectionCardsList.Count; i++)
        {
            championSelectionCardsList[i].OnDeselect();
        }

        TraitDropdown.Instance.DeselectAllTraits();

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
        SoundManager.Instance.PlaySelectCardSFX();

        /* do some logic to calculate whether this was a correct click, or incorrect*/
        Card card = cardSlotArray[indexInDeck];

        if (IsChoiceCorrect(card))
        {
            ChangeScore(1);
            cardsThatHaveBeenClickedOn.Add(card.GetName());
        }
        else
        {
            incorrectCardsNum += 1;
            ChangeScore(-1);
        }

        /* disable card that was clicked */
        cardSlotArray[indexInDeck].gameObject.SetActive(false);
    }

    bool IsChoiceCorrect(Card card)
    {
        for(int i = 0; i < selectedCardsUIArray.Length; i++)
        {
            if (card.GetName().Equals(selectedCardsUIArray[i].GetName()))
            {
                return true;
            }
        }

        return false;
    }

    public void OnClickRefresh()
    {
        if (!startedGame)
            return;

        SoundManager.Instance.PlayRefreshSFX();
        /* Check for missed cards
         iterate through all cards in the deck */
        for (int i = 0; i < cardSlotArray.Length; i++)
        {
            /* if this card is disabled, then skip this card */
            if (!cardSlotArray[i].gameObject.activeInHierarchy)
                continue;

            /* if the current card would be a correct pick, then the user missed this card. */
            if (IsChoiceCorrect(cardSlotArray[i]))
            {
                print("Missed a card");
                missedCardsNum += 1;
                ChangeScore(-1);
            }
        }

        refreshesNum++;
        refreshesNumText.text = refreshesNum.ToString();

        RefreshCardSlots();
    }

    void ChangeScore(int change)
    {
        score += change;
        if (change > 0)
        {
            correctCardsNum += change;
        }
        SetScoreText();

    }

    void SetScoreText()
    {
        scoreText.text = ((score > 0) ? "+" : "") + score;
        correctCardNumText.text = correctCardsNum.ToString();
        incorrectCardNumText.text = incorrectCardsNum.ToString();
        missedCardsNumText.text = missedCardsNum.ToString();
        refreshesNumText.text = refreshesNum.ToString();
    }

    public void RefreshCardSlots()
    {
        /* loops through each card slot */
        for (int i = 0; i < cardSlotArray.Length; i++)
        {
            int cost = 0;

            int random = Random.Range(1, 100);
            int lastTotal = 0;

             /* get length 1 will return the number of columns */
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

            /* this decides what cost card we're going to use
            //cost = Random.Range(1, 6); */
            ChampionCard[] champions;
            /* right here we're choosing which cards to use. It's like having 5 different decks of cards, each having a different cost. */
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

            /* selects random champion from the specific "deck" in which all cards have the same cost */
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

        if (useCardGlow)
        {
            if (IsChoiceCorrect(currCard))
            {
                bool setGlow = false;
                for (int i = 0; i < cardsThatHaveBeenClickedOn.Count; i++)
                {
                    // if this card was already selected
                    if (cardsThatHaveBeenClickedOn[i].Equals(currCard.GetName()))
                    {
                        currCard.SetGlow(true);
                        setGlow = true;
                        break;
                    }
                }
                if (!setGlow)
                    currCard.SetGlow(false);
            }
            else
            {
                currCard.SetGlow(false);
            }
        }       
    }
}
