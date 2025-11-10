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
            magnitude += deltaArray[i] * deltaArray[i];
         
            if (weighted)
            {
                magnitude *= (i + 1);
                denominator += (i + 1);
            }
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

//Possible to map data to a vector space clustered by topic/coherence and use the deltas (represent coordinats -1 to 1) to read data points(map coordinates to data set). (Efficient tree structure!?)
//Also one could leave empty spaces inside the data cloud to extend the data set later without losing progress

   public float delta = 0;
   private float bufferLength = 5;
   private float clampThreshold = .25f;

   public void Forward(float[][] network, int layer)
   {
       for(int i = 0; i < network[layer - 1].Length; i ++)
           delta += network[layer - 1][i].delta * weight[i];

       deltaArray[currentIndex] = deltaArray[0];
       deltaArray[0] = (float)Math.Tanh(delta + scalar);

       for (int i = 0; i < deltaArray.Length; i++)
           delta += (deltaArray[i] * deltaArray[i]) * deltaArray[i];

       delta = delta / bufferLength;

       if (delta > clampThreshold || delta < -clampThreshold)
           delta = 0f;

       currentIndex++;
       if (currentIndex == deltaArray.Length)
           currentIndex = 1;
   }

//---------------------------------------------------------------------------------------------------
   public int interval = 500;
   public int iteration = 0;
   public float delta = 0;
   private float bufferLength = 5;
   private float clampThreshold = .25f;

   public void Forward(float[][] network, int layer)
   {
       if(iteration != 0)
       { 
           iteration --;
           return;
       }
    
       for(int i = 0; i < network[layer - 1].Length; i ++)
           delta += network[layer - 1][i].delta * weight[i];

       deltaArray[currentIndex] = deltaArray[0];
       deltaArray[0] = (float)Math.Tanh(delta + scalar);

       for (int i = 0; i < deltaArray.Length; i++)
           delta += (deltaArray[i] * deltaArray[i]) * deltaArray[i];

       delta = delta / bufferLength;

       if (delta > clampThreshold || delta < -clampThreshold)
           delta = 0f;

       currentIndex++;
       if (currentIndex == deltaArray.Length)
           currentIndex = 1;

       iteration = interval;
   }
