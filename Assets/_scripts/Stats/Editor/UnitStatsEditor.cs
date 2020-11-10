using UnityEditor;

[CustomEditor(typeof(UnitStats))]
public class UnitStatsEditor : ReordableListEditor
{
    protected override string listName => "PossibleItemsList";
    protected override string headerName => "Possible Drops";
    protected override string elementName => "Element";   
}