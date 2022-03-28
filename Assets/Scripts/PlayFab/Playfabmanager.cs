using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class Playfabmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text messageText;
    [Header("Login UI")]
    public InputField emailInput;
    public InputField passwordInput;
    [Header("Register UI")]
    public InputField emailInput2;
    public InputField passwordInput2;
    public Toggle toggle;

    public void RegisterButton()
    {
        if (passwordInput2.text.Length < 6)
        {
            messageText.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput2.text,
            Password = passwordInput2.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void LoginButtonn()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnError(PlayFabError error)
    {
        messageText.text = "" + error.ErrorMessage;
        Debug.Log(error.Error.ToString());
        Debug.Log(error.ErrorMessage);
        Debug.Log(error.ErrorDetails);

    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";
        var request2 = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>{
                {"IsTeacher", toggle.isOn.ToString() },
                {"Hair", "0" },
                {"Face", "0" },
                {"EyeStyle", "0" },
                {"EyeColor", "0" }
            }
        };
        PlayFabClientAPI.UpdateUserData(request2, OnDataSend, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Registered and logged in!";
        Debug.Log("Succesfull login!");
        //getstat
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Succesful user data send");
    }
}
