using UnityEngine;
using UnityEngine.UI;

public class PlayerChangeWeapon : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private PlayerGun[] _weapons;
    [SerializeField] private Image[] _weaponsUI;
    [SerializeField] private Vector3 _maxScaleWeaponUI;
    [SerializeField] private float _alphaDeactive;
    private int _currentIndex;

    private void Start()
    {
        _currentIndex = 0;

        Change();
    }

    public void ChangeWeapon(float mouseSW)
    {
        if (mouseSW == 0) return;
        if (mouseSW > 0) _currentIndex++;
        if (mouseSW < 0) _currentIndex--;

        if(_currentIndex < 0) _currentIndex = _weapons.Length - 1;
        if(_currentIndex > _weapons.Length - 1) _currentIndex = 0;

        Change();
    }

    private void ChangeSend()
    {
        MultiplayerManager.Instance.SendMessage("changeWeapon", _currentIndex.ToString());
        Debug.Log("Отправили на сервер");
    }

    private void Change()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(false);
            _weaponsUI[i].rectTransform.localScale = Vector3.one;
            _weaponsUI[i].color = new Color(255, 255, 255, _alphaDeactive);
        }

        _weapons[_currentIndex].gameObject.SetActive(true);
        _weaponsUI[_currentIndex].rectTransform.localScale = _maxScaleWeaponUI;
        _weaponsUI[_currentIndex].color = Color.white;

        _inputController.PlayerGun = _weapons[_currentIndex];

        ChangeSend();
    }
}