using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 m_BulletDirection;

    private int m_Speed;

    private float m_DisableTimer;

    public void ActivateBullet(Vector3 bulletDirection, Vector3 rotation, Vector3 playerPosition)
    {
        transform.position = playerPosition;
        m_BulletDirection = bulletDirection;
        m_Speed = 50;
        m_DisableTimer = 4;
        transform.eulerAngles = rotation;
        gameObject.SetActive(true);
    }


    private void Update()
    {
        transform.position += (m_BulletDirection * (m_Speed * Time.deltaTime));
        m_DisableTimer -= Time.deltaTime;

        if (m_DisableTimer <= 0)
        {
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        gameObject.SetActive(false);
        m_DisableTimer = 5;
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
