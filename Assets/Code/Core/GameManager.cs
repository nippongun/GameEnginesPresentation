using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager { get; private set; }

        private void Awake()
        {
            if (!gameManager)
            {
                gameManager = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            Scenes.SceneLoader.sceneLoader.AddSceneByIndex(1);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Scenes.SceneLoader.sceneLoader.LoadNextScene();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Scenes.SceneLoader.sceneLoader.LoadPreviousScene();
            }
        }
    }
}