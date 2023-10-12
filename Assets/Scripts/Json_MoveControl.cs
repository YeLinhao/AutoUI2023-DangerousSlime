using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//��������
public class PlayerData
{
    public List<PlayerDataNode> dataList = new List<PlayerDataNode>();
}
[System.Serializable]
public class PlayerDataNode
{
    public Vector2 p;
    public Vector2 v;
    public Vector2 a;
    public float t;
    public Vector2 forward;

    public PlayerDataNode(Vector2 _p, Vector2 _v, Vector2 _a, float _t, Vector2 _forward)
    {
        p = _p;
        v = _v;
        a = _a;
        t = _t;
        forward = _forward;
    }
}

//·�˳�����
public class AllData
{
    public List<Data> allDataList = new List<Data>();
}
[System.Serializable]
public class Data
{
    public float time;
    public string type;
    public List<DataNode> dataList = new List<DataNode>();
    
}
[System.Serializable]
public class DataNode
{
    public Vector2 p;
    public Vector2 v;
    public Vector2 a;

    public DataNode(Vector2 _p, Vector2 _v, Vector2 _a)
    {
        p = _p;
        v = _v;
        a = _a;
   
    }
}


public class Json_MoveControl : MonoBehaviour
{
    static AllData moveData = new AllData();
    public static int IteratorNumber = 0;

    public List<bool> IsGenerated;
    public GameObject SlimePrefab;


    //ZXZ:��ұ��������ݣ���̬���ݷ��������ļ���ȡ
    public static PlayerData playerData = new PlayerData();
    public static int playerDataI = 0;

    private void Awake()
    {
//#if UNITY_EDITOR
        string jsonStr = File.ReadAllText(Application.streamingAssetsPath + "/allData.json");//����ʷ��ķ����
//#else
  //      string jsonStr = File.ReadAllText(Application.streamingAssetsPath + "allData.json");
//#endif
        //string jsonStr = Resources.Load<TextAsset>("allData").ToString();

        moveData = JsonUtility.FromJson<AllData>(jsonStr);
        Debug.Log("MoveData:" + moveData.allDataList[0].type);
        Debug.Log(moveData.allDataList.Count.ToString());


        for (int i = 0; i < 51; i++)
        {
            IsGenerated.Add(false);
        }


        //ZXZ:������ұ���������
//#if UNITY_EDITOR
        string playerJsonStr = File.ReadAllText(Application.streamingAssetsPath+"//playerData.json");
//#else
  //      string playerJsonStr = File.ReadAllText(Application.streamingAssetsPath + "/playerData.json");
//#endif
        // string playerJsonStr = Resources.Load<TextAsset>("playerData").ToString();
        playerData = JsonUtility.FromJson<PlayerData>(playerJsonStr);
        Debug.Log("playerData:" + playerData.dataList[0].p);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < moveData.allDataList.Count; i++) //ѭ��ÿ��NPC��
        {
            
            if (moveData.allDataList[i].time <= game_manager.TotalTimer && IsGenerated[i] == false)//�ж��Ƿ�Ҫ���ɸó�
            {
                Debug.Log("�ɹ�����ѭ��");
                Debug.Log(playerDataI);
                GameObject thisSlime = Instantiate(SlimePrefab);
                IsGenerated[i] = true;

                SlimeControl thisSlimeController = (SlimeControl)thisSlime.GetComponent(typeof(SlimeControl));
                thisSlimeController.dataList = moveData.allDataList[i].dataList;
            }

        }

        //ZXZ:���԰�����ű��ҵ���Ҵ��ĳ����ϣ�����˳����ɶԳ���λ�õĻ�ȡ��ӳ�䡣�������ű�
        playerDataI++;
    }
}
