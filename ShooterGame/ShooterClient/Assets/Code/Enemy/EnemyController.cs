using Colyseus.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SmoothMovement _smoothMovement; //

    private void Awake() => _smoothMovement = new SmoothMovement(this); //

    internal void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;

        foreach (DataChange change in changes)
        {
            switch(change.Field)
            {
                case "x":
                    position.x = (float)change.Value;
                    break;
                case "y":
                    position.z = (float)change.Value;
                    break;
                default:
                    Debug.LogWarning("���-�� ����� �� ��� :)");
                    break;
            }

            _smoothMovement.ChangePosition(transform, position); //
        }
    }
}

public class SmoothMovement // ������ �����, ������� ����� �������� �� ����������� ��������
{
    private MonoBehaviour _monoBeh; // ����� ��� ������� ������� (� ������, ����� �������� � ��� ����, ������ ����� �����������)
    private Transform _moveObject; // ������, ������� ����� ������������
    private Vector3 _nextPosition; // �����, ���� ����� ������������ ������
    private IEnumerator _corutine; // ��������, ��� ��������� ���������� (��������, ����� �������� �� Update, ����� �������� � ����������)

    private const float _speed = 10; // ������ ����� �� Model �������� ������. ��� ����� �����������

    public SmoothMovement(MonoBehaviour monoBeh) => _monoBeh = monoBeh; 

    public void ChangePosition(Transform moveObject, Vector3 nextPosition)
    {
        _moveObject = moveObject;
        _nextPosition = nextPosition;

        if (_corutine != null)
            _monoBeh.StopCoroutine(_corutine);

        _corutine = Movement();
        _monoBeh.StartCoroutine(_corutine);
    }

    private IEnumerator Movement()
    {
        while (!Mathf.Equals(_moveObject.position, _nextPosition))
        {
            _moveObject.position = Vector3.MoveTowards(_moveObject.position, _nextPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }
}