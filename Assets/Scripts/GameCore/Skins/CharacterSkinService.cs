using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace GameCore.Skins
{
    public class CharacterSkinService : IInitializable
    {
        private const string SelectedSkinPrefsKey = "SelectedSkin";
        private const string PurchasedSkinsPrefsKey = "PurchasedSkins";

        public SkinType SelectedSkin { get; private set; }
        
        private HashSet<SkinType> _purchasedSkins;
        private List<int> _tempSkins;
        
        public void Initialize()
        {
            InitializeSelected();
            InitializePurchased();
        }

        private void InitializeSelected()
        {
            if (!PlayerPrefs.HasKey(SelectedSkinPrefsKey))
            {
                SelectedSkin = SkinType.DefaultMale;
                return;
            }
            
            try
            {
                SelectedSkin = (SkinType)PlayerPrefs.GetInt(SelectedSkinPrefsKey);
            }
            catch
            {
                SelectedSkin = SkinType.DefaultMale;
            }
        }

        private void InitializePurchased()
        {
            _tempSkins = new List<int>();
            _purchasedSkins = new HashSet<SkinType>();
            _purchasedSkins.Add(SkinType.DefaultMale);
            
            if (!PlayerPrefs.HasKey(PurchasedSkinsPrefsKey))
                return;

            try
            {
                string json = PlayerPrefs.GetString(PurchasedSkinsPrefsKey);
                var purchasedSkins = JsonUtility.FromJson<List<int>>(json);
                foreach (int skin in purchasedSkins)
                {
                    _purchasedSkins.Add((SkinType)skin);
                }
            }
            catch
            {
                // do nothing
            }
        }

        public bool IsPurchased(SkinType skinType) => _purchasedSkins.Contains(skinType);

        public void Purchase(SkinType skinType)
        {
            _purchasedSkins.Add(skinType);
            SavePurchased();
        }

        private void SavePurchased()
        {
            _tempSkins.Clear();
            foreach (var skinType in _purchasedSkins)
            {
                _tempSkins.Add((int)skinType);
            }

            string json = JsonUtility.ToJson(_tempSkins);
            PlayerPrefs.SetString(PurchasedSkinsPrefsKey, json);
        }

        public void Select(SkinType skinType)
        {
            if (!_purchasedSkins.Contains(skinType))
            {
                Debug.LogError($"[CharacterSkinService] Trying to select not purchased skin {skinType}");
                return;
            }
            
            SelectedSkin = skinType;
            PlayerPrefs.SetInt(SelectedSkinPrefsKey, (int)SelectedSkin);
        }
    }
}