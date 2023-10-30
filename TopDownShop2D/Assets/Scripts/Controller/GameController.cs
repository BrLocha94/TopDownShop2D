namespace Project.Core
{
    using System;
    using UnityEngine;
    using Project.Utils;
    using Project.Enums;
    using Project.Contracts;
    using Project.UI.Windows;
    using Project.Structures.Iteraction.Dialog;
    using Project.Structures.Inventory;

    public class GameController : MonoSingleton<GameController>, IReceiver<GameState>
    {
        [SerializeField]
        private CameraControler cameraControler;
        [SerializeField]
        private Transform playerTransform;
        [SerializeField]
        private DialogWindow dialogWindow;
        [SerializeField]
        private ShopWindow shopWindow;
        [SerializeField]
        private InventoryWindow inventoryWindow;
        [SerializeField]
        private GameObject fade;
        [SerializeField]
        private DialogHolder helpDialog;
        [SerializeField]
        private DialogHolder tutorialDialog;

        GameState currentGameState = GameState.NULL;

        DialogHolder currentDialog = null;
        DialogHolder currentShopDialog = null;
        InventoryHolder currentInventory = null;

        protected override void ExecuteOnAwake()
        {
            base.ExecuteOnAwake();

            StateMachineController.InitializeStateMachine();
            cameraControler.SetCameraTarget(playerTransform);
        }

        private void Start()
        {
            dialogWindow.onTurnOffFinishEvent += OnDialogWindowCloseEvent;

            this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.INITIALIZING));
        }

        private void OnTutorialStarted()
        {
            if (currentGameState != GameState.INITIALIZING)
                return;

            dialogWindow.SetDialog(tutorialDialog.dialog);
            dialogWindow.onDialogFinishEvent += OnTutorialDialogFinishEvent;

            fade.SetActive(true);

            StateMachineController.ExecuteTransition(GameState.TUTORIAL);
        }

        private void OnTutorialDialogFinishEvent()
        {
            dialogWindow.onTurnOffFinishEvent -= OnTutorialDialogFinishEvent;
            dialogWindow.SetDialog(helpDialog.dialog);
            dialogWindow.onDialogFinishEvent += OnHelpDialogFinishEvent;

            StateMachineController.ExecuteTransition(GameState.HELP);
        }

        public void OnHelpClicked()
        {
            if (currentGameState != GameState.RUNNING)
                return;

            dialogWindow.SetDialog(helpDialog.dialog);
            dialogWindow.onDialogFinishEvent += OnHelpDialogFinishEvent;

            fade.SetActive(true);

            StateMachineController.ExecuteTransition(GameState.HELP);
        }

        private void OnHelpDialogFinishEvent()
        {
            dialogWindow.onTurnOffFinishEvent += OnHelpDialogWindowCloseEvent;
            dialogWindow.TurnOff();
        }

        private void OnHelpDialogWindowCloseEvent()
        {
            dialogWindow.onTurnOffFinishEvent -= OnHelpDialogWindowCloseEvent;
            fade.SetActive(false);
            StateMachineController.ExecuteTransition(GameState.RUNNING);
        }

        public void CloseDialog()
        {
            if (currentDialog == null && currentGameState == GameState.DIALOG)
            {
                dialogWindow.onTurnOffFinishEvent += OnDialogWindowCloseEvent;
                dialogWindow.TurnOff();
                return;
            }
        }

        private void OnDialogWindowCloseEvent()
        {
            dialogWindow.onTurnOffFinishEvent -= OnDialogWindowCloseEvent;
            StateMachineController.ExecuteTransition(GameState.RUNNING);
        }

        public void ExecuteSimpleDialog(DialogHolder dialogHolder)
        {
            // Cant open player inventory without and open dialog
            if (currentGameState != GameState.RUNNING && currentGameState != GameState.DIALOG) return;

            // Cant Execute new dialog while last is still in screen 
            if (currentDialog != null) return;

            currentDialog = dialogHolder;

            dialogWindow.SetDialog(dialogHolder.dialog);
            dialogWindow.onDialogFinishEvent += OnSimpleDialogFinishEvent;

            StateMachineController.ExecuteTransition(GameState.DIALOG);
        }

        private void OnSimpleDialogFinishEvent()
        {
            DialogHolder holder = currentDialog;
            currentDialog = null;

            dialogWindow.onDialogFinishEvent -= OnSimpleDialogFinishEvent;
            holder.onDialogFinishCallback?.Invoke();
        }

        public void ExecuteShopDialog(DialogHolder dialogHolder)
        {
            // Cant open player inventory without and open dialog
            if (currentGameState != GameState.SHOP) return;

            // Cant Execute new dialog while last is still in screen 
            if (currentShopDialog != null) return;

            currentShopDialog = dialogHolder;

            dialogWindow.SetDialog(dialogHolder.dialog, false, true);
            dialogWindow.onDialogFinishEvent += OnShopDialogFinishEvent;

            StateMachineController.ExecuteTransition(GameState.SHOP_DIALOG);
        }

        private void OnShopDialogFinishEvent()
        {
            DialogHolder holder = currentShopDialog;
            currentShopDialog = null;

            dialogWindow.onDialogFinishEvent -= OnShopDialogFinishEvent;
            holder.onDialogFinishCallback?.Invoke();

            StateMachineController.ExecuteTransition(GameState.SHOP);
        }

        public void OpenShop(InventoryHolder inventoryHolder)
        {
            // Cant open an shop without and open dialog 
            if (currentGameState != GameState.DIALOG) return;

            // Cant open an shop with an active one running
            if (currentInventory != null) return;

            currentInventory = inventoryHolder;

            shopWindow.SetGridInfo(inventoryHolder.inventory);
            shopWindow.onTurnOffFinishEvent += OnShopCloseCallback;
            shopWindow.TurnOn();

            StateMachineController.ExecuteTransition(GameState.SHOP);
        }

        public void OpenShopToSell(InventoryHolder inventoryHolder)
        {
            // Cant open an shop without and open dialog 
            if (currentGameState != GameState.DIALOG) return;

            // Cant open an shop with an active one running
            if (currentInventory != null) return;

            currentInventory = inventoryHolder;

            shopWindow.SetGridInfo(inventoryHolder.inventory, false);
            shopWindow.onTurnOffFinishEvent += OnShopCloseCallback;
            shopWindow.TurnOn();

            StateMachineController.ExecuteTransition(GameState.SHOP);
        }

        private void OnShopCloseCallback()
        {
            InventoryHolder holder = currentInventory;
            currentInventory = null;

            shopWindow.onTurnOffFinishEvent -= OnShopCloseCallback;

            StateMachineController.ExecuteTransition(GameState.DIALOG);
            holder.onInventoryClosedCallback?.Invoke();
        }

        public void OpenPlayerInventory()
        {
            // Cant open player inventory without and open dialog 
            if (currentGameState != GameState.RUNNING) return;

            cameraControler.ExecuteCameraMoveOn(() => {
                inventoryWindow.SetGridInfo(DataController.GetPlayerInventory());
                inventoryWindow.onTurnOffFinishEvent += OnInventoryCloseCallback;
                inventoryWindow.TurnOn();
            });

            StateMachineController.ExecuteTransition(GameState.INVENTORY);
        }

        private void OnInventoryCloseCallback()
        {
            inventoryWindow.onTurnOffFinishEvent -= OnInventoryCloseCallback;

            cameraControler.ExecuteCameraMoveOff(() => {
                StateMachineController.ExecuteTransition(GameState.RUNNING);
            });
        }

        public void ReceiveUpdate(GameState updatedValue)
        {
            currentGameState = updatedValue;

            // Remove this to an loading screen logic
            if (currentGameState == GameState.INITIALIZING)
            {
                DataController.Initialize();
                this.Invoke(0.5f, () => OnTutorialStarted());
                return;
            }
        }

        private void Update()
        {
            // Remove this to an centrilized input check logic
            if (currentGameState != GameState.RUNNING) return;

            if (Input.GetKeyDown(KeyCode.I))
                OpenPlayerInventory();
        }
    }
}