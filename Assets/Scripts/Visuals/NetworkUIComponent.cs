using System;
using Game;
using Game.Library;
using Game.Network;
using TMPro;
using UnityEngine;

namespace Visuals
{
    public class NetworkUIComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text joinCodeText;
        [SerializeField] private GameObject panel;
        
        private void Start()
        {
            if(SceneDataTransferComponent.Instance.TryReadSingle(out NetworkHostCookie cookie))
            {
                joinCodeText.SetText(cookie.JoinCode);
                GameController.Instance.OnGameStarted += () => panel.SetActive(false);
            }
            else
                gameObject.SetActive(false);
        }
    }
}