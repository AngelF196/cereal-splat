using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSpawner : MonoBehaviour
{
    public GameObject uhitbox;
    public GameObject dhitbox;
    public GameObject lhitbox;
    public GameObject rhitbox;
    public GameObject Downtilt;
    public GameObject SideTitleL;
    public GameObject SideTitleR;
    public GameObject Jab;
    public GameObject Uptilt;

    public GameObject hitcircle;
    public Transform hitboxSpawnPoint;
    public PlayerMove movement;

    public void EnableDowntilt()
    {
        Downtilt.SetActive(true);
    }
    public void EnableJab()
    {
        Jab.SetActive(true);
    }
    public void EnableUptilt()
    {
        Uptilt.SetActive(true);
    }
    public void EnableSideTilt()
    {
        if (movement.isFacingLeft)
        {
            SideTitleR.SetActive(true);
        }
        else
        {
            SideTitleL.SetActive(true);
        }
    }
    public void EnableUHitbox()
    {
        uhitbox.SetActive(true);
    }
    public void EnableDHitbox()
    {
        dhitbox.SetActive(true);
    }
    public void EnableLHitbox()
    {
        lhitbox.SetActive(true);
    }
    public void EnableRHitbox()
    {
        if (movement.isFacingLeft)
        {
            lhitbox.SetActive(true);
        }
        else
        {
            rhitbox.SetActive(true);
        }
    }
    public void DisableHitboxes()
    {
        uhitbox.SetActive(false);
        dhitbox.SetActive(false);
        lhitbox.SetActive(false);
        rhitbox.SetActive(false);
        Downtilt.SetActive(false);
        SideTitleL.SetActive(false);
        SideTitleR.SetActive(false);
        Jab.SetActive(false);
        Uptilt.SetActive(false);
    }
    public void EnableHitcircle()
    {
        hitcircle.SetActive(true);
    }
    public void DisableHitcircle()
    {
        hitcircle.SetActive(false);
    }
}
