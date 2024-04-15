using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Health _health;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private CharacterSquat _characterSquat;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private Transform _playerTranform;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _maxAngle = 50;
    [SerializeField] private float _minAngle = -70;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private float _jumpDelay = 0.2f;

    private Vector3 _direction;
    private float _rotateY;
    private float _rotateX;
    private float _jumpTime;

    private bool _squat;

    private void Start()
    {
        var camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localRotation = Quaternion.identity;
        camera.localPosition = Vector3.zero;

        _health.SetMax(MaxHealth);
        _health.SetCurrent(MaxHealth);
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
    }

    internal void OnChange(List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "currentHp":
                    _health.SetCurrent((sbyte)change.Value);
                    break;
                case "loss":
                    MultiplayerManager.Instance.LossCounter.SetPlayerLoss((byte)change.Value);
                    break;
                default:
                    Debug.LogWarning("Что-то пошло не так :)");
                    break;
            }
        }
    }


    public void SetInput(float inputH, float inputV, float rotateY)
    {
        _rotateY += rotateY;
        _direction = new Vector3(inputH, 0, inputV);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY, out bool squat)
    {
        position = _playerTranform.position;
        velocity = _rigidbody.velocity;

        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
        squat = _squat;
    }

    public void RotateX(float value)
    {
        _rotateX = Mathf.Clamp(_rotateX + value, _minAngle, _maxAngle);
        _head.localEulerAngles = new Vector3(_rotateX, 0, 0);
    }

    public void Squat(bool value)
    {
        _squat = value;
        _characterSquat.Squat(value);
    }

    public void Jump()
    {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDelay) return;
        
        _jumpTime = Time.time;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    private void Move()
    {
        Vector3 velocity = (transform.forward * _direction.z + transform.right * _direction.x).normalized * Speed;
        velocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velocity;

        Velocity = velocity;
    }
}