using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace CGames
{
    public class LevelController : MonoBehaviour, ILosableLevelController
    {        
        [Header("Ships")]
        [SerializeField] private StartingShip startingSailingShip;
        [SerializeField] private EscapeShip escapeShip;

        [Header("UI Elements")]
        [SerializeField] private GoalTracker goalTracker;
        [SerializeField] private NavigationArrow navigationArrow;
        [Space]
        [SerializeField] private GameOverPanel gameOverPanel;

        [HideInInspector] public UnityEvent OnEscapeShipEnterEvent;
        public event Action OnPlayerDeathAction;

        private PlayerDataView playerDV;
        private SceneLoader sceneLoader;

        private bool isTaskCompleted;

        [Inject]
        private void Construct(PlayerDataView playerDV, SceneLoader sceneLoader)
        {
            this.playerDV = playerDV;
            this.sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            PrepareShips();
            
            goalTracker.SetTaskText("Target_Find");
            navigationArrow.SetTargetTransform(escapeShip.transform);
        }

        private void Start()
        {
            PlayerInputHandler.DisablePlayerControls();
            PlayerInputHandler.DisablePauseListener();

            StartCoroutine(startingSailingShip.SailToTerrain());
        }

        private void PrepareShips()
        {
            startingSailingShip.Initialize();
            startingSailingShip.SetFlagToWind();

            escapeShip.Initialize();
            escapeShip.SetFlagToIdle();

            startingSailingShip.OnDestinationReached += OnStartingDestinationReached;
            escapeShip.OnPlayerEnteredShip += OnEscapeShipEnter;
            escapeShip.OnDestinationReached += sceneLoader.LoadShip;
        }

        private void OnStartingDestinationReached()
        {
            PlayerInputHandler.EnablePlayerControls();
            PlayerInputHandler.EnablePauseListener();
            goalTracker.ShowTask();
        }

        private void OnEscapeShipEnter()
        {
            OnEscapeShipEnterEvent?.Invoke();
            OnEscapeShipEnterEvent.RemoveAllListeners();

            PlayerInputHandler.DisablePlayerControls();
            PlayerInputHandler.DisablePauseListener();
            goalTracker.HideTask();
            navigationArrow.HideArrow();
        }

        public void SetAsLost()
        {
            OnPlayerDeathAction?.Invoke();

            PlayerInputHandler.DisablePlayerControls();
            PlayerInputHandler.DisablePauseListener();
            
            gameOverPanel.ShowPanel(playerDV.CoinsAmount, playerDV.ColoredGems);

            playerDV.LoseAllCoins();
            playerDV.SaveData();
        }

        public void SetAsCompleted()
        {
            if(isTaskCompleted)
                return;

            isTaskCompleted = true;

            navigationArrow.DisplayArrow();
            goalTracker.SetTaskText("Target_Escape");

            StartCoroutine(escapeShip.WaitForPlayerToAppear());
        }

        private void OnDestroy()
        {
            startingSailingShip.OnDestinationReached -= OnStartingDestinationReached;
            escapeShip.OnPlayerEnteredShip -= OnEscapeShipEnter;
            escapeShip.OnDestinationReached -= sceneLoader.LoadShip;
        }

    #region Context Menu Methods

    #if UNITY_EDITOR
        [ContextMenu("Find All Missing Scripts")]
        private void FindAllScripts()
        {
            if(startingSailingShip == null)
                startingSailingShip = FindObjectOfType<StartingShip>();

            if(escapeShip == null)
                escapeShip = FindObjectOfType<EscapeShip>();

            if(goalTracker == null)
                goalTracker = FindObjectOfType<GoalTracker>();

            if(navigationArrow == null)
                navigationArrow = FindObjectOfType<NavigationArrow>();

            if(gameOverPanel == null)
               gameOverPanel = FindObjectOfType<GameOverPanel>();

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
    #endif

        [ContextMenu("Break All Treasure Chests")]
        private void Editor_BreakAllChests()
        {
            FindObjectsOfType<TreasureChest>().ToList().ForEach(x => x.TakeDamage(1));
        }

        [ContextMenu("Move Player To Finish")]
        private void Editor_MovePlayerToFinish()
        {
            FindObjectOfType<PlayerCharacter>().transform.position = new(escapeShip.transform.position.x, escapeShip.transform.position.y + 5);
        }


    #endregion
    }
}