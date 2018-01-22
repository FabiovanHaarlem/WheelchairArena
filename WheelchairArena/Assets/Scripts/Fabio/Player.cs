using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CameraHolder;
    [SerializeField]
    private BulletPool m_BulletPool;
    private RaycastHit m_RaycastHit;
    private Rigidbody m_Rigidbody;

    private float m_KnockBackForce;
    private float m_ShootTimer;
    private float m_ShootTimerReset;

	void Start ()
    {
        m_KnockBackForce = 80f;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_ShootTimer = 0.1f;
        m_ShootTimerReset = m_ShootTimer;
	}
	
	void Update ()
    {
        m_ShootTimer -= Time.deltaTime;

		if (Input.GetMouseButton(0) && m_ShootTimer <= 0f)
        {
            Shoot();
        }

        HoldCameraInPlace();
	}

    private void FixedUpdate()
    {
        RaycastHit hit;

        Ray ray = (Camera.main.ScreenPointToRay(Input.mousePosition));

        Physics.Raycast(ray, out hit, Mathf.Infinity);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 50, Color.red);

        transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

        m_RaycastHit = hit;
    }

    private Vector3 GetBulletDirection()
    {
        Vector3 target = m_RaycastHit.point - transform.position;
        target.y = 0;

        return target.normalized;
    }

    private void Shoot()
    {
        Vector3 bulletDirection = GetBulletDirection();
        m_BulletPool.ActvateBullet(bulletDirection, bulletDirection, transform.position);

        m_Rigidbody.AddForce(-bulletDirection * m_KnockBackForce);

        m_ShootTimer = m_ShootTimerReset;
    }

    private Vector3 Normalize(Vector3 velocity)
    {
        Vector3 normalized = new Vector3();

        float x = velocity.x * velocity.x;
        float z = velocity.z * velocity.z;
        float amout = x + z;
        float sqrt = (float)Mathf.Sqrt(amout);

        if (sqrt > 0)
        {
            normalized.x = velocity.x / sqrt;
            normalized.z = velocity.z / sqrt;
        }

        return normalized;
    }

    private void HoldCameraInPlace()
    {
        m_CameraHolder.transform.position = transform.position;
    }

    private void Die()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Die();
        }
    }
}
