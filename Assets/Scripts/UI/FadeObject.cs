using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class FadeObject : MonoBehaviour
{
    public float fadeDuration = 1.0f; 

   
    public IEnumerator Fading(Image image, TextMeshProUGUI text)
    {
        yield return StartCoroutine(Fade(0f, 1f, image, text));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(Fade(1f, 0f, image, text));
    }

    
    private IEnumerator Fade(float startAlpha, float endAlpha, Image image, TextMeshProUGUI text)
    {
        if (image == null || text == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        Color initialImageColor = image.color;
        Color initialTextColor = text.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            image.color = new Color(initialImageColor.r, initialImageColor.g, initialImageColor.b, newAlpha);

           
            text.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, newAlpha);

            yield return null;
        }

       
        image.color = new Color(initialImageColor.r, initialImageColor.g, initialImageColor.b, endAlpha);
        text.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, endAlpha);
    }
}


