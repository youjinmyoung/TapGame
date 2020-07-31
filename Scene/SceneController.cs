using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : UnitySingleton<SceneController>
{
    [SerializeField] private GameObject[] _backgroundPref;

    public int backIndex;

    private GameObject CurrentBackground;

    private void Start()
    {
        CurrentBackground = Instantiate(_backgroundPref[backIndex], _backgroundPref[backIndex].transform.position, _backgroundPref[backIndex].transform.rotation);

        StartCoroutine(Save());
    }

    public void NextStage(int stage)
    {
        Destroy(CurrentBackground.gameObject);
        backIndex = stage % _backgroundPref.Length;
        CurrentBackground = Instantiate(_backgroundPref[backIndex], _backgroundPref[backIndex].transform.position, _backgroundPref[backIndex].transform.rotation);
    }

    public void ResetScene()
    {
        JsonHelper.Reincarnation();
        SceneManager.LoadScene("PlayScene");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        StopAllCoroutines();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        JsonHelper.Save();
    }

    IEnumerator Save()
    {
        while (true)
        {
            JsonHelper.Save();
            yield return new WaitForSeconds(1.0f);
        }
    }
}
