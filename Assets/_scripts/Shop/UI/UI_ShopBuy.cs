using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elysium.Shop
{
    public class UI_ShopBuy : MonoBehaviour
    {
        [Serializable]
        public class ShopBuySection
        {
            public string name;
            [HideInInspector] public GameObject tab;
            [HideInInspector] public GameObject panel;
            public Transform Viewport => panel.transform.GetChild(0);

            public List<Purchase> elementsToInstantiate;
        }

        [Header("Shop")]
        [SerializeField] private ScriptableShop scriptableShop;
        public List<ShopBuySection> sectionsToInstantiate => scriptableShop.shopSections;
        private ShopBuySection selectedSection { get; set; }

        [Header("Tabs")]
        [SerializeField] private GameObject pfTabElement;
        [SerializeField] private Transform tTabElement;
        private List<GameObject> instantiatedTabsList;

        [Header("Panels")]
        [SerializeField] public GameObject pfPanelElement;
        [SerializeField] private Transform tPanelElement;
        private List<GameObject> instantiatedPanelList;

        private List<GameObject> instantiatedElementsList;

        private event Action OnShopChanged;
        public event Action<ShopBuySection> OnTabSelected;
        public event Action<ShopBuySection> OnTabDeselected;

        private bool isInitialized;

        public void Initialize()
        {
            if (isInitialized) { return; }

            instantiatedTabsList = new List<GameObject>();
            instantiatedPanelList = new List<GameObject>();
            instantiatedElementsList = new List<GameObject>();

            CreatePanels();
            CreateTabs();
            RefreshUI();

            isInitialized = true;
        }

        public void OnEnable()
        {
            Initialize();
            RefreshUI();
            TabClicked(sectionsToInstantiate[0]);
            OnShopChanged += RefreshUI;
        }

        private void OnDisable() => OnShopChanged -= RefreshUI;

        public void CreateTabs()
        {
            foreach (var section in sectionsToInstantiate)
            {
                GameObject tab = Instantiate(pfTabElement, tTabElement);
                section.tab = tab;
                tab.GetComponentInChildren<TMP_Text>().text = section.name;
                tab.GetComponent<Button>().onClick.AddListener(delegate { TabClicked(section); });
                instantiatedTabsList.Add(tab);
            }
        }

        public void CreatePanels()
        {
            foreach (var section in sectionsToInstantiate)
            {
                GameObject panel = Instantiate(pfPanelElement, tPanelElement);
                section.panel = panel;
                instantiatedPanelList.Add(panel);
            }
        }

        public void RefreshUI()
        {
            foreach (GameObject obj in instantiatedElementsList) { Destroy(obj); }
            instantiatedElementsList.Clear();

            foreach (var section in sectionsToInstantiate)
            {
                Transform tShopElement = section.panel.transform.GetChild(0);

                foreach (var element in section.elementsToInstantiate)
                {
                    var obj = element.CreateUIElement(section.Viewport);
                    instantiatedElementsList.Add(obj);
                }
            }
        }

        public void Select(ShopBuySection section) => OnTabSelected?.Invoke(section);

        public void Deselect(ShopBuySection section) => OnTabDeselected?.Invoke(section);

        public void TabClicked(ShopBuySection section)
        {
            if (selectedSection != null) { Deselect(selectedSection); }
            selectedSection = section;
            Select(section);
            ResetTabs(section);
        }

        private void ResetTabs(ShopBuySection section)
        {
            foreach (var panel in instantiatedPanelList) { panel.SetActive(false); }
            section.panel.SetActive(true);
        }
    }
}