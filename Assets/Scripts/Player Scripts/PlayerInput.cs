using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]   // Makes it show up in the Inspector
public struct PlayerControls
{
    public string horizontalAxis;  // e.g., "Horizontal_P1"
    public string verticalAxis;    // e.g., "Vertical_P1"
    public KeyCode jumpKey;        // e.g., KeyCode.Space
    public KeyCode attackKey;      // e.g., KeyCode.Z
    public KeyCode dashKey;        // e.g., KeyCode.LeftShift
}

public class PlayerInput : MonoBehaviour
{
    [Header("Player Input")]
    public PlayerControls controls;
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    private bool jumpRec;
    private bool attackActRec;
    private bool jumpHeld;
    private bool dashRec;

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
        playerDirections = new Vector2(Input.GetAxis(controls.horizontalAxis), Input.GetAxis(controls.verticalAxis));
        rawPlayerDirections = new Vector2(Input.GetAxisRaw(controls.horizontalAxis), Input.GetAxisRaw(controls.verticalAxis));

        attackDir = GetDirection(rawPlayerDirections);

        jumpRec = Input.GetKeyDown(controls.jumpKey);
        jumpHeld = Input.GetKey(controls.jumpKey);
        dashRec = Input.GetKeyDown(controls.dashKey);
        attackActRec = Input.GetKeyDown(controls.attackKey);
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
