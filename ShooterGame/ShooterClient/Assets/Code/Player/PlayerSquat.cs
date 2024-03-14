using UnityEngine;

public class CharacterSquat : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _head;
    [SerializeField] private CapsuleCollider _collider;

    [SerializeField] private Vector3 _normalHeadPoisiton;
    [SerializeField] private Vector3 _normalBodyPoisiton;
    [SerializeField] private Vector3 _normalBodyRotation;
    [SerializeField] private Vector3 _normalCenterCollider;
    [SerializeField] private float _normalHeightCollider;

    [SerializeField] private Vector3 _squatHeadPoisiton;
    [SerializeField] private Vector3 _squatBodyPoisiton;
    [SerializeField] private Vector3 _squatBodyRotation;
    [SerializeField] private Vector3 _squatCenterCollider;
    [SerializeField] private float _squatHeightCollider;

    public void Squat(bool squat)
    {
        if(squat)
        {
            _head.transform.localPosition = _squatHeadPoisiton;
            _body.transform.localPosition = _squatBodyPoisiton;
            _body.transform.localRotation = Quaternion.Euler(_squatBodyRotation);
            _collider.center = _squatCenterCollider;
            _collider.height = _squatHeightCollider;
        }    
        else
        {
            _head.transform.localPosition = _normalHeadPoisiton;
            _body.transform.localPosition = _normalBodyPoisiton;
            _body.transform.localRotation = Quaternion.Euler(_normalBodyRotation);
            _collider.center = _normalCenterCollider;
            _collider.height = _normalHeightCollider;
        }
    }    
}