

namespace STUDENT_NAME
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;
	using SDD.Events;
    using UnityEngine.SceneManagement;

    public class LevelManager : Manager<LevelManager>
    {

        private int actualLevelID;
        #region Manager implementation
        protected override IEnumerator InitCoroutine()
        {
            yield break;
        }
        #endregion

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();

            EventManager.Instance.AddListener<PlayButtonClickedEvent>(GoLevel1);
            EventManager.Instance.AddListener<GoToLevel1Event>(GoLevel1);
            EventManager.Instance.AddListener<GoToLevel2Event>(GoLevel2);
            EventManager.Instance.AddListener<GoToLevel3Event>(GoLevel3);
            EventManager.Instance.AddListener<RestartLevelEvent>(RestartLevel);
            EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
        }

        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(GoLevel1);
            EventManager.Instance.RemoveListener<GoToLevel1Event>(GoLevel1);
            EventManager.Instance.RemoveListener<GoToLevel2Event>(GoLevel2);
            EventManager.Instance.RemoveListener<GoToLevel3Event>(GoLevel3);
            EventManager.Instance.RemoveListener<RestartLevelEvent>(RestartLevel);
            EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);

        }

        protected override void GameMenu(GameMenuEvent e)
        {
            if (SceneManager.GetActiveScene().name != "main")
            {
                SceneManager.LoadScene("main");
            }
        }

        void GoLevel1(PlayButtonClickedEvent e)
        {
            LoadLevel(1);
        }

        void LoadLevel(int lvl)
        { 
            SceneManager.LoadScene("level" + lvl);
            actualLevelID = lvl;
            EventManager.Instance.Raise(new SceneHasBeenLoadedEvent() {eLevelLoaded = lvl });
        }
        void GoLevel1(GoToLevel1Event e)
        {
            LoadLevel(1);
        }
        void GoLevel2(GoToLevel2Event e)
        {
            LoadLevel(2);
        }
        void GoLevel3(GoToLevel3Event e)
        {
            LoadLevel(3);
        }

        void GoToNextLevel(GoToNextLevelEvent e)
        {
            ++actualLevelID;
            LoadLevel(actualLevelID);
        }

        void RestartLevel(RestartLevelEvent e)
        {
            LoadLevel(actualLevelID);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}