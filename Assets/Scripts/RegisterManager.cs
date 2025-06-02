using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class RegisterManager : MonoBehaviour
{
    [Header("Register UI")]
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;

    public TMP_Text statusText;

    private string registerUrl = "https://symphony-unbound-api.vercel.app/api/register";

    public void OnRegisterButtonClicked()
    {
        StartCoroutine(Register(registerUsername.text, registerEmail.text, registerPassword.text));
    }

    IEnumerator Register(string username, string email, string password)
    {
        statusText.text = "Registering...";
        
        string json = JsonUtility.ToJson(new RegisterRequest(username, email, password));
        UnityWebRequest www = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            statusText.text = "Register success!";
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Login");
        }
        else
        {
            statusText.text = $"Register failed: {www.downloadHandler.text}";
        }
    }

    // Helper class untuk JSON
    [System.Serializable]
    public class RegisterRequest
    {
        public string username;
        public string email;
        public string password;

        public RegisterRequest(string u, string e, string p)
        {
            username = u;
            email = e;
            password = p;
        }
    }

}
