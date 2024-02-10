using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_Win : FightUnit
{
    public override void Init()
    {
        base.Init();
        Debug.Log("you win!");
        FightManager.Instance.StopAllCoroutines();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
