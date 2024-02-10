using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_Lose : FightUnit
{
    public override void Init()
    {
        base.Init();
        Debug.Log("you lose!");
        FightManager.Instance.StopAllCoroutines();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
