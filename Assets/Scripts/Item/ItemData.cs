using UnityEngine;
using System.Text;
using UnityEditor;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New item data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemId;
    [Range(0, 100)]
    public float dropChance;
    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }

}
