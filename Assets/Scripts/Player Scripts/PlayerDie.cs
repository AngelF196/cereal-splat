using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{

    private BoxCollider2D BoxCollider2D;
    private Transform Transform;
    private Knockback knockback;
    private Rigidbody2D rb;
    private PlayerAudio paudio;
    public int timesDied = 0;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        Transform = GetComponent<Transform>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        paudio = GetComponentInChildren<PlayerAudio>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            paudio.PlayRandom();
            timesDied++;
            knockback.percent = 0;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1.5f);
        rb.constraints = ~RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        Transform.position = new Vector3(0, 6.49f, 0);
    }
}
