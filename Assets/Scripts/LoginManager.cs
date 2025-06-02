using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{

    [Header("Login UI")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    public TMP_Text statusText;
    private string loginUrl = "https://symphony-unbound-api.vercel.app/api/login";

    public void OnLoginButtonClicked()
    {
        StartCoroutine(Login(loginEmail.text, loginPassword.text));
    }

    IEnumerator Login(string email, string password)
    {
        statusText.text = "Logging in...";

        string json = JsonUtility.ToJson(new LoginRequest(email, password));
        UnityWebRequest www = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            statusText.text = "Login success!";
            var response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
            PlayerPrefs.SetString("userId", response.user._id);
            SceneManager.LoadScene("MainMenu");

        }
        else
        {
            statusText.text = $"Login failed: {www.downloadHandler.text}";
        }
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;

        public LoginRequest(string e, string p)
        {
            email = e;
            password = p;
        }
    }
    
    [System.Serializable]
    public class UserData
    {
        public string _id;
        public string username;
        public string email;
        public bool ending_1;
        public bool ending_2;
        public bool ending_3;
        public int butterfly;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string token;
        public UserData user;
    }

}
