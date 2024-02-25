using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _playerTranform;
    private Vector3 _direction;

    private void Update()
    {
        Move();
    }

    public void SetInput(float inputH, float inputV)
    {
        _direction = new Vector3(inputH, 0, inputV).normalized;
    }

    private void Move()
    {
        _playerTranform.position += _direction * _speed * Time.deltaTime;
    }

    public Vector3 GetMoveInfo()
    {
        return _playerTranform.position;
    }
}