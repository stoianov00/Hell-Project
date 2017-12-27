using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
public class Test
{
    private HeroInventory sut;

    [SetUp]
    public void TestInit()
    {
        this.sut = new HeroInventory();
    }

    [Test]
    public void AddRecipeItem()
    {
        // Arrange
        var recipeItem = new RecipeItem("item", 10, 20, 30, 40, 50, new List<string>());

        // Act
        this.sut.AddRecipeItem(recipeItem);

        Type clazz = typeof(HeroInventory);
        FieldInfo field = clazz.GetField("recipeItems", BindingFlags.Instance | BindingFlags.NonPublic);
        var collection = (Dictionary<string, IRecipe>)field?.GetValue(this.sut);

        // Assert
        Assert.AreEqual(1, collection?.Count);
    }

    [Test]
    public void AddCommonItem()
    {
        // Arrange
        var commonItem = new CommonItem("item", 10, 20, 30, 40, 50);

        // Act
        this.sut.AddCommonItem(commonItem);

        Type clazz = typeof(HeroInventory);
        FieldInfo field = clazz.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .FirstOrDefault(f => f.GetCustomAttributes(typeof(ItemAttribute)) != null);

        var collection = (Dictionary<string, IItem>)field?.GetValue(this.sut);

        // Assert
        Assert.AreEqual(1, collection?.Count);
    }

    [Test]
    public void StrengthBonusFromItems()
    {
        // Arrange
        var commonItem = new CommonItem("item", 10, 20, 30, 40, 50);

        // Act
        this.sut.AddCommonItem(commonItem);

        // Assert
        Assert.AreEqual(10, this.sut.TotalStrengthBonus);
    }

    [Test]
    public void AgilityBonusFromItems()
    {
        // Arrange
        var commonItem = new CommonItem("item", 10, 20, 30, 40, 50);

        // Act
        this.sut.AddCommonItem(commonItem);

        // Assert
        Assert.AreEqual(20, this.sut.TotalAgilityBonus);
    }

    [Test]
    public void IntelligenceBonusFromItems()
    {
        // Arrange
        var commonItem = new CommonItem("item", 10, 20, 30, 40, 50);

        // Act
        this.sut.AddCommonItem(commonItem);

        // Assert
        Assert.AreEqual(30, this.sut.TotalIntelligenceBonus);
    }

    [Test]
    public void HitPointsBonusFromItems()
    {
        // Arrange
        var commonItem = new CommonItem("item", 10, 20, 30, 40, 50);

        // Act
        this.sut.AddCommonItem(commonItem);

        // Assert
        Assert.AreEqual(40, this.sut.TotalHitPointsBonus);
    }

    [Test]
    public void DamageBonusFromItems()
    {
        // Arrange
        var commonItem = new CommonItem("item", 10, 20, 30, 40, 50);

        // Act
        this.sut.AddCommonItem(commonItem);

        // Assert
        Assert.AreEqual(50, this.sut.TotalDamageBonus);
    }

}