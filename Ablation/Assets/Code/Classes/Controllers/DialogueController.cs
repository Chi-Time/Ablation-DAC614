using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
class DialogueText
{
    public string Name;
    public string Text;
}

class DialogueController : MonoBehaviour
{
    public float _TypeSpeed = 0.05f;
    public Text _NameLabel = null;
    public Text _DialogueLabel = null;
    public GameObject _DialogueMenu = null;
    public List<DialogueText> _Dialogues = new List<DialogueText> ();
    public Animator _Animator = null;
    public Camera _Camera = null;

    private int _Counter = 0;

    private void OnEnable ()
    {
        Signals.OnShowText += OnShowText;
    }

    private void OnShowText ()
    {
        GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController> ().enabled = false;
        _Counter++;

        if (_Counter >= 4)
        {
            _Animator.SetTrigger ("Sieze");
        }

        StopAllCoroutines ();
        _DialogueMenu.gameObject.SetActive (true);

        if (_Dialogues.Count > 0)
        {
            StartCoroutine (ShowLine (_Dialogues[0]));
            _Dialogues.Remove (_Dialogues[0]);

            return;
        }

        GameObject.FindGameObjectWithTag ("Player").SetActive (false);
        _DialogueMenu.SetActive (false);
        _Camera.gameObject.SetActive (true);

        Invoke ("Transition", 2.0f);
    }

    private void Transition ()
    {
        Time.timeScale = 0.0f;
        SceneManager.LoadScene (1);
    }

    private void OnDisable ()
    {
        Signals.OnShowText -= OnShowText;
    }

    private IEnumerator ShowLine (DialogueText line)
    {
        _DialogueLabel.text = "";
        _NameLabel.text = line.Name;
        foreach (char character in line.Text)
        {
            _DialogueLabel.text += character;
            yield return new WaitForSeconds (_TypeSpeed);
        }
    }
}
