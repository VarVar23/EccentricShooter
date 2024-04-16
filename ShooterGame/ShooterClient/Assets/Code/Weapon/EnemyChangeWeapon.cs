using System;
using UnityEngine;

public class EnemyChangeWeapon : MonoBehaviour
{
    [SerializeField] private EnemyGun[] _weapons;
    [SerializeField] private EnemyController _enemyController;

    public void ChangeWeapon(string number)
    {
        int index = Convert.ToInt32(number);

        if(index < 0 || index >= _weapons.Length)
        {
            Debug.LogError("Не правильный индекс оружия");
            return;
        }

        foreach(var weapon in _weapons)
            weapon.gameObject.SetActive(false);

        _weapons[index].gameObject.SetActive(true);
        _enemyController.EnemyGun = _weapons[index];
    }
}