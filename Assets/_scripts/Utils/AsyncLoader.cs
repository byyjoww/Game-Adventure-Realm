using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class AsyncLoader : MonoBehaviour
{
    public static string sceneName = "MainMenu";
    public static float delay = 0;

    [SerializeField] RectTransform loadingBar;
    [SerializeField] Text loadingBarText;
    [SerializeField] ScriptableEvent OnSceneLoad;
    [SerializeField] ScriptableEvent OnRemoteConfigLoadComplete;
    bool IsRemoteConfigLoaded { get; set; }

    IEnumerator Start ()
    {
        loadingBar.anchorMax = new Vector2(0, loadingBar.anchorMax.y);
        var initTime = Time.time;
        
        OnRemoteConfigLoadComplete.OnRaise += () => { IsRemoteConfigLoaded = true; };
        OnSceneLoad.Raise();

        var sw = new Stopwatch();
        sw.Start();
        yield return new WaitUntil(() => CheckIfRemoteConfigLoaded(sw, 5000));
        IsRemoteConfigLoaded = false;
        sw.Stop();

        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone || Time.time - initTime <= delay)
        {
            var progress = GetAsyncOperationTimeWithDelay(asyncLoad, initTime, delay);
            loadingBar.anchorMax = new Vector2(progress, loadingBar.anchorMax.y);
            loadingBarText.text = $"LOADING... {(int)(progress * 100)}%";
            yield return new WaitForEndOfFrame();

            if (asyncLoad.progress >= 0.85f && progress >= 0.85f)
            {
                asyncLoad.allowSceneActivation = true;
            }
        }

        asyncLoad.allowSceneActivation = true;
        yield return null;
    }

    bool CheckIfRemoteConfigLoaded(Stopwatch sw, int timeout)
    {
        if (IsRemoteConfigLoaded)
        {
            UnityEngine.Debug.Log("Remote Config is Loaded.");
            return true;
        }

        if (sw.ElapsedMilliseconds > timeout)
        {
            UnityEngine.Debug.LogError($"Remote Config Timed Out at {timeout}ms.");
            return true;
        }

        return false;
    }

    public static void LoadSceneAsync(string name, float delayWait)
    {
        sceneName = name;
        delay = delayWait;
        SceneManager.LoadScene("Loader");
    }

    float GetAsyncOperationTimeWithDelay(AsyncOperation asyncOperation, float initTime, float delay)
    {
        return delay > 0 ? Mathf.Min(asyncOperation.progress, (Time.time - initTime / delay)) : asyncOperation.progress;
    }
}
