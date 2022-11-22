using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NFT1Forge.OSY.System
{
    public class ControllerSystem : Singleton<ControllerSystem>
    {
        public bool IsDone { get; private set; } = false;
        private string m_CurrentScene;
        /// <summary>
        /// Startup system
        /// </summary>
        public void InitSystem()
        {
            StartCoroutine(Initialize());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator Initialize()
        {
            yield return null;
            //Do something?
        }
        /// <summary>
        /// Close current scene then load the new scene.
        /// </summary>
        /// <param name="scenePath"></param>
        public void SceneChange(string scenePath)
        {
            IsDone = false;
            StartCoroutine(SceneChangeAsync(scenePath));
        }
        /// <summary>
        /// Close current scene then load the new scene asynchronizely.
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        private IEnumerator SceneChangeAsync(string scenePath)
        {
            if (!string.IsNullOrEmpty(m_CurrentScene))
            {
                yield return UnloadScene(m_CurrentScene);
            }
            yield return LoadScene(scenePath);
            IsDone = true;
        }
        /// <summary>
        /// Load scene asynchronizely
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        private IEnumerator LoadScene(string scenePath)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(scenePath), LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            BaseController sceneController = FindObjectOfType<BaseController>();
            if (null != sceneController)
            {
                m_CurrentScene = scenePath;
                StartCoroutine(sceneController.Initialize());
            }
        }
        /// <summary>
        /// Unload scene asynchronizely
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        private IEnumerator UnloadScene(string scenePath)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(Path.GetFileNameWithoutExtension(scenePath));
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }
    }
}