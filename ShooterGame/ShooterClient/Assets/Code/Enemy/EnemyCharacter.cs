using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;
    [SerializeField] private PlayerSquat _characterSquat;
    public Vector3 TargetPosition { get; private set; }
    private float _velocityMagnitude;

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
}