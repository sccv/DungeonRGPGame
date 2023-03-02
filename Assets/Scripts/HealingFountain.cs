using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingFountain : Collidable
{
    public int healingPerMin = 1;

    private float healCooldown = 1.0f;
    private float lastHeal;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
            return;

        if (Time.time - lastHeal > healCooldown)
        {
            lastHeal = Time.time;
            GameManager.instance.player.Heal(healingPerMin);
        }
    }
}
