using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SetupClickableImage : MonoBehaviour
{
    [SerializeField] TMP_Text countTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(Sprite icon, int count, Action action)
    {
        imageComponent.sprite = icon;
        countTextComponent.text = count.ToString();
        buttonComponent.onClick.AddListener(delegate { action(); });
    }

    public void Setup(Sprite icon, Action action)
    {
        imageComponent.sprite = icon;
        countTextComponent.transform.parent.gameObject.SetActive(false);
        buttonComponent.onClick.AddListener(delegate { action(); });
    }

    public void AdjustCount(int count)
    {
        if (countTextComponent.gameObject.activeSelf == false)
        {
            countTextComponent.gameObject.SetActive(true);
        }

        countTextComponent.text = count.ToString();
    }
}
