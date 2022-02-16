using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader sceneLoader { get; private set; }
        [SerializeField] private int currentLevel = 0;
        // Persistent scene is buildindex 0 and must be taken in account when dealing with both currentLevel and maxAmountLevels
        [SerializeField] private int maxAmountLevels;
        private void Awake()
        {
            if (!sceneLoader)
            {
                sceneLoader = this;
            }
            else
            {
                Destroy(this);
            }

            maxAmountLevels = SceneManager.sceneCountInBuildSettings;
        }

        [SerializeField] private List<AsyncOperation> asyncOperations = new List<AsyncOperation>();

        public void LoadSceneByIndex(int index)
        {
            if (currentLevel < maxAmountLevels - 1)
            {
                asyncOperations.Add(SceneManager.UnloadSceneAsync(currentLevel));
                asyncOperations.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));

                currentLevel = index;
                HandleAsyncScenes();
            }
        }

        public void AddSceneByIndex(int index)
        {
            if (currentLevel < maxAmountLevels - 1)
            {
                asyncOperations.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));

                currentLevel = index;
                HandleAsyncScenes();
            }
        }

        public void LoadNextScene()
        {
            if (currentLevel < maxAmountLevels - 1)
            {
                asyncOperations.Add(SceneManager.UnloadSceneAsync(currentLevel));
                asyncOperations.Add(SceneManager.LoadSceneAsync(++currentLevel, LoadSceneMode.Additive));

                HandleAsyncScenes();
            }
        }
        public void LoadPreviousScene()
        {
            if (currentLevel > 1)
            {
                asyncOperations.Add(SceneManager.UnloadSceneAsync(currentLevel));
                asyncOperations.Add(SceneManager.LoadSceneAsync(--currentLevel, LoadSceneMode.Additive));

                HandleAsyncScenes();
            }
        }

        async void HandleAsyncScenes()
        {
            foreach (AsyncOperation scene in asyncOperations)
            {
                scene.allowSceneActivation = false;

                do
                {
                    await Task.Yield();
                } while (scene.progress < 0.9f);

                scene.allowSceneActivation = true;
            }
            asyncOperations.Clear();
        }
    }
}