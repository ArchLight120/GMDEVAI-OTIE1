using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject turret;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Shoot on Spacebar
        {
            GameObject bullet = Instantiate(bulletPrefab, turret.transform.position, turret.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
            bullet.GetComponent<Bullet>().shooterTag = this.tag;
        }
    }
}

