using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _enemyGun;
    private Player _player;
    private List<float> _timeInterval = new List<float>() { 0, 0, 0, 0, 0 };
    private float AverageInterval
    {
        get
        {
            int length = _timeInterval.Count;
            float summ = 0;

            for(int i = 0; i < length; i++)
            {
                summ += _timeInterval[i];
            }

            return summ / length;
        }
    }

    private float _lastTime = 0;

    public void Init(Player player)
    {
        _player = player;
        _character.SetSpeed(_player.speed);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

        _enemyGun.Shoot(position, velocity);
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveTime()
    {
        float interval = Time.time - _lastTime;
        _lastTime = Time.time;

        _timeInterval.Add(interval);
        _timeInterval.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        SaveTime();

        Vector3 position = _character.TargetPosition;
        Vector3 velocity = _character.Velocity;

        foreach (DataChange change in changes)
        {
            switch(change.Field)
            {
                case "pX": position.x = (float)change.Value;
                    break;
                case "pY": position.y = (float)change.Value;
                    break;
                case "pZ": position.z = (float)change.Value;
                    break;
                case "vX": velocity.x = (float)change.Value;
                    break;
                case "vY": velocity.y = (float)change.Value;
                    break;
                case "vZ": velocity.z = (float)change.Value;
                    break;
                case "rX":
                    _character.SetRotateX((float)change.Value);
                    break;
                case "rY":
                    _character.SetRotateY((float)change.Value);
                    break;
                case "s":
                    _character.SetSquat((bool)change.Value);
                    break;
                default:
                    Debug.LogWarning("���-�� ����� �� ��� :)");
                    break;
            }
        }

        _character.SetMovement(position, velocity, AverageInterval);
    }
}