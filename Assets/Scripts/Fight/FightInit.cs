using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInit : FightUnit
{
    public override void Init()
    {
        base.Init();
        // Switch bgm
        AudioManager.Instance.PlayBGM("battle", true);
        // Show battle UI
        UIManager.Instance.ShowUI<FightUI>("FightUI");
        // Initialize level 1 enemy
        EnemyManeger.Instance.LoadRes("10003");
        // Initialze cards
        FightCardManager.Instance.Init();
        // Initialize status
        FightManager.Instance.Init();
        // Switch to player turn
        FightManager.Instance.ChangeType(FightType.PlayerTurn);



    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
