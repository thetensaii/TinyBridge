
namespace STUDENT_NAME
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using SDD.Events;

	public class MenuManager : Manager<MenuManager>
	{

		[Header("MenuManager")]

		#region Panels
		[Header("Panels")]
		[SerializeField] GameObject m_PanelMainMenu;
		[SerializeField] GameObject m_PanelInGameMenu;
        [SerializeField] GameObject m_PanelLevelsMenu;
		[SerializeField] GameObject m_PanelGameOver;
		[SerializeField] GameObject m_PanelNextLevel;
		[SerializeField] GameObject m_PanelVictory;

        List<GameObject> m_AllPanels;
		#endregion

		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();

            // GameManager Events
            EventManager.Instance.AddListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
            EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);

        }

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();

            // GameManager Events
            EventManager.Instance.RemoveListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
            EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);

        }
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Monobehaviour lifecycle
		protected override void Awake()
		{
			base.Awake();
			RegisterPanels();
		}

		private void Update()
		{
			if (Input.GetButtonDown("Cancel"))
			{
				EscapeButtonHasBeenClicked();
			}
		}
		#endregion

		#region Panel Methods
		void RegisterPanels()
		{
			m_AllPanels = new List<GameObject>();
			m_AllPanels.Add(m_PanelMainMenu);
			m_AllPanels.Add(m_PanelInGameMenu);
            m_AllPanels.Add(m_PanelLevelsMenu);
			m_AllPanels.Add(m_PanelGameOver);
			m_AllPanels.Add(m_PanelNextLevel);
			m_AllPanels.Add(m_PanelVictory);
		}

		void OpenPanel(GameObject panel)
		{
			foreach (var item in m_AllPanels)
				if (item) item.SetActive(item == panel);
		}
		#endregion

		#region UI OnClick Events
		public void EscapeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new EscapeButtonClickedEvent());
		}

		public void PlayButtonHasBeenClicked()
		{
            EventManager.Instance.Raise(new PlayButtonClickedEvent());
        }

		public void ResumeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new ResumeButtonClickedEvent());
		}

		public void MainMenuButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
		}

		public void QuitButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new QuitButtonClickedEvent());
		}

        public void LevelsButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new LevelsButtonClickedEvent());
        }

        public void BackButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new BackButtonClickedEvent());
        }
        public void Level1ButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new Level1ButtonClickedEvent());
        }
        public void Level2ButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new Level2ButtonClickedEvent());
        }
        public void Level3ButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new Level3ButtonClickedEvent());
        }
        public void RestartButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new RestartButtonClickedEvent());
        }

        public void NextLevelButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new NextLevelButtonClickedEvent());
        }
        #endregion

        #region Callbacks to GameManager events
        private void AskToGoToNextLevel(AskToGoToNextLevelEvent e)
        {
            OpenPanel(m_PanelNextLevel);
        }

        private void GoToNextLevel(GoToNextLevelEvent e)
        {

            OpenPanel(null);
        }

        protected override void GameMenu(GameMenuEvent e)
		{
			OpenPanel(m_PanelMainMenu);
		}

        protected override void GameLevelsMenu(GameLevelsMenuEvent e)
        {
            OpenPanel(m_PanelLevelsMenu);
        }

        protected override void GamePlay(GamePlayEvent e)
		{
			OpenPanel(null);
		}

		protected override void GamePause(GamePauseEvent e)
		{
			OpenPanel(m_PanelInGameMenu);
		}

		protected override void GameResume(GameResumeEvent e)
		{
			OpenPanel(null);
		}

		protected override void GameOver(GameOverEvent e)
		{
			OpenPanel(m_PanelGameOver);
		}

        protected override void GameVictory(GameVictoryEvent e)
        {
            OpenPanel(m_PanelVictory);
        }
        #endregion

    }

}
