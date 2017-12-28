using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public abstract class AbstractHero : IHero, IComparable<AbstractHero>
{
    private readonly IInventory inventory;
    private long strength;
    private long agility;
    private long intelligence;
    private long hitPoints;
    private long damage;

    protected AbstractHero(string name, int strength, int agility, int intelligence, int hitPoints, int damage)
    {
        this.Name = name;
        this.strength = strength;
        this.agility = agility;
        this.intelligence = intelligence;
        this.hitPoints = hitPoints;
        this.damage = damage;
        this.inventory = new HeroInventory();
    }

    public string Name { get; private set; }

    public long Strength
    {
        get => this.strength + this.inventory.TotalStrengthBonus;
        set => this.strength = value;
    }

    public long Agility
    {
        get => this.agility + this.inventory.TotalAgilityBonus;
        set => this.agility = value;
    }

    public long Intelligence
    {
        get => this.intelligence + this.inventory.TotalIntelligenceBonus;
        set => this.intelligence = value;
    }

    public long HitPoints
    {
        get => this.hitPoints + this.inventory.TotalHitPointsBonus;
        set => this.hitPoints = value;
    }

    public long Damage
    {
        get => this.damage + this.inventory.TotalDamageBonus;
        set => this.damage = value;
    }

    public long PrimaryStats
    {
        get => this.Strength + this.Agility + this.Intelligence;
    }

    public long SecondaryStats
    {
        get => this.HitPoints + this.Damage;
    }

    // REFLECTION
    public ICollection<IItem> Items
    {
        get
        {
            Type clazz = typeof(HeroInventory);
            FieldInfo field = clazz.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(f => f.GetCustomAttributes(typeof(ItemAttribute)) != null);

            var collection = (Dictionary<string, IItem>)field?.GetValue(this.inventory);

            return collection?.Values.ToList();
        }
    }

    /// <summary>
    /// Added item to heroes inventories
    /// </summary>
    public void AddItem(IItem item)
    {
        this.inventory.AddCommonItem(item);
    }

    /// <summary>
    /// Add special item to heroes inventories
    /// </summary>
    /// <param name="recipe"></param>
    public void AddRecipe(IRecipe recipe)
    {
        this.inventory.AddRecipeItem(recipe);
    }

    /// <summary>
    /// Compared heroes
    /// </summary>
    /// <param name="other"></param>
    public int CompareTo(AbstractHero other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        var primary = this.PrimaryStats.CompareTo(other.PrimaryStats);

        return primary != 0 ? primary : this.SecondaryStats.CompareTo(other.SecondaryStats);
    }

    /// <summary>
    /// Formated Output
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Hero: {this.Name}, Class: {this.GetType().Name}")
            .AppendLine($"HitPoints: {this.HitPoints}, Damage: {this.Damage}")
            .AppendLine($"Strength: {this.Strength}")
            .AppendLine($"Agility: {this.Agility}")
            .AppendLine($"Intelligence: {this.Intelligence}");

        if (this.Items.Count == 0)
        {
            sb.AppendLine("Items: None");
        }
        else
        {
            sb.AppendLine($"Items:");
            foreach (var item in this.Items)
            {
                sb.AppendLine(item.ToString());
            }
        }

        return sb.ToString().Trim();
    }

}