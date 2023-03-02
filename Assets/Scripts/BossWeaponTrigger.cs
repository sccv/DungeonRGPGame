
using UnityEngine;

public class BossWeaponTrigger : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    public float triggerLength = 1; // if we get close to enemy within 1m, it'll start attacking
    public float cooldown = 2.0f;
    public int damagePoint = 3;
    public float lockBackForce = 0.5f;

    private float lastTrigger;
    
    private Transform playerTransform;
    private Animator weaponAni;
    

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        weaponAni = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) < triggerLength) 
        {
            if ((Time.time - lastTrigger) > cooldown)
            {
                lastTrigger = Time.time;
                weaponAni.SetTrigger("Attack");
            }
        }
    }

    private void Update()
    {
        // Collision work
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }
            OnCollide(hits[i]);

            // The array is not cleaned up, clean up the array
            hits[i] = null;

        }
    }

    private void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
            {
                // Create a new damage object, then send it to the fighter we've hit
                Damage damage = new Damage
                {
                    damageAmount = damagePoint,
                    pushForce = lockBackForce,
                    origin = transform.position
                };
                coll.SendMessage("ReceiveDamage", damage);
            }
        }
    }
}
