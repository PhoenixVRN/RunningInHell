using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textLastMessage;
    [SerializeField] private TMP_InputField _textMessageField;

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void SendButton()
    {
        _photonView.RPC("SendData", RpcTarget.AllBuffered, PhotonNetwork.NickName, _textMessageField.text);
    }

    [PunRPC]
    private void SendData(string nickName, string message)
    {
        _textLastMessage.text = nickName + ": " + message;
    }
}
