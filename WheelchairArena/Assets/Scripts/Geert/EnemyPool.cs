using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    [SerializeField]
    private GameObject _EnemyPrefab;
    [SerializeField]
    private int _Amount;
    private GameObject[] _EnemyPool;
    [SerializeField]
    private GameObject _Player;
	// Use this for initialization
	void Start () {
        _EnemyPool = new GameObject[_Amount];
        for(int i = 0; i < _Amount; i++)
        {
            _EnemyPool[i] = Instantiate(_EnemyPrefab, gameObject.transform);
            _EnemyPool[i].SetActive(false);
            _EnemyPool[i].GetComponent<Enemy>().Set_player(_Player);
        }
	}

    public GameObject[] GetEnemyPool
    {
        get { return _EnemyPool; }
    }
	
	
}
