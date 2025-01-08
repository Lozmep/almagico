using UnityEngine;
using TMPro;
using System.Collections; // Necesario para trabajar con TextMeshProUGUI

public class TextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText; // Asigna esto en el Inspector
    [SerializeField] private GameObject gameObjectText; // Asigna esto en el Inspector
    [SerializeField] private float fadeDuration = 1f; // Duración para hacer el texto completamente visible

    private void Start()
    {
        gameObjectText.SetActive(false);
    }

    // Método para hacer visible el texto gradualmente
    public void ShowTextWithFade(string newText)
    {
        if (myText != null)
        {
            gameObjectText.SetActive(true);
            myText.text = newText; // Cambia el texto
            StartCoroutine(FadeInText()); // Inicia la coroutine para que el texto se haga visible gradualmente
        }
        else
        {
            Debug.LogError("TextMeshProUGUI no está asignado.");
        }
    }

    // Coroutine para hacer que el texto sea visible gradualmente
    private IEnumerator FadeInText()
    {

        // Inicializa el alpha del texto en 0 (invisible)
        Color textColor = myText.color;
        textColor.a = 0f;
        myText.color = textColor;

        // Gradualmente aumenta el alpha hasta que sea 1 (totalmente visible)
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            textColor.a = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            myText.color = textColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegura que el alpha sea 1 al final
        textColor.a = 1f;
        myText.color = textColor;

        timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            textColor.a = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            myText.color = textColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gameObjectText.SetActive(false);
    }


}