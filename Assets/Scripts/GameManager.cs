using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI highScoreText;
    public string playerName;
    public int highScore = 0;
    public string highScoreName;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        LoadHighScore();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(this.gameObject);
    }

    public void ExitGame()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
        Application.Quit();
        }
    }

    // Save the player name from the input field
    public void SavePlayerName()
    {
        playerName = playerNameInput.text;
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScoreName;
    }

    // Load the high score from the file
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        Debug.Log(path);
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
            highScoreName = data.highScoreName;

            highScoreText.text = $"Best Score : {highScoreName} : {highScore}";
        }
    }
}
