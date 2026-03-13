using System.Collections.Generic;
using Newtonsoft.Json;

// CLASSE QUI PERMET D'INSTANCIER AVEC LE JSON
[System.Serializable]
public class ObjectList
{
    [JsonProperty("objects")]
    public Object[] objects { get; set; }
}

[System.Serializable]
public class Object
{
    [JsonProperty("id")]
    public string id { get; private set; }
    [JsonProperty("tagName")]
    public string tagName { get; private set; }
    [JsonProperty("meshes")]
    public List<string> meshes { get; private set; }
    [JsonProperty("mass")]
    public float mass { get; private set; }
    [JsonProperty("grabbable")]
    public bool grabbable { get; private set; }
    public bool isGrabbed { get; private set; }

    // CONSTRUCTEUR
    [JsonConstructor]
    public Object(string id, string tagName, List<string> meshes, float mass, bool grabbable)
    {
        this.id = id;
        this.tagName = tagName;
        this.meshes = meshes;
        this.mass = mass;
        this.grabbable = grabbable;
        isGrabbed = false;
    }

    // METHODS

    public void grab()
    {
        if (grabbable && !isGrabbed)
        {
            isGrabbed = true;
            System.Console.WriteLine("Item " + id + " grabbed.");
        }
    }

    public void drop()
    {
        if (isGrabbed)
        {
            isGrabbed = false;
            System.Console.WriteLine("Item " + id + " dropped.");
        }
    }
}
