sampler2D input1 : register(s0);
sampler2D input2 : register(s1);

float4 D1DxS1Sx : register(c0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 Back = tex2D(input1, uv);
    float4 Front = tex2D(input2, uv);

    float2 F = D1DxS1Sx.xz + D1DxS1Sx.yw * float2(Front.a, Back.a);
    // final alpha
    float a = Back.a * F.x + Front.a * F.y;

    // blend
    float3 Color = Back.rgb * Front.rgb;
    // pre-composite
    Color = Back.a * Color + (1.0 - Back.a) * Front.rgb;
    // composite
    Color = (Back.a * F.x * Back.rgb + Front.a * F.y * Color) / (a + 1e-6);
    return float4(Color, a);
}