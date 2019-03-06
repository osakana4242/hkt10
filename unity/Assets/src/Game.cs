using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
  public Text uiText;
  public GameObject player;
  public GameObject floor;

  public float timeLimit = 60f;
  public float time = 0f;
  public int score = 0;

  public delegate void StateFunc(Game game);
  StateFunc stateFunc_ = stateEmpty;
  StateFunc nextStateFunc_;
  int stateFrameCount_;
  // Use this for initialization
  void Start()
  {
    nextStateFunc_ = stateInit;
  }

  // Update is called once per frame
  void Update()
  {
    if (nextStateFunc_ != null)
    {
      var next = nextStateFunc_;
      nextStateFunc_ = null;
      stateFunc_ = next;
      stateFrameCount_ = 0;
    }
    stateFunc_(this);
    stateFrameCount_++;
  }

  static StateFunc stateInit = (game) =>
  {
    game.uiText.text = "";
    game.time = 0f;
    game.nextStateFunc_ = stateReady;
    game.player.SetActive(false);
    game.floorAngleZ_ = game.floor.transform.rotation.eulerAngles.z;
  };

  static StateFunc stateReady = (game) =>
  {
    game.uiText.text = "バランス\n" +
      "Z キーではじまるよ\n";
    if (Input.GetKeyDown(KeyCode.Z))
    {
      game.player.SetActive(true);
      game.nextStateFunc_ = stateMain;
    }
  };

  float floorAngleZ_;
  float floorDeltaAngleZ_;
  static StateFunc stateMain = (game) =>
  {
    game.time += Time.deltaTime;
    float restTime = Mathf.Max(0, game.timeLimit - game.time);

    var angleZ = game.floor.transform.rotation.eulerAngles.z;
    var deltaAngleZ = Mathf.Abs(Mathf.DeltaAngle(angleZ, game.floorAngleZ_));
    game.floorAngleZ_ = angleZ;
    game.floorDeltaAngleZ_ += deltaAngleZ;
    if (1f <= game.floorDeltaAngleZ_)
    {
      var deltaScore = (int)game.floorDeltaAngleZ_;
      game.floorDeltaAngleZ_ -= deltaScore;
      game.score += deltaScore;
    }

    if (restTime <= 0)
    {
      game.nextStateFunc_ = stateResult;
    }

    if (game.player.transform.position.y <= -10)
    {
      game.nextStateFunc_ = stateResult;
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
      reset();
      return;
    }

    game.uiText.text =
			string.Format("スコア {0}度\n", game.score) +
      string.Format("残り時間 {0}\n", restTime.ToString("F2")) +
			"左右キーでキューブが動くよ";
;
  };

  static StateFunc stateResult = (game) =>
  {
    game.uiText.text =
			"おしまい\n" +
			string.Format( "スコア {0}度\n", game.score ) +
			"Z キーで次へ\n";
    if (Input.GetKeyDown(KeyCode.Z))
    {
			game.nextStateFunc_ = stateEmpty;
      reset();
      return;
    }
  };

	static StateFunc stateEmpty = (game) => {};

  static void reset()
  {
    // 現在のScene名を取得する
    Scene loadScene = SceneManager.GetActiveScene();
    // Sceneの読み直し
    SceneManager.LoadScene(loadScene.name);
  }

}
