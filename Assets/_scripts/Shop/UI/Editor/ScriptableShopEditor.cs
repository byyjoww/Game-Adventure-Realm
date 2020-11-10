using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Elysium.Shop
{
    [CustomEditor(typeof(ScriptableShop))]
    public class ScriptableShopEditor : ReordableListEditor
    {
        protected override string listName => "shopSections";
        protected override string headerName => "Section";
        protected override string elementName => "Element";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            DrawPropertiesExcluding(serializedObject, listName);

            EditorGUILayout.Space();

            m_ReorderableList.DoLayoutList();

            if (GUILayout.Button("Add Section")) { AddSection(); }

            serializedObject.ApplyModifiedProperties();            
        }

        public void AddSection()
        {
            ScriptableShop shopUI = (ScriptableShop)target;

            shopUI.shopSections.RemoveAll(x => x == null);
            shopUI.shopSections.Add(new UI_ShopBuy.ShopBuySection());
        }
    }    
}