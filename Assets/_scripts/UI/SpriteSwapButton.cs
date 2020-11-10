using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpriteSwapButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button buttonComponent;
    [SerializeField] private Image imageComponent;   

    [Header("Sprites")]
    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;

    [Header("Menu")]
    [SerializeField] private GameObject menu;

    [Header("Default State")]
    [SerializeField] private bool isActive;

    private void Start()
    {
        buttonComponent.onClick.AddListener(ChangeSprite);

        if (isActive) { SetActive(); }
        else { SetInactive(); }
    }

    private void ChangeSprite()
    {
        if (isActive) { SetInactive(); }
        else { SetActive(); }
    }

    private void SetActive()
    {
        isActive = true;
        imageComponent.sprite = active;
        menu.SetActive(isActive);
    }

    private void SetInactive()
    {
        isActive = false;
        imageComponent.sprite = inactive;
        menu.SetActive(isActive);
    }

    private void OnValidate()
    {
        if (buttonComponent == null) buttonComponent = GetComponent<Button>();
    }
}
