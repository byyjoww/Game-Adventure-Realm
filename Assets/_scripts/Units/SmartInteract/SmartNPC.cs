using Elysium.Attributes;
using PolyPerfect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;

public class SmartNPC : SmartObject
{
    [SerializeField] private GameObject focusPanel;
    [SerializeField] private NPCPanel interactionPanel;

    private SmartController cachedController { get; set; }

    private void Start() 
    { 
        OnFocusSet += () => ToggleFocusPanel(true);
        OnFocusRemoved += () => ToggleFocusPanel(false);
        focusable = true;
        interactable = true;        
    }

    public override void Interact(SmartController _smartController)
    {
        cachedController = _smartController;

        ToggleInteractablePanel(true);
        RestrictMovement();
        _smartController.GetComponent<FirstPersonController>().MouseLook.UnlockCursor();

        interactionPanel.OnObjectDeactivated += ReturnMovement;
    }

    public void ToggleFocusPanel(bool _focusable) => focusPanel.SetActive(_focusable);
    public void ToggleInteractablePanel(bool _interactible) => interactionPanel.SetActive(_interactible);

    private void RestrictMovement() 
    {
        cachedController.GetComponent<IMovement>().RestrictMovement(true);
    }
    private void ReturnMovement() 
    {
        cachedController.GetComponent<IMovement>().RestrictMovement(false);
        cachedController.GetComponent<FirstPersonController>().MouseLook.LockCursor();
        interactionPanel.OnObjectDeactivated -= ReturnMovement;
    }
}
