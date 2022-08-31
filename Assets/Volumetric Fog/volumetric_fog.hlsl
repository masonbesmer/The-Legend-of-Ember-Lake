#ifndef FOG_VOLUMETRIC
#define FOG_VOLUMETRIC

// This is a neat trick to work around a bug in the shader graph when
// enabling shadow keywords. Created by @cyanilux
// https://github.com/Cyanilux/URP_ShaderGraphCustomLighting
#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

#define G_SCATTERING 0.0 // 0.0 for no scattering, 1.0 for full scattering (and -1.0 for reverse scattering)

// Compute the scattering of the fog using
// the scattering equation
float ComputeScattering(float lightDotView)
{
    float result = 1.0f - G_SCATTERING * G_SCATTERING;
    result /= (4.0f * PI * pow(abs(1.0f + G_SCATTERING * G_SCATTERING - (2.0f * G_SCATTERING) * lightDotView), 1.5f));
    return result;
}

float Random01(float2 UV){
    return frac(sin(dot(UV, float2(41, 289)))*45758.5453 );
}

void VolumetricFog_float(float2 UV, float3 Position, float3 ScreenPosition, float3 CameraPosition, float DensityMultiplier, int Steps, float MaxDistance, float Jitter, uint AdditionalLightSkipCount, out float Density, out float3 Color){
    // Get the vector from the position to the camera
    float3 rayVector = Position - CameraPosition;

    // Get the length and direction of the ray
    float rayLength = length(rayVector);
    rayLength = min(rayLength, MaxDistance);

    float3 rayDirection = rayVector / rayLength;

    // Calculate the ray step size
    float stepLength = rayLength / Steps;
    float3 step = rayDirection * stepLength;

    // Initialize the ray position 
    float3 rayPosition = CameraPosition;
    rayPosition = rayPosition + (stepLength * Random01(UV) * Jitter/100) * rayDirection;

    // Create variables to store the accumulated color and density
    // Color - The accumulated color from the lights
    // Density - The accumulated density from the lights and scattering
    float3 accumulatedColor = float3(0.0f, 0.0f, 0.0f);

    // Store the total steps taken
    int totalSteps = 0;

    for (int i = 0; i < Steps; i++){

    // Only compute if we're not previewing
    #ifndef SHADERGRAPH_PREVIEW
        //Get the shadow value from the main directional light
        float4 ShadowCoord = TransformWorldToShadowCoord(rayPosition);
        Light mainLight = GetMainLight(ShadowCoord, rayPosition, 1.0);
        float ShadowValue = mainLight.shadowAttenuation;

        // If the shadow value is above 0 we're not in shadow, so we can compute the lighting
        if (ShadowValue > 0.0){
            accumulatedColor += ComputeScattering(dot(rayDirection, mainLight.direction)) * mainLight.color;
        }

        // If we're using additional lights, we need to loop through them and compute the lighting
        #ifdef _ADDITIONAL_LIGHTS   

            // Additional light pass - 8 lights supported. We also skip every 4 steps to save performance.
            // Note that shadow maps are *very* expensive!
            if (i % AdditionalLightSkipCount == 0){
            #if defined(_FORWARD_PLUS) || defined(USE_CLUSTERED_LIGHTING)
                for (uint lightIndex = 0; lightIndex < max(8, MAX_VISIBLE_LIGHTS_UBO); lightIndex++){
                    #if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
                    float4 lightPositionWS = _AdditionalLightsBuffer[lightIndex].position;
                    #else
                    float4 lightPositionWS = _AdditionalLightsPosition[lightIndex];
                    #endif

                    // Only compute if the light position is less than the maximum distance from the camera (Position)
                    if (length(lightPositionWS - rayPosition) < MaxDistance){                 
                        Light light = GetAdditionalPerObjectLight(lightIndex, rayPosition);

                        // Shadow value = shadow attenuation * distance attenuation
                        ShadowValue = light.shadowAttenuation;

                        if (ShadowValue > 0.0){
                            accumulatedColor += ComputeScattering(dot(rayDirection, light.direction)) * light.color * light.distanceAttenuation;
                        }
                    }
                }
            #else
                // Check we're using clustered lighting
                LIGHT_LOOP_BEGIN(MAX_VISIBLE_LIGHTS_UBO)
                    // Get the light, calulate the shadow value and add the lighting (same as above)
                    #if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
                    float4 lightPositionWS = _AdditionalLightsBuffer[lightIndex].position;
                    #else
                    float4 lightPositionWS = _AdditionalLightsPosition[lightIndex];
                    #endif
                    
                    // Only compute if the light position is less than the maximum distance from the camera (Position)
                    if (length(lightPositionWS - rayPosition) < MaxDistance){                 
                        Light light = GetAdditionalPerObjectLight(lightIndex, rayPosition);

                        // Shadow value = shadow attenuation * distance attenuation
                        ShadowValue = light.shadowAttenuation;

                        if (ShadowValue > 0.0){
                            accumulatedColor += ComputeScattering(dot(rayDirection, light.direction)) * light.color * light.distanceAttenuation;
                        }
                    }
                LIGHT_LOOP_END
                
            #endif
            }
        #endif

        // Now we step the ray forward, and add another step to the ray position
        rayPosition += step;
        totalSteps++;
    #endif
    }

    // Once we've finished the loop, we can compute the density and color
    // We normalize the color and density by the number of steps taken
    accumulatedColor /= totalSteps;

    // Then we can return the density and color
    Density = accumulatedColor.x * DensityMultiplier;
    Color = accumulatedColor;
}


#endif // FOG_VOLUMETRIC
