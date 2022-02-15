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
        }

        [SerializeField] private List<AsyncOperation> asyncOperations = new List<AsyncOperation>();

        public void LoadSceneByIndex(int index)
        {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(currentLevel));
            asyncOperations.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));

            currentLevel = index;
            HandleAsyncScenes();
        }

        public void AddSceneByIndex(int index)
        {
            asyncOperations.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));

            currentLevel = index;
            HandleAsyncScenes();
        }

        public void LoadNextScene()
        {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(currentLevel));
            asyncOperations.Add(SceneManager.LoadSceneAsync(++currentLevel, LoadSceneMode.Additive));

            HandleAsyncScenes();
        }
        public void LoadPreviousScene()
        {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(currentLevel));
            asyncOperations.Add(SceneManager.LoadSceneAsync(--currentLevel, LoadSceneMode.Additive));

            HandleAsyncScenes();
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