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
    [SerializeField] private bool dashRec;

    [Header("Input Buffering")]
    [SerializeField] private float inputBuffer;
    private float jumpTimer;
    private float attackTimer;
    private float dashTimer;

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
        attack,
        dash
    }

    //PlayerAttacking Access
    public AttackDirections attackDir;
    public enum AttackDirections
    {
        Neutral,
        Left,
        Right,
        Up,
        Down,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
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
            attackTimer = inputBuffer;
        }
        if (dashRec)
        {
            dashTimer = inputBuffer;
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

        attackDir = GetDirection(rawPlayerDirections);

        jumpRec = Input.GetKeyDown(KeyCode.Space);
        jumpHeld = Input.GetKey(KeyCode.Space);
        dashRec = Input.GetKeyDown(KeyCode.LeftShift);
        attackActRec = Input.GetKeyDown(KeyCode.Z);
    }

    AttackDirections GetDirection(Vector2 axis)
    {
        if (axis == Vector2.zero)
        {
            return AttackDirections.Neutral;
        }

        int x = Mathf.RoundToInt(axis.x);
        int y = Mathf.RoundToInt(axis.y);

        if (x == -1 && y == 0) return AttackDirections.Left;
        if (x == 1 && y == 0) return AttackDirections.Right;
        if (x == 0 && y == 1) return AttackDirections.Up;
        if (x == 0 && y == -1) return AttackDirections.Down;

        if (x == -1 && y == 1) return AttackDirections.UpLeft;
        if (x == 1 && y == 1) return AttackDirections.UpRight;
        if (x == -1 && y == -1) return AttackDirections.DownLeft;
        if (x == 1 && y == -1) return AttackDirections.DownRight;

        return AttackDirections.Neutral;
    }
}
