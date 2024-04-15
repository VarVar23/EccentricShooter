using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    [SerializeField] private CharacterSquat _characterSquat;
    public Vector3 TargetPosition { get; private set; }
    private float _velocityMagnitude;
    private string _sessionID;

    public void Init(string sessionID)
    {
        _sessionID = sessionID;
    }

    private void Start()
    {
        TargetPosition = transform.position;
    }

    private void Update()
    {
        if(_velocityMagnitude > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, _velocityMagnitude * Time.deltaTime);
        }
        else
        {
            transform.position = TargetPosition;
        }

    }

    public void SetRotateX(float value)
    {
        _head.localEulerAngles = new Vector3(value, 0, 0);
    }

    public void SetRotateY(float value)
    {
        transform.localEulerAngles = new Vector3(0, value, 0);
    }

    public void SetSpeed(float value) => Speed = value;

    public void SetMaxHp(int value)
    {
        MaxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }

    public void RestoreHp(int value)
    {
        _health.SetCurrent(value);
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        TargetPosition = position + velocity * averageInterval;
        _velocityMagnitude = velocity.magnitude;
        Velocity = velocity;
    }

    public void SetSquat(bool value)
    {
        _characterSquat.Squat(value);
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "id", _sessionID },
            { "value", damage }
        };

        MultiplayerManager.Instance.SendMessage("damage", data);
    }
}