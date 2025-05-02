using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

namespace General
{
    public class Tp : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] float transitionTime = 0.5f;

        private CanvasGroup canvas;


        public static Tp Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }


        public void newTp(GameObject gameObject, Transform newPos)
        {
            canvas.DOFade(1f, transitionTime)
                .SetUpdate(true)
                .OnStart(() =>
                {
                    canvas.interactable = true;
                    canvas.blocksRaycasts = true;
                })
                .OnComplete(() =>
                {
                    StartCoroutine(waitTime(gameObject, newPos));
                });
        }

        private IEnumerator waitTime(GameObject gameObject, Transform newPos)
        {
            yield return new WaitForSecondsRealtime(1f);

            gameObject.transform.position = newPos.position;
            gameObject.transform.rotation = newPos.rotation;
            gameObject.transform.localScale = newPos.localScale; // ?? Añade esta línea

            yield return new WaitForSecondsRealtime(1f);

            canvas.DOFade(0f, transitionTime)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    canvas.interactable = false;
                    canvas.blocksRaycasts = false;
                });
        }

        private void Start()
        {
            canvas = GetComponent<CanvasGroup>();
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
    }
}
