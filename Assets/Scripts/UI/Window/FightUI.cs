using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

//战斗界面
public class FightUI : UIBase
{
    private Text cardCountTxt;      // Cards number count
    private Text noCardCountTxt;    // Discard cards number
    private Text powerTxt;
    private Text hpTxt;
    private Image hpImg;
    private Text fyTxt;             // Shield data
    private List<CardItem> cardItemList;    // Card list

    private void Awake()
    {
        cardCountTxt = transform.Find("hasCard/icon/Text").GetComponent<Text>();
        noCardCountTxt = transform.Find("noCard/icon/Text").GetComponent<Text>();
        powerTxt = transform.Find("mana/Text").GetComponent<Text>();
        hpTxt = transform.Find("hp/healthText").GetComponent<Text>();
        hpImg = transform.Find("hp/fill").GetComponent<Image>();
        fyTxt = transform.Find("hp/shield/Text").GetComponent<Text>();
        cardItemList = new List<CardItem>();
        // Turn end button
        transform.Find("turnBtn").GetComponent<Button>().onClick.AddListener(onChangeTurnBtn);
    }

    private void Start()
    {
        UpdateHp();
        UpdatePower();
        UpdateDefense();
        UpdateCardCount();
        UpdateUsedCardCount();
    }

    // Update health
    public void UpdateHp()
    {
        hpTxt.text = FightManager.Instance.CurHp + "/" + FightManager.Instance.MaxHp;
        hpImg.fillAmount = (float)FightManager.Instance.CurHp / (float)FightManager.Instance.MaxHp;
    }

    // Update energy
    public void UpdatePower()
    {
        powerTxt.text = FightManager.Instance.CurPowerCount + "/" + FightManager.Instance.MaxPowerCount;
    }

    // Update shield data
    public void UpdateDefense()
    {
        fyTxt.text = FightManager.Instance.DefenseCount.ToString();
    }
    // Update cards number
    public void UpdateCardCount()
    {

        cardCountTxt.text = FightCardManager.Instance.cardList.Count.ToString();
    }
    // Update discard cards number
    public void UpdateUsedCardCount()
    {

        noCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
    }

    // Create card item
    public void CreateCardItem(int count)
    {
        if (count > FightCardManager.Instance.cardList.Count)
        {
            count = FightCardManager.Instance.cardList.Count;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, -700);
            // var item = obj.AddComponent<CardItem>();
            string cardId = FightCardManager.Instance.DrawCard();
            Dictionary<string, string> data = GameConfigManager.Instance.GetCardById(cardId);
            CardItem item = obj.AddComponent(System.Type.GetType(data["Script"])) as CardItem;
            // CardItem item = obj.AddComponent(System.Type.GetType("DefendCard")) as CardItem;
            item.Init(data);
            cardItemList.Add(item);
        }
    }

    // Update card position
    public void UpdateCardItemPos()
    {

        float offset = 800f / cardItemList.Count;
        Vector2 startPos = new Vector2(-cardItemList.Count / 2f * offset + offset * 0.5f, -500);
        for (int i = 0; i < cardItemList.Count; i++)
        {
            cardItemList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.5f);
            startPos.x = startPos.x + offset;
        }
    }

    // Remove card
    public void RemoveCard(CardItem item)
    {
        AudioManager.Instance.PlayEffect("Cards/cardShove");    // Remove audio
        item.enabled = false;
        FightCardManager.Instance.usedCardList.Add(item.data["Id"]);
        // Refresh card number
        noCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
        // Remove from list
        cardItemList.Remove(item);
        // Refresh card position
        UpdateCardItemPos();
        // Discard card
        item.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, -700), 0.25f);
        item.transform.DOScale(0, 0.25f);
        Destroy(item.gameObject, 1);
    }

    //玩家回合结束，切换到敌人回合
    private void onChangeTurnBtn()
    {
        //只有玩家回合才能切换
        if (FightManager.Instance.fightUnit is Fight_PlayerTurn) FightManager.Instance.ChangeType(FightType.EnemyTurn);
    }

    //删除所有卡牌
    public void RemoveAllCards()
    {
        for (int i = cardItemList.Count - 1; i > 0; i--)
        {
            RemoveCard(cardItemList[i]);
        }
    }

}
