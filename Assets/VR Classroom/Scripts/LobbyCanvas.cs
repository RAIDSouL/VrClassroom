using TMPro;
using UnityEngine;

public class LobbyCanvas : CanvasManager {
    public static LobbyCanvas instance;

    [Header("VR")]
    [SerializeField] GameObject[] toggleObj;
    [SerializeField] LaserPointer laser;
    [SerializeField] VRKeys.Keyboard vrKey;
    [SerializeField] GameObject initializeVRKeyboard;

    [Header("Android")]
    public GameObject LoginGroup, JoinGroup;
    public TMP_InputField RoomnameInput;

    //cache
    bool enableUsername = false;
    bool enablePassword = false;
    bool enableRoomname = false;

    public override void SetInstance(bool t) {
        gameObject.SetActive(t);
        if (t)
            instance = this;
    }

    public void OnLogin() {
        NetworkManager.instance.Connect();
    }

    public void JoinRoomClick() {
        NetworkManager.instance.ConnectToRoom();
    }

    private void Update() {
        if (PlatformSetting.Instance.platform.Equals(Platform.VR)) {
            if (enableUsername) {
                LoginGroup.GetComponent<Playfabmanager>().usernameInput.text = vrKey.text;
            } else if (enablePassword) {
                LoginGroup.GetComponent<Playfabmanager>().passwordInput.text = vrKey.text;
            } else if (enableRoomname) {
                RoomnameInput.text = vrKey.text;
            }

            if (vrKey.disabled) {
                VRKeyBoard(false);
            }
        }
    }

    public void CallVRUsernameInput() {
        VRKeyBoard(true);
        enableUsername = true;
        enablePassword = false;
        enableRoomname = false;
    }

    public void CallVRPasswordInput() {
        VRKeyBoard(true);
        enableUsername = false;
        enablePassword = true;
        enableRoomname = false;
    }

    public void CallVRRoomnameInput() {
        VRKeyBoard(true);
        enableUsername = false;
        enablePassword = false;
        enableRoomname = true;
    }

    public void VRKeyBoard(bool index) {
        if (index == true) {
            vrKey.text = "";
            if (!initializeVRKeyboard.activeInHierarchy) {
                initializeVRKeyboard.SetActive(true);
            }
            foreach (GameObject item in toggleObj) {
                item.transform.localScale = Vector3.zero;
            }
            laser.enabled = false;
            laser.gameObject.GetComponent<LineRenderer>().enabled = false;
        } else if (index == false) {
            if (initializeVRKeyboard.activeInHierarchy) {
                initializeVRKeyboard.SetActive(false);
            }
            foreach (GameObject item in toggleObj) {
                item.transform.localScale = Vector3.one;
            }
            laser.enabled = true;
            enableUsername = false;
            enablePassword = false;
            enableRoomname = false;
        }
    }
}
