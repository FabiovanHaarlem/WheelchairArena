using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private List<Bullet> m_Bullets;
	
	void Start ()
    {
        m_Bullets = new List<Bullet>();

        for (int i = 0; i < 50; i++)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));
            m_Bullets.Add(bullet.GetComponent<Bullet>());
            bullet.SetActive(false);
        }
	}

    public void ActvateBullet(Vector3 bulletDirection, Vector3 rotation, Vector3 playerPosition)
    {
        for (int i = 0; i < m_Bullets.Count; i++)
        {
            if (!m_Bullets[i].gameObject.activeInHierarchy)
            {
                m_Bullets[i].ActivateBullet(bulletDirection, rotation, playerPosition);
                break;
            }
        }
    }

}
