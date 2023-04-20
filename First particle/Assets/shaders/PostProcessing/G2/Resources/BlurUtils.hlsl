#ifndef BLUR_INCLUDED
#define BLUR_INCLUDED

void BoxBlur3X3_float(UnityTexture2D Texture, UnitySamplerState Ss, float2 Uv, float2 PixelOffset, out float4 Result)
{
    Result = Texture.Sample(Ss, Uv); //Center
    Result += Texture.Sample(Ss, Uv + float2(-1,1) * PixelOffset); // Upper Left
    Result += Texture.Sample(Ss, Uv + float2(0,1) * PixelOffset); // Up
    Result += Texture.Sample(Ss, Uv + float2(1,1) * PixelOffset); // Upper Right
    Result += Texture.Sample(Ss, Uv + float2(-1,0) * PixelOffset); //  Left
    Result += Texture.Sample(Ss, Uv + float2(1,0) * PixelOffset); // Right
    Result += Texture.Sample(Ss, Uv + float2(-1,-1) * PixelOffset); // Lower Left
    Result += Texture.Sample(Ss, Uv + float2(0,-1) * PixelOffset); // Down
    Result += Texture.Sample(Ss, Uv + float2(1,-1) * PixelOffset); // Lower Right

    Result /= 9;
}

void BoxBlur5X5_float(UnityTexture2D Texture, UnitySamplerState Ss, float2 Uv, float2 PixelOffset, out float4 Result)
{
    [unroll(25)]
    for(int y = -2; y < 3; y++)
    {
        for(int x = -2; x < 3; x++)
        {
            Result += Texture.Sample(Ss, Uv + float2(x,y) * PixelOffset);
        }
    }
    Result/=25;
}

void BilateralBoxBlur5X5_float(UnityTexture2D Texture, UnitySamplerState Ss, float2 Uv, float2 PixelOffset, out float4 Result)
{
    Result = 0;
    [unroll(10)]
    for(int i = 0; i < 2; i++)
    {
        Result += Texture.Sample(Ss,Uv + float2(i,0) * PixelOffset);
        Result += Texture.Sample(Ss,Uv + float2(-i,0) * PixelOffset);
        Result += Texture.Sample(Ss,Uv + float2(0,i) * PixelOffset);
        Result += Texture.Sample(Ss,Uv + float2(0,-i) * PixelOffset);
    }
    Result /= 10;
}

#endif