#pragma once
#include <windows.h>
#include <iostream>
#include <time.h>
#include <conio.h>
#include <vector>
#include <list>
#include <ctime>
#include <complex>
using namespace std;
HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
HANDLE hin = GetStdHandle(STD_INPUT_HANDLE);
CONSOLE_CURSOR_INFO cci;
class D_Array
{
    std::string** maze;
    short height;
    short width;
public:
    D_Array(std::string hall, short _height, short _width, int choice = 0)
    {
        height = _height;
        width = _width;
        maze = new std::string * [height];
        for (short i = 0; i < height; i++)
            maze[i] = new std::string[width];
        for (short y = 0; y < height; y++)
        {
            for (short x = 0; x < width; x++)
            {
                maze[y][x] = hall;
            }
        };
    }
    short getHeight()
    {
        return height;
    }
    short getWidth()
    {
        return width;
    }
    std::string** getArray()
    {
        return maze;
    }
    ~D_Array()
    {
        for (int i = 0; i < height; i++)
            delete[] maze[i];
        delete[] maze;
    }
};
class FindPath
{
    short** check;
    short index;
    int height;
    int width;
    std::list <COORD> qcoord;
    std::list <COORD*> path;
    D_Array* game;
    COORD* unit;
    COORD* tmp;
    void CreateCheck(int height, int width, short count = -1)
    {
        check = new short* [height];
        for (short i = 0; i < height; i++)
            check[i] = new short[width];
        short y = 0;
    n:
        for (short x = 0; x < width; x++)
            check[y][x] = count;
        y++;
        if (y != height)
            goto n;
    }
    bool moveRight(COORD c)
    {
        if (check[c.Y][c.X + 1] == -1 && game->getArray()[c.Y][c.X + 1] == "Hall")
        {
            index = check[c.Y][c.X] + 1;
            check[c.Y][c.X + 1] = index;
            qcoord.push_back({ c.X + 1, c.Y });
            return true;
        }
        else
            return false;
    }
    bool moveLeft(COORD c)
    {
        if (check[c.Y][c.X - 1] == -1 && game->getArray()[c.Y][c.X - 1] == "Hall")
        {
            index = check[c.Y][c.X] + 1;
            check[c.Y][c.X - 1] = index;
            qcoord.push_back({ c.X - 1, c.Y });
            return true;
        }
        else
            return false;
    }
    bool moveUp(COORD c)
    {
        if (check[c.Y - 1][c.X] == -1 && game->getArray()[c.Y - 1][c.X] == "Hall" )
        {
            index = check[c.Y][c.X] + 1;
            check[c.Y - 1][c.X] = index;
            qcoord.push_back({ c.X, c.Y - 1 });
            return true;
        }
        else
            return false;
    }
    bool moveDown(COORD c)
    {
        if (check[c.Y + 1][c.X] == -1 && game->getArray()[c.Y + 1][c.X] == "Hall")
        {
            index = check[c.Y][c.X] + 1;
            check[c.Y + 1][c.X] = index;
            qcoord.push_back({ c.X, c.Y + 1 });
            return true;
        }
        else
            return false;
    }
    bool Finder(COORD* u)
    {
        tmp = new COORD{ 0, 0 };
        tmp->X = u->X;
        tmp->Y = u->Y;
    z:
        if (qcoord.empty() == false)
        {
            moveRight(qcoord.front());
            moveLeft(qcoord.front());
            moveUp(qcoord.front());
            moveDown(qcoord.front());
            if (qcoord.front().X == u->X && qcoord.front().Y == u->Y)
                qcoord.erase(qcoord.begin(), qcoord.end());
            else
                qcoord.pop_front();
            goto z;
        }
        else
        {
            if (check[u->Y][u->X] == -1)
                return false;
            index = check[u->Y][u->X];
        a:
            path.push_back(new COORD{ tmp->X, tmp->Y });
            if (index == 1)
                return true;            
            if (check[tmp->Y][tmp->X + 1] == index - 1 && game->getArray()[tmp->Y][tmp->X + 1] == "Hall")
            {
                tmp->X++;
                index--;
                goto a;
            }
            else if (check[tmp->Y][tmp->X - 1] == index - 1 && game->getArray()[tmp->Y][tmp->X - 1] == "Hall")
            {
                tmp->X--;
                index--;
                goto a;
            }
            else if (check[tmp->Y - 1][tmp->X] == index - 1 && game->getArray()[tmp->Y - 1][tmp->X] == "Hall")
            {
                tmp->Y--;
                index--;
                goto a;
            }
            else if (check[tmp->Y + 1][tmp->X] == index - 1 && game->getArray()[tmp->Y + 1][tmp->X] == "Hall")
            {
                tmp->Y++;
                index--;
                goto a;
            }
        }
    }
public:
    FindPath(D_Array* _game, COORD* u)
    {
        height = _game->getHeight();
        width = _game->getWidth();
        index = 0;
        CreateCheck(height, width);
        check[u->Y][u->X] = index;
        unit = u;
        game = _game;
        tmp = new COORD;
        qcoord.push_back({ unit->X, unit->Y });
    }
    std::list <COORD*> getCOORD(COORD* u)
    {
        if (Finder(u) == false)
        {
            if (path.empty() == false)
            {
                for (auto it : path)
                    delete it;
               path.erase(path.begin(), path.end());
            }    
            return path;
        }
        else if (Finder(u) == true)
        {
            delete tmp;
            if (check != nullptr)
            {
                for (int i = 0; i < height; i++)
                    delete[] check[i];
                delete[] check;
            }
            return path;
        }
    }
    ~FindPath()
    {
        if (check != nullptr)
        {
            for (int i = 0; i < height; i++)
                delete[] check[i];
            delete[] check;
        }
    }
};
class Maze
{
    int width;
    int height;
    D_Array* maze;
    int _stime()
    {
        int stime;
        long ltime;
        ltime = time(NULL);
        stime = (unsigned int)ltime / 2;
        return stime;
    }
    bool deadend(int x, int y) 
    {
        int a = 0;
        if (x != 1) 
        {
            if (maze->getArray()[y][x - 2] == "Hall")
                a += 1;
        }
        else a += 1;
        if (y != 1) 
        {
            if (maze->getArray()[y - 2][x] == "Hall")
                a += 1;
        }
        else a += 1;
        if (x != width - 2) 
        {
            if (maze->getArray()[y][x + 2] == "Hall")
                a += 1;
        }
        else a += 1;
        if (y != height - 2) 
        {
            if (maze->getArray()[y + 2][x] == "Hall")
                a += 1;
        }
        else a += 1;
        if (a == 4)
            return 1;
        else
            return 0;
    }
    void Do_Exit()
    {
        srand(_stime());
        int x, y, c, a;
        x = 1; y = 1; // Точка приземления крота 
        for (int i = 0; i < height * width; i++)
        { 
            maze->getArray()[y][x] = "Hall";
            while (true)
            { // Бесконечный цикл, который прерывается только тупиком
                c = rand() % 4; // Напоминаю, что крот прорывает
                switch (c)
                {  // по две клетки в одном направлении за прыжок
                case 0: if (y != 1)
                    if (maze->getArray()[y - 2][x] == "Wall")
                    { // Вверх
                        maze->getArray()[y - 1][x] = "Hall";
                        maze->getArray()[y - 2][x] = "Hall";
                        y -= 2;
                    }
                case 1: if (y != height - 2)
                    if (maze->getArray()[y + 2][x] == "Wall")
                    { // Вниз
                        maze->getArray()[y + 1][x] = "Hall";
                        maze->getArray()[y + 2][x] = "Hall";
                        y += 2;
                    }
                case 2: if (x != 1)
                    if (maze->getArray()[y][x - 2] == "Wall")
                    { // Налево
                        maze->getArray()[y][x - 1] = "Hall";
                        maze->getArray()[y][x - 2] = "Hall";
                        x -= 2;
                    }
                case 3: if (x != width - 2)
                    if (maze->getArray()[y][x + 2] == "Wall")
                    { // Направо
                        maze->getArray()[y][x + 1] = "Hall";
                        maze->getArray()[y][x + 2] = "Hall";
                        x += 2;
                    }
                }
                if (deadend(x, y))
                    break;
            }
            if (deadend(x, y)) // Вытаскиваем крота из тупика
                do
                {
                    x = 2 * (rand() % ((width - 1) / 2)) + 1;
                    y = 2 * (rand() % ((height - 1) / 2)) + 1;
                } while (maze->getArray()[y][x] != "Hall");
        } 
    }
    void Algorithm_Maze_G_1()
    {
        srand(_stime());
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                maze->getArray()[y][x] = "Wall"; 
                if (y == 0 || x == 0 || x == width - 1 || y == height - 1)
                    maze->getArray()[y][x] = "Wall";
                if ((x <= 2 && y == 2) || (x >= width - 2 && y == height - 3))
                    maze->getArray()[y][x] = "Hall";
            }
        }
        Do_Exit();
    }
    void Algorithm_Maze_G_2()
    {
        int r;
        srand(_stime());
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                maze->getArray()[y][x] = "Wall";
                r = rand() % 4;
                if (r == 1 || r == 3)
                    maze->getArray()[y][x] = "Hall";
                if (x % 2 == 0 && y % 2 == 0)
                    maze->getArray()[y][x] = "Wall";
                if (x % 3 == 0 && y % 3 == 0)
                    maze->getArray()[y][x] = "Wall";
                if (x % 4 == 0 && y % 3 == 0)
                    maze->getArray()[y][x] = "Wall";
                if (x % 5 == 0 && y % 2 == 0)
                    maze->getArray()[y][x] = "Wall";
                if (y == 0 || x == 0 || x == width - 1 || y == height - 1)
                    maze->getArray()[y][x] = "Wall";
                if ((x <= 3 && y == 2) || (x >= width - 3 && y == height - 3))
                    maze->getArray()[y][x] = "Hall";
            }
        }
        Do_Exit();
    }
public:
    Maze()
    {
        maze = nullptr;
        width = 0;
        height = 0;
    }
    D_Array* get_arr()
    {
        return maze;
    }
    void Generate_Maze(int h, int w, int n)
    {
        height = h;
        width = w;
        maze = new D_Array("Hall", h, w);
        if (n == 1)
            Algorithm_Maze_G_1();
        if (n == 2)
            Algorithm_Maze_G_2();
        else if (n <= 0 || n > 2)
        {
            cout << "\nError";
            return;
        }
    }
    std::string Get_Maze(int x, int y)
    {
        return maze->getArray()[y][x];
    }
    void Print_Maze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (maze->getArray()[y][x] == "Wall")
                {
                    SetConsoleTextAttribute(h, 7);
                    cout << (char)177;
                }
                if (maze->getArray()[y][x] == "Hall")
                    cout << " ";
            }
            cout << endl;
        }
    }
    ~Maze()
    {
        delete maze;
    }
};

