using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEventBinder : MonoBehaviour
{
    [SerializeField] private AttackController AttackController;

    public void Execute() => AttackController.StartAttack();
}
