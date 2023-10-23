using UnityEngine;
using System.Collections.Generic;

public class LightmapPixelPicker : MonoBehaviour
{
    public Color surfaceColor;
    public float brightness1;
    public float brightness2;
    public LayerMask layerMask;
    public Light directionalLight;
    public Material enhancedShaderMaterial;
    public float maximumPossibleBrightness = 1.0f;

    void Update()
    {
        Raycast();
        // BRIGHTNESS APPROX
        brightness1 = (surfaceColor.r + surfaceColor.r + surfaceColor.b + surfaceColor.g + surfaceColor.g + surfaceColor.g) / 6;
        // BRIGHTNESS
        brightness2 = Mathf.Sqrt((surfaceColor.r * surfaceColor.r * 0.2126f + surfaceColor.g * surfaceColor.g * 0.7152f + surfaceColor.b * surfaceColor.b * 0.0722f));
        float normalizedBrightness = brightness2 / maximumPossibleBrightness;
        enhancedShaderMaterial.SetFloat("_DirectionalLightEnabled", normalizedBrightness);
    }

void Raycast()
    {
        // RAY TO PLAYER'S FEET
        Ray ray = new Ray(transform.position, -Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.magenta);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            // Calculate light direction
            Vector3 lightDirection = -directionalLight.transform.forward;

            // Calculate light intensity based on the directional light
            float lightIntensity = Mathf.Max(0f, Vector3.Dot(lightDirection, hitInfo.normal));

            // Check if the point is in shadow
            bool inShadow = Physics.Raycast(hitInfo.point, lightDirection, out RaycastHit shadowHit, Mathf.Infinity, layerMask);

            // Calculate surface color based on light intensity and shadow
            Color baseColor = Color.white; // You can set your base color here
            Color litColor = baseColor * lightIntensity;
            surfaceColor = inShadow ? litColor * 0.5f : litColor; // Dim color if in shadow
        }
    }
}