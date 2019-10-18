using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*  IDEAS
 *  
 *  Make it so that you analyise the song and find the average over every 8th measure.
 *  Track these averages and then through the song change the average per channel on every 8th measure
 *  so that the song has a normalised form of averages for bullets.
 *  
 *  Make it so that you can stat track the seconds of the audio and then mark points to change
 *  the BPMType for each channel so that they can go from slower to faster bullets at more important
 *  parts of the song.
 *  
 *  Along with the BPM do this for the averages as well so that you can define where to catch and not catch bullets.
 *  
 *  Finally, maybe add some form of animation for the bullets based on the audio using the BPMType. 
 *  For example, lerp the outer turrets in a rotation so that every 2 beats they rotate in and out.
 * 
 */

public enum LevelType
{
    Normal,
    Mean,
    Peak
}

public enum BandType
{
    Lows,
    Mids,
    Highs
}

class ScaleVisualiser : MonoBehaviour
{
    [SerializeField] private LevelType _LevelsType;
    [SerializeField] private BandType _BandType;
    [SerializeField] private float _PrintDelay = 2.5f;
    [SerializeField] private float _MinAmplitude = 0.0f;
    [SerializeField] private float _MaxAmplitude = 0.0f;
    [SerializeField] private float _RandomAmplitude = 0.0f;
    [SerializeField] private Vector3 _StartScale = Vector3.zero;

    [SerializeField] private AudioSpectrum _Spectrum = null;
    private int _BandCount = 0;

    private void OnEnable ()
    {
        Signals.OnTimeEvent += OnTimeEvent;
    }

    void OnTimeEvent (float speedOFfset, Range forceOffset, Range spawnOffset, Range exposureOffset, float fadeTime)
    {

    }

    private void OnDisable ()
    {
        Signals.OnTimeEvent -= OnTimeEvent;
    }

    private void Start ()
    {
        _StartScale = transform.localScale;

        //Invoke ("DoIt", 2f);
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

        transform.localScale = new Vector3 ((packagedData * _RandomAmplitude) + _StartScale.x, (packagedData * _RandomAmplitude) + _StartScale.y, (packagedData * _RandomAmplitude) + _StartScale.z);
    }
}
