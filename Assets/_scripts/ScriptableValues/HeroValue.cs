using UnityEngine;

[CreateAssetMenu(fileName = "HeroValue", menuName = "Scriptable Objects/Scriptable Value/Complex/Hero", order = 1)]
public class HeroValue : GenericValue<HeroID>
{
}

[System.Serializable]
public struct HeroID
{
    public int heroName;
    public int heroLevel;

    public HeroID(int heroName, int heroLevel)
    {
        this.heroName = heroName;
        this.heroLevel = heroLevel;
    }
}
