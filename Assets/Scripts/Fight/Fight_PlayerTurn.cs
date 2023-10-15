using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.ShowTip("Your turn", Color.green, delegate (){ 
            Debug.Log("draw");
            UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(4);//��4��
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();//���¿���λ��
        });

        Debug.Log("player round");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
