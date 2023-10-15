using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AttackCardItem : CardItem, IPointerDownHandler
{

    // Mouse key press
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
    Enemy hitEnemy;// enemy casted
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
            //Œ¥…‰µΩπ÷ŒÔ
            if (hitEnemy != null)
            {
                hitEnemy.OnUnSelect();
                hitEnemy = null;
            }

        }

    }
}
