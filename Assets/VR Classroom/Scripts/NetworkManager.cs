using UnityEngine;
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
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_InputField UserName, RoomInput;
    public TMPro.TMP_Dropdown Type;
    public TMPro.TextMeshProUGUI status;

    public GameObject LoginCanvas, JoinCanvas, Character;

    private void Start()
    {
        LoginCanvas.SetActive(true);
        JoinCanvas.SetActive(false);
        Character.SetActive(false);
    }

    public void Connect()
    {
        ConnectToMaster();
    }


    public void ConnectToMaster()
    {
        PhotonNetwork.OfflineMode = false; //true would "fake" an online connection
        PhotonNetwork.NickName = UserName.text; //we can use a input to change this 
        PhotonNetwork.AutomaticallySyncScene = true; //To call PhotonNetwork.LoadLevel()
        PhotonNetwork.GameVersion = "v1"; //only people with the same game version can play together


        //PhotonNetwork.ConnectToMaster(ip, port, appid); //manual connection
        PhotonNetwork.ConnectUsingSettings(); //automatic connection based on the config file
        setstatus("Connecting...");
    }

    public void setstatus(string des)
    {
        status.gameObject.SetActive(true);
        status.text = des;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        string plyertype = Type.captionText.text;
        if (Type.captionText.text == Type.options[0].text)
        {
            plyertype = PlayerType.Teacher;
        }
        else if(Type.captionText.text == Type.options[1].text)
        {
            plyertype = PlayerType.Student;
        }

        Hashtable ConnectHash = new Hashtable();
        ConnectHash.Add(PropertiesKey.PlayerType, plyertype);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ConnectHash);
        StartCoroutine(WaitFrameAndConnect());
        Debug.Log("Connected to master!");
        setstatus("Connected");

    }

    IEnumerator WaitFrameAndConnect()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Connecting");
        LoginCanvas.SetActive(false);
        JoinCanvas.SetActive(true);
        Character.SetActive(true);
        //ConnectToRoom();
    }

    public void ConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        Debug.Log("ConnectToRoom");
        //PhotonNetwork.CreateRoom("name"); //Create a specific room - Callback OnCreateRoomFailed
        PhotonNetwork.JoinRoom(RoomInput.text); //Join a specific room - Callback OnJoinRoomFailed
        
        //PhotonNetwork.JoinRandomRoom(); // Join a random room - Callback OnJoinRandomRoomFailed
                                        //PhotonNetwork.JoinRoom(RoomInput.text);
    }

   /* public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        CreateRoom();
    }*/

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed " + message);
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        //Go to next scene after joining the room
        base.OnJoinedRoom();
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);

        SceneManager.LoadScene(SceneKey.Classroom); //go to the room scene
    }

    public void ConnectRoom()
    {
        PhotonNetwork.JoinRoom(RoomInput.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("CreateRoom");
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 15 });
        PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 15 });
    }
}
//}