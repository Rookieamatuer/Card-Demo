using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendCard : CardItem
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (TryUse() == true)
        {
            // effect
            int val = int.Parse(data["Arg0"]);
            // Audio active
            AudioManager.Instance.PlayEffect("Effect/healspell");

            // Add shield
            FightManager.Instance.DefenseCount += val;
            // refresh text
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateDefense();
            Vector3 pos = Camera.main.transform.position;
            pos.y = 0;
            PlayEffect(pos);
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}

