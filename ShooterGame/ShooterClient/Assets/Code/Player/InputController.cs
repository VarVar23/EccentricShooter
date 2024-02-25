using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private float _rawHorizontal;
    private float _rawVertical;

    private void Update()
    {
        _rawHorizontal = Input.GetAxisRaw(_horizontal);
        _rawVertical = Input.GetAxisRaw(_vertical);

        _player.SetInput(_rawHorizontal, _rawVertical);
        SendMove();
    }

    private void SendMove()
    {
        Vector3 position = _player.GetMoveInfo();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"x", position.x },
            {"y", position.z }
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
