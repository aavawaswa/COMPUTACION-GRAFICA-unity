#ifndef BLUR_INCLUDED
#define BLUR_INCLUDED

void BoxBlur3x3_float(UnityTexture2D Texture, UnitySamplerState Ss, float2 Uv, float2 PixelOffset, out float4 Color)
{
    Color = Texture.Sample (Ss,Uv + float2(-1,1)* PixelOffset);
    Color += Texture.Sample (Ss,Uv+ float2(0,1)* PixelOffset);
    Color += Texture.Sample (Ss,Uv+ float2(1,1)* PixelOffset);
    Color += Texture.Sample (Ss,Uv+ float2(-1,0)* PixelOffset);
    Color += Texture.Sample (Ss,Uv + float2(1,0)* PixelOffset);
    Color += Texture.Sample (Ss,Uv + float2(-1,-1)* PixelOffset);
    Color += Texture.Sample (Ss,Uv + float2(0,-1)* PixelOffset);
    Color += Texture.Sample (Ss,Uv + float2(1,1)* PixelOffset);
    
    Color /= 9;

}
void BoxBlur5x5_float(UnityTexture2D Texture, UnitySamplerState Ss, float2 Uv, float2 PixelOffset, out float4 Color)
{
    [uroll(25)]
    for (int y = -2; y < 2; y++)
    {
        for (int x = -2; x < 2; x++)
        {
            Color += Texture.Sample(Ss, Uv + float2(x,y)* PixelOffset);
        }

    }
    Color /= 25;
}
void BilateralBoxBlur5x5_float(UnityTexture2D Texture, UnitySamplerState Ss, float2 Uv, float2 PixelOffset, out float4 Color)
{
    //Color = 0;
    [uroll(10)]
    for (int i = -2; i < 2; i++)
    {
            Color += Texture.Sample(Ss, Uv + float2(i,0)* PixelOffset);
            Color += Texture.Sample(Ss, Uv + float2(-i,0)* PixelOffset);
            Color += Texture.Sample(Ss, Uv + float2(0,i)* PixelOffset);
            Color += Texture.Sample(Ss, Uv + float2(0,-i)* PixelOffset);
    }
    Color /= 10;
}

#endif