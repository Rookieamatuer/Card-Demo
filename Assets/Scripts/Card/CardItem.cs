using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Dictionary<string, string> data; // Card data
    Vector2 initPos;    // Initial card position

    public void Init(Dictionary<string, string> data)
    {
        this.data = data;
    }

    private void Start()
    {
        transform.Find("bg").GetComponent<Image>().sprite = Resources.Load<Sprite>(data["BgIcon"]);
        transform.Find("bg/icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(data["Icon"]);
        transform.Find("bg/msgTxt").GetComponent<Text>().text = string.Format(data["Des"], data["Arg0"]);
        transform.Find("bg/nameTxt").GetComponent<Text>().text = data["Name"];
        transform.Find("bg/useTxt").GetComponent<Text>().text = data["Expend"];
        transform.Find("bg/Text").GetComponent<Text>().text = GameConfigManager.Instance.GetCardTypeById(data["Type"])["Name"];
    }

    // Start drag
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        initPos = transform.GetComponent<RectTransform>().anchoredPosition;
        // Audio active
        AudioManager.Instance.PlayEffect("Cards/draw");
    }

    // On drag
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out pos
        ))
        {
            transform.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    // End drag
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = initPos;
        // transform.SetSiblingIndex(index);
    }

    // Try use card
    public virtual bool TryUse()
    {
        // Cost need
        int cost = int.Parse(data["Expend"]);
        if (cost > FightManager.Instance.CurPowerCount)
        {
            // Cost lack
            AudioManager.Instance.PlayEffect("Effect/lose");
            UIManager.Instance.ShowTip("·ÑÓÃ²»×ã", Color.red);
            return false;
        }
        else
        {
            // Cut cost
            FightManager.Instance.CurPowerCount -= cost;
            // Refresh text
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdatePower();
            // Remove card
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this);
            return true;
        }
    }

    // Create effect
    public void PlayEffect(Vector3 pos)
    {
        GameObject effectobj = Instantiate(Resources.Load(data["Effects"])) as GameObject;
        effectobj.transform.position = pos;
        Destroy(effectobj, 2);
    }

}
