using UnityEngine;

using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class CollectableObject : MonoBehaviour
{
    public string objectID;
    public TextMeshPro objectLabel;
    public Color labelColor = Color.black;

    void Start()
    {
        if (objectLabel != null)
        {
            objectLabel.text = objectID;
            objectLabel.color = labelColor;
        }
    }
}
