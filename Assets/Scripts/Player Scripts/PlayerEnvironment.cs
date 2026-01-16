using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerEnvironment : MonoBehaviour
{

    [Header("Collision Layers")]
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Cast Params")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Vector2 wallBoxSize;
    [SerializeField] private Vector3 wallBoxOrigin;
    [SerializeField] private float castDistance;
    [SerializeField] private float rightCastDistance;
    [SerializeField] private float leftCastDistance;

    [Header("Wall Params")]
    private float wallTimer;
    [SerializeField] private float wallBuffer;
    [SerializeField] public bool DetectWalls;

    private void FixedUpdate()
    {
        if (!DetectWalls)
        {
            wallTimer -= Time.fixedDeltaTime;

            if (wallTimer <= 0)
            {
                DetectWalls = true;
                wallTimer = wallBuffer;
            }
        }

    }
    public bool FloorDetect()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, floorLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int WallDirectionDetect()
    {
        RaycastHit2D leftwallcast = Physics2D.BoxCast(transform.position + wallBoxOrigin, wallBoxSize, 0, -transform.right, leftCastDistance, wallLayer);
        RaycastHit2D rightwallcast = Physics2D.BoxCast(transform.position + wallBoxOrigin, wallBoxSize, 0, transform.right, rightCastDistance, wallLayer);
        if (DetectWalls)
        {
            if (leftwallcast && rightwallcast)
            {
                return 2;
            }
            else if (leftwallcast)
            {
                return -1;
            }
            else if (rightwallcast)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 3;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
        Gizmos.DrawWireCube(transform.position + wallBoxOrigin - transform.right * leftCastDistance, wallBoxSize);
        Gizmos.DrawWireCube(transform.position + wallBoxOrigin + transform.right * rightCastDistance, wallBoxSize);
    }
}
