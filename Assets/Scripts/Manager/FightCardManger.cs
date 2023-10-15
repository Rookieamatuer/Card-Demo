using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Battle cards manager
public class FightCardManager
{
    public static FightCardManager Instance = new FightCardManager();
    public List<string> cardList;   // Card list
    public List<string> usedCardList;   // Discard card list
    // Initialize
    public void Init()
    {
        cardList = new List<string>();
        usedCardList = new List<string>();
        // Temp list ( used to keep player's cards )
        List<string> tempList = new List<string>();
        tempList.AddRange(RoleManager.Instance.cardList);
        // Randomly add to card list
        while (tempList.Count > 0)
        {
            int tempIndex = Random.Range(0, tempList.Count);
            
            cardList.Add(tempList[tempIndex]);
            
            tempList.RemoveAt(tempIndex);
        }
        Debug.Log(cardList.Count);
    }

    // Has card
    public bool HasCard()
    {
        return cardList.Count > 0;
    }
    // Draw card
    public string DrawCard()
    {
        string id = cardList[cardList.Count - 1];
        cardList.RemoveAt(cardList.Count - 1);
        return id;
    }


}
