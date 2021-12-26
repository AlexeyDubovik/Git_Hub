#include "Header.h"
int main()
{
    HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
	system("mode con cols=90 lines=21");
	system("title Snake");
    //пременные и масив
	int arr[height][width];
	int dificult = 0;
    int length = 1;
    int dir = 0;
    int score = 0;
    int best_score = 0;
    //считывание очков с файла
    FILE* file;
    fopen_s(&file, "./Data.dat", "rb");
    fread(&best_score, sizeof(best_score), 1, file);
    fclose(file);
    //скрытие курсора
	CONSOLE_CURSOR_INFO cci;
	cci.bVisible = false;
	cci.dwSize = 100;
	SetConsoleCursorInfo(h, &cci);
    //сложность
	Dificult(&dificult);
    //отрисовка поля
    Draw(arr, height, width);
    Sleep(300);
    //отрисовка змеии
    COORD snake[862];
    snake[0] = { width / 2, height / 2 };
    print_head(snake, arr);
    //фрукт
    Fruit(arr);
    //цикл игры
    Info(width, score, best_score);
    int k = width + 4;
    while (true)
    {
        if (_kbhit())
            Control_Snake(dir);
        else
        {
            Sleep(dificult);
            control_tail(snake, arr, length);
            Move_Snake(snake, dir);
            Game_Over(snake, arr, score, best_score, length);
            Fruit_and_body(snake, arr, score, dificult, length);
            Info(width, score, best_score);
        }
    }
}