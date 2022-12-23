using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private ListItem _itemPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private string _nickName;

    private List<RoomInfo> _allRoomInfo = new List<RoomInfo>();

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        // PhotonNetwork.ConnectToRegion(_region);
        PhotonNetwork.ConnectToBestCloudServer();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Подрубились к " + PhotonNetwork.CloudRegion);
        PhotonNetwork.NickName = _nickName;
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Отключились");
    }

    public void CreateRoomButton()
    {
        if (!PhotonNetwork.IsConnected) return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
        
        PhotonNetwork.LoadLevel("Game_Scene");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Создана комата " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось создать комату!");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            for (int i = 0; i < _allRoomInfo.Count; i++)
            {
                if (_allRoomInfo[i].masterClientId == info.masterClientId)
                    return;
            }
            ListItem listItem = Instantiate(_itemPrefab, _content);
            if (listItem != null)
            {
                listItem.SetInfo(info);
                _allRoomInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game_Scene");
    }

    public void JoinRanromRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinButton()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Intro");
    }
}
