using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyTurn : FightUnit
{
    public override void Init()
    {
        base.Init();
        //ɾ�����п���
        UIManager.Instance.GetUI<FightUI>("FightUI").RemoveAllCards();
        //��ʾ���˻غ���ʾ
        UIManager.Instance.ShowTip("���˻غ�", Color.red, delegate ()
        {
            FightManager.Instance.StartCoroutine(EnemyManeger.Instance.DoAllEnemyAction());
            Debug.Log("ִ�е���ai");
        });
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
