using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _gun.ShootAction += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger("shoot");
        Debug.Log("Выстрел");
    }

    private void OnDestroy() => _gun.ShootAction -= Shoot;
}