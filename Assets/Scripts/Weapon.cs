
using UnityEngine;

public class Weapon : Collidable
{
    // Damage structure
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 };
    public float[] lockBackForce = { 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3, 3.2f };

    // Upgrade weapon
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    private float cooldown = 0.5f; // time needed for the next swing
    private float lastSwing; // last time swing
    private Animator anim;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.B))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                if (Input.GetKeyDown(KeyCode.Space))
                    Swing();
                else if (Input.GetKeyDown(KeyCode.B))
                    Swing2();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;

            // Create a new damage object, then send it to the fighter we've hit
            Damage damage = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                pushForce = lockBackForce[weaponLevel],
                origin = transform.position
            };

            coll.SendMessage("ReceiveDamage", damage);

            Debug.Log(coll.name);
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

    private void Swing2()
    {
        anim.SetTrigger("Swing2");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        // Change weapon stats

    }

    // Save weapon level for load state
    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

}
