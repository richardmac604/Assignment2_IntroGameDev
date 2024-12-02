using UnityEngine;

public class DayNightToggle : MonoBehaviour
{
    [SerializeField] private GameObject dayNightMaterial;
    [SerializeField] private GameObject Fog;
    [SerializeField] public Light lightSource;

    private Vector3 rotation = Vector3.zero;
    private bool isDay = true;
    private bool isFog = true;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            isDay = !isDay;
            dayNightMaterial.SetActive(isDay);
            if (isDay)
            {
              
                rotation.x = rotation.x + 190f;
                lightSource.transform.Rotate(rotation, Space.World);
            }
            else
            {
                
                rotation.x = rotation.x + 190f;
                lightSource.transform.Rotate(rotation, Space.World);
            }
           
        }

       
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFog = !isFog;
            Fog.SetActive(isFog);
          
        }
    }

  
}
