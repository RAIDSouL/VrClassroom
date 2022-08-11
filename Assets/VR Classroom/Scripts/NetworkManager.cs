using UnityEngine;
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
    public static NetworkManager instance;

    public Text status;

    [Header("VR")]
    public Playfabmanager playfabmanagerVR;
    [SerializeField] GameObject[] vrObjs;

    [Header("Android")]
    public Playfabmanager playfabmanagerAndroid;
    [SerializeField] GameObject[] androidObjs;

    //cache
    public Playfabmanager current;
    public LobbyCanvas LobbyCanvas;
    public OVRRaycaster OVRRaycaster;
    public GraphicRaycaster GraphicRaycaster;
    public CharecterEditor charecterEditor;

    [SerializeField] Text[] errorStatus;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        //charecterEditor = FindObjectOfType<CharecterEditor>();
    }

    protected override void Start() 
    {
        Debug.LogError("IsConnected  " + PhotonNetwork.IsConnected);

        switch (PlatformSetting.Instance.platform)
        {
            case Platform.VR:
                VrCanvas.SetInstance(true);
                AndroidCanvas.SetInstance(false);
                LobbyCanvas = VrCanvas.GetComponent<LobbyCanvas>();
                current = playfabmanagerVR;
                OVRRaycaster.enabled = true;
                foreach (GameObject item in vrObjs)
                {
                    item.SetActive(true);
                }
                break;
            case Platform.ANDROID:
                AndroidCanvas.SetInstance(true);
                VrCanvas.SetInstance(false);
                LobbyCanvas = AndroidCanvas.GetComponent<LobbyCanvas>();
                current = playfabmanagerAndroid;
                foreach (GameObject item in androidObjs)
                {
                    item.SetActive(true);
                }
                GraphicRaycaster.enabled = true;
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
        PhotonNetwork.NickName = UserData.Username; //we can use a input to change this 
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
        Debug.Log(returnCode);
        if (returnCode == -2)
        {
            setErrorStatus("Room name require.."); return;
        }
        if (Playfabmanager._instance.getTeacher())
        CreateRoom();
        else 
        {
            setErrorStatus("Room not exist..");
        }
    }
    public void setErrorStatus(string st) { 
        errorStatus[0].text =st ; 
        errorStatus[1].text =st ; 
    }

    void CreateRoom() 
    {
        Hashtable roomproperties = new Hashtable();
        roomproperties[PropertiesKey.bg] = PlayerPrefs.GetInt("classRoom", 0); ;
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 16,
            CustomRoomProperties = roomproperties,
        };

        Debug.Log("CreateRoom");
        PhotonNetwork.CreateRoom(LobbyCanvas.RoomnameInput.text, options );
    }
}
