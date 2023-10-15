using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Transform canvasTransform; // Canvas transform module

    private List<UIBase> uiList; // List of loaded interface

    private void Awake()
    {
        Instance = this;
        // Get Canvas
        canvasTransform = GameObject.Find("Canvas").transform;
        // Initialize list
        uiList = new List<UIBase>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UIBase ShowUI<T>(string uiName) where T : UIBase // Show
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

    public void HideUI(string uiName)   // Hide
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            ui.Hide();
        }
    }

    public void CloseUI(string uiName)  // Close
    {
        UIBase ui = Find (uiName);
        if (ui != null)
        {
            uiList.Remove(ui);
            Destroy(ui.gameObject);
        } 
            
    }

    public void CloseAll()  // CloseAll
    {
        for(int i = uiList.Count - 1; i >= 0; i--)
        {
            Destroy(uiList[i].gameObject);
        }
        uiList.Clear(); // Clear list
    }

    public UIBase Find(string uiName)   // Find
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            if (uiList[i].name == uiName)
            {
                return uiList[i];
            }
        }
        return null;
    }

    // Create Action Icon
    public GameObject CreateActionIcon()
    {
        GameObject obj = Instantiate(Resources.Load("UI/actionIcon"), canvasTransform) as GameObject;
        obj.transform.SetAsFirstSibling();  // Set in parent top
        return obj;
    }

    // Create Health bar
    public GameObject CreateHpItem()
    {
        GameObject obj = Instantiate(Resources.Load("UI/HpItem"), canvasTransform) as GameObject;
        obj.transform.SetAsFirstSibling();  // Set in parent top
        return obj;
    }

    // Round information
    public void ShowTip(string msg, Color color, System.Action callback = null)
    {
        GameObject obj = Instantiate(Resources.Load("UI/Tips"), canvasTransform) as GameObject;
        Text text = obj.transform.Find("bg/Text").GetComponent<Text>();
        text.color = color;
        text.text = msg;
        Tween scale1 = obj.transform.Find("bg").DOScaleY(1, 0.4f);
        Tween scale2 = obj.transform.Find("bg").DOScaleY(0, 0.4f);
        Sequence seq = DOTween.Sequence();
        seq.Append(scale1);
        seq.AppendInterval(0.5f);
        seq.Append(scale2);
        seq.AppendCallback(delegate ()
        {
            if (callback != null) callback();
        });
        MonoBehaviour.Destroy(obj, 2);
    }

    // Get UI
    public T GetUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            return ui.GetComponent<T>();
        }
        return null;
    }

}
