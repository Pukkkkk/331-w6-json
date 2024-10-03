using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankUIManager : MonoBehaviour
{
    public GameObject rankDataPrefab;
    public Transform rankPanel;

    public List<PlayerData> playerDatas = new List<PlayerData>();
    public List<GameObject> createdPlayerDatas = new List<GameObject>();

    public RankData yourRankData;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateRankData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRankData()
    {
        for (int i = 0; i < playerDatas.Count; i++)
        {
            GameObject rankObj = Instantiate(rankDataPrefab,rankPanel) as GameObject;
            RankData rankData = rankObj.GetComponent<RankData>();
            rankData.playerData = new PlayerData(playerDatas[i].rankNumber 
                , playerDatas[i].playerName, playerDatas[i].playerScore, null);

            rankData.UpdateData();
            createdPlayerDatas.Add(rankObj);
        }
    }

    private void SortRankData()
    {
        
        List<PlayerData> sortRankPlayers = new List<PlayerData>();
        sortRankPlayers = playerDatas.OrderByDescending(data => data.playerScore).ToList();

        for (int i = 0;i < sortRankPlayers.Count; i++)
        {
            PlayerData changedRankNum = sortRankPlayers[i];
            changedRankNum.rankNumber = i + 1;

            sortRankPlayers[i] = changedRankNum;
        }

        playerDatas = sortRankPlayers;
       

    }

    private void ClearRankData()
    {
        foreach (GameObject createdData in createdPlayerDatas)
        {
            Destroy(createdData);
        }
        createdPlayerDatas.Clear();
    }

    [ContextMenu("Reload")]
    public void ReloadRankData()
    {
        ClearRankData();
        //SortRankData();
        CreateRankData();
    }
}
