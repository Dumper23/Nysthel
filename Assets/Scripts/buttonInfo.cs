using UnityEngine;
using UnityEngine.EventSystems;


public class buttonInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    #region IPointerEnterHandler implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null && eventData.hovered.Count >= 3 && eventData.hovered[2].transform.parent.parent.parent != null && eventData.hovered[2].transform.parent.parent.parent.GetComponent<UIInventory>() != null && eventData.hovered[2].transform.parent.childCount >= 6)
        {
            eventData.hovered[2].transform.parent.parent.parent.GetComponent<UIInventory>().isPointerIn = true;
            eventData.hovered[2].transform.parent.parent.parent.GetComponent<UIInventory>().showItemInfo(eventData.hovered[2].transform.parent.GetChild(5).name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null && eventData.hovered.Count >= 2 && eventData.hovered[1] != null && eventData.hovered[1].GetComponent<UIInventory>() != null)
        {
            eventData.hovered[1].GetComponent<UIInventory>().isPointerIn = false;
        }
        
    }


    #endregion

}