

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SuitcaseZone_AutoHand - Utilise un trigger physique simple et fiable.
/// 
/// SETUP :
///   1. Placer ce script sur un enfant de la valise nommé "DropZone"
///   2. Ajouter un Box Collider sur ce même GameObject → cocher Is Trigger
///   3. Ajuster la taille du collider pour couvrir l'intérieur de la valise
/// </summary>
public class SuitcaseZone_AutoHand : MonoBehaviour
{
    [Header("Feedback visuel (optionnel)")]
    public Renderer suitcaseRenderer;
    public Color emptyColor  = Color.white;
    public Color filledColor = Color.green;

    private List<CollectableObject> itemsInSuitcase = new List<CollectableObject>();

    // ---------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        // Cherche CollectableObject sur l'objet ou ses parents
        CollectableObject item = other.GetComponentInParent<CollectableObject>();
        if (item == null) item = other.GetComponent<CollectableObject>();
        if (item == null) return;

        if (!itemsInSuitcase.Contains(item))
        {
            itemsInSuitcase.Add(item);
            GameManager.Instance.RegisterCollectedObject(item.GetID());
            Debug.Log($"[Valise] Objet ajouté : {item.GetID()}");
            UpdateVisual();
        }
    }

    void OnTriggerExit(Collider other)
    {
        CollectableObject item = other.GetComponentInParent<CollectableObject>();
        if (item == null) item = other.GetComponent<CollectableObject>();
        if (item == null) return;

        if (itemsInSuitcase.Contains(item))
        {
            itemsInSuitcase.Remove(item);
            GameManager.Instance.UnregisterCollectedObject(item.GetID());
            Debug.Log($"[Valise] Objet retiré : {item.GetID()}");
            UpdateVisual();
        }
    }

    // ---------------------------------------------------------------
    private void UpdateVisual()
    {
        if (suitcaseRenderer == null) return;
        suitcaseRenderer.material.color = itemsInSuitcase.Count > 0 ? filledColor : emptyColor;
    }

    public List<string> GetCollectedIDs()
    {
        List<string> ids = new List<string>();
        foreach (var item in itemsInSuitcase)
            ids.Add(item.GetID());
        return ids;
    }
}