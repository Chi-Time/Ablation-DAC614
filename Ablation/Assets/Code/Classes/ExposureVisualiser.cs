
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ExposureVisualiser : MonoBehaviour
{
    [SerializeField] private LevelType _LevelsType;
    [SerializeField] private BandType _BandType;
    [SerializeField] private float _PrintDelay = 2.5f;
    [SerializeField] private float _MinAmplitude = 0.0f;
    [SerializeField] private float _MaxAmplitude = 0.0f;
    [SerializeField] private float _RandomAmplitude = 0.0f;
    [SerializeField] private Material _Material = null;

    [SerializeField] private float _StartIntensity = 0.0f;
    [SerializeField] private AudioSpectrum _Spectrum = null;
    private int _BandCount = 0;

    private void OnEnable ()
    {
        Signals.OnTimeEvent += OnTimeEvent;
        Signals.OnGameOver += OnGameOver;
    }
    
    private void OnGameOver ()
    {
        print ("It's Over");
        this.enabled = false;
        _Material.SetFloat ("_Exposure", -2f);
    }

    void OnTimeEvent (float speedOFfset, Range forceOffset, Range spawnOffset, Range exposureOffset, float fadeTime)
    {
        _MinAmplitude = exposureOffset.Min;
        _MaxAmplitude = exposureOffset.Max;
    }

    private void Start ()
    {
        _StartIntensity = _Material.GetFloat ("_Exposure");


        //Invoke ("DoIt", 2f);
    }

    private void OnDisable ()
    {
        Signals.OnTimeEvent -= OnTimeEvent;
        Signals.OnGameOver -= OnGameOver;
        
    }

    private void OnDestroy ()
    {
        _Material.SetFloat ("_Exposure", _StartIntensity);
    }

    private void Update ()
    {
        if (_Spectrum == null)
            _Spectrum = GameObject.Find ("Spectrum").GetComponent<AudioSpectrum> ();

        var packagedData = 0.0f;
        float[] spectrumData = null;
        _RandomAmplitude = Random.Range (_MinAmplitude, _MaxAmplitude);

        switch (_LevelsType)
        {
            case LevelType.Normal:
                spectrumData = _Spectrum.Levels;
                break;
            case LevelType.Mean:
                spectrumData = _Spectrum.MeanLevels;
                break;
            case LevelType.Peak:
                spectrumData = _Spectrum.PeakLevels;
                break;
            default:
                break;
        }

        //print (Mathf.RoundToInt (spectrumData.Length / 3f));
        //print (Mathf.RoundToInt (spectrumData.Length / 3f) * 2);

        switch (_BandType)
        {
            case BandType.Lows:
                if (spectrumData != null)
                {
                    int numberOfBands = Mathf.RoundToInt (spectrumData.Length / 3f);

                    for (int i = 0; i < spectrumData.Length; i++)
                    {
                        _BandCount++;

                        if (_BandCount > numberOfBands)
                            break;

                        packagedData += System.Math.Abs (spectrumData[i]);
                    }
                }

                _BandCount = 0;

                break;
            case BandType.Mids:
                if (spectrumData != null)
                {
                    int numberOfBands = Mathf.RoundToInt (spectrumData.Length / 3f);

                    for (int i = 0; i < spectrumData.Length; i++)
                    {
                        _BandCount++;

                        if (_BandCount > (numberOfBands * 2))
                            break;

                        if (_BandCount > numberOfBands)
                            packagedData += System.Math.Abs (spectrumData[i]);
                    }
                }

                _BandCount = 0;

                break;
            case BandType.Highs:
                if (spectrumData != null)
                {
                    int numberOfBands = Mathf.RoundToInt (spectrumData.Length / 3f);

                    for (int i = 0; i < spectrumData.Length; i++)
                    {
                        _BandCount++;

                        if (_BandCount > (numberOfBands * 2))
                            packagedData += System.Math.Abs (spectrumData[i]);
                    }
                }

                _BandCount = 0;

                break;
            default:
                break;
        }

        //if (spectrumData != null)
        //{
        //    //print ("Data Length: " + spectrumData.Length);
        //    for (int i = 0; i < spectrumData.Length; i++)
        //    {
        //        _BandCount++;

        //        if (_BandCount >= 4)
        //            break;

        //        packagedData += System.Math.Abs (spectrumData[i]);
        //        //print ("Data Value: " + packagedData);
        //    }


        //    //for (int i = spectrumData.Length - 1; i >= 0; i--)
        //    //{

        //    //    bandCount++;

        //    //    if (bandCount >= 4)
        //    //        break;

        //    //    packagedData += System.Math.Abs (spectrumData[i]);
        //    //    //print ("Data Value: " + packagedData);
        //    //}

        //    _BandCount = 0;
        //}

        _Material.SetFloat ("_Exposure", (packagedData * _RandomAmplitude) + _StartIntensity);

        //transform.localScale = new Vector3 ((packagedData * _RandomAmplitude) + _StartScale.x, (packagedData * _RandomAmplitude) + _StartScale.y, (packagedData * _RandomAmplitude) + _StartScale.z);
    }
}
