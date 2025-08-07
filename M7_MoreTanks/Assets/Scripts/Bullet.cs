using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    public int damage = 20;

    public string shooterTag; // Set this when the bullet is instantiated

    void OnCollisionEnter(Collision col)
    {
        GameObject target = col.gameObject;
        Debug.Log("Bullet hit: " + target.name + " with tag: " + target.tag);
        // Check if target has Health and is not the shooter
        if (target.CompareTag("Player") && shooterTag != "Player")
        {
            ApplyDamage(target);
        }
        else if (target.CompareTag("Enemy") && shooterTag != "Enemy")
        {
            ApplyDamage(target);
        }

        // Explosion effect
        GameObject e = Instantiate(explosion, this.transform.position, Quaternion.identity);
        Destroy(e, 1.5f);

        Destroy(this.gameObject);
    }

    void ApplyDamage(GameObject target)
    {
        Health hp = target.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }
    }
}
