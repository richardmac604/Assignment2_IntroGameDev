using UnityEngine;
using UnityEngine.InputSystem;

public class FogController : MonoBehaviour
{
    public Color fogColor = Color.gray;
    public float fogDensity = 0.05f;

    public Material fogMaterial;
    private bool isFogEnabled = true;

    private FogControl fogControls;

    private void Awake()
    {
        fogControls = new FogControl();
        fogMaterial = new Material(Shader.Find("Custom/Fog"));
    }

    private void OnEnable()
    {
        fogControls.Enable();
        fogControls.ControlFog.FogToggle.performed += OnToggleFog;
    }

    private void OnDisable()
    {
        fogControls.ControlFog.FogToggle.performed -= OnToggleFog;
        fogControls.Disable();
    }

    private void Start()
    {
        ApplyFogSettings();
    }

    private void OnToggleFog(InputAction.CallbackContext context)   
    {
        isFogEnabled = !isFogEnabled;
        Shader.SetGlobalFloat("_FogDensity", isFogEnabled ? fogDensity : 0f);
    }

    private void ApplyFogSettings()
    {
        if (fogMaterial != null)
        {
            Shader.SetGlobalColor("_FogColor", fogColor);
            Shader.SetGlobalFloat("_FogDensity", fogDensity);
        }
        else
        {
            Debug.LogError("Fog material is not assigned!");
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (isFogEnabled && fogMaterial != null)
        {
            Graphics.Blit(src, dest, fogMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
