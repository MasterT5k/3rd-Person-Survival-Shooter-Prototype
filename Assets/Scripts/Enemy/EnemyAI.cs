using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyState
    {
        idle,
        chase,
        attack
    }

    [SerializeField]
    private EnemyState _currentState = EnemyState.chase;
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _gravity = 20f;
    [SerializeField]
    private int _damageAmount;
    [SerializeField]
    private float _attackDelay = 1.5f;
    private float _nextAttack = -1f;

    private CharacterController _controller;
    private Player _player;
    private Health _playerHealth;
    private Vector3 _velocity;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (_controller == null)
        {
            Debug.LogError("Charater Controller is NULL");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _playerHealth = _player.GetComponent<Health>();

        if (_player == null)
        {
            Debug.LogError("Player's Health is NULL");
        }
    }

    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.idle:
                break;
            case EnemyState.chase:
                CalculaterMovement();
                break;
            case EnemyState.attack:
                EnemyAttack();
                break;
            default:
                break;
        }
    }

    private void CalculaterMovement()
    {
        if (_player != null)
        {
            if (_controller.isGrounded == true)
            {
                Vector3 directionToPlayer = _player.transform.position - transform.position;
                directionToPlayer.Normalize();
                directionToPlayer.y = 0f;
                transform.localRotation = Quaternion.LookRotation(directionToPlayer);
                _velocity = directionToPlayer * _speed;
            }

            _velocity.y -= _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }

    private void EnemyAttack()
    {
        if (_nextAttack < Time.time)
        {
            _nextAttack = Time.time + _attackDelay;
            if (_playerHealth != null)
            {
                _playerHealth.Damage(_damageAmount);
            }
            else
            {
                _currentState = EnemyState.idle;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _currentState = EnemyState.attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _currentState = EnemyState.chase;
        }
    }
}
