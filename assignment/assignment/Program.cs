using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class Program
{
    static Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();
    static string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recipes.json");


    static void UpdateRecipe()
    {
        Console.WriteLine("----- Update Recipe -----");
        Console.WriteLine("1. Update ID");
        Console.WriteLine("2. Update Name");
        Console.WriteLine("3. Update Ingredients");
        Console.WriteLine("4. Update Steps");
        Console.WriteLine("5. Update Difficulty");
        Console.Write("Enter your choice (1-5): ");

        string updateChoice = Console.ReadLine();
        Console.Write("Enter Recipe ID to update: ");
        string updateId = Console.ReadLine();

        if (recipes.ContainsKey(updateId))
        {
            Recipe recipeToUpdate = recipes[updateId];

            switch (updateChoice)
            {
                case "1":
                    Console.Write("Enter new Recipe ID: ");
                    string newId = Console.ReadLine();
                    recipeToUpdate.Id = newId;
                    break;
                case "2":
                    Console.Write("Enter new Recipe Name: ");
                    string newName = Console.ReadLine();
                    recipeToUpdate.Name = newName;
                    break;
                case "3":
                    Console.Write("Enter new Ingredients (comma-separated): ");
                    string newIngredientsInput = Console.ReadLine();
                    recipeToUpdate.Ingredients = new List<string>(newIngredientsInput.Split(','));
                    break;
                case "4":
                    Console.Write("Enter new Steps (comma-separated): ");
                    string newStepsInput = Console.ReadLine();
                    recipeToUpdate.Steps = new List<string>(newStepsInput.Split(','));
                    break;
                case "5":
                    Console.Write("Enter new Recipe Difficulty Level: ");
                    string newDifficulty = Console.ReadLine();
                    recipeToUpdate.Difficulty = newDifficulty;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }

            Console.WriteLine($"Recipe {updateId} updated successfully!");

            
            SaveRecipesToFile(recipes, jsonFilePath);
        }
        else
        {
            Console.WriteLine($"Recipe with ID {updateId} not found.");
        }
    }

    static void DeleteRecipe()
    {
        Console.WriteLine("----- Delete Recipe -----");
        Console.Write("Enter Recipe ID to delete: ");
        string deleteId = Console.ReadLine();

        if (recipes.ContainsKey(deleteId))
        {
            Recipe deletedRecipe = recipes[deleteId];
            recipes.Remove(deleteId);

            Console.WriteLine($"Recipe {deletedRecipe.Name} with ID {deleteId} deleted successfully!");

            
            SaveRecipesToFile(recipes, jsonFilePath);
        }
        else
        {
            Console.WriteLine($"Recipe with ID {deleteId} not found.");
        }
    }
    static void Main()
    {
        LoadRecipesFromFile(jsonFilePath);
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("----- Food & Recipe Management System -----");
            Console.WriteLine("1. Add Recipe");
            Console.WriteLine("2. View Recipe by ID");
            Console.WriteLine("3. Update Recipe");
            Console.WriteLine("4. Delete Recipe");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice (1-5): ");
            
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddRecipe();
                    break;
                case "2":
                    ViewRecipe();
                    break;
                case "3":
                    UpdateRecipe();
                    break;
                case "4":
                    DeleteRecipe();
                    break;
                case "5":
                    exit = true;
                    Console.WriteLine("See you soon!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }
        }
    }

    static void AddRecipe()
    {
        foreach (Recipe r in recipes.Values)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Recipe ID= " + r.Id);
            Console.WriteLine("Recipe Name= " + r.Name);

            Console.WriteLine("Recipe Ingredients= ");
            foreach (string ingredient in r.Ingredients)
            {
                Console.WriteLine("- " + ingredient);
            }

            Console.WriteLine("Recipe Steps= ");
            foreach (string step in r.Steps)
            {
                Console.WriteLine("- " + step);
            }

            Console.WriteLine("Recipe Difficulty Level= " + r.Difficulty);
            Console.WriteLine("------------------------------------");
            Console.WriteLine("------------------------------------");
            Console.WriteLine();
        }

        Console.WriteLine("Do you want to add a new recipe? (yes/no)");
        string response = Console.ReadLine();

        if (response.ToLower() == "yes")
        {
            Console.Write("Enter Recipe ID: ");
            string newId = Console.ReadLine();

            Console.Write("Enter Recipe Name: ");
            string newName = Console.ReadLine();

            Console.Write("Enter Ingredients (comma-separated): ");
            string ingredientsInput = Console.ReadLine();
            List<string> newIngredients = new List<string>(ingredientsInput.Split(','));

            Console.Write("Enter Steps (comma-separated): ");
            string stepsInput = Console.ReadLine();
            List<string> newSteps = new List<string>(stepsInput.Split(','));

            Console.Write("Enter Recipe Difficulty Level: ");
            string newDifficulty = Console.ReadLine();

            Recipe newRecipe = new Recipe(newId, newName, newIngredients, newSteps, newDifficulty);
            recipes.Add(newId, newRecipe);

            Console.WriteLine($"Recipe {newName} added successfully!");

            DisplayRecipes(recipes.Values);
            SaveRecipesToFile(recipes, jsonFilePath);
        }
    }

    static void ViewRecipe()
    {
        Console.WriteLine("Do you want to view a recipe by ID? (yes/no)");
        string viewResponse = Console.ReadLine();

        if (viewResponse.ToLower() == "yes")
        {
            Console.Write("Enter Recipe ID to view: ");
            string viewId = Console.ReadLine();

            if (recipes.ContainsKey(viewId))
            {
                Recipe viewedRecipe = recipes[viewId];
                Console.WriteLine("Viewing Recipe:");
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Recipe ID= " + viewedRecipe.Id);
                Console.WriteLine("Recipe Name= " + viewedRecipe.Name);
                Console.WriteLine("Recipe Ingredients= " + string.Join(", ", viewedRecipe.Ingredients));
                Console.WriteLine("Recipe Steps= " + string.Join(", ", viewedRecipe.Steps));
                Console.WriteLine("Recipe Difficulty Level= " + viewedRecipe.Difficulty);
                Console.WriteLine("------------------------------------");
            }
            else
            {
                Console.WriteLine($"Recipe with ID {viewId} not found.");
            }
        }
    }

    static void LoadRecipesFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    json = reader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        
                        recipes = JsonConvert.DeserializeObject<Dictionary<string, Recipe>>(json);
                        Console.WriteLine($"Recipes loaded from {filePath} successfully!");
                    }
                    catch (JsonSerializationException)
                    {
                        
                        List<Recipe> recipeList = JsonConvert.DeserializeObject<List<Recipe>>(json);
                        recipes = recipeList.ToDictionary(r => r.Id, r => r);
                        Console.WriteLine($"Recipes loaded from {filePath} successfully!");
                    }
                }
                else
                {
                    Console.WriteLine("No recipes found in the JSON file.");
                }
            }
            else
            {
                Console.WriteLine($"File '{filePath}' not found. Creating a new file.");
                SaveRecipesToFile(new Dictionary<string, Recipe>(), filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading recipes: {ex.Message}");
        }
    }

    static void SaveRecipesToFile(Dictionary<string, Recipe> recipes, string filePath)
    {
        try
        {
            string json = JsonConvert.SerializeObject(recipes.Values, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Recipes saved to {filePath} successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving recipes: {ex.Message}");
        }
    }

    static void DisplayRecipes(IEnumerable<Recipe> recipes)
    {
        foreach (Recipe r in recipes)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Recipe ID= " + r.Id);
            Console.WriteLine("Recipe Name= " + r.Name);
            Console.WriteLine("Recipe Ingredients= " + string.Join(", ", r.Ingredients));
            Console.WriteLine("Recipe Steps= " + string.Join(", ", r.Steps));
            Console.WriteLine("Recipe Difficulty Level= " + r.Difficulty);
            Console.WriteLine("------------------------------------");
            Console.WriteLine("------------------------------------");
        }
    }

    class Recipe
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        public string Difficulty { get; set; }

        public Recipe(string id, string name, List<string> ingredients, List<string> steps, string difficulty)
        {
            this.Id = id;
            this.Name = name;
            this.Ingredients = ingredients;
            this.Steps = steps;
            this.Difficulty = difficulty;
        }
    }
}