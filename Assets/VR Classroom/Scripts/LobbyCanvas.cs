using Photon.Pun;
using Photon.Realtime;
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

    bool enableRegUsername = false;
    bool enableRegEmail = false;
    bool enableRegPassWord = false;
    bool enableRegPassWordC = false;

    public override void SetInstance(bool t) {
        
        if (t)
        {
            instance = this;
            if (UserData.Username != null)
            {
                LoginGroup.SetActive(false);
                LoginGroup.GetComponent<Playfabmanager>().hasCharacterSave = true;
                OnLogin();
            }
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
            
    }

    public void OnLogin() {
        NetworkManager.instance.Connect();
    }

    public void JoinRoomClick() {     
        NetworkManager.instance.ConnectToRoom();     
    }

    private void Update() {
        if (PlatformSetting.Instance.platform.Equals(Platform.VR)) {
            if (enableUsername) 
            {
                LoginGroup.GetComponent<Playfabmanager>().usernameInput.text = vrKey.text;
            }
            else if (enablePassword) 
            {
                LoginGroup.GetComponent<Playfabmanager>().passwordInput.text = vrKey.text;
            } 
            else if (enableRoomname) 
            {
                RoomnameInput.text = vrKey.text;
            }
            else if (enableRegUsername)//reg
            {
                LoginGroup.GetComponent<Playfabmanager>().username2.text = vrKey.text;
            }
            else if (enableRegEmail)
            {
                LoginGroup.GetComponent<Playfabmanager>().emailInput2.text = vrKey.text;
            }
            else if (enableRegPassWord)
            {
                LoginGroup.GetComponent<Playfabmanager>().passwordInput2.text = vrKey.text;
            }
            else if (enableRegPassWordC)
            {
                LoginGroup.GetComponent<Playfabmanager>().cfpasswordInput2.text = vrKey.text;
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

    public void CallVRRegisterInput(int index)
    {
        enableRegUsername = false;
        enableRegEmail = false;
        enableRegPassWord = false;
        enableRegPassWordC = false;
        VRKeyBoard(true);
        switch (index) 
        {
            case 1: enableRegUsername = true; break;
            case 2: enableRegEmail = true; break;
            case 3: enableRegPassWord = true; break;
            case 4: enableRegPassWordC = true; break;
        }
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
        } 
        else if (index == false) {
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
            enableRegUsername = false;
            enableRegEmail = false;
            enableRegPassWord = false;
            enableRegPassWordC = false;
        }
    }

    public void _reloadScene() 
    {
        if (PlatformSetting.Instance.platform == Platform.ANDROID) 
        {
            gameObject.transform.Find("PlayFab").gameObject.SetActive(true);
            gameObject.transform.Find("Join").gameObject.SetActive(false);
            GameObject.Find("TKVRBoyA(Clone)").gameObject.SetActive(false);
            GameObject.Find("TKVRGirlA(Clone)").gameObject.SetActive(false);
            
        }
        else if (PlatformSetting.Instance.platform == Platform.VR)
        {
            gameObject.transform.Find("PlayFab").gameObject.SetActive(true);
            gameObject.transform.Find("Join").gameObject.SetActive(false);
            GameObject.Find("TKVRBoyA(Clone)").gameObject.SetActive(false);
            GameObject.Find("TKVRGirlA(Clone)").gameObject.SetActive(false);
        }

    }
}
