#ifndef LIGHTNING_INCLUDED
#define LIGHTNING_INCLUDED

void GetMainLight_float(float3 WorldSpaceVertex, out float3 Direction, out float3 Color, out float DistanceAttenuation, out float ShadowAttenuation)
{
    #ifdef SHADERGRAPH_PREVIEW
    Direction = float3 (1,1,-1);
    Color = 1;
    DistanceAttenuation = 1;
    ShadowAttenuation = 1;
    #else

    float4 shadowCoords = TransformWorldToShadowCoord(WorldSpaceVertex);
    Light mainLight = GetMainLight(shadowCoords);
    Direction = mainLight.cirection;
    Color = mainLight.color;
    DistanceAttenuation = mainLight.distanceAttenuation;
    ShadowAttenuation = 1;

    #endif
}
 #endif