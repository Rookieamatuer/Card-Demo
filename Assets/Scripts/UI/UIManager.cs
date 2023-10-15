using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
}
