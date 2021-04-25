using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;
    //public GameObject tooltipPrefab;
    public Transform canvas;

    public TextMeshProUGUI text;

    public GameObject tooltip;

    Trait currentTrait;

    float hoverTime = 0.2f;

    IEnumerator timer;
    bool stopTimer = false;
    Vector2 size;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }

        tooltip.transform.position = Vector2.zero;
        tooltip.SetActive(false);
    }

    public void StartTooltipTimer(Trait trait)
    {
        currentTrait = trait;
        timer = Countdown();
        string description = TraitDescriptions.descriptions[(int)currentTrait];

        text.text = description;
        StartCoroutine(timer);
    }

    public void ShowTooltip()
    {
        int i = (int)currentTrait;
        tooltip.transform.position = new Vector3(-100, -100);
        tooltip.SetActive(true);
        string description = TraitDescriptions.descriptions[i];

        text.SetText(description);
        size = text.textBounds.size;
        StartCoroutine(SetText());     
    }

    IEnumerator SetText()
    {
        yield return new WaitForSeconds(0.1f);
        tooltip.SetActive(true);

        size = (Vector2)text.textBounds.size;
        float lines = text.textInfo.lineCount;
        //tooltip.GetComponent<RectTransform>().sizeDelta = size + Vector2.up * 10 + Vector2.right * 10;
        //size = (Vector2)text.textBounds.size;
        tooltip.GetComponent<RectTransform>().sizeDelta = size + Vector2.up * 20 + Vector2.right * 10;

        tooltip.transform.position = Input.mousePosition + new Vector3(size.x / 2 + 5, (Input.mousePosition.y > Screen.height / 2 ? -1 : 1) * size.y / 2);
    }

    public void DisableTooltip()
    {
        tooltip.SetActive(false);
    }

    public void ResetTimer()
    {
        stopTimer = true;
        StopCoroutine(timer);
    }


    IEnumerator Countdown()
    {
        stopTimer = false;
        yield return new WaitForSeconds(hoverTime);
        if (!stopTimer)
        {
            ShowTooltip();
        }
        stopTimer = false;
    }
}
