using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Camera;
    [SerializeField]
    private Transform m_CameraMainPosition;
    [SerializeField]
    private GameObject m_CameraHolder;
    [SerializeField]
    private BulletPool m_BulletPool;
    private RaycastHit m_RaycastHit;
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private Transform m_RightGun;
    [SerializeField]
    private Transform m_LeftGun;

    private float m_KnockBackForce;
    private float m_ShootTimer;
    private float m_ShootTimerReset;
    private float m_PowerUpTimer;

    private float m_ShakeTimer;
    private float m_ShakeAmount;

    private bool m_Invisible;
    private bool m_GunSwitch;

    void Start()
    {
        m_PowerUpTimer = 0f;
        m_Invisible = false;
        m_KnockBackForce = 50f;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_ShootTimer = 0.1f;
        m_ShootTimerReset = m_ShootTimer;
    }

    void Update()
    {
        m_ShootTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && m_ShootTimer <= 0f)
        {
            Shoot();
        }

        if (m_PowerUpTimer >= 0f)
        {
            m_PowerUpTimer -= Time.deltaTime;
        }

        if (m_PowerUpTimer <= 0f)
        {
            DeactivatePowerUp();
        }


        ScreenShake();
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
        Vector3 bulletStartPosition = transform.position;

        if (m_GunSwitch)
        {
            bulletStartPosition = m_RightGun.position;
        }
        else if (!m_GunSwitch)
        {
            bulletStartPosition = m_LeftGun.position;
        }

        m_GunSwitch = !m_GunSwitch;

        Vector3 bulletDirection = GetBulletDirection();
        m_BulletPool.ActvateBullet(bulletDirection, bulletDirection, bulletStartPosition);

        m_Rigidbody.AddForce(-bulletDirection * m_KnockBackForce);

        if (m_PowerUpTimer <= 0)
        {
            ShakeCameraShoot();
        }
        else if (m_PowerUpTimer >= 0)
        {
            ShakeCameraPowerUp();
        }

        m_ShootTimer = m_ShootTimerReset;
    }

    private void HoldCameraInPlace()
    {
        m_CameraHolder.transform.position = transform.position;
    }

    private void Die()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ActivatePowerUp()
    {
        m_KnockBackForce = 150f;
        m_ShootTimerReset = 0.03f;
        m_Invisible = true;
        m_PowerUpTimer = 5f;
    }

    private void DeactivatePowerUp()
    {
        m_KnockBackForce = 50f;
        m_ShootTimerReset = 0.1f;
        m_Invisible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Invisible)
        {
            if (other.CompareTag("Enemy"))
            {
                Die();
            }
        }

        if (other.CompareTag("PowerUp"))
        {
            ActivatePowerUp();
            other.gameObject.SetActive(false);
            ShakeCameraPowerUp();
        }
    }

    private void ScreenShake()
    {
        if (m_ShakeTimer >= 0)
        {
            Vector2 shakePosition = Random.insideUnitCircle * m_ShakeAmount;

            m_Camera.transform.position = new Vector3(m_CameraMainPosition.position.x + shakePosition.x,
                m_CameraMainPosition.position.y + shakePosition.y, m_CameraMainPosition.position.z);

            m_ShakeTimer -= Time.deltaTime;
        }
        else
        {
            m_Camera.transform.position = m_CameraMainPosition.position;
        }
    }

    public void ShakeCameraShoot()
    {
        float shakeStrength = 0.03f;
        float shakeDuration = 0.1f;
        m_ShakeAmount = shakeStrength;
        m_ShakeTimer = shakeDuration;
    }

    public void ShakeCameraPowerUp()
    {
        float shakeStrength = 0.4f;
        float shakeDuration = 0.1f;
        m_ShakeAmount = shakeStrength;
        m_ShakeTimer = shakeDuration;
    }
}
