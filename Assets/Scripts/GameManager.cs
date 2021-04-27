namespace STUDENT_NAME
{
	using System.Collections;
	using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using SDD.Events;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public enum GameState { gameMenu, gamePlay, gameNextLevel, gamePause, gameOver, gameVictory }

	public class GameManager : Manager<GameManager>
	{
		#region Game State
		private GameState m_GameState;
		public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
		#endregion

        private int actualLevelIndex;
        
		#region Nodes
		private int NNodesLeft;
        private int[] NNodesAllowedByLevel = { 3, 2, 19 };

		void SetNodesNumber(int nodesNumber)
		{
            NNodesLeft = nodesNumber;
			EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNodesNumber = nodesNumber});
		}

        void DecrementNNodesLeft(int value)
        {
            SetNodesNumber(NNodesLeft - value);
        }
		#endregion
        
		#region Time
		void SetTimeScale(float newTimeScale)
		{
			Time.timeScale = newTimeScale;
		}
		#endregion
        
		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();
			
			//MainMenuManager
			EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.AddListener<LevelsButtonClickedEvent>(LevelsButtonClicked);
            EventManager.Instance.AddListener<BackButtonClickedEvent>(BackButtonClicked);
            EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

            //LevelManager
            EventManager.Instance.AddListener<SceneHasBeenLoadedEvent>(SceneLoaded);
            EventManager.Instance.AddListener<Level1ButtonClickedEvent>(Level1ButtonClicked);
            EventManager.Instance.AddListener<Level2ButtonClickedEvent>(Level2ButtonClicked);
            EventManager.Instance.AddListener<Level3ButtonClickedEvent>(Level3ButtonClicked);
            EventManager.Instance.AddListener<RestartButtonClickedEvent>(RestartButtonClicked);
            EventManager.Instance.AddListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);

            //HUDManager 
            EventManager.Instance.AddListener<StructureFinishedButtonClickedEvent>(StructureFinishedButtonClicked);

            //Node Event
            EventManager.Instance.AddListener<NodeHasBeenCreatedEvent>(NodeHasBeenCreated);

            //Goal Event
            EventManager.Instance.AddListener<GoalHasBeenReachedEvent>(GoalHasBeenReached);

            //DeathZone Event
            EventManager.Instance.AddListener<PlayerHasHitDeathZoneEvent>(PlayerHasHitDeathZone);
		}

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();

			//MainMenuManager
			EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.RemoveListener<LevelsButtonClickedEvent>(LevelsButtonClicked);
            EventManager.Instance.RemoveListener<BackButtonClickedEvent>(BackButtonClicked);
            EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

            //LevelManager
            EventManager.Instance.RemoveListener<SceneHasBeenLoadedEvent>(SceneLoaded);
            EventManager.Instance.RemoveListener<Level1ButtonClickedEvent>(Level1ButtonClicked);
            EventManager.Instance.RemoveListener<Level2ButtonClickedEvent>(Level2ButtonClicked);
            EventManager.Instance.RemoveListener<Level3ButtonClickedEvent>(Level3ButtonClicked);
            EventManager.Instance.RemoveListener<RestartButtonClickedEvent>(RestartButtonClicked);
            EventManager.Instance.RemoveListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);

            //HUDManager 
            EventManager.Instance.RemoveListener<StructureFinishedButtonClickedEvent>(StructureFinishedButtonClicked);

            //Node Event
            EventManager.Instance.RemoveListener<NodeHasBeenCreatedEvent>(NodeHasBeenCreated);

            //Goal Event
            EventManager.Instance.RemoveListener<GoalHasBeenReachedEvent>(GoalHasBeenReached);

            //DeathZone Event
            EventManager.Instance.AddListener<PlayerHasHitDeathZoneEvent>(PlayerHasHitDeathZone);
        }
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			Menu();
			InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
			yield break;
		}
		#endregion

		#region Game flow & Gameplay
		//Game initialization
		void InitNewGame(bool raiseStatsEvent = true)
		{

		}
		#endregion

		#region Callbacks to Events issued by MenuManager
		private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
		{
			Menu();
		}

        private void LevelsButtonClicked(LevelsButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new GameLevelsMenuEvent());
        }
        
        private void BackButtonClicked(BackButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new GameMenuEvent());
        }

		private void PlayButtonClicked(PlayButtonClickedEvent e)
		{
            Play();
        }

		private void ResumeButtonClicked(ResumeButtonClickedEvent e)
		{
			Resume();
		}

		private void EscapeButtonClicked(EscapeButtonClickedEvent e)
		{
			if (IsPlaying) Pause();
		}

		private void QuitButtonClicked(QuitButtonClickedEvent e)
		{
			Application.Quit();
		}

        private void Level1ButtonClicked(Level1ButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new GoToLevel1Event());
        }

        private void Level2ButtonClicked(Level2ButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new GoToLevel2Event());
        }
        private void Level3ButtonClicked(Level3ButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new GoToLevel3Event());
        }
        private void RestartButtonClicked(RestartButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new RestartLevelEvent());
        }

        private void NextLevelButtonClicked(NextLevelButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new GoToNextLevelEvent());
        }
        #endregion

        #region Callbacks to Events issued by LevelManager

        void SceneLoaded(SceneHasBeenLoadedEvent e)
        {
            actualLevelIndex = e.eLevelLoaded;
            SetNodesNumber(NNodesAllowedByLevel[actualLevelIndex - 1]);
            Play();
        }

        #endregion

        #region Callbacks to Events issued by HUDManager
        void StructureFinishedButtonClicked(StructureFinishedButtonClickedEvent e)
        {
            EventManager.Instance.Raise(new StructureFinishedEvent());
        }
        #endregion

        #region Callbacks to Events issued by Node
        void NodeHasBeenCreated(NodeHasBeenCreatedEvent e)
        {
            DecrementNNodesLeft(1);
            if (SfxManager.Instance) SfxManager.Instance.PlaySfx2D("Tick");
            if (NNodesLeft == 0)
            {
                EventManager.Instance.Raise(new StructureFinishedEvent());
            }
        }
        #endregion

        #region Callbacks to Events issued by Goal
        void GoalHasBeenReached(GoalHasBeenReachedEvent e)
        {
            if (!IsPlaying) return;

            SetTimeScale(0);
            
            if(actualLevelIndex != 3)
            {
                m_GameState = GameState.gameNextLevel;
                EventManager.Instance.Raise(new AskToGoToNextLevelEvent());
            }
            else
            {
                Victory();
            }
        }

        #endregion

        #region Callbacks to Events issued by DeathZone

        void PlayerHasHitDeathZone(PlayerHasHitDeathZoneEvent e)
        {
            Over();
        }

        #endregion

        #region GameState methods
        private void Menu()
		{
			SetTimeScale(1);
			m_GameState = GameState.gameMenu;
			if(MusicLoopsManager.Instance)MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
			EventManager.Instance.Raise(new GameMenuEvent());
		}
        
		private void Play()
		{
			InitNewGame();
			SetTimeScale(1);
			m_GameState = GameState.gamePlay;

			if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
			EventManager.Instance.Raise(new GamePlayEvent());
		}

		private void Pause()
		{
			if (!IsPlaying) return;

			SetTimeScale(0);
			m_GameState = GameState.gamePause;
			EventManager.Instance.Raise(new GamePauseEvent());
		}

		private void Resume()
		{
			if (IsPlaying) return;

			SetTimeScale(1);
			m_GameState = GameState.gamePlay;
			EventManager.Instance.Raise(new GameResumeEvent());
		}

		private void Over()
		{
			SetTimeScale(0);
			m_GameState = GameState.gameOver;
			EventManager.Instance.Raise(new GameOverEvent());
			if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX);
		}
        private void Victory()
		{
			SetTimeScale(0);
			m_GameState = GameState.gameVictory;
			EventManager.Instance.Raise(new GameVictoryEvent());
		}
		#endregion
	}
}

