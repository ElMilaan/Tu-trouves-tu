using UnityEngine;

using TMPro;

/// <summary>
/// CollectableObject - À placer sur chaque objet grabbable de la scène.
/// 
/// SETUP AutoHand :
///   1. Ajouter le component "Grabbable" (AutoHand) sur le même GameObject.
///   2. Ajouter un TextMeshPro 3D enfant pour afficher l'ID (assigné dans objectLabel).
///   3. Remplir "objectID" avec un identifiant unique (ex : "A1", "B2", "C3"...).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CollectableObject : MonoBehaviour
{
    [Header("Identifiant")]
    [Tooltip("ID unique de cet objet (doit correspondre à ceux du GameManager)")]
    public string objectID = "A1";

    [Header("Visuel")]
    [Tooltip("TextMeshPro 3D enfant qui affiche l'ID (optionnel)")]
    public TextMeshPro objectLabel;

    [Tooltip("Couleur du label")]
    public Color labelColor = Color.black;

    // ---------------------------------------------------------------
    void Start()
    {
        if (objectLabel != null)
        {
            objectLabel.text = objectID;
            objectLabel.color = labelColor;
        }
    }

    // ---------------------------------------------------------------
    // Ces méthodes sont appelées par SuitcaseZone via GetComponent<CollectableObject>()

    public string GetID() => objectID;
}
