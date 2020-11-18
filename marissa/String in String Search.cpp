// String in String Search.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include <vector>

using namespace std;

string getStrInput() {
	string input;
	cout << "enter string: ";
	cin >> input;
	return input;
}

string strToLower(string s) {
	return "";
}

string rmWord(string mainStr, string toErase) {
	//cout << " remove " << toErase << " from " << mainStr << " > ";
	int pos = string::npos;
	while ((pos = mainStr.find(toErase)) != std::string::npos)
	{
		// If found then erase it from string
		mainStr = mainStr.erase(pos, toErase.length());
	}
	//cout << mainStr << endl;
	return mainStr;
	/*string mainstr = "this is a test";	//remove is
	string toerase = "is";
	/remove 1st occurance of substring from string
	int pos = mainstr.find(toerase);
	if (pos != string::npos) {
		mainstr.erase(pos, toerase.length());
	}

	//remove all occurances of substring from string
	int pos = string::npos;
	while ((pos = mainstr.find(toerase)) != std::string::npos)
	{
		// If found then erase it from string
		mainstr.erase(pos, toerase.length());
	}
	cout << mainstr;*/
}

string refineStr(string s) {
	//lowercase


	//remove numbers
	//remove measurements ie cup cups oz ounce ounces 
	vector<string> invalidWords = { "cup", "cups", "oz", "ounce", "ounces", "teaspoon", "teaspoons", "tsp",
		"tablespoon", "tablespoons", "tbsp", "pound", "pounds", "lbs" "lb",
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", ",", "/"
	};

	for (int i = 0; i < invalidWords.size(); i++) {
		//remove word
		s = rmWord(s, invalidWords.at(i));
	}
	//cout << s << endl;
	return s;
}


void findStrInStr(string input, string *list, int listSize) {

	for (int i = 0; i < listSize; i++) {

		string check = list[i];
		if (check.find(input) != -1) {
			cout << "Found at pos " << check.find(input) << endl;	//find returns position it was found at
		}
		else { cout << "Not found " << check.find(input) << endl; }	//find position returns random value if not found
	}
	return;
}

bool findStrInStrVec(string searchStr, vector<string> list) {
	for (int i = 0; i < list.size(); i++) {

		string check = list.at(i);
		if (check.find(searchStr) != -1) {
			//cout << "Found at pos " << check.find(searchStr) << endl;	//find returns position it was found at
			return true;
		}
		else { 
			//cout << "Not found " << check.find(searchStr) << endl; 
		}	//find position returns random value if not found
	}
	return false;
}

int cntFound(vector<string> invIng, vector<string> recIng){
	int count = 0;

	for (int i = 0; i < recIng.size(); i++) {
		string input = recIng.at(i);
		if (findStrInStrVec(input, invIng)) {
			count++;
		}

	}


	return count;	//max count is num of ingredients in recipe
}

int scoreRec(vector<string> invIng, vector<string> recIng) {
	int score = 0;
	int count = cntFound(invIng, recIng) + cntFound(recIng, invIng);	//will be double # of ingredients in recipe

	score = count * 100 / 2 / recIng.size();	//rounded % of ingredients found

	return score;
}

int main() {
	
	////////////////////////
	//////////////////////// ONLY  METHODS NEEDED ARE findStrInStrVec() cntFound(), and scoreRec()
	////////////////////////
	
	// *note: removing numbers and measurement words do not affect comparison, so not necessary
	
	vector<string> invVec = { "boneless chicken", "12 oz chicken", "pepper", "cheese", "basil" };	//inventory items

	string d = "chicken breasts";
	string e = "garlic";
	string f = "crushed red pepper";
	string g = "canned crushed tomatoes";
	string h = "parmesan cheese";
	string i = "basil";
	string j = "tomato paste";

	vector<string> ingVec;			//recipe ingredients
	ingVec.push_back(d);
	ingVec.push_back(e);
	ingVec.push_back(f);
	ingVec.push_back(g);
	ingVec.push_back(h);
	ingVec.push_back(i);
	ingVec.push_back(j);

	vector<string> recIng = {"skinless, boneless Chicken breast halves", "salt and freshly ground black pepper", "2 eggs", "1 cup panko bread crumbs", "1/4 cup grated Parmesan cheese", "2 tablespoons all - purpose flour", "1 cup olive oil", "1/2 cup prepared tomato sauce", "1/4 cup fresh mozzarella, cut into small cubes", "1/4 cup chopped fresh basil", "1/2 cup grated provolone cheese", "1/4 cup grated Parmesan cheese", "tablespoon olive oil "};


	cout << scoreRec(invVec, recIng) << " " << scoreRec(recIng, recIng);



	
}
