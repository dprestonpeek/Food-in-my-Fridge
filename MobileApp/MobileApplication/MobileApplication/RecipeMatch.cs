using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApplication
{
    static class RecipeMatch
    {
        public static int GetRecipeScore(List<string> inventoryProducts, List<string> recipeIngredients)
        {
            int score = 0;
            int count1 = cntFound(inventoryProducts, recipeIngredients);
            int count2 = cntFound(recipeIngredients, inventoryProducts);
            int count = count1 + count2;
            if (recipeIngredients.Count > 0)
            {
                score = count * 100 / 2 / recipeIngredients.Count;
            }
            else
            {
                score = -1;
            }

            return score;
        }

        static int cntFound(List<string> invIng, List<string> recIng)
        {
            int count = 0;

            for (int i = 0; i < recIng.Count; i++)
            {
                string input = recIng[i];
                if (input != null)
                {
                    if (findStrInStrVec(input, invIng))
                    {
                        count++;
                    }
                }
            }
            return count;   //max count is num of recipe ingredients in inventory
        }

        static bool findStrInStrVec(string searchStr, List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string check = list[i];
                if (check.Contains(searchStr))
                {
                    //cout << "Found at pos " << check.find(searchStr) << endl;	//find returns position it was found at
                    return true;
                }
                else
                {
                    //find position returns random value if not found
                }
            }
            return false;
        }
    }
}
