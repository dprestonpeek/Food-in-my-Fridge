using System;
using System.Collections.Generic;

namespace String_in_String_Search_CSharp
{
    class Program
    {
		bool findStrInStrVec(string searchStr, List<string> list)
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


		int cntFound(List<string> invIng, List<string> recIng)
		{
			int count = 0;

			for (int i = 0; i<recIng.Count; i++)
			{
				string input = recIng[i];
				if (findStrInStrVec(input, invIng))
				{
					count++;
				}
			}
			return count;   //max count is num of recipe ingredients in inventory
		}

		public int scoreRec(List<string> invIng, List<string> recIng)
        {
			int score = 0;
			int count = cntFound(invIng, recIng) + cntFound(recIng, invIng); ;
			score = count * 100 / 2 / recIng.Count;

			return score;
        }

		static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


			List<string> invIng = new List<string>(new string[] { "boneless chicken", "12 oz chicken", "pepper", "cheese", "basil" });
			List<string> recIng = new List<string>(new string[] { "skinless, boneless Chicken breast halves", "salt and freshly ground black pepper", "2 eggs", "1 cup panko bread crumbs", "1/4 cup grated Parmesan cheese", "2 tablespoons all - purpose flour", "1 cup olive oil", "1/2 cup prepared tomato sauce", "1/4 cup fresh mozzarella, cut into small cubes", "1/4 cup chopped fresh basil", "1/2 cup grated provolone cheese", "1/4 cup grated Parmesan cheese", "tablespoon olive oil " });

			int score = scoreRec(invIng, recIng);
			Console.WriteLine(scoreRec(invIng, recIng) + " " + scoreRec(recIng, recIng));

		}
	}
}
