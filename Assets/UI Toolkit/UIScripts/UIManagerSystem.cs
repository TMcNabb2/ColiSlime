using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UIManagerSystem : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private Button colorChangeButton;
    private bool isColorChanging = false;

    private void Start()
    {
        // Get reference to UI Document
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        // Get button reference
        var root = uiDocument.rootVisualElement;
        colorChangeButton = root.Q<Button>("game-hud-ability__button-1");

        // Register button click callback
        colorChangeButton.clicked += OnColorChangeButtonClicked;
    }

    private void OnColorChangeButtonClicked()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Renderer playerRenderer = player.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                StartCoroutine(ChangeColorTemporarily(playerRenderer));
            }
        }
    }

    private IEnumerator ChangeColorTemporarily(Renderer targetRenderer)
    {
        if (isColorChanging) yield break;
        isColorChanging = true;
        Material originalMaterial = targetRenderer.material;
        Material tempMaterial = new Material(originalMaterial);
        targetRenderer.material = tempMaterial;
        tempMaterial.color = Color.green;
        yield return new WaitForSeconds(1f);
        targetRenderer.material = originalMaterial;
        Destroy(tempMaterial);
        isColorChanging = false;
    }
}