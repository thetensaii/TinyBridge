namespace STUDENT_NAME
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using SDD.Events;

	public class HudManager : Manager<HudManager>
	{
        [SerializeField] GameObject m_PanelHUD;

        #region Labels & Values
        [SerializeField] private Text m_TxtNodesNumber;
        #endregion

        private bool isStructureFinished = false;

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
        #endregion

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();

            EventManager.Instance.AddListener<StructureFinishedEvent>(StructureFinished);
        }

        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            EventManager.Instance.RemoveListener<StructureFinishedEvent>(StructureFinished);
        }

        #region Callbacks to GameManager events
        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
		{
            m_TxtNodesNumber.text = e.eNodesNumber.ToString();
        }

        void StructureFinished(StructureFinishedEvent e)
        {
            isStructureFinished = true;
            m_PanelHUD.SetActive(false);
        }

        protected override void GameResume(GameResumeEvent e)
        {
            m_PanelHUD.SetActive(!isStructureFinished);
        }

        protected override void GamePlay(GamePlayEvent e)
        {
            
            isStructureFinished = false;
            m_PanelHUD.SetActive(true);
        }

        protected override void GamePause(GamePauseEvent e)
        {
            m_PanelHUD.SetActive(!isStructureFinished);
        }

        protected override void GameMenu(GameMenuEvent e)
        {
            isStructureFinished = false;
            m_PanelHUD.SetActive(false);
        }

        protected override void GameOver(GameOverEvent e)
        {
            isStructureFinished = false;
            m_PanelHUD.SetActive(false);
        }

        protected override void GameVictory(GameVictoryEvent e)
        {
            isStructureFinished = false;
            m_PanelHUD.SetActive(false);
        }
        #endregion


        #region UI OnClick Events
        public void StructureFinishedButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new StructureFinishedButtonClickedEvent());
        }
        #endregion
        
    }
}