using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _playerGun;
    [SerializeField] private float _restartDelay = 3;
    [SerializeField] private float _mouseSens;
    private MultiplayerManager _multiplayerManager;
    private float _rawHorizontal;
    private float _rawVertical;
    private float _mouseX;
    private float _mouseY;
    private bool _space;
    private bool _isShoot;
    private bool _hold = false;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update()
    {
        if (_hold) return;

        _rawHorizontal = Input.GetAxisRaw("Horizontal");
        _rawVertical = Input.GetAxisRaw("Vertical");
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        _space = Input.GetKeyDown(KeyCode.Space);

        _isShoot = Input.GetMouseButton(0);

        _player.RotateX(-_mouseY * _mouseSens);
        _player.SetInput(_rawHorizontal, _rawVertical, _mouseX * _mouseSens);

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

    public void Restart(string jsonRestartInfo)
    {
        Debug.Log("�������!!");
        RestartInfo info = JsonUtility.FromJson<RestartInfo>(jsonRestartInfo);

        StartCoroutine(Hold());

        _player.transform.position = new Vector3(info.x, 0, info.z);
        _player.SetInput(0, 0, 0);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", info.x },
            {"pY", 0 },
            {"pZ", info.z },

            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},

            { "rX", 0 },
            { "rY", 0 },

            { "s", false}
        };

        _multiplayerManager.SendMessage("move", data);
    }

    private System.Collections.IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSeconds(_restartDelay);

        _hold = false;
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

[Serializable]
public struct RestartInfo
{
    public float x;
    public float z;
}