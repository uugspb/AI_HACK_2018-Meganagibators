public class NPCModel
{
    public float HP;
    public float Armor;
    public float Speed;
    public float Damage;
    public float FireRate;
    public float SpawnRate;

    public NPCModel(float HP, float Armor, float Speed, float Damage, float Rate, float SpawnRate)
    {
        this.HP = HP;
        this.Armor = Armor;
        this.Speed = Speed;
        this.Damage = Damage;
        this.FireRate = Rate;
        this.SpawnRate = SpawnRate;
    }

    public NPCModel(NPCModel from) : this(from.HP, from.Armor, from.Speed, from.Damage, from.FireRate, from.SpawnRate) { }
}
