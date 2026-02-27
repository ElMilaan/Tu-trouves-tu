using UnityEngine;
using Newtonsoft.Json;

public class ItemManager : MonoBehaviour
{
    public string jsonFileName = "objects.json";

    void Start()
    {
        LoadObjects();
    }

    void LoadObjects()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found!");
            return;
        }

        ObjectList objectList = JsonConvert.DeserializeObject<ObjectList>(jsonFile.text);

        if (objectList?.objects == null)
        {
            Debug.LogError("JSON could not be deserialized!");
            return;
        }

        Debug.Log("Loaded " + objectList.objects.Length + " objects from JSON.");

        foreach (var item in objectList.objects)
        {
            Debug.Log($"Object: {item.getTagName()}, Mass: {item.getMass()}, Meshes: {string.Join(", ", item.getMeshes())}");
        }

        /*foreach (Object obj in objectList.objects)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + obj.getMeshes()[0]);
            if (prefab == null)
            {
                Debug.LogError("Prefab not found: " + obj.getMeshes()[0]);
                continue;
            }

            GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            instance.name = obj.getTagName();
            instance.tag = obj.getTagName();

            System.Console.WriteLine("Instantiated object: " + obj.getId() + " with tag: " + obj.getTagName());

            instance.AddComponent<ItemData>().obj = obj;
        }*/
    }
}

public class ItemData : MonoBehaviour
{
    public Object obj;
}