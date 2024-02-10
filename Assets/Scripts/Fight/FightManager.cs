using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//play state enum
public enum FightType
{
    None,
    Init,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}
//Battle manager
public class FightManager : MonoBehaviour
{

    public static FightManager Instance;
    public FightUnit fightUnit;
    public int MaxHp;   // Max Health
    public int CurHp;   // Current health
    public int MaxPowerCount;   // Max power
    public int CurPowerCount;   // Current power
    public int DefenseCount;    // Shield

    public void Init()
    {
        if (StartMenuUI.isEasy)
        {
            MaxHp = 20;
            CurHp = 20;
            MaxPowerCount = 15;
            CurPowerCount = 15;
        } else
        {
            MaxHp = 10;
            CurHp = 10;
            MaxPowerCount = 10;
            CurPowerCount = 10;
        }
        DefenseCount = 10;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        
    }
    //switch play state
    public void ChangeType(FightType type)
    {
        switch (type)
        {
            case FightType.None:
                break;
            case FightType.Init:
                fightUnit = new FightInit();
                break;
            case FightType.PlayerTurn:
                UIManager.Instance.ShowUI<FightUI>("FightUI");
                fightUnit = new Fight_PlayerTurn();
                break;
            case FightType.EnemyTurn:
                // UIManager.Instance.HideUI("FightUI");
                fightUnit = new Fight_EnemyTurn();
                break;
            case FightType.Win:
                fightUnit = new Fight_Win();
                break;
            case FightType.Lose:
                fightUnit = new Fight_Lose();
                break;
        }
        fightUnit.Init();// 初始化
    }
    private void Update()
    {
        if (fightUnit != null)
        {
            fightUnit.OnUpdate();
        }
    }

    //玩家受击
    public void GetPlayerHit(int hit)
    {
        //扣护盾
        if (DefenseCount > hit)
        {
            DefenseCount -= hit;
            Debug.Log("Hit");
        }
        else
        {
            hit = hit - DefenseCount;
            DefenseCount = 0;
            CurHp -= hit;
            if (CurHp <= 0)
            {
                CurHp = 0;
                //切换到游戏失败状态
                ChangeType(FightType.Lose);
            }

        }
        // 更新界面
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateDefense();
    }

}
