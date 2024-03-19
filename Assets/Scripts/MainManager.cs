using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    public Text highScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        SetHighScoreText();
        GameManager.Instance.LoadHighScore();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScoreName;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Load the Json
                string path = Application.persistentDataPath + "/highscore.json";
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    SaveData data = JsonUtility.FromJson<SaveData>(json);
                }
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    // Set the high score text
    public void SetHighScoreText()
    {
        highScoreText.text = $"Best Score : {GameManager.Instance.highScoreName} : {GameManager.Instance.highScore}";
    }


    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if(GameManager.Instance.playerName != null)
        {
            if (m_Points > GameManager.Instance.highScore)
            {
                GameManager.Instance.highScore = m_Points;
                GameManager.Instance.highScoreName = GameManager.Instance.playerName;

                string path = Application.persistentDataPath + "/highscore.json";
                SaveData data = new SaveData
                {
                    highScore = GameManager.Instance.highScore,
                    highScoreName = GameManager.Instance.highScoreName
                };

                string newJson = JsonUtility.ToJson(data);
                File.WriteAllText(path, newJson);
            
            }
        }
        else
        {
            GameManager.Instance.highScore = m_Points;
            GameManager.Instance.highScoreName = "Player";
        }
    }



}
