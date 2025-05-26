using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;
using TMPro;

public class SignUpFormHandler : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField reenterPasswordInput;
    public Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    void OnSubmit()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string rePassword = reenterPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
        {
            Debug.LogWarning("All fields are required!");
            return;
        }

        if (password != rePassword)
        {
            Debug.LogWarning("Passwords do not match!");
            return;
        }

        SignUpData signUpData = new SignUpData
        {
            email = email,
            password = password,
            createDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")
        };

        string jsonData = JsonUtility.ToJson(signUpData);
        Debug.Log("JSON to Send: " + jsonData);

        StartCoroutine(SendSignUpData(jsonData));
    }

    IEnumerator SendSignUpData(string json)
    {
        string url = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sign Up Success: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Sign Up Failed: " + request.error);
        }
    }
}
