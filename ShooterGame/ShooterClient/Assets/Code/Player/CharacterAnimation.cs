using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;   
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Character _character;
    
    private void Update()
    {
        var localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
        float speed = localVelocity.magnitude / _character.Speed;
        float sign = Mathf.Sign(localVelocity.z);

        _animator.SetFloat("Speed", speed * sign);
        _animator.SetBool("Grounded", !_checkFly.IsFly);
    }
}