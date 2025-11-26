sampler2D input1 : register(s0);
float4 color : register(c0);
static const float4 transparent = float4(0, 0, 0, 0);
static const float3 threshold = float3(0.00392f, 0.00392f, 0.00392f);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 source = tex2D(input1, uv);
    float3 comparison = step(abs(source.rgb - color.rgb), threshold);
    float isTransparent = min(min(comparison.x, comparison.y), comparison.z);
    return lerp(source, transparent, isTransparent);
}