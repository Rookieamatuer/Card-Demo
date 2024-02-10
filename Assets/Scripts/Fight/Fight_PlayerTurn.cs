using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.ShowTip("Your turn", Color.green, delegate (){
            // Update power
            if (StartMenuUI.isEasy)
            {
                FightManager.Instance.CurPowerCount = 5;
            } else
            {
                FightManager.Instance.CurPowerCount = 3;
            }
                
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdatePower();
            // No card
            if (FightCardManager.Instance.HasCard() == false)
            {

                FightCardManager.Instance.Init();
                // Update discard number
                UIManager.Instance.GetUI<FightUI>("FightUI").UpdateUsedCardCount();
            }
            Debug.Log("draw");
            if (StartMenuUI.isEasy)
            {
                UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(4);
            }
            else { 
                UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(3); 
            }     // Draw 4
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();   // Update card position
            // Update card number
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardCount();
        });

        Debug.Log("player round");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
