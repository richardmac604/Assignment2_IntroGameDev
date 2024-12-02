using UnityEngine;

public class DayNightToggle : MonoBehaviour
{
    [SerializeField] private Material dayNightMaterial;
    [SerializeField] private GameObject Fog;
    private bool isDay = true;
    private bool isFog = true;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            isDay = !isDay;
            UpdateLighting();
            Debug.Log("Change Night or day");
        }

       
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFog = !isFog;
            Fog.SetActive(isFog);
        }
    }

    private void UpdateLighting()
    {
        // Set the blend factor based on the current mode
        dayNightMaterial.SetFloat("_BlendFactor", isDay ? 1f : 0f);
    }
}
