using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;

    [SerializeField] private float fireRate;
    private float timeSinceLastShot;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot > fireRate)
        {
            timeSinceLastShot = 0;

            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
}
