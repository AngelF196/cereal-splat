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
    [SerializeField] private bool dashRec; //hide
    [SerializeField] private bool attackRec; //hide
    [SerializeField] private bool jumpHeld;

    [Header("Input Buffering")]
    [SerializeField] private float inputBuffer;
    private float jumpTimer;
    private float dashTimer;
    private float attackTimer;

    //PlayerMove Access
    public bool saysJump => jumpTimer > 0f;
    public bool saysDash => dashTimer > 0f;
    public bool saysAttack => attackTimer > 0f;
    public bool jumpCutRec => !jumpHeld;
    public Vector2 RawDirections => rawPlayerDirections;
    public Vector2 SmoothedDirections => playerDirections;
    public enum Action
    {
        jump,
        dash,
        attack
    }
    void Update()
    {
        InputGather();

        if (jumpRec)
        {
            jumpTimer = inputBuffer;
        }
        if (dashRec)
        {
            dashTimer = inputBuffer;
        }
        if (attackRec)
        {
            attackTimer = inputBuffer;
        }

        if (jumpTimer > 0f) jumpTimer -= Time.deltaTime;
        if (dashTimer > 0f) dashTimer -= Time.deltaTime;
        if (attackTimer > 0f) attackTimer -= Time.deltaTime;
    }

    public void Consume(Action action)
    {
        switch (action)
        {
            case Action.jump:
                jumpTimer = 0f;
            break;
            case Action.dash:
                dashTimer = 0f;
            break;
            case Action.attack:
                attackTimer = 0f;
            break;
        }
    }
    private void InputGather()
    {
        playerDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawPlayerDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        jumpRec = Input.GetKeyDown(KeyCode.Space);
        jumpHeld = Input.GetKey(KeyCode.Space);
        dashRec = Input.GetKeyDown(KeyCode.LeftShift);
        attackRec = Input.GetKeyDown(KeyCode.Z);
    }
}
