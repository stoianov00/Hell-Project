using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public class HeroManager : IManager
{
    private readonly IDictionary<string, IHero> heroes;

    public HeroManager()
    {
        this.heroes = new Dictionary<string, IHero>();
    }

    /// <summary>
    /// Added heroes to dictionary 
    /// </summary>
    /// <param name="arguments"></param>
    public string AddHero(IList<string> arguments)
    {
        string result = null;

        string heroName = arguments[0];
        string heroType = arguments[1];

        try
        {
            Type clazz = Type.GetType(heroType);
            ConstructorInfo[] constructors = clazz?.GetConstructors();
            IHero hero = (IHero)constructors?[0].Invoke(new object[] { heroName });
            this.heroes.Add(hero.Name, hero);

            result = string.Format(Constants.HeroCreateMessage, heroType, hero.Name);
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return result;
    }

    /// <summary>
    /// Added collection with items in heroes inventory 
    /// </summary>
    /// <param name="arguments"></param>
    public string AddItem(IList<string> arguments)
    {
        string itemName = arguments[0];
        string heroName = arguments[1];
        int strengthBonus = int.Parse(arguments[2]);
        int agilityBonus = int.Parse(arguments[3]);
        int intelligenceBonus = int.Parse(arguments[4]);
        int hitPointsBonus = int.Parse(arguments[5]);
        int damageBonus = int.Parse(arguments[6]);

        IItem newItem = new CommonItem(itemName, strengthBonus, agilityBonus, intelligenceBonus, hitPointsBonus, damageBonus);
        this.heroes[heroName].AddItem(newItem);

        string result = null;

        result = string.Format(Constants.ItemCreateMessage, newItem.Name, heroName);
        return result;
    }

    /// <summary>
    /// Formated the Output
    /// </summary>
    /// <param name="argsList"></param>
    public string Quit(object argsList)
    {
        var sb = new StringBuilder();

        int counter = 1;

        var orderedHeroes = this.heroes
            .OrderByDescending(h => h.Value.PrimaryStats)
            .ThenByDescending(h => h.Value.SecondaryStats)
            .ToDictionary(x => x.Key, y => y.Value);

        foreach (var hero in orderedHeroes)
        {
            sb.AppendLine($"{counter++}. {hero.Value.GetType().Name}: {hero.Key}")
                .AppendLine($"###HitPoints: {hero.Value.HitPoints}")
                .AppendLine($"###Damage: {hero.Value.Damage}")
                .AppendLine($"###Strength: {hero.Value.Strength}")
                .AppendLine($"###Agility: {hero.Value.Agility}")
                .AppendLine($"###Intelligence: {hero.Value.Intelligence}");

            if (hero.Value.Items.Count == 0)
            {
                sb.AppendLine($"###Items: None");
            }
            else
            {
                sb.Append($"###Items: ");
                var items = hero.Value.Items.Select(i => i.Name).ToList();
                sb.AppendLine(string.Join(", ", items));
            }
        }

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Added collection with special items in heroes
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public string AddRecipe(IList<string> arguments)
    {
        string itemName = arguments[0];
        string heroName = arguments[1];
        int strengthBonus = int.Parse(arguments[2]);
        int agilityBonus = int.Parse(arguments[3]);
        int intelligenceBonus = int.Parse(arguments[4]);
        int hitPointsBonus = int.Parse(arguments[5]);
        int damageBonus = int.Parse(arguments[6]);

        var requiredItems = arguments.Skip(7).ToList();

        IRecipe newRecipe = new RecipeItem(itemName, strengthBonus, agilityBonus, intelligenceBonus, hitPointsBonus, damageBonus, requiredItems);
        this.heroes[heroName].AddRecipe(newRecipe);

        string result = null;

        result = string.Format(Constants.RecipeCreatedMessage, newRecipe.Name, heroName);
        return result;
    }

    public string Inspect(IList<string> arguments)
    {
        string heroName = arguments[0];

        return this.heroes[heroName].ToString();
    }

}