using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lifeTime = 2;
    private int _damage;

    public void Init(Vector3 velocity, int damage = 0)
    {
        _damage = damage;
        _rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.ApplyDamage(_damage);
        }

        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy();
    }
}