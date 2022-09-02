#ifndef FOG_VOLUMETRIC
#define FOG_VOLUMETRIC

#define G_SCATTERING 0.2
#define NB_STEPS 150

float ComputeScattering(float lightDotView)
{
    float result = 1.0f - G_SCATTERING * G_SCATTERING;
    result /= (4.0f * PI * abs(pow(1.0f + G_SCATTERING * G_SCATTERING - (2.0f * G_SCATTERING) * lightDotView, 1.5f)));
    return result;
}

void VolumetricFog_float(float2 UV, float3 Position, float3 CameraPosition, float DensityMultiplier, out float Density, out float3 Color){
    float3 rayVector = Position - CameraPosition;

    float rayLength = length(rayVector);
    float3 rayDirection = rayVector / rayLength;

    float stepLength = rayLength / NB_STEPS;

    float3 step = rayDirection * stepLength;

    float3 rayPosition = CameraPosition;

    float3 accumulatedColor = float3(0.0f, 0.0f, 0.0f);
    float3 lightColor = float3(1.0f, 1.0f, 1.0f);

    int totalSteps = 0;

    for (int i = 0; i < NB_STEPS; i++){

    #ifndef SHADERGRAPH_PREVIEW
        float4 ShadowCoord = TransformWorldToShadowCoord(rayPosition);
        Light mainLight = GetMainLight(ShadowCoord, rayPosition, 1.0);
        float ShadowValue = mainLight.shadowAttenuation;

        if (ShadowValue > 0.0){
            accumulatedColor += ComputeScattering(dot(rayDirection, mainLight.direction)) * mainLight.color;
            lightColor += mainLight.color;
        }


    #ifdef _ADDITIONAL_LIGHTS   
        // Additional light pass
        int additionalLights = 1;
        for (int i = 0; i < additionalLights; i++){
            Light light = GetAdditionalLight(i, rayPosition, 1.0);
            // Shadow value = shadow attenuation * distance attenuation
            ShadowValue = light.shadowAttenuation * light.distanceAttenuation;

            if (ShadowValue > 0.0){
                accumulatedColor += ComputeScattering(dot(rayDirection, light.direction)) * light.color;
                lightColor += light.color;
            }
            totalSteps++;
        }
    #endif

        rayPosition += step;
        totalSteps++;
    #endif
    }

    accumulatedColor /= totalSteps;
    lightColor /= totalSteps;

    Density = accumulatedColor.x * DensityMultiplier;
    Color = lightColor;
}

#endif
