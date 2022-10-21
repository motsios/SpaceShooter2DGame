using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 2.5f;
    int speedBoost = 2;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] float _fireRate = 0.5f;
    [SerializeField] int _lives = 3;
    [SerializeField] bool _isTripleShotActive = false;
    [SerializeField] bool _isSpeedBoostActive = false;
    bool _isShieldActive = false;
    float _canFire = -1f;
    SpawnManager _spawnManager;
    [SerializeField] GameObject _shieldVisualizer;
    [SerializeField] GameObject _turnLeftVisualizer;
    [SerializeField] GameObject _turnRightVisualizer;
    [SerializeField] int _score = 0;
    UiManager _uiManager;
    [SerializeField] GameObject _rightEngine, _leftEngine;

    [SerializeField] AudioClip _laserClip;
    AudioSource _audioSource;


    void Awake()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
        _uiManager = FindObjectOfType<Canvas>().GetComponent<UiManager>();
        _audioSource = GetComponent<AudioSource>();

    }
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _audioSource.clip = _laserClip;
    }

    void Update()
    {
        CalculateMove();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            SpawnLaser();
        }
    }

    void SpawnLaser()
    {
        _canFire = Time.time + _fireRate;
        if (!_isTripleShotActive)
        {
            GameObject newLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1.05f, 0f), Quaternion.identity);
        }
        else
        {
            GameObject newLaser = Instantiate(_tripleShotPrefab, transform.position + new Vector3(-0.6f, 0f, 0f), Quaternion.identity);

        }
        _audioSource.Play();
    }

    void CalculateMove()
    {
        float horinput = Input.GetAxis("Horizontal");
        float verinput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnRightVisualizer.gameObject.SetActive(false);
            _turnLeftVisualizer.gameObject.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnRightVisualizer.gameObject.SetActive(true);
            _turnLeftVisualizer.gameObject.SetActive(false);
        }
        else
        {
            _turnRightVisualizer.gameObject.SetActive(false);
            _turnLeftVisualizer.gameObject.SetActive(false);
        }

        // transform.Translate(Vector3.right * horinput * speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verinput * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        if (_isSpeedBoostActive)
        {
            transform.Translate(new Vector3(horinput, verinput, 0) * _speed * speedBoost * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horinput, verinput, 0) * _speed * Time.deltaTime);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.gameObject.SetActive(false);
            return;
        }

        _lives--;
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(DisableTripleShot());
    }

    IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(DisableSpeedBoost());
    }

    IEnumerator DisableSpeedBoost()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
        _uiManager.CheckForBestScore(_score);
    }
}
