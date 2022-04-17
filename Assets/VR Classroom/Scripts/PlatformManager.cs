using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace ChiliGames.VRClassroom {
    //This script handles the different modes: Teacher, StudentVR, StudentPhone
    public class PlatformManager : MonoBehaviourPunCallbacks {
        [SerializeField] GameObject teacherRig;
        public GameObject studentRig;
        public ModelLoader ModelLoader;
        public Transform[] studentPositions;

        public Avatar avatar;
        private FollowVRRig teacherBodyFollow;
        [SerializeField] GameObject[] teacherAvatars;

        public Transform[] teacherRigParts;
        public Transform[] studentRigParts;

        [SerializeField] GameObject teacherBody;
        [SerializeField] GameObject studentBody;
        [SerializeField] GameObject studentBodyNonVR;

        [SerializeField] GameObject teacherSpecificTools;

        //Seats
        Hashtable h = new Hashtable();
        bool initialized;
        bool seated;
        int actorNum;

        //Modes
        public enum Mode { Teacher, StudentVR, StudentPhone };
        [Tooltip("Choose the mode before building")]
        public Mode mode;

        //Whiteboards
        public Whiteboard smallWhiteboard;

        public static PlatformManager instance;

        void Awake() {
            //If not connected go to lobby to connect
            if (!PhotonNetwork.IsConnected) {
                SceneManager.LoadScene(0);
            }
            instance = this;
            actorNum = PhotonNetwork.LocalPlayer.ActorNumber;

            string mytype = PhotonNetwork.LocalPlayer.CustomProperties[PropertiesKey.PlayerType].ToString();

            if(mytype == PlayerType.Teacher)
            {
                mode = Mode.Teacher;
            }
            else if(mytype == PlayerType.Student)
            {
               
                if (PlatformSetting.Instance.platform == Platform.ANDROID)
                {
                    mode = Mode.StudentPhone;
                }
                else
                {
                    mode = Mode.StudentVR;
                }
            }

            //If student connecting from phone, limit the fps to save battery. Also avoid sleep.
            if (mode == Mode.StudentPhone) {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }

        private IEnumerator Start() {

            


            //if this is the first player to connect, initialize the students list
            if (PhotonNetwork.IsMasterClient && !initialized) {
                InitializeStudentList();
            }
            yield return new WaitUntil(()=>PhotonNetwork.CurrentRoom.CustomProperties["Initialized"] != null);
            //if this is the teacher, activate its rig and create the body
            if (mode == Mode.Teacher) {
                teacherRig.SetActive(true);
                CreateTeacherBody();
                smallWhiteboard.GetComponent<PhotonView>().RequestOwnership();
                teacherSpecificTools.SetActive(true);
            }
            //if it's a student, create it's body and sit in right position if the student list already exists
            else if (mode == Mode.StudentVR || mode == Mode.StudentPhone) {
                studentRig.SetActive(true);
                CreateStudentBody();
                /*if (PhotonNetwork.CurrentRoom.CustomProperties["Initialized"] != null) {
                    Sit(GetFreeSeat());
                }*/
            }
        }

        void CreateTeacherBody() {
            object[] d = new object[] { GetAvatarData(),"teacher", -1 };
            teacherBodyFollow = PhotonNetwork.Instantiate(teacherBody.name, transform.position, transform.rotation,0, d).GetComponent<FollowVRRig>();
            foreach (var item in teacherAvatars) {
                item.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
        }

        public void SetMaleAvatar() {
            photonView.RPC("ChangeTeacherAvatar", RpcTarget.AllBuffered, "male");
            teacherBodyFollow.GetComponent<PhotonView>().RPC("SetAvatarFollow", RpcTarget.AllBuffered);
        }

        public void SetFemaleAvatar() {
            photonView.RPC("ChangeTeacherAvatar", RpcTarget.AllBuffered, "female");
            teacherBodyFollow.GetComponent<PhotonView>().RPC("SetAvatarFollow", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void ChangeTeacherAvatar(string gender) {
            if (gender == "female") {
                teacherAvatars[0].SetActive(false);
                teacherAvatars[1].SetActive(true);
                avatar = teacherAvatars[1].GetComponent<Avatar>();
            } else if (gender == "male") {
                teacherAvatars[1].SetActive(false);
                teacherAvatars[0].SetActive(true);
                avatar = teacherAvatars[0].GetComponent<Avatar>();
            }
        }

        void CreateStudentBody() {
            int sit = GetFreeSeat();
            object[] d = new object[] { GetAvatarData(), "student" , sit };
            if (mode == Mode.StudentVR) {
                PhotonNetwork.Instantiate(studentBody.name, transform.position, transform.rotation,0, d);
            } else if (mode == Mode.StudentPhone) {
                PhotonNetwork.Instantiate(studentBodyNonVR.name, transform.position, transform.rotation,0, d);
            }
            Sit(sit);
        }


        //So we stop loading scenes if we quit app
        private void OnApplicationQuit() {
            StopAllCoroutines();
        }

        //This creates an empty list of students matching the number of seats
        void InitializeStudentList() {
            if (PhotonNetwork.CurrentRoom.CustomProperties["Initialized"] == null) {
                h.Add("Initialized", actorNum);
                for (int i = 0; i < studentPositions.Length; i++) {
                    h.Add("" + i, 0);
                }
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        }

        //Gets the first sit that is free (that has a value of 0)
        int GetFreeSeat() {
            for (int i = 0; i < studentPositions.Length; i++) {
                if ((int)PhotonNetwork.CurrentRoom.CustomProperties["" + i] == 0) {
                    return i;
                }
            }
            return -1;
        }

        //Puts the student in the correspondant desk
        void Sit(int n) {
            if (n == -1) {
                Debug.LogError("No sits available");
            }
            Debug.Log("Sitting student in seat " + n);
            if (mode == Mode.StudentVR) {
                studentRig.transform.position = studentPositions[n].position + (studentPositions[n].forward * 0.4f) - (studentPositions[n].up * 0.3f);
            } else if (mode == Mode.StudentPhone) {
                studentRig.transform.position = studentPositions[n].position + (studentPositions[n].forward * 0.4f) + (studentPositions[n].up * 0.5f);
            }
            studentRig.transform.forward = -studentPositions[n].forward;
            seated = true;
            h["" + n] = actorNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        }

        //This is called when the room properties are updated, for example, when the student list is created
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            if (propertiesThatChanged.ContainsKey("Initialized") && !initialized) {
                Debug.Log("Student list initialized");
                initialized = true;
                for (int i = 0; i < studentPositions.Length; i++) {
                    Debug.Log((int)propertiesThatChanged["" + i]);
                }
                if ((mode == Mode.StudentVR || mode == Mode.StudentPhone) && !seated) {
                    Sit(GetFreeSeat());
                }
            }
        }

        //This is called when a player leaves the room, so we can free the student's place
        public override void OnPlayerLeftRoom(Player otherPlayer) {
            //get the seat number of plaer that left the room, and update room properties with the free seat (value to 0)
            if (PhotonNetwork.IsMasterClient) {
                for (int i = 0; i < studentPositions.Length; i++) {
                    if ((int)PhotonNetwork.CurrentRoom.CustomProperties["" + i] == otherPlayer.ActorNumber) {
                        h["" + i] = 0;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
                        Debug.Log("User " + otherPlayer.ActorNumber + " left room, freeing up seat " + i);
                        return;
                    }
                }
            }
        }

        //If the new master client is this client, get a copy of the room properties
        public override void OnMasterClientSwitched(Player newMasterClient) {
            base.OnMasterClientSwitched(newMasterClient);
            if (newMasterClient.ActorNumber == actorNum) {
                h = PhotonNetwork.CurrentRoom.CustomProperties;
            }
        }

        //If disconnected from server, return to lobby to reconnect.
        public override void OnDisconnected(DisconnectCause cause) {
            base.OnDisconnected(cause);
            GoToScene(0);
        }

        //Class to load scenes async
        void GoToScene(int n) {
            StartCoroutine(LoadScene(n));
        }

        IEnumerator LoadScene(int n) {
            yield return new WaitForSeconds(0.5f);

            AsyncOperation async = SceneManager.LoadSceneAsync(n);
            async.allowSceneActivation = false;

            yield return new WaitForSeconds(1);
            async.allowSceneActivation = true;
            if (n == 0) //if going back to menu destroy instance
            {
                Destroy(gameObject);
            }
        }

        int[] GetAvatarData()
        {
            int[] Data = new int[] 
            { 
                PlayerPrefs.GetInt("Gender"), 
                PlayerPrefs.GetInt("Model"), 
                PlayerPrefs.GetInt("Hair"), 
                PlayerPrefs.GetInt("Skintone"), 
                PlayerPrefs.GetInt("Chest"), 
                PlayerPrefs.GetInt("Leg"), 
                PlayerPrefs.GetInt("Feet"), 
            };
            return Data;
        }
    }
}
