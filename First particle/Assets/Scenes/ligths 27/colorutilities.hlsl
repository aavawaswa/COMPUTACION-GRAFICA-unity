
void RGBtoHSV_float(float3 RGBcolor, out float3 HSVcolor)
{
float3 c = RGBcolor;

float cMin = min(min(c.r,c.g),c.b);
float cMax = max(max(c.r,c.g),c.b);
float delta = cMax-cMin;

HSVcolor=0;
HSVcolor.x=1;
HSVcolor.y = cMax == 0 ? 0 : (delta/cMax);
HSVcolor.z = cMax;
}







