public class Population
{
    public Variation[] parents;
    public Variation[] variations;

    public Population(Variation[] variations, Variation[] parents)
    {
        this.variations = variations;
        this.parents = parents;
    }

    public Variation GetBestVariation()
    {
        Variation bestVariation = null;
        for (int i = 0; i < variations.Length; i++)
            if (!float.IsNaN(variations[i].fitnessValue)
                && (bestVariation == null || bestVariation.fitnessValue < variations[i].fitnessValue))
            {
                bestVariation = variations[i];
            }

        return bestVariation;
    }
}
