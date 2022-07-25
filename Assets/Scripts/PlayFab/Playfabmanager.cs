using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class Playfabmanager : MonoBehaviour
{
    public static Playfabmanager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    public GameObject Login, Register,RegisterType;
    public Text messageText;
    public Text registerMessageText;
    [Header("Login UI")]
    public InputField usernameInput;
    public InputField passwordInput;
    [Header("Register UI")]
    public InputField username2;
    public InputField emailInput2;
    public InputField passwordInput2;
    public Toggle toggle;

    //cache
    bool isTeacher;
    // hasSave
    public bool hasCharacterSave;
    public void RegisterButton()
    {
        if (passwordInput2.text.Length < 6)
        {
            registerMessageText.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Username = username2.text,
            Email = emailInput2.text,
            Password = passwordInput2.text,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    public void setTeacher(bool input) { isTeacher = input; }
    public bool getTeacher() { return isTeacher; }
    public void LoginButtonn()
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

       if(usernameInput.text==""|| usernameInput.text==null)
        {
            request = new LoginWithPlayFabRequest
            {
                //Username = "Student2",
                //Password = "Student2"
                Username = "teacher02",
                Password = "teacher02"
            };
        }
////???????????????????????????????????????
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);
    }

    private void OnError(PlayFabError error)
    {
        messageText.text = "" + error.ErrorMessage;
        registerMessageText.text = "" + error.ErrorMessage;
        Debug.Log(error.Error.ToString());
        Debug.Log(error.ErrorMessage);
        Debug.Log(error.ErrorDetails);

    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        registerMessageText.text = "Registered and logged in!";
        var request2 = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>{
                {"IsTeacher",isTeacher.ToString() },
             //   {"IsTeacher", toggle.isOn.ToString() },
                {"Model" , "0"},
                {"Hair", "0" },
                {"Skintone", "0" },
                {"Chest", "0" },
                { "Leg", "0" },
                { "Feet", "0" }
            }
        };
        isTeacher = toggle.isOn;
        PlayFabClientAPI.UpdateUserData(request2, OnDataSend, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "logged in!";
        Debug.Log("Succesfull login!");
        GetAppearance();
        //getstat
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        LobbyCanvas.instance.OnLogin();
    }

    public void ToggleInputType()
    {
        if (this.passwordInput2 != null)
        {
            if (this.passwordInput2.contentType == InputField.ContentType.Password)
            {
                this.passwordInput2.contentType = InputField.ContentType.Standard;
            }
            else
            {
                this.passwordInput2.contentType = InputField.ContentType.Password;
            }

            this.passwordInput2.ForceLabelUpdate();
        }
    }

    public void CheckIfTeacher()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnLoadTeacherComplete, OnError);
    }

    private void OnLoadTeacherComplete(GetUserDataResult result)
    {
        isTeacher = result.Data["IsTeacher"].Value == "True";
        NetworkManager.instance.LoadAfterGetUserData(isTeacher);
        Debug.Log("isTeacher = " + isTeacher);
    }

    public bool GetTeacherValue()
    {
        return isTeacher;
    }

    public void PlayFabSaveAvatar(int boyPrefIndex, BoyVRTKPrefabMaker boy)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>{
                {"Gender", "0" },
                {"Model" , boyPrefIndex.ToString()},
                {"Hair", boy.Hair.ToString() },
                {"Skintone", boy.Skintone.ToString() },
                {"Chest", boy.Chest.ToString() }
                //,
                //{ "Leg", boy.Legs.ToString() },
                //{ "Feet", boy.Feet.ToString() }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSave, OnError);
    }

    public void PlayFabSaveAvatar(int GirlPrefIndex, GirlVRTKPrefabMaker girl)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>{
                {"Gender", "1" },
                {"Model" , GirlPrefIndex.ToString()},
                {"Hair", girl.Hair.ToString() },
                {"Skintone", girl.Skintone.ToString() },
                {"Chest", girl.Chest.ToString() }
                //,
                //{ "Leg", girl.Legs.ToString() },
                //{ "Feet", girl.Feet.ToString() }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSave, OnError);
    }
    void OnDataSave(UpdateUserDataResult result)
    {
        Debug.Log("Done Save avatar to playfab");
    }

    void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, (callback) => LobbyCanvas.instance.OnLogin());
    }

    void OnDataRecieved(GetUserDataResult result)
    {
        Debug.Log("Get Data!");
        if (result.Data != null && result.Data.ContainsKey("Gender")) {
            PlayerPrefs.SetInt("Gender", int.Parse(result.Data["Gender"].Value));
            PlayerPrefs.SetInt("Model", int.Parse(result.Data["Model"].Value));
            PlayerPrefs.SetInt("Hair", int.Parse(result.Data["Hair"].Value));
            PlayerPrefs.SetInt("Skintone", int.Parse(result.Data["Skintone"].Value));
            PlayerPrefs.SetInt("Chest", int.Parse(result.Data["Chest"].Value));
            PlayerPrefs.SetInt("Leg", int.Parse(result.Data["Leg"].Value));
            PlayerPrefs.SetInt("Feet", int.Parse(result.Data["Feet"].Value));
            CharecterEditor._instance.CallOldSaveModel();
            hasCharacterSave = true;
        }
        else // create first model ????????
            hasCharacterSave = true;
        LobbyCanvas.instance.OnLogin();
    }
    

}
