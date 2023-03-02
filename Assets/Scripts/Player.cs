using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }

    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAni.SetTrigger("Show");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinIndex)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinIndex];
    }

    public void OnlevelUp()
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
        GameManager.instance.OnHitpointChange();
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnlevelUp();
        }
    }

    public void Heal(int healAmount)
    {
        if (hitpoint == maxHitpoint)
            return;

        hitpoint += healAmount;

        if (hitpoint > maxHitpoint)
            hitpoint = maxHitpoint;

        GameManager.instance.ShowText("+ " + healAmount + " hp", 25, Color.green, transform.position, Vector3.up * 50, 1.0f);
        GameManager.instance.OnHitpointChange();

    }

    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        immuneTime = Time.time;
        pushDirection = Vector3.zero;
    }
}
