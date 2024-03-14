using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _mouseSens;
    [SerializeField] private PlayerGun _playerGun;
    private MultiplayerManager _multiplayerManager;
    private float _rawHorizontal;
    private float _rawVertical;
    private float _mouseX;
    private float _mouseY;
    private bool _space;
    private bool _isShoot;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update()
    {
        _rawHorizontal = Input.GetAxisRaw("Horizontal");
        _rawVertical = Input.GetAxisRaw("Vertical");
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        _space = Input.GetKeyDown(KeyCode.Space);

        _isShoot = Input.GetMouseButton(0);

        _player.RotateX(-_mouseY * _mouseSens);

        if(_space) _player.Jump();
        if (Input.GetKeyDown(KeyCode.LeftControl)) _player.Squat(true);
        if (Input.GetKeyUp(KeyCode.LeftControl)) _player.Squat(false);

        if (_isShoot && _playerGun.TryShoot(out ShootInfo info))
        {
            SendShoot(ref info);
        }

        SendMove();
    }

    private void SendShoot(ref ShootInfo info)
    {
        info.key = _multiplayerManager.GetSessionId();
        string json = JsonUtility.ToJson(info);

        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY, out bool squat);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x },
            {"pY", position.y },
            {"pZ", position.z },

            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},

            { "rX", rotateX },
            { "rY", rotateY },

            { "s", squat }
        };

        _multiplayerManager.SendMessage("move", data);
    }
}

[Serializable]
public struct ShootInfo
{
    public string key;

    public float dX;
    public float dY;
    public float dZ;

    public float pX;
    public float pY;
    public float pZ;
}