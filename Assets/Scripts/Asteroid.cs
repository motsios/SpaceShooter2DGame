using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 19f;
    [SerializeField] GameObject _explosionPrefab;
    SpawnManager _spawnManager;
    UiManager _uiManager;


    void Awake()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
        _uiManager = FindObjectOfType<Canvas>().GetComponent<UiManager>();
    }
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.StartSpawning();
        }
        else if (other.tag == "Player")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
            _uiManager.UpdateLives(0);
            _spawnManager.OnPlayerDeath();
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }
}
