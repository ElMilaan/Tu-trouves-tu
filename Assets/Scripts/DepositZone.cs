using System.Collections.Generic;
using UnityEngine;

public class DepositZone : MonoBehaviour
{
    public Renderer depositRenderer;
    public Color emptyColor  = Color.white;
    public Color filledColor = Color.green;
    private List<CollectableObject> itemsInDeposit = new List<CollectableObject>();

    void OnTriggerEnter(Collider other)
    {
        CollectableObject item = other.GetComponentInParent<CollectableObject>();
        if (item == null) item = other.GetComponent<CollectableObject>();
        if (item == null)
        {
            Debug.Log("L'objet n'est pas prenable");
            return;
        }

        if (!itemsInDeposit.Contains(item))
        {
            itemsInDeposit.Add(item);
            GameManager.Instance.RegisterCollectedObject(item.objectID);
            Debug.Log($"[Depôt] Objet ajouté : {item.objectID}");
            UpdateVisual();
        }
    }

    void OnTriggerExit(Collider other)
    {
        CollectableObject item = other.GetComponentInParent<CollectableObject>();
        if (item == null) item = other.GetComponent<CollectableObject>();
        if (item == null)
        {
            Debug.Log("L'objet n'est pas prenable");
            return;
        }

        if (itemsInDeposit.Contains(item))
        {
            itemsInDeposit.Remove(item);
            GameManager.Instance.UnregisterCollectedObject(item.objectID);
            Debug.Log($"[Dépôt] Objet retiré : {item.objectID}");
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        if ( depositRenderer== null) return;
        depositRenderer.material.color = itemsInDeposit.Count > 0 ? filledColor : emptyColor;
    }

    public List<string> GetCollectedIDs()
    {
        List<string> ids = new List<string>();
        foreach (var item in itemsInDeposit)
            ids.Add(item.objectID);
        return ids;
    }
}