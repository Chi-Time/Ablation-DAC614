using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof (MeshRenderer))]
class FadeIn : MonoBehaviour
{
    [Tooltip ("How long the object should take to fade in.")]
    [SerializeField] private float _FadeLength = 2.0f;

    private Material _Material = null;

    private void Awake ()
    {
        var renderer = GetComponent<MeshRenderer> ();
        _Material = renderer.material;
    }

    private void OnEnable ()
    {
        _Material.color = new Color (_Material.color.r, _Material.color.g, _Material.color.b, 0.0f);
        StartCoroutine (Fade (_FadeLength, 1.0f));
    }

    IEnumerator Fade (float length, float targetOpacity)
    {
        float timer = 0.0f;
        float currentOpacity = _Material.color.a;

        while (timer < length)
        {
            timer += Time.deltaTime;
            float opacity = Mathf.Lerp (currentOpacity, targetOpacity, timer / length);
            _Material.color = new Color (_Material.color.r, _Material.color.g, _Material.color.b, opacity);

            yield return new WaitForEndOfFrame ();
        }

        _Material.color = new Color (_Material.color.r, _Material.color.g, _Material.color.b, targetOpacity);
    }
}
