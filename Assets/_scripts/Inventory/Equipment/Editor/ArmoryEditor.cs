using UnityEditor;

[CustomEditor(typeof(Armory))]
public class ArmoryEditor : ReordableListEditor
{
    protected override string listName => "armorySlots";
    protected override string headerName => "Equipments";
    protected override string elementName => "Slot";
}