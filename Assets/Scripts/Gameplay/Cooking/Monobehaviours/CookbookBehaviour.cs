using System;
using System.Collections.Generic;
using Gameplay.Cooking;
using Gameplay.Cooking.ScriptableObjects;
using TMPro;
using UnityEngine;
using Util;
using Util.Services;

public class CookbookBehaviour : MonoBehaviour
{
    private EventService es;

    [SerializeField] private TMP_Text cookbookText;

    // TODO: Make this not as jank. But it works for now
    [SerializeField] private List<FoodObject> foodList;

    [SerializeField] private List<RecipeObject> recipieList;

    private Dictionary<FoodObject, RecipeObject> recipies = new Dictionary<FoodObject, RecipeObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        es = ServicesLocator.Instance.Get<EventService>();
        es.Add(EventNames.SLIME_NEW_DESIRED_FOOD, OnSlimeNewDesiredFood());

        foreach (var recipeObject in recipieList)
        {
            recipies.Add(recipeObject.Results[0], recipeObject);
        }
    }

    private void OnDisable()
    {
        es.Remove(EventNames.SLIME_NEW_DESIRED_FOOD, OnSlimeNewDesiredFood());
    }

    private EventHandler OnSlimeNewDesiredFood()
    {
        return (sender, args) =>
        {
            Debug.Log($"[{GetType().Name}] New food desired, displaying!");
            FoodObjectEventArgs recipeArgs = args as FoodObjectEventArgs;
            
            if (args is null)
            {
                Debug.LogError($"[{GetType().Name}] Failed to cast to RecipieArgs, cookbook failing!");
                return;
            }

            RecipeObject recipie = recipies[recipeArgs.food];
            
            FoodObject result = recipie.Results[0];

            int recipeResult = foodList.IndexOf(result);

            List<int> ingredients = new List<int>();
            foreach (var ingredient in recipie.Ingredients)
            {
                ingredients.Add(foodList.IndexOf(ingredient));
            }

            string recipe = "";
            for (int i = 0; i < ingredients.Count; ++i)
            {
                recipe += $"<sprite={ingredients[i]}>";
                if (i < ingredients.Count - 1)
                {
                    recipe += " + ";
                }
            }

            recipe += $" = <sprite={recipeResult}>";

            cookbookText.text = recipe;
        };
    }
}
