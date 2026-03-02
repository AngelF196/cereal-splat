using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerIndex;

    public RuntimeAnimatorController[] animatorControllers;

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Initialize(int index)
    {
        playerIndex = index;
        AssignAnimator();
        AssignControls();
    }
    void AssignAnimator()
    {
        if (playerIndex < animatorControllers.Length)
        {
            animator.runtimeAnimatorController = animatorControllers[playerIndex];
        }
        else
        {
            Debug.LogWarning("No animator assigned for player " + playerIndex);
        }
    }
    void AssignControls()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        input.controls = GameManager.Instance.allPlayerControls[playerIndex];
        Debug.Log($"I'm on control index {playerIndex}");
    }
}
