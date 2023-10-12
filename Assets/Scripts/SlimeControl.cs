using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeControl : MonoBehaviour
{
    public List<DataNode> dataList = new List<DataNode>();
    
    public int Rank = 3;
   
    public Vector2 p;
    public Vector2 v;
    public Vector2 a;

    int i = 0;
    bool IsStayingRank3 = false;
    private void FixedUpdate()
    {

  

        Vector2 a_opposite = a - Json_MoveControl.playerData.dataList[Json_MoveControl.playerDataI].a;
        Vector2 p_opposite = p - Json_MoveControl.playerData.dataList[Json_MoveControl.playerDataI].p;
        Vector2 v_opposite = v - Json_MoveControl.playerData.dataList[Json_MoveControl.playerDataI].v;



        float RankScore = Vector2.Dot(a_opposite, p_opposite.normalized); 
            
        //Debug.Log("�ȼ�����Ϊ��" + RankScore);

        Rank = CalculateRank(RankScore, Rank);
        UpdateRank(Rank); //����Σ�յȼ� SetActive��Ӧ������ 


        p = dataList[i].p;
        v = dataList[i].v;
        a = dataList[i].a;

        if (i >= dataList.Count - 1) //����NPC����
        {
            Debug.Log("����NPC������");
            Destroy(gameObject);
        }
       

        UpdateCoord(coordTransfer());
        i = i + 1;
        

    }
    public int CalculateRank(float RankScore , int lastRank)
    {
        int rank = 1;

        if (IsStayingRank3)//���Э�̣�ʹΣ������ĺ�ɫʷ��ķ�����ϳ�ʱ��
        {
            rank = 3;
        }
        else if (IsStayingRank3 == false)
        {
            switch (lastRank)
            {
                case 1:

                    if (RankScore < 1 || RankScore > 500)
                    {
                        rank = 1;
                    }
                    else if (RankScore >= 1 && RankScore <= 10)
                    {
                        rank = 2;
                    }
                    else if (RankScore >= 8)
                    {
                        StartCoroutine(StayRank3());
                    }

                    break;

                case 2:
                    if (RankScore < 0)
                    {
                        rank = 1;
                    }
                    else if ((RankScore >= 0 && RankScore <= 10) || RankScore > 500)
                    {
                        rank = 2;
                    }
                    else if (RankScore >= 8)
                    {
                        StartCoroutine(StayRank3());
                    }
                    break;

                case 3:

                    if (RankScore < 0)
                    {
                        rank = 1;
                    }
                    else if (RankScore >= 0 && RankScore <= 10)
                    {
                        rank = 2;
                    }
                    else if (RankScore >= 10)
                    {
                        rank = 3;
                    }


                    break;


                default:
                    break;
            }
        }
 

        return rank;
    }

    IEnumerator StayRank3()
    {
        IsStayingRank3 = true;
        yield return new WaitForSeconds(1.0f);
        IsStayingRank3 = false;
    }
    public void UpdateRank(int Rank)
    {
        if (Rank == 1)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }

        else if (Rank == 2)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (Rank == 3)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    //ZXZ:
    //������������ҳ���������
    public Vector2 coordTransfer()
    {
        //��ȡ��ҳ�����λ��
        Vector2 pp = Json_MoveControl.playerData.dataList[Json_MoveControl.playerDataI].p;
        Vector2 fd = Json_MoveControl.playerData.dataList[Json_MoveControl.playerDataI].forward;
        float dy = pp.y - p.y;//Vector2.Dot(p - pp, fd);//fdΪ��λ�����������һ��
        //float angle = Mathf.Acos(dy / (p - pp).magnitude);//p-pp��fd�н�
        float dx = -39.455f - p.x;//(p-pp).magnitude*Mathf.Sin(angle);//����������Ҫ����s
        //float xsituation = (p - pp).x * fd.y - (p - pp).y * fd.x;
        //if (xsituation < 0)
        //{
        //    dx *= (-1);
        //}

        return new Vector2(dx, dy);
    }
    public void UpdateCoord(Vector2 coord)
    {
         gameObject.transform.position = new Vector2(coord.x  , coord.y / 5) ;   
    }
}