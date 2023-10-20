一、进入战斗

FightInit.cs：

调用Init初始环境，加载1)bgm，2)战斗UI，3)关卡，4)初始手牌，5)玩家状态6）切换玩家回合

1.Bgm由audiomanager控制（选）

2.UIManager调用show<**FightUI**>:战斗UI包括血条、盾量和费用条

```c#
// Show battle UI
UIManager.Instance.ShowUI<FightUI>("FightUI");

// Method ShowUI in UIManager
public UIBase ShowUI<T>(string uiName) where T : UIBase 
{
    UIBase ui = Find(uiName);
    if (ui == null)
    {
        // Load from the Resource/UI folder
        GameObject go = Instantiate(Resources.Load("UI/" + uiName), canvasTransform) as GameObject;    // Clone GameObject
        // Change name
        go.name = uiName;
        // Add scripts
        ui = go.AddComponent<T>();
        // Save to list
        uiList.Add(ui);
    }
    else
    {
        // Show
        ui.Show();
    }
    return ui;
}


```

所有UI继承**UIBase**，UIBase包含注册(开启鼠标监听UIEventTrigger)、显示、隐藏、关闭方法

```c#
public UIEventTrigger Register(string uiName)
{
    Transform tf = transform.Find(uiName);
    return UIEventTrigger.Get(tf.gameObject);
}
public virtual void Show()    // Show
{
    gameObject.SetActive(true);
}

public virtual void Hide()    // Hide
{
    gameObject.SetActive(false);
}

public virtual void Close()    // Close(Destroy)
{
    Debug.Log(gameObject.name);
    UIManager.Instance.CloseUI(gameObject.name);
}
```

3.加载怪物

```c#
public void LoadRes(string id)
{
    enemyList = new List<Enemy>();
    /* 
     *  Id  Name    	    EnemyIds    	        Pos	
     *  Id  关卡名称	    敌人Id的数组  	    所有怪物的位置	
     *  10003	3	    10001=10002=10003	3,0,1=0,0,1=-3,0,1	
     */
    // Load level
    Dictionary<string, string> levelData = GameConfigManager.Instance.GetLevelById(id);
    // Splite string, get enemy id
    string[] enemyIds = levelData["EnemyIds"].Split('=');

    string[] enemyPos = levelData["Pos"].Split('=');// Enemy position
    for (int i = 0; i < enemyIds.Length; i++)
    {
        string enemyId = enemyIds[i];
        string[] posArr = enemyPos[i].Split(',');

        float x = float.Parse(posArr[0]);
        float y = float.Parse(posArr[1]);
        float z = float.Parse(posArr[2]);
        // Get enemy by id
        Dictionary<string, string> enemyData = GameConfigManager.Instance.GetEnemyById(enemyId);
        GameObject obj = Object.Instantiate(Resources.Load(enemyData["Model"])) as GameObject;  // Load enemy

        Enemy enemy = obj.AddComponent<Enemy>();    // Add enemy scripts
        enemy.Init(enemyData);  // Initialize enemy
        enemyList.Add(enemy);   // Add to list

        obj.transform.position = new Vector3(x, y, z);
    }
}
```

4.加载牌库**FightCardManager**

Init()方法，从RoleManager(该类存入了所有卡牌)加载所有牌并随即排序存入牌库，抽牌按照该顺序执行

HasCard()方法，判断牌库中是否还有牌

Draw Card()方法，抽牌

```c#
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
```

5.加载玩家状态**FightManager**

Init()方法，初始化玩家血量，费用状态

ChangeType()方法，切换游戏状态，包括：无动作、初始化(调用FightInit)、玩家回合、怪物回合、胜利和失败（所有状态均继承FightUnit）

GetPlayerHit()方法，更新玩家血量和盾量

更新玩家状态UI

