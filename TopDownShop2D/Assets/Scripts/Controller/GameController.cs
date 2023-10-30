namespace Project.Core
{
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
        private DialogWindow dialogWindow;
        [SerializeField]
        private ShopWindow shopWindow;
        [SerializeField]
        private InventoryWindow inventoryWindow;

        GameState currentGameState = GameState.NULL;

        DialogHolder currentDialog = null;
        InventoryHolder currentInventory = null;

        protected override void ExecuteOnAwake()
        {
            base.ExecuteOnAwake();

            StateMachineController.InitializeStateMachine();
        }

        private void Start()
        {
            dialogWindow.onTurnOffFinishEvent += OnDialogWindowCloseEvent;

            this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.INITIALIZING));
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
                this.Invoke(0.5f, () => StateMachineController.ExecuteTransition(GameState.RUNNING));
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