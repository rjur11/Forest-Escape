using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabLoader : MonoBehaviour
{
    public Text scoreTextOverlay;
    public Text livesOverlay;
    public Text timerText;
    public Text messageOverlay;
    public Text bossOverlay;

    public GameObject stagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.S.SetStagePrefab(stagePrefab, scoreTextOverlay, livesOverlay, timerText, messageOverlay);
        GameManager.S.RefreshStage();
    }
}