```c#
//玩家受击
public void GetPlayerHit(int hit)
{
    //扣护盾
    if (DefenseCount > hit)
    {
        DefenseCount -= hit;
        Debug.Log("Hit");
    }
    else
    {
        hit = hit - DefenseCount;
        DefenseCount = 0;
        CurHp -= hit;
        if (CurHp <= 0)
        {
            CurHp = 0;
            //切换到游戏失败状态
            ChangeType(FightType.Lose);
        }

    }
    // 更新界面
    UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
    UIManager.Instance.GetUI<FightUI>("FightUI").UpdateDefense();
}
```

6.切换到玩家回合

```c#
// Switch to player turn
FightManager.Instance.ChangeType(FightType.PlayerTurn);
```

二、开始战斗

玩家回合：

初始化玩家费用和UI显示，创建卡牌（4张）并显示

```c#
// Class FightUI
public override void Init()
{
    base.Init();
    UIManager.Instance.ShowTip("Your turn", Color.green, delegate (){
        // Update power
        FightManager.Instance.CurPowerCount = 3;
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdatePower();
        // No card
        if (FightCardManager.Instance.HasCard() == false)
        {

            FightCardManager.Instance.Init();
            // Update discard number
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateUsedCardCount();
        }
        Debug.Log("draw");
        UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(4);     // Draw 4
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();   // Update card position
        // Update card number
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardCount();
    });
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
```

根据卡牌采取不同行动（所有卡牌继承CardItem，该类包含开始拖拽，正在拖拽，结束拖拽三种状态方法，卡牌使用方法tryuse，根据费用判断该卡是否使用）

```c#
// Class CardItem
public virtual bool TryUse()
{
    // Cost need
    int cost = int.Parse(data["Expend"]);
    if (cost > FightManager.Instance.CurPowerCount)
    {
        // Cost lack
        AudioManager.Instance.PlayEffect("Effect/lose");
        UIManager.Instance.ShowTip("费用不足", Color.red);
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
```

Defend：结束拖拽后更新玩家盾量

```c#
// Class DefendCard
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
```

AttackItem：新增方法OnPointerDown，检测到鼠标点击事件后激活选中敌人曲线UI，OnMouseDownRight判断右键点击取消选择；

```c#
// Class AttackCardItem
public void OnPointerDown(PointerEventData eventData)
{
    // Audio active
    AudioManager.Instance.PlayEffect("Cards/draw");
    // Set line UI
    UIManager.Instance.ShowUI<LineUI>("LineUI");
    // Set start position
    UIManager.Instance.GetUI<LineUI>("LineUI").SetStartPos(transform.GetComponent<RectTransform>().anchoredPosition);
    // Mouse invisible
    Cursor.visible = false;
    // Stop all coroutine
    StopAllCoroutines();
    // Start mouse coroutine
    StartCoroutine(OnMouseDownRight(eventData));

}

IEnumerator OnMouseDownRight(PointerEventData pData)
{
    while (true)
    {
        // Mouse key right down to out loop
        if (Input.GetMouseButton(1)) { Debug.Log("right"); break; }
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            pData.position,
            pData.pressEventCamera,
            out pos
        ))
        {
            // Set line position
            UIManager.Instance.GetUI<LineUI>("LineUI").SetEndPos(pos);
            // Check enemy
            CheckRayToEnemy();
        }

        yield return null;
    }
    Cursor.visible = true;
    UIManager.Instance.CloseUI("LineUI");
}
```

CheckRayToEnemy方法通过射线检测判断是否选中敌人，判断选中后执行tryuse，成功使用后根据效果更新玩家状态

