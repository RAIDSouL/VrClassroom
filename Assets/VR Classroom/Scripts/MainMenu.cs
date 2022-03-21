using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInput;
    [SerializeField]
    private string roomname;


    public void ConnectRoom()
    {
        roomname = roomInput.text;
        PhotonNetwork.JoinRoom(roomname);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 15 });
        //PhotonNetwork.CreateRoom(roomname);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        SceneManager.LoadScene(SceneKey.Classroom);
    }
}
