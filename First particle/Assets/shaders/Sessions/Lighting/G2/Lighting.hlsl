#ifndef LIGHTING_INCLUDED
#define LIGHTING_INCLUDED

void GetMainLight_float(float3 WorldSpaceVertex ,out float3 Direction, out float3 Color, out float DistanceAttenuation, out float ShadowAttenuation)
{
    #ifdef SHADERGRAPH_PREVIEW
    //No existe ni escena ni  luz ni nada
    Direction = float3(1,1,-1);
    Color = 1;
    DistanceAttenuation = 1;
    ShadowAttenuation = 1;
    #else
    float4 shadowCoords = TransformWorldToShadowCoord(WorldSpaceVertex);
    Light mainLight = GetMainLight(shadowCoords);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAttenuation = mainLight.distanceAttenuation;
    //TODO: Sample shadow maps
    ShadowAttenuation = 1;
    #endif
}
#endif