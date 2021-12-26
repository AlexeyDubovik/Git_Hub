#include <iostream>
#include <windows.h>
#include <conio.h>
#pragma comment (lib,   "Winmm.lib")

using namespace std;

const int height = 20;
const int width = 50;

enum objects { HALL, WALL, SNAKE, FOOD };
enum direction { DOWN = 115, UP = 119, LEFT = 97, RIGHT = 100 };

void Dificult(int *dif);
void Draw(int arr[][50], const int height, const int width);
void Fruit(int arr[][50]);
void Info(const int width, int score, int bestscore);
void print_head(COORD* snake, int arr[][50]);
void print_body(COORD* snake, int arr[][50], int length);
void control_tail(COORD* snake, int arr[][50], int length);
void Control_Snake(int& dir);
void Move_Snake(COORD* snake, int& dir);
void Game_Over(COORD* snake, int arr[][50], int& score, int& best_score, int length);
void Fruit_and_body(COORD* snake, int arr[][50], int& score, int& dificult, int& length);
