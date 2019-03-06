using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OSK42
{
  public class Init : MonoBehaviour
  {

    // Use this for initialization
    IEnumerator Start()
    {
      yield return 0.1f;
      SceneManager.LoadScene("main");
    }
  }
}
