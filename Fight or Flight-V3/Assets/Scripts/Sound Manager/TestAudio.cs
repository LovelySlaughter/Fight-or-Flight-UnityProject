using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
  public AudioController controller;

    #region Unity Functions
#if UNITY_EDITOR

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.T)) { controller.PlayAudio(testSound); }
        //if (Input.GetKeyUp(KeyCode.G)) { controller.StopAudio(testSound); }
        //if (Input.GetKeyUp(KeyCode.B)) { controller.RestartAudio(testSound); }
    }

#endif
    #endregion
}
