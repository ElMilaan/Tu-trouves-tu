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
    private string id;
    [JsonProperty("tagName")]
    private string tagName;
    [JsonProperty("meshes")]
    private List<string> meshes;
    [JsonProperty("mass")]
    private float mass;
    [JsonProperty("grabbable")]
    private bool grabbable;
    private bool isGrabbed;

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

    // GETTERS
    public string getId() { return id; }

    public string getTagName() { return tagName; }

    public List<string> getMeshes() { return meshes; }

    public float getMass() { return mass; }

    public bool getGrabbable() { return grabbable; }

    public bool getIsGrabbed() { return isGrabbed; }

    // SETTERS
    public void setIsGrabbed(bool isGrabbed) { this.isGrabbed = isGrabbed; }

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
