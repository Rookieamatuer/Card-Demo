using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enemy manager
public class EnemyManeger
{
    public static EnemyManeger Instance = new EnemyManeger();

    private List<Enemy> enemyList;  // Enemy list

    // Load enemy resource, id refer to level
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

    public void DeleteEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);

        //TODO:后续还要做击杀所有怪物的判断
    }

    //执行活着的怪物的行为
    public IEnumerator DoAllEnemyAction()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            yield return FightManager.Instance.StartCoroutine(enemyList[i].DoAction());
        }
        // 行动完后更新所有敌人行为
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetRandomAction();

        }
        // 切换到玩家回合
        FightManager.Instance.ChangeType(FightType.PlayerTurn);
    }

}
