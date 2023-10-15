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
        MaxHp = 10;
        CurHp = 10;
        MaxPowerCount = 10;
        CurPowerCount = 10;
        DefenseCount = 10;
    }

    private void Awake()
    {

        Instance = this;
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
                fightUnit = new Fight_PlayerTurn();
                break;
            case FightType.EnemyTurn:
                fightUnit = new Fight_EnemyTurn();
                break;
            case FightType.Win:
                fightUnit = new Fight_Win();
                break;
            case FightType.Lose:
                fightUnit = new Fight_Lose();
                break;
        }
        fightUnit.Init();// ≥ı ºªØ
    }
    private void Update()
    {
        if (fightUnit != null)
        {
            fightUnit.OnUpdate();
        }
    }

}
