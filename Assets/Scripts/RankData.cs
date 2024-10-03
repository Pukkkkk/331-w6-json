using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct PlayerData
{
    public string playerName;    
    public int rankNumber;    
    public int playerScore;
    public Sprite profileSprite;

    public PlayerData(int rankNumber, string playerName, int playerScore, Sprite profileSprite)
    {
        this.playerName = playerName;
        this.rankNumber = rankNumber;
        this.playerScore = playerScore;
        this.profileSprite = profileSprite;
    }
    
}

public class RankData : MonoBehaviour
{
    public PlayerData playerData;
    
    [SerializeField] private Image profileImg;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateData()
    {
        profileImg.sprite = playerData.profileSprite;
        rankText.text = playerData.rankNumber.ToString();
        playerNameText.text = playerData.playerName;
        scoreText.text = playerData.playerScore.ToString("0");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
