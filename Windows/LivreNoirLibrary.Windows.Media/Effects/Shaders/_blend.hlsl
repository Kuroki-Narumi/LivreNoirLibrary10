// see https://qiita.com/kerupani129/items/4bf75d9f44a5b926df58
// see https://www.w3.org/TR/compositing-1/#blending

sampler2D input1 : register(s0);
sampler2D input2 : register(s1);
float blendMode : register(c1);

float4 D1DxS1Sx : register(c0); // [{0, 1}, {-1, 0, 1}, {0, 1}, {-1, 0, 1}]
/*
* コンポジットモードとPorter Duff定数の対応
*        mode     : [D1, Dx, S1, Sx] -> [  DstF  ,  SrcF   ]
*       Clear     : [ 0,  0,  0,  0] -> [ 0      , 0       ]
*      Source     : [ 0,  0,  1,  0] -> [ 0      , 1       ]
* Destination     : [ 1,  0,  0,  0] -> [ 1      , 0       ]
*      Source Over: [ 1, -1,  1,  0] -> [ 1 - S.a, 1       ]
* Destination Over: [ 1,  0,  1, -1] -> [ 1      , 1 - D.a ]
*      Source In  : [ 0,  0,  0,  1] -> [ 0      ,     D.a ]
* Destination In  : [ 0,  1,  0,  0] -> [     S.a, 0       ]
*      Source Out : [ 0,  0,  1, -1] -> [ 0      , 1 - D.a ]
* Destination Out : [ 1, -1,  0,  0] -> [ 1 - S.a, 0       ]
*      Source Atop: [ 1, -1,  0,  1] -> [ 1 - S.a,     D.a ]
* Destination Atop: [ 0,  1,  1, -1] -> [     S.a, 1 - D.a ]
*         Xor     : [ 1, -1,  1, -1] -> [ 1 - S.a, 1 - D.a ]
*     Lighter     : [ 0,  1,  0,  1] -> [ 1      , 1       ]
*/

#define multiply(a, b) (a * b)
#define screen(a, b) (1.0 - (1.0 - a) * (1.0 - b))
#define hardLight(a, b) lerp(screen(a, b * 2.0 - 1.0), multiply(a, b * 2.0), step(b, 0.5))

float4 softLight(float4 Back, float4 Front)
{
    float4 D = lerp(sqrt(Back), ((Back * 16.0 - 12.0) * Back + 4) * Back, step(Back, 0.25));
    return lerp(Back + (Front * 2.0 - 1.0) * (D - Back), Back - (1.0 - Front * 2.0) * Back * (1.0 - Back), step(Front, 0.5));
}

float luminous(float4 color)
{
    return color.r * 0.3 + color.g * 0.59 + color.b * 0.11;
}

float4 clipColor(float4 color) {
    float l = luminous(color);
    float mn = min(min(color.r, color.g), color.b);
    float mx = max(max(color.r, color.g), color.b);
    if (mn < 0)
    {
        color = l + (color - l) * saturate(l / (l - mn));
    }
    if (mx > 1)
    {
        color = l + (color - l) * saturate((1.0 - l) / (mx - l));
    }
    return color;
}

float4 setLum(float4 color, float l)
{
    return clipColor(color + (l - luminous(color)));
}

float sat(float4 color)
{
    return max(max(color.r, color.g), color.b) - min(min(color.r, color.g), color.b);
}

float4 setSat(float4 color, float s)
{
    float maxVal = max(max(color.x, color.y), color.z);
    float minVal = min(min(color.x, color.y), color.z);
    float midVal = color.x + color.y + color.z - maxVal - minVal; // 中間値

    float3 result = float3(0.0, 0.0, 0.0);
    if (maxVal > minVal) {
        float range = maxVal - minVal;
        result = float3(s, ((midVal - minVal) * s) / range, 0.0);
    }

    // 最大値の位置を特定（スウィズルで並べ替え）
    float3 sorted = float3(maxVal, midVal, minVal);
    if (color.x == maxVal) {
        return float4(result.x, result.y, result.z, color.a);
    }
    else if (color.y == maxVal) {
        return float4(result.y, result.x, result.z, color.a);
    }
    else {
        return float4(result.z, result.y, result.x, color.a);
    }
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    /*
    *   C := 合成後の色
    *   a := 合成後のアルファ
    *   Back := ベースの色
    *   Front := 合成する画像の色
    *   B(Back, Front) := ブレンド関数(アルファを考慮しない)
    *   BackF, FrontF := Porter Duff 演算の定数
    *   C' := ブレンド後コンポジット前の色
    *
    *   a = Back.a * BackF + Front.a * FrontF
    *   C' = Back.a * B(Back,Front) + (1 - Back.a) * Front
    *   C = (Back.a * BackF * Back + Front.a * FrontF * C') / a
    */

    float4 Back = tex2D(input1, uv);
    float4 Front = tex2D(input2, uv);

    float2 F = D1DxS1Sx.xz + D1DxS1Sx.yw * float2(Front.a, Back.a);
    // 合成後アルファ
    float a = Back.a * F.x + Front.a * F.y;

    // ブレンドモード
    float4 Color = Front;
    // 分離成分ブレンド
    switch ((int)max(blendMode, 0))
    {
    case 0: // Alpha
        break;
    case 1: // Add
        Color = min(Back + Front, 1.0);
        break;
    case 2: // Subtract
        Color = max(Back - Front, 0.0);
        break;
    case 3: // Multiply
        Color = multiply(Back, Front);
        break;
    case 4: // Screen
        Color = screen(Back, Front);
        break;
    case 5: // Overlay
        Color = hardLight(Front, Back);
        break;
    case 6: // Darken
        Color = min(Back, Front);
        break;
    case 7: // Lighten
        Color = max(Back, Front);
        break;
    case 8: // Color Dodge
        Color = min(Back / (1.0 - Front), 1.0);
        break;
    case 9: // Color Burn
        Color = 1.0 - min((1.0 - Back) / Front, 1.0);
        break;
    case 10: // Hard Light
        Color = hardLight(Back, Front);
        break;
    case 11: // Soft Light
        Color = softLight(Back, Front);
        break;
    case 12: // Difference
        Color = abs(Back - Front);
        break;
    case 13: // Exclusion
        Color = Back + Front - Back * Front * 2.0;
        break;
    case 14: // Hue
        Color = setLum(setSat(Front, sat(Back)), luminous(Back));
        break;
    case 15: // Saturation
        Color = setLum(setSat(Back, sat(Front)), luminous(Back));
        break;
    case 16: // Color
        Color = setLum(Front, luminous(Back));
        break;
    case 17: // Luminosity
        Color = setLum(Back, luminous(Front));
        break;
    }
    // pre-composite
    Color = Back.a * Color + (1.0 - Back.a) * Front;
    // composite
    Color = (Back.a * F.x * Back + Front.a * F.y * Color) / (a + 1e-6);
    return float4(Color.rgb, a);
}