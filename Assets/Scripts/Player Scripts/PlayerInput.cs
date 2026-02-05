using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerInput : MonoBehaviour
{
    [Header("Player Input")]
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    [SerializeField] private bool jumpRec; //hide
    [SerializeField] private bool attackActRec; //hide
    [SerializeField] private bool jumpHeld;

    [Header("Input Buffering")]
    [SerializeField] private float inputBuffer;
    private float jumpTimer;
    private float diveTimer;
    private float flipTimer;

    //PlayerMove Access
    public bool saysJump => jumpTimer > 0f;
    public bool saysDive => diveTimer > 0f;
    public bool saysFlip => flipTimer > 0f;
    public bool jumpCutRec => !jumpHeld;
    public Vector2 RawDirections => rawPlayerDirections;
    public Vector2 SmoothedDirections => playerDirections;
    public enum Action
    {
        jump,
        dive,
        flip
    }
    void Update()
    {
        InputGather();

        if (jumpRec)
        {
            jumpTimer = inputBuffer;
        }
        if (attackActRec)
        {
            flipTimer = inputBuffer;
        }

        if (jumpTimer > 0f) jumpTimer -= Time.deltaTime;
        if (diveTimer > 0f) diveTimer -= Time.deltaTime;
        if (flipTimer > 0f) flipTimer -= Time.deltaTime;
    }

    public void Consume(Action action)
    {
        switch (action)
        {
            case Action.jump:
                jumpTimer = 0f;
            break;
            case Action.dive:
                diveTimer = 0f;
            break;
            case Action.flip:
                flipTimer = 0f;
            break;
        }
    }
    private void InputGather()
    {
        playerDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawPlayerDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        jumpRec = Input.GetKeyDown(KeyCode.Space);
        jumpHeld = Input.GetKey(KeyCode.Space);
        attackActRec = Input.GetKeyDown(KeyCode.Z);
    }
}
