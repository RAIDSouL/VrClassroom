﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

using Hashtable = ExitGames.Client.Photon.Hashtable;

//
//This script connects to PHOTON servers and creates a room if there is none, then automatically joins
//
//namespace Networking.Pun2
//{
public class NetworkManager : Scene {
    LobbyCanvas LobbyCanvas;
    public Text status;
    public GameObject Character;
    public Playfabmanager playfabmanagerAndroid;
    public Playfabmanager playfabmanagerVR;
    [SerializeField] GameObject[] androidObjs;
    [SerializeField] GameObject[] vrObjs;

    public static NetworkManager instance;

    Playfabmanager current;

    private void Awake() {
        instance = this;
    }

    protected override void Start() {
        //playfabmanager = FindObjectOfType<Playfabmanager>();
        switch (PlatformSetting.Instance.platform) {
        case Platform.VR:
        VrCanvas.SetInstance(true);
        AndroidCanvas.SetInstance(false);
        LobbyCanvas = VrCanvas.GetComponent<LobbyCanvas>();
        current = playfabmanagerVR;
        FindObjectOfType<CharecterEditor>().ChildObj.GetComponent<OVRRaycaster>().enabled = true;
        foreach (GameObject item in vrObjs) {
            item.SetActive(true);
        }
        break;
        case Platform.ANDROID:
        AndroidCanvas.SetInstance(true);
        VrCanvas.SetInstance(false);
        LobbyCanvas = AndroidCanvas.GetComponent<LobbyCanvas>();
        current = playfabmanagerAndroid;
        FindObjectOfType<CharecterEditor>().ChildObj.GetComponent<GraphicRaycaster>().enabled = true;
        foreach (GameObject item in androidObjs) {
            item.SetActive(true);
        }
        break;
        default:
        break;
        }
    }

    public void Connect() {
        ConnectToMaster();
    }


    public void ConnectToMaster() {
        PhotonNetwork.OfflineMode = false; //true would "fake" an online connection
        PhotonNetwork.NickName = "Test" + Random.Range(0, 100);//LobbyCanvas.UsernameInput.text; //we can use a input to change this 
        PhotonNetwork.AutomaticallySyncScene = true; //To call PhotonNetwork.LoadLevel()
        PhotonNetwork.GameVersion = "v1"; //only people with the same game version can play together


        //PhotonNetwork.ConnectToMaster(ip, port, appid); //manual connection
        PhotonNetwork.ConnectUsingSettings(); //automatic connection based on the config file
        setstatus("Connecting...");
    }

    public void setstatus(string des) {
        status.gameObject.SetActive(true);
        status.text = des;
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        Debug.Log(cause);
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        current.CheckIfTeacher();
        //Debug.Log(playfabmanager.GetTeacherValue());
        //string plyertype = playfabmanager.GetTeacherValue() ? PlayerType.Teacher : PlayerType.Student;
        /*if (LobbyCanvas.UserType.captionText.text == LobbyCanvas.UserType.options[0].text)
        {
            plyertype = PlayerType.Teacher;
        }
        else if(LobbyCanvas.UserType.captionText.text == LobbyCanvas.UserType.options[1].text)
        {
            plyertype = PlayerType.Student;
        }*/

    }

    public void LoadAfterGetUserData(bool plyertype) {
        Hashtable ConnectHash = new Hashtable();
        string plyer = plyertype ? PlayerType.Teacher : PlayerType.Student;
        ConnectHash.Add(PropertiesKey.PlayerType, plyer);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ConnectHash);
        StartCoroutine(WaitFrameAndConnect());
        Debug.Log("Connected to master!");
        setstatus("Connected");
    }
    IEnumerator WaitFrameAndConnect() {
        yield return new WaitForEndOfFrame();
        Debug.Log("Connecting");

        LobbyCanvas.LoginGroup.SetActive(false);
        CharecterEditor._instance.TogglePanel(true);

        //LobbyCanvas.JoinGroup.SetActive(true);
        //Character.SetActive(true);

        //ConnectToRoom();
    }

    public void ConnectToRoom() {
        if (!PhotonNetwork.IsConnected)
            return;
        Debug.Log("ConnectToRoom " + LobbyCanvas.RoomnameInput.text);
        //PhotonNetwork.CreateRoom("name"); //Create a specific room - Callback OnCreateRoomFailed
        PhotonNetwork.JoinRoom(LobbyCanvas.RoomnameInput.text); //Join a specific room - Callback OnJoinRoomFailed

        //PhotonNetwork.JoinRandomRoom(); // Join a random room - Callback OnJoinRandomRoomFailed
        //PhotonNetwork.JoinRoom(RoomInput.text);
    }

    /* public override void OnJoinRandomFailed(short returnCode, string message)
     {
         Debug.Log("OnJoinRandomFailed");
         CreateRoom();
     }*/

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("OnCreateRoomFailed " + message);
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinedRoom() {
        //Go to next scene after joining the room
        base.OnJoinedRoom();
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);

        SceneManager.LoadScene(SceneKey.Classroom); //go to the room scene
    }

    public void ConnectRoom() {
        PhotonNetwork.JoinRoom(LobbyCanvas.RoomnameInput.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("OnJoinRoomFailed");
        CreateRoom();
    }

    void CreateRoom() {
        Debug.Log("CreateRoom");
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 15 });
        PhotonNetwork.CreateRoom(LobbyCanvas.RoomnameInput.text, new RoomOptions { MaxPlayers = 15 });
    }
}
