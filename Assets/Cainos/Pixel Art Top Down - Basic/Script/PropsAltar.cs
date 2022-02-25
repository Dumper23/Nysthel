using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public List<Light2D> lights;
        public float lerpSpeed;

        private bool inPortal = false;
        private Color curColor;
        private Color targetColor;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                inPortal = true;
                targetColor = new Color(1, 1, 1, 1);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                inPortal = false;
                targetColor = new Color(1, 1, 1, 0);
            }
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
            if (inPortal)
            {
                foreach (Light2D l in lights)
                {
                    l.intensity = Mathf.Lerp(l.intensity, 1f, lerpSpeed * Time.deltaTime);
                }
            }
            else
            {
                foreach (Light2D l in lights)
                {
                    l.intensity = Mathf.Lerp(l.intensity, 0f, lerpSpeed * Time.deltaTime);
                }
            }
        }
    }
}
