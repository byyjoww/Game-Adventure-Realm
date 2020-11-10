using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.Shop
{
    public class UI_ShopSell : MonoBehaviour
    {        
        public class ShopSellSection
        {
            public ShopSellSection(string name)
            {
                this.name = name;
            }

            public string name;
            [HideInInspector] public GameObject tab;
            [HideInInspector] public GameObject panel;
            public Transform Viewport => panel.transform.GetChild(0);

            public List<InventoryStack> elementsToInstantiate;
        }

        [Header("Shop")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private Currency currency;
        [SerializeField] private ScriptableEvent OnPurchaseFailed;
        [SerializeField] private ScriptableEvent OnPurchaseSucceeded;
        [SerializeField] private GameObject prefab;

        public List<ShopSellSection> sectionsToInstantiate;
        public Dictionary<string, Func<List<InventoryStack>>> sectionDictionary;
        private ShopSellSection selectedSection { get; set; }

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
        public event Action<ShopSellSection> OnTabSelected;
        public event Action<ShopSellSection> OnTabDeselected;

        private bool isInitialized;

        public void Initialize()
        {
            if (isInitialized) { return; }

            instantiatedTabsList = new List<GameObject>();
            instantiatedPanelList = new List<GameObject>();
            instantiatedElementsList = new List<GameObject>();

            CreateShopSections();
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
            inventory.OnInventoryChanged += RaiseOnShopChanged;
        }

        private void OnDisable()
        { 
            OnShopChanged -= RefreshUI; 
            inventory.OnInventoryChanged += RaiseOnShopChanged;
        }

        private void RaiseOnShopChanged() => OnShopChanged?.Invoke();

        private void CreateShopSections()
        {
            sectionsToInstantiate = new List<ShopSellSection>();
            sectionDictionary = new Dictionary<string, Func<List<InventoryStack>>>();

            sectionDictionary.Add("All", () => inventory.AllInventoryItems.Where(x => x.Item.GetType() == typeof(Item) || x.Item.GetType() == typeof(Equipment)).ToList());
            sectionDictionary.Add("Items", () => inventory.AllInventoryItems.Where(x => x.Item.GetType() == typeof(Item)).ToList());
            sectionDictionary.Add("Equips", () => inventory.AllInventoryItems.Where(x => x.Item.GetType() == typeof(Equipment)).ToList());

            foreach (var entry in sectionDictionary) { sectionsToInstantiate.Add(new ShopSellSection(entry.Key)); }

            RefreshShopSections();
        }

        private void RefreshShopSections()
        {
            foreach (var section in sectionsToInstantiate) { section.elementsToInstantiate = sectionDictionary[section.name]?.Invoke(); }
        }

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

            RefreshShopSections();

            foreach (var section in sectionsToInstantiate)
            {
                Transform tShopElement = section.panel.transform.GetChild(0);

                foreach (var element in section.elementsToInstantiate)
                {
                    var obj = ShopItemLayoutElement.Create(prefab, tShopElement, element.Item as Item, element.Item.ItemValue, () => Sell(element.Item, element.amount), element.amount, "Sell");
                    instantiatedElementsList.Add(obj);
                }
            }
        }

        protected void Sell(IInventoryElement item, int amount)
        {
            if (inventory.LoseItem(item, amount))
            {
                // PopupSystem.CreatePopup("Success", $"You've sold {amount} {item.ItemName} for {item.ItemValue * amount}!");
                OnPurchaseSucceeded.Raise();
                currency.GetCurrency(item.ItemValue * amount);
            }
            else
            {
                // PopupSystem.CreatePopup("Error", $"Your sale attempt failed.");
                OnPurchaseFailed.Raise();
            }

            RaiseOnShopChanged();
        }

        public void Select(ShopSellSection section) => OnTabSelected?.Invoke(section);

        public void Deselect(ShopSellSection section) => OnTabDeselected?.Invoke(section);

        public void TabClicked(ShopSellSection section)
        {
            if (selectedSection != null) { Deselect(selectedSection); }
            selectedSection = section;
            Select(section);
            ResetTabs(section);
        }

        private void ResetTabs(ShopSellSection section)
        {
            foreach (var panel in instantiatedPanelList) { panel.SetActive(false); }
            section.panel.SetActive(true);
        }
    }
}