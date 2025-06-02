using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class UserProfileDisplay : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text emailText;
    public TMP_Text ending1Text;
    public TMP_Text ending2Text;
    public TMP_Text ending3Text;
    public TMP_Text butterflyText;

    private string apiUrl = "https://symphony-unbound-api.vercel.app/api/user/";

    void Start()
    {
        string userId = PlayerPrefs.GetString("userId");
        StartCoroutine(GetUserData(userId));
    }

    IEnumerator GetUserData(string userId)
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl + userId);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            UserData user = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
            usernameText.text = "Username: " + user.username;
            emailText.text = "Email: " + user.email;
            ending1Text.text = "Ending 1: " + (user.ending_1 ? "Unlocked" : "Locked");
            ending2Text.text = "Ending 2: " + (user.ending_2 ? "Unlocked" : "Locked");
            ending3Text.text = "Ending 3: " + (user.ending_3 ? "Unlocked" : "Locked");
            butterflyText.text = "Butterflies: " + user.butterfly;
        }
        else
        {
            Debug.LogError("Failed to load user data: " + www.downloadHandler.text);
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
}
