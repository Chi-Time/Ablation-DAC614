using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

class GameController : MonoBehaviour
{
    [SerializeField] private int _Lives = 0;
    [SerializeField] private long _Score = 0;
    [SerializeField] private int _ScoreNegative = -25;
    [SerializeField] private float _FOVOFfset = 0.025f;
    [SerializeField] private float _SaturationOffset = 0.025f;
    [SerializeField] private Camera _Camera;
    [SerializeField] private PostProcessProfile _Attribute;
    [SerializeField] private GameObject _EndingScreen;
    [SerializeField] private Text _BadEnding;
    [SerializeField] private Text _GoodEnding;

    private bool _IsGameOver = false;

    private void OnEnable ()
    {
        Signals.OnGameOver += OnGameOver;
        Signals.OnCharacterHit += OnCharacterHit;
    }

    private void OnGameOver ()
    {
        _IsGameOver = true;
        _EndingScreen.SetActive (true);

        if (_Lives <= 0)
        {
            _BadEnding.gameObject.SetActive(true);
            return;
        }

        _GoodEnding.gameObject.SetActive (true);
    }

    private void OnCharacterHit ()
    {
        _Lives--;

        _Camera.fieldOfView -= _FOVOFfset;
        _Score -= _ScoreNegative;
        //_Attribute.GetSetting<ColorGrading> ().;

        //if (_Lives <= 0)
        //    Signals.GameOver ();
    }

    private void Update ()
    {
        if (_IsGameOver && (Input.GetButtonDown ("Fire1") || Input.GetMouseButton (0)))
        {
            SceneManager.LoadScene (0);
        }
    }

    private void OnDisable ()
    {
        Signals.OnGameOver -= OnGameOver;
        Signals.OnCharacterHit -= OnCharacterHit;
    }
}
