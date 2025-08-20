    sampler2D input1 : register(s0);
    float4 borderColor : register(c0);
    float4 params : register(c1); // [original width, original height, border width, border height]

    float4 main(float2 uv : TEXCOORD) : COLOR
    {
        float4 originalColor = tex2D(input1, uv);
        // 縁取り範囲の計算
        float maxAlpha = 0.0;
        for (int x = -4; x <= 4; x++)
        {
            for (int y = -4; y <= 4; y++)
            {
                float2 offset = float2(x, y) * 0.01;
                float alpha = tex2D(input1, uv + offset).a;
                maxAlpha = max(maxAlpha, alpha);
            }
        }
        // アルファブレンド
        float fa = originalColor.a + 1e-10;
        float ba = maxAlpha * (1 - fa);
        float alpha = ba + fa;
        float3 color = ba * borderColor.rgb + fa * originalColor.rgb;
        return float4(color / alpha, alpha);
    }