#include "Header.h"
HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
void Dificult(int *dif)
{
	system("chcp 1251 > nul");
	COORD c; 
	HANDLE hin = GetStdHandle(STD_INPUT_HANDLE);
	SetConsoleMode(hin, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
	const int events_count = 256;
	INPUT_RECORD all_events[events_count]; 
	DWORD read_event; 
	COORD Dificult = { (short)(0), 0 };
	SetConsoleCursorPosition(h, Dificult);
	SetConsoleTextAttribute(h, 96);
	cout << "Выберете уровень сложности" << endl;
	Sleep(1000);
	bool w = true;
	COORD light = { (short)(0), 1 };
	SetConsoleCursorPosition(h, light);
	SetConsoleTextAttribute(h, 6);
	cout << "Легко" << endl;
	Sleep(100);
	COORD medium = { (short)(0), 2 };
	SetConsoleCursorPosition(h, medium);
	SetConsoleTextAttribute(h, 6);
	cout << "Средне" << endl;
	Sleep(100);
	COORD hard = { (short)(0), 3 };
	SetConsoleCursorPosition(h, hard);
	SetConsoleTextAttribute(h, 6);
	cout << "Сложно" << endl;
	while (w)
	{
		ReadConsoleInput(hin, all_events, events_count, &read_event); 
		for (int i = 0; i < read_event; i++)
		{
			c.X = all_events[i].Event.MouseEvent.dwMousePosition.X;
			c.Y = all_events[i].Event.MouseEvent.dwMousePosition.Y;
			SetConsoleCursorPosition(h, light);
			SetConsoleTextAttribute(h, 6);
			cout << "Легко" << endl;
			SetConsoleCursorPosition(h, medium);
			SetConsoleTextAttribute(h, 6);
			cout << "Средне" << endl;
			SetConsoleCursorPosition(h, hard);
			SetConsoleTextAttribute(h, 6);
			cout << "Сложно" << endl;
			if (c.X <= 4 && c.Y == 1)
			{
				SetConsoleCursorPosition(h, light);
				SetConsoleTextAttribute(h, 4);
				cout << "Легко" << endl;
				mciSendString(L"open 1.mp3 TYPE MpegVideo ALIAS song1", NULL, 0, 0);
				mciSendString(L"PLAY song1 from 60 to 300", NULL, 0, 0);
			}
			if (c.X <= 5 && c.Y == 2)
			{
				SetConsoleCursorPosition(h, medium);
				SetConsoleTextAttribute(h, 4);
				cout << "Средне" << endl;
				mciSendString(L"open 1.mp3 TYPE MpegVideo ALIAS song1", NULL, 0, 0);
				mciSendString(L"PLAY song1 from 60 to 300", NULL, 0, 0);
			}
			if (c.X <= 5 && c.Y == 3)
			{
				SetConsoleCursorPosition(h, hard);
				SetConsoleTextAttribute(h, 4);
				cout << "Сложно" << endl;
				mciSendString(L"open 1.mp3 TYPE MpegVideo ALIAS song1", NULL, 0, 0);
				mciSendString(L"PLAY song1 from 60 to 300", NULL, 0, 0);
			}
			if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED && (c.X <= 4 && c.Y == 1))
			{
				mciSendString(L"open 2.mp3 TYPE MpegVideo ALIAS song2", NULL, 0, 0);
				mciSendString(L"PLAY song2 from 0", NULL, 0, 0);
				*dif = 300;
				system("cls");
				w = false;
				break;
			}
			if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED && (c.X <= 5 && c.Y == 2))
			{
				mciSendString(L"open 2.mp3 TYPE MpegVideo ALIAS song2", NULL, 0, 0);
				mciSendString(L"PLAY song2 from 0", NULL, 0, 0);
				*dif = 200;
				system("cls");
				w = false;
				break;
			}
			if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED && (c.X <= 5 && c.Y == 3))
			{
				mciSendString(L"open 2.mp3 TYPE MpegVideo ALIAS song2", NULL, 0, 0);
				mciSendString(L"PLAY song2 from 0", NULL, 0, 0);
				*dif = 100;
				system("cls");
				w = false;
				break;
			}
		}
	}
}
void Draw(int arr[][50], const int height, const int width)
{
	system("chcp 866 > nul");
	for (int y = 0; y < height; y++)
	{
		for (int x = 0; x < width; x++)
		{
			if (y == 0 || x == 0 || x == width - 1 || y == height - 1)
			{
				arr[y][x] = WALL;
			}
			else 
			{
				arr[y][x] = HALL;
			}
			if (arr[y][x] == WALL)
			{
				SetConsoleTextAttribute(h, 9);
				cout << (char)219;
			}
			if (arr[y][x] == HALL)
			{
				cout << " ";
			}
		}
		cout << endl;
	}	
}
void Fruit(int arr[][50])
{
	srand(time(0)); rand();	
	int x = (rand() % (width - 2)) + 1;
	int y = (rand() % (height - 2)) + 1;
	for (int i = 0; i < 10; i++)
	{
		if (arr[y][x] == SNAKE)
		{
			x = (rand() % (width - 2)) + 1;
			y = (rand() % (height - 2)) + 1;
		}
	}
	arr[y][x] = FOOD;
	COORD food = { x,y };
	SetConsoleCursorPosition(h, food);
	SetConsoleTextAttribute(h, 6);
	cout << (char)253;
}
void Info(const int width, int score, int bestscore)
{
	COORD control = { (short)(width + 4), 1 };
	SetConsoleCursorPosition(h, control);
	SetConsoleTextAttribute(h, 7);
	cout << "Press: ";
	SetConsoleTextAttribute(h, 10);
	cout << "'w' 'a' 's' 'd' ";
	SetConsoleTextAttribute(h, 7);
	cout << "to move.";
	COORD perscription = { (short)(width + 4), 2 };
	SetConsoleCursorPosition(h, perscription);
	SetConsoleTextAttribute(h, 7);
	cout << "Don't eat your tail.";
	COORD Score = { (short)(width + 4), 3 };
	SetConsoleCursorPosition(h, Score);
	SetConsoleTextAttribute(h, 7);
	cout << "Score: ";
	SetConsoleTextAttribute(h, 10);
	cout << score << "              ";
	COORD best_score = { (short)(width + 4), 4 };
	SetConsoleCursorPosition(h, best_score);
	SetConsoleTextAttribute(h, 7);
	cout << "Best Score: ";
	SetConsoleTextAttribute(h, 4);
	cout << bestscore << "              ";
}
void print_head(COORD* snake, int arr[][50])
{
	SetConsoleCursorPosition(h, snake[0]);
	SetConsoleTextAttribute(h, 4);
	cout << (char)1;
	arr[snake[0].Y][snake[0].X] = SNAKE;
}
void print_body(COORD* snake, int arr[][50], int length)
{
	for (int i = 0; i < length; i++)
	{
		print_head(snake, arr);
		if (length > 1)
		{
			SetConsoleCursorPosition(h, snake[i]);
			SetConsoleTextAttribute(h, 10);
			cout << (char)174;
		}
	}
}
void control_tail(COORD* snake, int arr[][50], int length)
{
	if (length > 1)
		arr[snake[length - 2].Y][snake[length - 2].X] = HALL;
	else
		arr[snake[0].Y][snake[0].X] = HALL;
	if (arr[snake[0].Y][snake[0].X] != FOOD)
	{
		SetConsoleCursorPosition(h, snake[length - 1]);
		cout << " ";
		for (int i = length - 1; i > 0; i--)
		{
			snake[i] = snake[i - 1];
		}
	}
}
void Control_Snake(int &dir)
{
	int direct = _getch();
	if (direct == RIGHT && dir != 1)
	{
		dir = 0;
	}
	else if (direct == LEFT && dir != 0)
	{
		dir = 1;
	}
	else if (direct == UP && dir != 3)
	{
		dir = 2;
	}
	else if (direct == DOWN && dir != 2)
	{
		dir = 3;
	}
}
void Move_Snake(COORD* snake, int& dir)
{
	if (dir == 0)
	{
		snake[0].X++;
	}
	else if (dir == 1)
	{
		snake[0].X--;
	}
	else if (dir == 2)
	{
		snake[0].Y--;
	}
	else if (dir == 3)
	{
		snake[0].Y++;
	}
}
void Game_Over(COORD* snake, int arr[][50], int& score, int& best_score, int length)
{
	if (arr[snake[0].Y][snake[0].X] == WALL)
	{
		mciSendString(L"open 4.mp3 TYPE MpegVideo ALIAS song4", NULL, 0, 0);
		mciSendString(L"PLAY song4 from 0", NULL, 0, 0);
		if (score > best_score)
		{
			FILE* file;
			fopen_s(&file, "./Data.dat", "wb");
			if (file)
			{
				fwrite(&score, sizeof(score), 1, file);
				fclose(file);
			}
		}
		system("cls");
		COORD g = { 40, 9 };
		SetConsoleCursorPosition(h, g);
		SetConsoleTextAttribute(h, 4);
		cout << "Game Over";
		COORD Score = { 38, 10 };
		SetConsoleCursorPosition(h, Score);
		SetConsoleTextAttribute(h, 4);
		cout << "Your Score ";
		SetConsoleTextAttribute(h, 4);
		cout << score << "              ";
		SetConsoleTextAttribute(h, 0);
		Sleep(2000);
		exit(0);
	}
	for (int i = 1; i < length; i++)
	{
		if (snake[i].X == snake[0].X && snake[i].Y == snake[0].Y)
		{
			mciSendString(L"open 4.mp3 TYPE MpegVideo ALIAS song4", NULL, 0, 0);
			mciSendString(L"PLAY song4 from 0", NULL, 0, 0);
			if (score > best_score)
			{
				FILE* file;
				fopen_s(&file, "./Data.dat", "wb");

				if (file)
				{
					fwrite(&score, sizeof(score), 1, file);
					fclose(file);
				}
			}
			system("cls");
			COORD g = { 40, 9 };
			SetConsoleCursorPosition(h, g);
			SetConsoleTextAttribute(h, 4);
			cout << "Game Over";
			COORD Score = { 38, 10 };
			SetConsoleCursorPosition(h, Score);
			SetConsoleTextAttribute(h, 4);
			cout << "Your Score ";
			SetConsoleTextAttribute(h, 4);
			cout << score << "              ";
			SetConsoleTextAttribute(h, 0);
			Sleep(2000);
			exit(0);
		}
	}
}
void Fruit_and_body(COORD* snake, int arr[][50], int& score,int &dificult, int &length)
{
	if (arr[snake[0].Y][snake[0].X] == FOOD)
	{
		mciSendString(L"open 3.mp3 TYPE MpegVideo ALIAS song3", NULL, 0, 0);
		mciSendString(L"PLAY song3 from 60 to 800", NULL, 0, 0);
		if (dificult == 100)
			score += 15;
		if (dificult == 200)
			score += 10;
		if (dificult == 300)
			score += 5;
		length++;
		Fruit(arr);
		print_head(snake, arr);
		arr[snake[0].Y][snake[0].X] = SNAKE;
	}
	else
		print_body(snake, arr, length);
}