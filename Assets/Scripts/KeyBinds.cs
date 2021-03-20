using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KeyBinds : MonoBehaviour
{
    public static KeyBinds Instance;

    public enum Action // order matters
    {
        StartGame,
        Refresh,
        Restart
    }

    public TextMeshProUGUI[] buttonText; // order matters
    public TextMeshProUGUI startKeybindText, refreshKeybindText, restartKeybindText;

    static Dictionary<Action, KeyCode> keybinds = new Dictionary<Action, KeyCode>();

    const string keybindsPrefsKey = "Keybinds";

    bool isDetectingKey = false;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    Action actionToModify;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        string s = PlayerPrefs.GetString(keybindsPrefsKey, "StartGame:S,Refresh:D,Restart:R");
        string[] pairs = s.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < pairs.Length; i++)
        {
            string[] temp = pairs[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            keybinds.Add(
                (Action)System.Enum.Parse(typeof(Action), temp[0], true), 
                (KeyCode)System.Enum.Parse(typeof(KeyCode), temp[1], true));
        }

        // set button text to match keybind from retrieved data
        UpdateTextUI();
    }

    void UpdateTextUI()
    {
        for(int i = 0; i < buttonText.Length; i++)
        {
            buttonText[i].text = keybinds[(Action)i].ToString();
        }

        startKeybindText.text = $"({keybinds[Action.StartGame]})";
        refreshKeybindText.text = $"({keybinds[Action.Refresh]})";

        restartKeybindText.text = $"({keybinds[Action.Restart]})";

    }

    public static KeyCode GetKeyBind(Action action)
    {
        return keybinds[action];
    }

    public void SetKeyBind(Action action, KeyCode keyCode)
    {
        keybinds[action] = keyCode;
    }

    public void OnClickKeybindButton(String action)
    {
        actionToModify = (Action)System.Enum.Parse(typeof(Action), action, true);
        isDetectingKey = true;
    }


    void Update()
    {
        if (isDetectingKey && Input.anyKeyDown)
        {
            KeyCode key = KeyCode.None;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isDetectingKey = false;
                // disable keybind set ui

            }
            foreach (KeyCode keyCode in keyCodes)
            {
                if (Input.GetKey(keyCode))
                {
                    key = keyCode;
                    break;
                }
            }
            keybinds[actionToModify] = key;
            // update that keybind button

            print("new keybind: " + key);
            UpdateTextUI();
            isDetectingKey = false;
            SavePreferences();
        }
    }

    void SavePreferences()
    {
        string s = "";
        for(int i = 0; i < keybinds.Count; i++)
        {
            s += (Action)i + ":" + keybinds[(Action)i] + ",";
        }
        PlayerPrefs.SetString(keybindsPrefsKey, s);
        PlayerPrefs.Save();
    }
}
