using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    void Update()
    {
        ShootWeapon();
    }

    void ShootWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 center = new Vector3(0.5f, 0.5f, 0);

            Ray rayOrigin = Camera.main.ViewportPointToRay(center);

            RaycastHit hitinfo;

            if (Physics.Raycast(rayOrigin, out hitinfo))
            {
                Debug.Log("Hit: " + hitinfo.collider.name);

                Health health = hitinfo.collider.GetComponent<Health>();

                if (health != null)
                {
                    health.Damage(_damage);
                }
            }
        }
    }
}
