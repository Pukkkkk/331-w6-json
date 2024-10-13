using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using SimpleJSON;
using System.Linq;
using System.Security;

[System.Serializable]
public struct Ranking
{
    public List<PlayerData> playerDatas;
}

public class FirebaseRankingManager : MonoBehaviour
{
    public const string url = "https://testleaderboard-33a01-default-rtdb.asia-southeast1.firebasedatabase.app";
    public const string secret = "HWMVyRMPBSisFQZn1loT1MUP3s58LfVgsCfKb4MF";

    #region Test

    [Header("Test")] 
    public int testNum;
    [System.Serializable]
    public struct TestData
    {
        public int num;
        public string name;
    }

    [System.Serializable]
    public struct TestObjectData
    {
        public string name;
        public TestData testData;
    }
    public TestData testData = new TestData();
    public TestObjectData testObjectData = new TestObjectData();

    public void TestSetData()
    {
        string urlData = $"{url}/TestData.json?auth={secret}";
        
        testData.name = "AAA";
        testData.num = 1;
        
        RestClient.Put<TestData>(urlData, testData).Then(response =>
        {
            Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log("error no set to server");
            Debug.Log(error.Message);
        });
    }
    
    public void TestSetData2()
    {
        string urlData = $"{url}/TestData.json?auth={secret}";
        
        testData.name = "BBB";
        testData.num = 2;
        
        RestClient.Put<TestData>(urlData, testData).Then(response =>
        {
            Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log("error no set to server");
            Debug.Log(error.Message);
        });
    }

    public void TestGetData()
    {
        string urlData = $"{url}/TestData.json?auth={secret}";

        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);
            testNum = jsonNode["num"];
        }).Catch(error =>
        {
            Debug.Log("error");
        });
    }

    #endregion

    [Header("Main")] 
    public RankUIManager rankUIManager;
    public Ranking ranking;

    [Header("New Data")]
    public PlayerData currentPlayerData;
    private List<PlayerData> sortPlayerDatas = new List<PlayerData>();


    // Start is called before the first frame update
    void Start()
    {
        ReloadSortingData();
    }

    [ContextMenu("Set Local Data to Database")]
    public void SetLocalDataToDatabase()
    {
        string urlData = $"{url}/ranking.json?auth={secret}";
        RestClient.Put<Ranking>(urlData, ranking).Then(response =>
        {
            Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log("Error to set ranking data to server");
        });
    }
    
    private void CalculateSortRankData()
    {
        List<PlayerData> sortRankPlayers = new List<PlayerData>();
        sortRankPlayers = ranking.playerDatas.OrderByDescending(data => data.playerScore).ToList();

        for (int i = 0;i < sortRankPlayers.Count; i++)
        {
            PlayerData changedRankNum = sortRankPlayers[i];
            changedRankNum.rankNumber = i + 1;

            sortRankPlayers[i] = changedRankNum;
        }
        ranking.playerDatas = sortRankPlayers;
    }

    public void FindYourDataInRanking()
    {
        rankUIManager.yourRankData.playerData = ranking.playerDatas
            .Where(data => data.playerName == currentPlayerData.playerName).FirstOrDefault();
        rankUIManager.yourRankData.UpdateData();
    }

    public void ReloadSortingData()
    {
        string urlData = $"{url}/ranking/playerDatas.json>auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            ranking = new Ranking();
            ranking.playerDatas = new List<PlayerData>();
            for (int i = 0; i < jsonNode.Count; i++)
            {
                ranking.playerDatas.Add(new PlayerData(
                    jsonNode[i]["rankNumber"],
                    jsonNode[i]["plyerName"],
                    jsonNode[i]["playerScore"],
                    null));
            }
            CalculateSortRankData();

            string urlPlayerData = $"{url}/ranking/.json?auth={secret}";

            RestClient.Put<Ranking>(urlPlayerData, ranking).Then(response =>
            {
                Debug.Log("Upload Data Complete");
                rankUIManager.playerDatas = ranking.playerDatas;
                rankUIManager.ReloadRankData();
                FindYourDataInRanking();
            }).Catch(error =>
            {
                Debug.Log("error on set to server");
            });
        }).Catch(error =>
        {
            Debug.Log("Error to get data from server");
        });
    }

    public void AddDataWithSorting()
    {
        string urlData = $"{url}/ranking/playerDatas.json?auth={secret}";

        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            ranking = new Ranking();
            ranking.playerDatas = new List<PlayerData>();
            for(int i = 0; i < jsonNode.Count; i++)
            {
                ranking.playerDatas.Add(new PlayerData(jsonNode[i]["rankNumber"], jsonNode[i]["playerName"], jsonNode[i]["playerScore"], null));
            }
        });
    }
}