```c#
// Class CardItem
private void CheckRayToEnemy()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy")))
    {
        hitEnemy = hit.transform.GetComponent<Enemy>();
        hitEnemy.OnSelect();
        if (Input.GetMouseButtonDown(0))
        {
            // Stop all routine
            StopAllCoroutines();
            // Mouse visible
            Cursor.visible = true;
            // Close line UI
            UIManager.Instance.CloseUI("LineUI");
            if (TryUse() == true)
            {
                // Audio active
                PlayEffect(hitEnemy.transform.position);
                // Hit effect
                AudioManager.Instance.PlayEffect("Effect/sword");
                // Hit enemy
                int val = int.Parse(data["Arg0"]);
                hitEnemy.Hit(val);
            }
            // Enemy on unselect
            hitEnemy.OnUnSelect();
            
            hitEnemy = null;
        }
    }
    else
    {
        //未射到怪物
        if (hitEnemy != null)
        {
            hitEnemy.OnUnSelect();
            hitEnemy = null;
        }

    }

}

// Class Enemy
// 怪物受击函数Hit（val)
public void Hit(int val)
{
    // Shield first
    if (Defend > val)
    {
        // Minor shield
        Defend -= val;
        // Play hit anim
        ani.Play("hit", 0, 0);
    }
    else
    {
        val = val - Defend;
        Defend = 0;
        CurHp -= val;
        if (CurHp <= 0)
        {
            CurHp = 0;
            // Play dead anim
            ani.Play("die");
            // Remove enemy
            EnemyManeger.Instance.DeleteEnemy(this);
            Destroy(gameObject, 1);
            Destroy(actionObj);
            Destroy(hpItemObj);
        }
        else
        {
            // play hit anim
            ani.Play("hit", 0, 0);
        }
    }
    // Update ui
    UpdateDefend();
    UpdateHp();
}
```

AddCard：抽牌（抽牌数根据txt提供的数据设置）

```c#
public override void OnEndDrag(PointerEventData eventData)
{
    if (TryUse() == true)
    {
        int val = int.Parse(data["Arg0"]);
        if (FightCardManager.Instance.HasCard() == true)
        {
            UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(val);
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f));
            PlayEffect(pos);
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
    else
    {
        base.OnEndDrag(eventData);
    }
}
```

三、结束战斗，切换怪物回合

切换回合用ChangeType()方法，移除玩家手牌，开始怪物回合

```c#
// Class Enemy
public override void Init()
{
    base.Init();
    // Remove all cards
    UIManager.Instance.GetUI<FightUI>("FightUI").RemoveAllCards();
    // Enemy turn
    UIManager.Instance.ShowTip("Enemy turn", Color.red, delegate ()
    {
        FightManager.Instance.StartCoroutine(EnemyManeger.Instance.DoAllEnemyAction());
    });
}

// Class EnemyManager
public IEnumerator DoAllEnemyAction()
{
    for (int i = 0; i < enemyList.Count; i++)
    {
        yield return FightManager.Instance.StartCoroutine(enemyList[i].DoAction());
    }
    // refresh enemy actions
    for (int i = 0; i < enemyList.Count; i++)
    {
        enemyList[i].SetRandomAction();

    }
    // Switch player turn
    FightManager.Instance.ChangeType(FightType.PlayerTurn);
}

// Class Enemy
public IEnumerator DoAction()
{
    HideAction();
    Debug.Log("DoAction " + type);
    // ActionType type = (ActionType)(Random.Range(1, 2));
    switch (type)
    {
        case ActionType.None:
            break;
        case ActionType.Defend:
            // Add shield
            Defend += 1;
            UpdateDefend();
            // Play effect
            break;
        case ActionType.Attack:
            // Play attack anim
            ani.Play("attack");
            // Wait for anim finish
            yield return new WaitForSeconds(0.5f);
            // Shack screen
            Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
            // Minor player health
            FightManager.Instance.GetPlayerHit(Attack);
            break;
    }
    // Wait for anim finish
    yield return new WaitForSeconds(1);
    // Play idle anim
    ani.Play("idle");
}
```

怪物行动包括攻击和加盾两种，每回合采取的行动通过Random随机

```c#
// Set a random action
public void SetRandomAction()
{
    int rand = Random.Range(1, 3);
    type = (ActionType)rand;
    Debug.Log(type);
    switch (type)
    {
        case ActionType.None:
            break;
        case ActionType.Defend:
            attackTf.gameObject.SetActive(false);
            defendTf.gameObject.SetActive(true);
            break;
        case ActionType.Attack:
            attackTf.gameObject.SetActive(true);
            defendTf.gameObject.SetActive(false);
            break;
    }
}
```

怪物状态的改变
