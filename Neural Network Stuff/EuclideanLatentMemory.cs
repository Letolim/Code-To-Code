//Store delta over x iterations;
//
//for(int i = 0; i > layer - 1; i ++)
//	delta += [layer -1][n] * weight + bias;
//
//
//deltaArray.push(tanh(delta));
//deltaArray.RemoveAt(deltaArray.Length -1);
//
//float d = 0;
//
//for(int i = 0; i < deltaArray.Length; i ++)
//	d += deltaArray[i] * deltaArray[i] + deltaArray[i] * deltaArray[i];
//
//d = sqrt(d) / deltaArray.Length;
//
//delta = 1 - d;


//GPT-OSS edit
 public class MemoryNeurode
 {
    float delta = 0f;

    public void Forward(float[][] network, int layer)
    {
        // Reset delta for this forward pass
        delta = 0f;

        // -------------------------------------------------
        // 1. Compute raw weighted sum for the previous layer
        // -------------------------------------------------
        // network[layer‑1] holds the activations of the previous layer
            delta += network[layer - 1][i] * weight[i] + bias[i];
   
        // -------------------------------------------------
        // 2. Apply non‑linearity (tanh) and store in the ring buffer
        // -------------------------------------------------

        float tanhDelta = (float)Math.Tanh(delta);
        deltaArray[currentIndex] = deltaArray[0];
        deltaArray[0] = tanhDelta;

        // -------------------------------------------------
        // 3. Compute magnitude based on the ring buffer
        // -------------------------------------------------
        float magnitude = 0f;
        float denominator = 0f;

        for (int i = 0; i < deltaArray.Length; i++)
        {
            magnitude += deltaArray[i] * (i + 1);
            if (weighted)
                denominator += (i + 1);
        }

        // -------------------------------------------------
        // 4. Final delta calculation
        // -------------------------------------------------
        if (weighted && denominator != 0f)
        {
            delta = 1f - (float)Math.Sqrt(magnitude) / denominator;
        }
        else
        {
            delta = 1f - (float)Math.Sqrt(magnitude);
        }

        // -------------------------------------------------
        // 5. Advance circular index
        // -------------------------------------------------
        currentIndex++;
        if (currentIndex == deltaArray.Length)
            currentIndex = 1;   // keep index in [1, Length‑1]
    }
}