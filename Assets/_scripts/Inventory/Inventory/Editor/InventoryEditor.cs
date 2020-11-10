using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : ReordableListEditor
{
    protected override string listName => "allInventoryItems";
    protected override string headerName => "Items";
    protected override string elementName => "Element";
}