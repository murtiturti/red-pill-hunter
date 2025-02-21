using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject win, lose, buttons;

        public IntVariable inClip, count;
        public TextMeshProUGUI ammoInfo;

        private void Update()
        {
            ammoInfo.text = inClip.Value.ToString() + "/" + count.Value.ToString();
        }

        public void ShowWin()
        {
            win.SetActive(true);
            buttons.SetActive(true);
        }

        public void ShowLose()
        {
            lose.SetActive(true);
            buttons.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}