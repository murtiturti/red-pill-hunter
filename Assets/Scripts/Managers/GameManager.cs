using System;
using GameEventsSystem;
using UnityEngine;
using Util;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private IntVariable killCount;
        [SerializeField]
        private int totalEnemies;
        
        public static GameManager Instance { get; private set; }
        public UIManager uiManager;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Time.timeScale = 1;
            killCount.Value = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (killCount.Value == totalEnemies)
            {
                GameWin();
            }
        }

        private void GameFail()
        {
            Time.timeScale = 0;
            uiManager.ShowLose();
            UnlockCursor();
        }

        private void GameWin()
        {
            Time.timeScale = 0;
            uiManager.ShowWin();
            UnlockCursor();
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerDeath += GameFail;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDeath -= GameFail;
        }
    }
}
