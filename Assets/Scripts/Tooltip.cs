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

    float hoverTime = 1;

    IEnumerator timer;
    bool stopTimer = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }

        //tooltip = Instantiate(tooltipPrefab, canvas);
        //text = tooltipPrefab.GetComponentInChildren<TextMeshProUGUI>();

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
        string description = TraitDescriptions.descriptions[i];

        text.text = description;

        StartCoroutine(SetText());     
    }

    IEnumerator SetText()
    {
        yield return new WaitForSeconds(0.1f);
        Vector2 size = (Vector2)text.textBounds.size;
        //tooltip.GetComponent<RectTransform>().sizeDelta = size + Vector2.up * 10 + Vector2.right * 10;

        tooltip.SetActive(true);
        tooltip.transform.position = Input.mousePosition + Vector3.up * (50) + Vector3.right * 175;
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
