using Currency;
using Framework;
using Framework.DI;
using Framework.GUI;
using Framework.Pools;
using GameCore.Character;
using GameCore.Skins;
using LocalMessages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class SkinShopWindow : WindowBase, IMessageListener<PlayerCurrencyChangedMessage>
    {
        [Header("Main")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _prevButton;
        
        [Header("Purchase")]
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Color _canPurchaseColor;
        [SerializeField] private Color _cannotPurchaseColor;
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _selectedState;
        
        [Header("Preview")]
        [SerializeField] private Transform _visualsRoot;
        [SerializeField] private CharacterVisuals _visualsPrefab;

        [Inject] private readonly CharacterSkinService _skinService;
        [Inject] private readonly CharacterSkinsConfig _skinsConfig;
        [Inject] private readonly PlayerCurrencyController _playerCurrencyController;
        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        private CharacterVisuals _visualsPreview;
        private SkinType _currentSkin;
        private int _currentSkinConfigId;

        protected override void Start()
        {
            base.Start();
            _closeButton.onClick.AddListener(Close);
            _purchaseButton.onClick.AddListener(OnPurchaseClick);
            _selectButton.onClick.AddListener(OnSelectClick);
            _nextButton.onClick.AddListener(OnNextClick);
            _prevButton.onClick.AddListener(OnPrevClick);

            _visualsPreview = PrefabMonoPool<CharacterVisuals>.GetPrefabInstance(_visualsPrefab, _visualsRoot);

            for (int i = 0; i < _skinsConfig.SkinPrices.Length; i++)
            {
                var priceContainer = _skinsConfig.SkinPrices[i];
                if (priceContainer.Item1 == _skinService.SelectedSkin)
                    _currentSkinConfigId = i;
            }

            ShowSkin(_currentSkinConfigId);
            
            _localMessageBroker.Subscribe(this);
        }

        public override void Destroy()
        {
            base.Destroy();
            _localMessageBroker.Unsubscribe(this);
        }

        private void OnPurchaseClick()
        {
            if (_skinService.IsPurchased(_currentSkin))
            {
                Debug.LogError($"[SkinShopWindow] Skin already purchased: {_currentSkin}");
                ShowCurrentSkin();
                return;
            }

            var container = _skinsConfig.SkinPrices[_currentSkinConfigId];
            if (_playerCurrencyController.GetCurrencyAmount(CurrencyType.Coins) < container.Item2)
            {
                // TODO: show notification
                return;
            }

            _playerCurrencyController.AddCurrency(CurrencyType.Coins, -container.Item2);
            _skinService.Purchase(_currentSkin);
            _skinService.Select(_currentSkin);
            ShowCurrentSkin();
        }

        private void OnSelectClick()
        {
            if (_skinService.SelectedSkin == _currentSkin || !_skinService.IsPurchased(_currentSkin))
            {
                Debug.LogError($"[SkinShopWindow] Trying to select already selected or not purchased skin {_currentSkin}");
                ShowCurrentSkin();
                return;
            }
            
            _skinService.Select(_currentSkin);
            ShowCurrentSkin();
        }

        private void OnNextClick()
        {
            int nextId = _currentSkinConfigId + 1;
            nextId %= _skinsConfig.SkinPrices.Length;
            ShowSkin(nextId);
        }

        private void OnPrevClick()
        {
            int nextId = _currentSkinConfigId - 1;
            nextId = nextId < 0 ? _skinsConfig.SkinPrices.Length - 1 : nextId;
            ShowSkin(nextId);
        }

        private void ShowSkin(int id)
        {
            _currentSkinConfigId = id;
            var container = _skinsConfig.SkinPrices[id];
            _currentSkin = container.Item1;
            ShowCurrentSkin();
        }

        private void ShowCurrentSkin()
        {
            _visualsPreview.SetSkin(_currentSkin);
            if (_currentSkin == _skinService.SelectedSkin)
            {
                _selectedState.SetActive(true);
                _purchaseButton.gameObject.SetActive(false);
                _selectButton.gameObject.SetActive(false);
                return;
            }
            
            _selectedState.SetActive(false);
            if (_skinService.IsPurchased(_currentSkin))
            {
                _purchaseButton.gameObject.SetActive(false);
                _selectButton.gameObject.SetActive(true);
            }
            else
            {
                _purchaseButton.gameObject.SetActive(true);
                _selectButton.gameObject.SetActive(false);

                var container = _skinsConfig.SkinPrices[_currentSkinConfigId];
                bool hasCurrency = _playerCurrencyController.GetCurrencyAmount(CurrencyType.Coins) >= container.Item2;
                _priceText.text = container.Item2.ToString();
                _priceText.color = hasCurrency ? _canPurchaseColor : _cannotPurchaseColor;
            }
        }

        public void OnMessage(in PlayerCurrencyChangedMessage message)
        {
            if (message.Type != CurrencyType.Coins)
                return;
            
            if (_skinService.IsPurchased(_currentSkin))
                return;
            
            var container = _skinsConfig.SkinPrices[_currentSkinConfigId];
            bool hasCurrency = _playerCurrencyController.GetCurrencyAmount(CurrencyType.Coins) >= container.Item2;
            _priceText.color = hasCurrency ? _canPurchaseColor : _cannotPurchaseColor;
        }
    }
}