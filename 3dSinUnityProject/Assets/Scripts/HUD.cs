using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _velocityText = null;
    [SerializeField]
    private FPSPlayer _player = null;

    private void Update()
    {
        if (_player != null)
        {
            // Vector3 horVel = _player.GetVelocity(); horVel.y = 0f;
            // _velocityText.SetText(horVel.magnitude.ToString());
            _velocityText.SetText(_player.GetVelocity().magnitude.ToString());
        }
    }
}
