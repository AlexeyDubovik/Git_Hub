#pragma once
#include <windows.h>
#include <iostream>
#include <vector>
#include <List>
#include <cmath>
#include <conio.h>
#include <mmsystem.h>
#include <thread>
#pragma comment (lib,   "Winmm.lib")
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
//----------------------------------------------------------------
class FindPath
{
    short** check;
    short index;
    int height;
    int width;
    std::list <COORD> qcoord;
    std::list <COORD*> path;
    std::vector<D_Array*> game;
    COORD* unit;
    COORD* folower;
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
        if (check[c.Y][c.X + 1] == -1 && game[0]->getArray()[c.Y][c.X + 1] != "Wall")
        {
            index = check[c.Y][c.X] + 1;
            check[c.Y][c.X + 1] = index;
            qcoord.push_back({ c.X + 1, c.Y });
            return true;
        }
        else
            return false;
    }
    bool moveLeft (COORD c)
    {
        if (check[c.Y][c.X - 1] == -1 && game[0]->getArray()[c.Y][c.X - 1] != "Wall" )
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
        if (check[c.Y - 1][c.X] == -1 && game[0]->getArray()[c.Y - 1][c.X] != "Wall")
        {
            index = check[c.Y][c.X] + 1;
            check[c.Y - 1][c.X] = index;
            qcoord.push_back({ c.X, c.Y - 1});
            return true;
        }
        else
            return false;
    }
    bool moveDown(COORD c)
    {
        if (check[c.Y + 1][c.X] == -1 && game[0]->getArray()[c.Y + 1][c.X] != "Wall")
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
        moveInMaze:
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
            goto moveInMaze;
        }
        else
        {
            if (check[u->Y][u->X] == -1)
                return false;
            index = check[u->Y][u->X];
        createPath:
            path.push_back(new COORD{ folower->X, folower->Y });
            if (index == 1)
                return true;
            if (check[folower->Y][folower->X + 1] == index - 1 && game[0]->getArray()[folower->Y][folower->X + 1] == "Hall")
            {
                folower->X++;
                index--;
                goto createPath;
            }
            else if (check[folower->Y][folower->X - 1] == index - 1 && game[0]->getArray()[folower->Y][folower->X - 1] == "Hall")
            {
                folower->X--;
                index--;
                goto createPath;
            }
            else if (check[folower->Y - 1][folower->X] == index - 1 && game[0]->getArray()[folower->Y - 1][folower->X] == "Hall")
            {
                folower->Y--;
                index--;
                goto createPath;
            }
            else if (check[folower->Y + 1][folower->X] == index - 1 && game[0]->getArray()[folower->Y + 1][folower->X] == "Hall")
            {
                folower->Y++;
                index--;
                goto createPath;
            }
        }
    }
public:
    FindPath(std::vector<D_Array*> _game, COORD* u)
    {
        folower = new COORD{ 0, 0 };
        height = _game[0]->getHeight();
        width = _game[0]->getWidth();
        index = 0;
        CreateCheck(height, width);
        check[u->Y][u->X] = index;
        unit = u;
        game = _game;
        folower = new COORD;
        qcoord.push_back({ unit->X, unit->Y });
    }
    std::list <COORD*> getCOORD(COORD* u)
    {
        folower->X = u->X;
        folower->Y = u->Y;
        bool find = Finder(u);
        if (find == false)
        {
            if (path.empty() == false)
                path.erase(path.begin(), path.end());
            return path;
        }
        else
            return path;
    }
    ~FindPath()
    {
        delete folower;
        if (check != nullptr)
        {
            for (int i = 0; i < height; i++)
                delete[] check[i];
            delete[] check;
        }
    }
};
//----------------------------------------------------------------
class Object
{
protected:
    std::string ID;
    int color;
    char symbol;
    Object(std::string _id, int _color = 0, char _symbol = '0') : ID(_id), color(_color), symbol(_symbol) {}
public:
    std::string getID()
    {
        return ID;
    }
    void print(COORD coord)
    {
        SetConsoleCursorPosition(h, coord);
        SetConsoleTextAttribute(h, color);
        std::cout << symbol;
    }
};
class _Hall : public Object
{
public:
    _Hall(std::string _id = "Hall", int _color = 0, char _symbol = 32) : Object(_id, _color, _symbol) {}
};
class _Wall : public Object
{
public:
    _Wall(std::string _id = "Wall", int _color = 101, char _symbol = 177) : Object(_id, _color, _symbol) {}
};
class _ExiT : public Object
{
public:
    _ExiT(std::string _id = "Exit", int _color = 13, char _symbol = 12) : Object(_id, _color, _symbol) {}
};
class _Gate : public Object
{
public:
    _Gate(std::string _id = "Gate", int _color = 12, char _symbol = 186) : Object(_id, _color, _symbol) {}
};
//----------------------------------------------------------------
class Item
{
protected:
    std::string ID;
    int count;
    int color;
    char symbol;
    Item(int _count, std::string _ID = "0", int _color = 0, char _symbol = '0') : count(_count), ID(_ID), color(_color), symbol(_symbol) {}
public:
    void MinusCount(int _count)
    {
        count -= _count;
    }
    int GetCount()
    {
        return count;
    }
    std::string getID()
    {
        return ID;
    }
    void print(COORD coord)
    {
        SetConsoleCursorPosition(h, coord);
        SetConsoleTextAttribute(h, color);
        std::cout << symbol;
    }
};
class _Gold : public Item
{
public:
    _Gold(int _count = 0, std::string _ID = "Gold", int _color = 6, char _symbol = 36) : Item(_count, _ID, _color, _symbol) {}
};
class _Healer : public Item
{
public:
    _Healer(int _count = 0, std::string _ID = "Heal", int _color = 10, char _symbol = 3) : Item(_count, _ID, _color, _symbol) {}
};
class _Energy : public Item
{
public:
    _Energy(int _count = 0, std::string _ID = "Energy", int _color = 11, char _symbol = 14) : Item(_count, _ID, _color, _symbol) {}
};
class _Bomb : public Item
{
public:
    _Bomb(int _count = 0, std::string _ID = "Bomb", int _color = 6, char _symbol = 19) : Item(_count, _ID, _color, _symbol) {}
};
//----------------------------------------------------------------
class Action
{
public:
    virtual bool action (std::string id, COORD* u, std::vector <D_Array*> _arr, char direct = 0) = 0;
};
class _Move : public Action
{
public:
    bool action(std::string id, COORD* u, std::vector<D_Array*> _arr, char direct = 0) override
    {
        if (_arr.empty() == false)
        {
            std::string** maze = _arr[0]->getArray();
            std::string hall = "Hall";
            std::string wall = "Wall";
            std::string gate = "Gate";
            std::string unit = id;
            maze[u->Y][u->X] = hall;
            if (direct == 'd' && maze[u->Y][u->X + 1] == hall && maze[u->Y][u->X + 1] != unit && maze[u->Y][u->X + 1] != gate)
            {
                u->X++;
                maze[u->Y][u->X] = unit;
                return true;
            }
            else if (direct == 'a' && maze[u->Y][u->X - 1] == hall && maze[u->Y][u->X - 1] != unit && maze[u->Y][u->X - 1] != gate)
            {
                u->X--;
                maze[u->Y][u->X] = unit;
                return true;
            }
            else if (direct == 'w' && maze[u->Y - 1][u->X] == hall && maze[u->Y - 1][u->X] != unit && maze[u->Y - 1][u->X] != gate)
            {
                u->Y--;
                maze[u->Y][u->X] = unit;
                return true;
            }
            else if (direct == 's' && maze[u->Y + 1][u->X] == hall && maze[u->Y + 1][u->X] != unit && maze[u->Y + 1][u->X] != gate)
            {
                u->Y++;
                maze[u->Y][u->X] = unit;
                return true;
            }
            else
            {
                maze[u->Y][u->X] = unit;
                return false;
            }
        }
        else
            return false;
    }
};
class _Plant : public Action
{
public:
    bool action (std::string id, COORD* u, std::vector <D_Array*> _arr, char direct = 0) override
    {
        if (_arr.empty() != true)
        {
            if (_arr[0]->getArray()[u->Y][u->X] == "Hall" || _arr[1]->getArray()[u->Y][u->X] == "Hall")
            {
                _arr[1]->getArray()[u->Y][u->X] = id;
                return true;
            }
        }
        else 
            return false;
    }
};
class _Detonate : public Action
{
public:
    bool action(std::string id, COORD* u, std::vector <D_Array*> _arr, char direct = 0) override
    {
        if (_arr.empty() != true)
        {
            _arr[1]->getArray()[u->Y][u->X] = "Hall";
            short y = (u->Y - 2);
            short x = (u->X - 2);
            short pY = (u->Y + 2);
            short pX = (u->X + 2);
        a:
            for (x; x <= pX; x++)
            {
                if (y > 0 && x > 0 && x < _arr[0]->getWidth() - 1 && y < _arr[0]->getHeight() - 1 && _arr[0]->getArray()[y][x] != "Gate")
                    _arr[0]->getArray()[y][x] = id;
                if (x == pX)
                {
                    y++;
                    if (y < pY + 1)
                    {
                        x = (u->X - 2);
                        goto a;
                    }
                    else if (y == pY + 1)
                    {
                        delete u;
                        return true;
                    }
                }
            }
        }
        else
            return false;
    }
};
//----------------------------------------------------------------
class Unit
{
protected:
    Action* action;
    COORD* unit;
    COORD* prev;
    FindPath* find;
    std::vector <D_Array*> arr;
    std::string ID;
    int health;
    int maxHealth;
    int energy;
    int maxEnergy;
    int color;
    char symbol;
    Unit()
    {
        action = nullptr;
        unit = nullptr;
        prev = nullptr;
        health = 0;
        maxHealth = 0;
        energy = 0;
        maxEnergy = 0;
        color = 0;
        symbol = 0;
    }
public:
    COORD* GetCoord(int ID)
    {
        if (ID == 1)
            return unit;
        if (ID == 2)
            return prev;
    }
    void setCoord(int x, int y)
    {
        unit->X = x;
        unit->Y = y;
    }
    std::string getID()
    {
        return ID;
    }
    void changeHealth(int h)
    {
        if ((health + h) > maxHealth)
            health = maxHealth;
        else
            health += h;
    }
    void changeEnergy(int e)
    {
        if ((energy + e) > maxEnergy)
            energy = maxEnergy;
        else
            energy += e;
    }
    int getHealth()
    {
        return health;
    }
    int getEnergy()
    {
        return energy;
    }
    void print()
    {
        SetConsoleCursorPosition(h, { unit->X, unit->Y });
        SetConsoleTextAttribute(h, color);
        std::cout << symbol;
    }
    virtual bool Action(short direct, bool* ifMove = nullptr, Item* item = nullptr, COORD* u = nullptr) = 0;
    virtual ~Unit()
    {
        if (unit != nullptr)
            delete unit;
        if (prev != nullptr)
            delete prev;
        if (action != nullptr)
            delete action;
        if (find != nullptr)
            delete find;
    }
};
class _Pers : public Unit
{
    bool bombON;
    bool act;
public:
    _Pers(std::vector <D_Array*> _arr, int _health = 100, int _energy = 500)
    {
        ID = "Pers";
        health = _health;
        maxHealth = _health;
        energy = _energy;
        maxEnergy = _energy;
        unit = new COORD{ 0, 2 };
        prev = new COORD{ 0, 0 };
        action = new _Move;
        arr = _arr;
        color = 3;
        symbol = 11;
        bombON = false;
    }
    bool Action(short direct, bool* ifMove = nullptr, Item* item = nullptr, COORD* u = nullptr) override
    {
        prev->X = unit->X;
        prev->Y = unit->Y;
        if (direct == 13 && bombON == false && item != nullptr)
        {
            delete action;
            action = new _Plant;
            if (action->action(item->getID(), unit, arr, direct) == true)
            {
                bombON = true;
                changeEnergy(-50);
            }
        }
        if (direct == 32 && bombON == true  && item != nullptr)
        {
            delete action;
            action = new _Detonate;
            for (short y = 0; y < arr[1]->getHeight(); y++)
            {
                for (short x = 0; x < arr[1]->getWidth(); x++)
                {
                    if (arr[1]->getArray()[y][x] == item->getID())
                    {
                        if (action->action("Crush", new COORD{x, y}, arr, direct) == true)
                            bombON = false;
                        break;
                    }
                }
            }
        }
        else
        {
            delete action;
            action = new _Move;
            act = action->action(ID, unit, arr, direct);
            if (act == true)
            {
                changeEnergy(-1);
                return act;
            }
            else
                return act;
        }
    }
};
class _Enemy : public Unit
{
    int* Count_Finder;
    int _Sqrt;
    int k_sqrt;
    char direction;
    bool finder;
    std::list <COORD*> path;
public:
    _Enemy(std::vector <D_Array*> _arr, int _health = 100, int _energy = 500)
    {
        ID = "Enemy";
        health = _health;
        maxHealth = _health;
        energy = _energy;
        maxEnergy = _energy;
        unit = new COORD{ 0, 0 };
        prev = new COORD{ 0, 0 };
        action = new _Move;
        arr = _arr;
        color = 4;
        symbol = 15;
        find = nullptr;
        finder = true;
        direction = 'w';
        k_sqrt = 15;
        Count_Finder = new int(0);
    }
    bool Action(short direct, bool* ifPersMove = nullptr, Item* item = nullptr, COORD* u = nullptr) override
    {
        prev->X = unit->X;
        prev->Y = unit->Y;
        _Sqrt = sqrt((unit->X - u->X) * (unit->X - u->X) + (unit->Y - u->Y) * (unit->Y - u->Y));
        if (_Sqrt <= k_sqrt)
        {
            if (*ifPersMove == true)
            {
                finder = true;
                *ifPersMove = false;
            }
            if (finder == true)
            {
                finder = false;
                if (find != nullptr)
                    delete find;
                find = new FindPath(arr, unit);
                if (path.empty() == false)
                    path.clear();
               /* std::thread thr(path = find->getCOORD(u), u, ref(path));
                thr.join();*/
                path = find->getCOORD(u);
            }
            if (path.empty() == false)
            {
            /*    std::thread thr(unit, path,
                    [](COORD* unit, std::list <COORD*> path)
                    {
                        unit->X = path.back()->X;
                        unit->Y = path.back()->Y;
                        path.pop_back();
                    }
                );
                thr.join();*/
                unit->X = path.back()->X;
                unit->Y = path.back()->Y;
                path.pop_back();
            }
        }
        else
        {
            finder = false;
            if (path.empty() == false) 
                path.erase(path.begin(), path.end());
            else
            {
                direct = rand() % 4;
                switch (direct)
                {
                case 0:
                    direction = 'w';
                    break;
                case 1:
                    direction = 's';
                    break;
                case 2:
                    direction = 'a';
                    break;
                case 3:
                    direction = 'd';
                    break;
                default:
                    break;
                }
                return action->action(ID, unit, arr, direction);
            }
        }
    }
};
//----------------------------------------------------------------
class Menu
{
    std::vector <std::string> text_Main;
    std::vector <COORD> coordtext;
    std::vector <std::string> text_Credits;
    std::vector <COORD> coordtext_Credits;
    std::vector <std::string> text_Option;
    std::vector <COORD> coordtext_Option;
public:
    Menu(short width = 41, short height = 10)
    {
        short tmp = 0;
        coordtext.push_back({ width, height });
        text_Main.push_back("New Game");
        height++;
        coordtext.push_back({ width, height });
        text_Main.push_back("Option");
        height++;
        coordtext.push_back({ width, height });
        text_Main.push_back("Credits");
        height++;
        coordtext.push_back({ width, height });
        text_Main.push_back("Exit");
        //__________________________________________________
        height = 8;
        coordtext_Credits.push_back({ width-5, height });
        text_Credits.push_back("Author: Alexey Dubovik");
        height+=2;
        coordtext_Credits.push_back({ width - 5, height });
        text_Credits.push_back("With best regards to:");
        height+=2;
        coordtext_Credits.push_back({ width - 5, height });
        text_Credits.push_back("Pogudin Ivan");
        height++;
        coordtext_Credits.push_back({ width - 5, height });
        text_Credits.push_back("Vladislav Turchinsky");
        height+=2;
        coordtext_Credits.push_back({ width + 2, height });
        text_Credits.push_back("_Back_");
        //__________________________________________________
        height = 9;
        width -= 2;
        coordtext_Option.push_back({ width, height });
        text_Option.push_back("Enemy ");
        height++;
        coordtext_Option.push_back({ width, height });
        text_Option.push_back("Health");
        height++;
        coordtext_Option.push_back({ width, height });
        text_Option.push_back("Energy  ");
        height++;
        coordtext_Option.push_back({ width, height });
        text_Option.push_back("Money ");
        height++;
        coordtext_Option.push_back({ width, height });
        text_Option.push_back("Sound ");
        height++;
        coordtext_Option.push_back({ width, height });
        text_Option.push_back("Music ");
        height++;
        coordtext_Option.push_back({ width + 3, height });
        text_Option.push_back("_Back_");
        for (short i = 0; i < 4; i++)
        {
            if (i == 2)
                tmp = 2;
            else
                tmp = 0;
            coordtext_Option.push_back({(short)coordtext_Option[i].X + (short)text_Option[i].length() + 2 - tmp, (short)coordtext_Option[i].Y });
            text_Option.push_back("<");
            coordtext_Option.push_back({(short)coordtext_Option[i].X + (short)text_Option[i].length() + 6 - tmp, (short)coordtext_Option[i].Y });
            text_Option.push_back(">");
        }
        text_Option.push_back("On");
        text_Option.push_back("Off");
    }
    void printMenu(short change = 0, short color = 14, short width = 0, short height = 11)
    {
        int i = 0;
        if (change == 0)
            for (auto it : coordtext)
            {
                SetConsoleCursorPosition(h, { it.X, it.Y });
                SetConsoleTextAttribute(h, color);
                std::cout << text_Main[i];
                i++;
            }
        if (change == 1)
            for (auto it : coordtext_Credits)
            {
                SetConsoleCursorPosition(h, { it.X, it.Y });
                SetConsoleTextAttribute(h, color);
                std::cout << text_Credits[i];
                i++;
            }
        if (change == 2)
            for (auto it : coordtext_Option)
            {
                SetConsoleCursorPosition(h, { it.X, it.Y });
                SetConsoleTextAttribute(h, color);
                std::cout << text_Option[i];
                i++;
            }
    }
    bool selectMenu(COORD mouse, short color, short menu = 0)
    {
        if (menu == 0)
        {
            for (short i = 0; i < text_Main.size(); i++)
            {
                if (coordtext[i].Y == mouse.Y && coordtext[i].X <= mouse.X && (coordtext[i].X + text_Main[i].length() - 1) >= mouse.X)
                {
                    SetConsoleCursorPosition(h, coordtext[i]);
                    SetConsoleTextAttribute(h, color);
                    std::cout << text_Main[i];
                    return true;
                }
            }
            return false;
        }
        if (menu == 1)
        {
            int index = 4;
            if (coordtext_Credits[index].Y == mouse.Y && coordtext_Credits[index].X <= mouse.X &&
                (coordtext_Credits[index].X + text_Credits[index].length() - 1) >= mouse.X)
            {
                SetConsoleCursorPosition(h, coordtext_Credits[index]);
                SetConsoleTextAttribute(h, color);
                std::cout << text_Credits[index];
                return true;
            }
            return false;
        }
        if (menu == 2)
        {
            int index = 6;
            if (coordtext_Option[index].Y == mouse.Y && coordtext_Option[index].X <= mouse.X &&
                (coordtext_Option[index].X + text_Option[index].length() - 1) >= mouse.X)
            {
                SetConsoleCursorPosition(h, coordtext_Option[index]);
                SetConsoleTextAttribute(h, color);
                std::cout << text_Option[index];
                return true;
            }
            for (short i = 1; i < 9; i++)
            {
                if (coordtext_Option[index + i].Y == mouse.Y && coordtext_Option[index + i].X <= mouse.X &&
                    (coordtext_Option[index + i].X + text_Option[index + i].length() - 1) >= mouse.X)
                {
                    SetConsoleCursorPosition(h, coordtext_Option[index + i]);
                    SetConsoleTextAttribute(h, color);
                    std::cout << text_Option[index + i];
                    return true;
                }
            }
            return false;
        }
    }
    std::string getText(int index, short menu = 0)
    {
        if (menu == 0)
            return text_Main[index];
        if (menu == 1)
            return text_Credits[index];
        if (menu == 2)
            return text_Option[index];
    }
    COORD getCoordText(int index, short menu = 0)
    {
        if (menu == 0)
            return coordtext[index];
        if (menu == 1)
            return coordtext_Credits[index];
        if (menu == 2)
            return coordtext_Option[index];
    }
    ~Menu()
    {
        text_Main.erase(text_Main.begin(), text_Main.end());
        coordtext.erase(coordtext.begin(), coordtext.end());
        text_Credits.erase(text_Credits.begin(), text_Credits.end());
        coordtext_Credits.erase(coordtext_Credits.begin(), coordtext_Credits.end());
        text_Option.erase(text_Option.begin(), text_Option.end());
        coordtext_Option.erase(coordtext_Option.begin(), coordtext_Option.end());
    }
};
//----------------------------------------------------------------
class Maze
{
    COORD lastPosPlayer;
    COORD PosPlayer;
    COORD _Exit;
    std::vector<D_Array*> Arr_s;
    std::vector<Unit*> players;
    std::vector<Unit*> enemys;
    std::vector<Item*> item;
    std::vector<Object*> obj;
    short width;
    short height;
    bool* ifMove;
    bool sound;
    bool music;
    bool control_gate;
    int goldLeft;
    int Enemy_Count;
    int Money_Count;
    int Energy_Count;
    int Heal_Count;
    const int _stime()
    {
        int stime;
        long ltime;
        ltime = time(NULL);
        stime = (unsigned int)ltime / 2;
        return stime;
    }
    const void MazeGeneration()
    {
        std::string** maze = Arr_s[0]->getArray();
        std::string hall = obj[0]->getID();
        std::string wall = obj[1]->getID();
        std::string exit = obj[2]->getID();
        std::string gate = obj[3]->getID();
        int r, x, y;
        srand(_stime());
        for (y = 0; y < Arr_s[0]->getHeight(); y++)
        {
            for (x = 0; x < Arr_s[0]->getWidth(); x++)
            {
                maze[y][x] = wall;
                if (x % 2 == 0 && y % 2 == 2)
                    maze[y][x] = hall;
                if (x % 3 == 0 && y % 3 == 2)
                    maze[y][x] = wall;
                if (x % 4 == 0 && y % 3 == 5)
                    maze[y][x] = hall;
                if (x % 5 == 0 && y % 2 == 2)
                    maze[y][x] = wall;
                if (y == 0 || x == 0 || x == Arr_s[0]->getWidth() - 1 || y == Arr_s[0]->getHeight() - 1)
                    maze[y][x] = wall;
            }
        }
        x = 3; y = 3;
        for (int i = 0; i < Arr_s[0]->getHeight() * Arr_s[0]->getWidth(); i++)
        {
            maze[y][x] = hall;
            while (true)
            {
                r = rand() % 4;
                switch (r)
                {
                case 0: if (y != 1)
                    if (maze[y - 2][x] == wall)
                    {
                        maze[y - 1][x] = hall;
                        maze[y - 2][x] = hall;
                        y -= 2;
                    }
                case 1: if (y != Arr_s[0]->getHeight() - 2)
                    if (maze[y + 2][x] == wall)
                    {
                        maze[y + 1][x] = hall;
                        maze[y + 2][x] = hall;
                        y += 2;
                    }
                case 2: if (x != 1)
                    if (maze[y][x - 2] == wall)
                    {
                        maze[y][x - 1] = hall;
                        maze[y][x - 2] = hall;
                        x -= 2;
                    }
                case 3: if (x != Arr_s[0]->getWidth() - 2)
                    if (maze[y][x + 2] == wall)
                    {
                        maze[y][x + 1] = hall;
                        maze[y][x + 2] = hall;
                        x += 2;
                    }
                }
                int a = 0;
                if (x != 1)
                {
                    if (maze[y][x - 2] == hall)
                        a += 1;
                }
                else a += 1;
                if (y != 1)
                {
                    if (maze[y - 2][x] == hall)
                        a += 1;
                }
                else a += 1;
                if (x != Arr_s[0]->getWidth() - 2)
                {
                    if (maze[y][x + 2] == hall)
                        a += 1;
                }
                else a += 1;
                if (y != Arr_s[0]->getHeight() - 2)
                {
                    if (maze[y + 2][x] == hall)
                        a += 1;
                }
                else a += 1;
                if (a == 4)
                    break;
            }
            int a = 0;
            if (x != 1)
            {
                if (maze[y][x - 2] == hall)
                    a += 1;
            }
            else a += 1;
            if (y != 1)
            {
                if (maze[y - 2][x] == hall)
                    a += 1;
            }
            else a += 1;
            if (x != Arr_s[0]->getWidth() - 2)
            {
                if (maze[y][x + 2] == hall)
                    a += 1;
            }
            else a += 1;
            if (y != Arr_s[0]->getHeight() - 2)
            {
                if (maze[y + 2][x] == hall)
                    a += 1;
            }
            else a += 1;
            if (a == 4)
                do
                {
                    x = 2 * (rand() % ((Arr_s[0]->getWidth() - 1) / 2)) + 1;
                    y = 2 * (rand() % ((Arr_s[0]->getHeight() - 1) / 2)) + 1;
                } while (maze[y][x] != hall);
        }
        for (y = 0; y < Arr_s[0]->getHeight(); y++)
        {
            for (x = 0; x < Arr_s[0]->getWidth(); x++)
            {
                if (x == Arr_s[0]->getWidth() - 2 && y >= Arr_s[0]->getHeight() - 4)
                    maze[y][x] = wall;
                if (x == Arr_s[0]->getWidth() - 2 && y == Arr_s[0]->getHeight() - 3)
                    maze[y][x] = gate;
                if ((x <= 5 && y == 2) || ((x <= Arr_s[0]->getWidth() - 3 && x >= Arr_s[0]->getWidth() - 6) && (y >= Arr_s[0]->getHeight() - 5 && y <= Arr_s[0]->getHeight() - 2)))
                    maze[y][x] = hall;
                if (x == Arr_s[0]->getWidth() - 1 && y == Arr_s[0]->getHeight() - 3)
                {
                    _Exit = { (short)x, (short)y };
                    maze[y][x] = exit;
                }
            }
        }
    }
    void cleen()
    {
        for (short y = 0; y < height + 10; y++)
            for (short x = 0; x < width + 10; x++)
            {
                SetConsoleCursorPosition(h, { x, y });
                std::cout << " ";
            }
    }
    void Menu_Render()
    {
        int _select = 0;
        const int events_count = 256;
        Menu menu;
        INPUT_RECORD all_events[events_count];
        DWORD read_event;
        COORD c;
        bool control_loop = true;
        music:
        if (music == true)
            PlaySound(TEXT("1.wav"), NULL, SND_FILENAME | SND_ASYNC | SND_LOOP);
        else
            PlaySound(0,0,0);
        while (control_loop)
        {
            ReadConsoleInput(hin, all_events, events_count, &read_event);
            for (int i = 0; i < read_event; i++)
            {
                c.X = all_events[i].Event.MouseEvent.dwMousePosition.X;
                c.Y = all_events[i].Event.MouseEvent.dwMousePosition.Y;
                if (_select == 0)
                {
                    for (short i = 40; i < 50 ; i++)
                    {
                        SetConsoleCursorPosition(h, { i, 15 });
                        std::cout << " ";
                    }
                    menu.printMenu();
                    menu.selectMenu(c, 4);
                    if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED)
                    {
                        if (menu.selectMenu(c, 2) == true &&
                            (c.Y == menu.getCoordText(0).Y && c.X <= (menu.getCoordText(0).X + menu.getText(0).length())))
                        {
                            control_loop = false;
                        }
                        if (menu.selectMenu(c, 2) == true &&
                            (c.Y == menu.getCoordText(1).Y && c.X <= (menu.getCoordText(1).X + menu.getText(1).length())))
                        {
                            cleen();
                            _select = 2;
                        }
                        if (menu.selectMenu(c, 2) == true &&
                            (c.Y == menu.getCoordText(2).Y && c.X <= (menu.getCoordText(2).X + menu.getText(2).length())))
                        {
                            cleen();
                            _select = 1;
                        }
                        if (menu.selectMenu(c, 2) == true &&
                            (c.Y == menu.getCoordText(3).Y && c.X <= (menu.getCoordText(3).X + menu.getText(3).length())))
                        {
                            system("cls");
                            SetConsoleCursorPosition(h, { 40, 11 });
                            std::cout << "Press key...";
                            SetConsoleTextAttribute(h, 0);
                            exit(0);
                        }
                    }
                }
                if (_select == 1)
                {
                    menu.printMenu(1);
                    menu.selectMenu(c, 4, 1);
                    if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED)
                    {
                        if (menu.selectMenu(c, 2, 1) == true &&
                            (c.Y == menu.getCoordText(4, 1).Y && c.X <= (menu.getCoordText(4, 1).X + menu.getText(4, 1).length())))
                        {
                            for (short y = 0; y < height; y++)
                                for (short x = 0; x < width + 10; x++)
                                {
                                    SetConsoleCursorPosition(h, { x, y });
                                    std::cout << " ";
                                }
                            _select = 0;
                        }
                    }
                }
                if (_select == 2)
                {
                    menu.printMenu(2);
                    menu.selectMenu(c, 4, 2); 
                    for (short i = 0; i < 4; i++)
                    {
                        SetConsoleCursorPosition(h, { menu.getCoordText(i, 2).X + (short)menu.getText(i, 2).length() + 4, menu.getCoordText(i, 2).Y });
                        SetConsoleTextAttribute(h, 14);
                        if (i == 0)
                            std::cout << Enemy_Count;
                        if (i == 1)
                            std::cout << Heal_Count;
                        if (i == 2)
                        {
                            SetConsoleCursorPosition(h, { menu.getCoordText(i, 2).X + (short)menu.getText(i, 2).length() + 2, menu.getCoordText(i, 2).Y });
                            std::cout << Energy_Count;
                        }
                        if (i == 3)
                            std::cout << Money_Count;
                    }
                    for (short j = 4; j < 6; j++)
                    {
                        if (c.Y == menu.getCoordText(j, 2).Y && c.X >= menu.getCoordText(j, 2).X + (short)menu.getText(j, 2).length() + 3 && 
                            c.X < menu.getCoordText(j, 2).X + (short)menu.getText(j, 2).length() + 6)
                        {
                            SetConsoleCursorPosition(h, { menu.getCoordText(j, 2).X + (short)menu.getText(j, 2).length() + 3, menu.getCoordText(j, 2).Y });
                            SetConsoleTextAttribute(h, 3);
                        }
                        else
                        {
                            SetConsoleCursorPosition(h, { menu.getCoordText(j, 2).X + (short)menu.getText(j, 2).length() + 3, menu.getCoordText(j, 2).Y });
                            SetConsoleTextAttribute(h, 14);
                        }
                        if (j == 4 && sound == true)
                            std::cout << menu.getText(15, 2);
                        else if (j == 4 && sound == false)
                            std::cout << menu.getText(16, 2);
                        if (j == 5 && music == true)
                            std::cout << menu.getText(15, 2);
                        else if (j == 5 && music == false)
                            std::cout << menu.getText(16, 2);
                    }
                    if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED)
                    {
                        if (c.Y == menu.getCoordText(4, 2).Y && c.X >= menu.getCoordText(4, 2).X + (short)menu.getText(4, 2).length() + 3 &&
                            c.X < menu.getCoordText(4, 2).X + (short)menu.getText(4, 2).length() + 6)
                        {
                            if (sound == true)
                            {
                                sound = false;
                            }
                            else if (sound == false)
                            {
                                sound = true;
                                SetConsoleCursorPosition(h, { menu.getCoordText(4, 2).X + (short)menu.getText(4, 2).length() + 5, menu.getCoordText(4, 2).Y });
                                std::cout << " ";
                            }
                        }
                        if (c.Y == menu.getCoordText(5, 2).Y && c.X >= menu.getCoordText(4, 2).X + (short)menu.getText(5, 2).length() + 3 &&
                            c.X < menu.getCoordText(5, 2).X + (short)menu.getText(5, 2).length() + 6)
                        {
                            if (music == true)
                            {
                                music = false;
                                goto music;
                            }
                            else if (music == false)
                            {
                                music = true;
                                SetConsoleCursorPosition(h, { menu.getCoordText(5, 2).X + (short)menu.getText(5, 2).length() + 5, menu.getCoordText(5, 2).Y });
                                std::cout << " ";
                                goto music;
                            }
                        }
                        if (menu.selectMenu(c, 2, 2) == true && (c.Y == menu.getCoordText(6, 2).Y && c.X <= (menu.getCoordText(6, 2).X + menu.getText(6, 2).length())))
                        {
                            cleen();
                            _select = 0;
                        }
                        if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(7, 2).Y && c.X <= menu.getCoordText(7, 2).X)
                        {
                            if (Enemy_Count > 0)
                                Enemy_Count--;
                        }
                        else if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(8, 2).Y && c.X <= menu.getCoordText(8, 2).X)
                        {
                            if (Enemy_Count < 10)
                                Enemy_Count++;
                        }
                        if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(9, 2).Y && c.X <= menu.getCoordText(9, 2).X)
                        {
                            if (Heal_Count > 1)
                                Heal_Count--;
                        }
                        else if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(10, 2).Y && c.X <= menu.getCoordText(10, 2).X)
                        {
                            if (Heal_Count < 5)
                                Heal_Count++;
                        }
                        if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(11, 2).Y && c.X <= menu.getCoordText(11, 2).X)
                        {
                            if (Energy_Count > 1)
                                Energy_Count--;
                        }
                        else if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(12, 2).Y && c.X <= menu.getCoordText(12, 2).X)
                        {
                            if (Energy_Count < 5)
                                Energy_Count++;
                        }
                        if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(13, 2).Y && c.X <= menu.getCoordText(13, 2).X)
                        {
                            if (Money_Count > 1)
                                Money_Count--;
                            if (Money_Count <= 9)
                            {
                                SetConsoleCursorPosition(h, { menu.getCoordText(13, 2).X + 3,menu.getCoordText(14, 2).Y });
                                std::cout << " ";
                            }
                        }
                        else if (menu.selectMenu(c, 2, 2) == true && c.Y == menu.getCoordText(14, 2).Y && c.X <= menu.getCoordText(14, 2).X)
                        {
                            if (Money_Count < 20)
                            {
                                Money_Count++;
                            }
                        }
                    }
                }
            }
        }
    }
public:
    Maze(int h = 43, int w = 71)
    {
        width = w;
        height = h;
        Enemy_Count = 4;
        Money_Count = 10;
        Energy_Count = 1;
        Heal_Count = 1;
        sound = true;
        music = true;
        Menu_Render();
        ifMove = new bool(false);
        control_gate = true;
        obj.push_back(new _Hall);
        obj.push_back(new _Wall);
        obj.push_back(new _ExiT);
        obj.push_back(new _Gate);
        Arr_s.push_back(new D_Array(obj[0]->getID(), h, w));
        Arr_s.push_back(new D_Array(obj[0]->getID(), h, w));
        MazeGeneration();
        generateEnemy(Enemy_Count);
        insertItem(new _Gold(Money_Count));
        insertItem(new _Energy(Energy_Count));
        insertItem(new _Healer(Heal_Count));
        insertItem(new _Bomb);
    }
    std::vector<D_Array*> getArray()
    {
        return Arr_s;
    }
    void drowMaze()
    {
        std::string** maze = Arr_s[0]->getArray();
        std::string** Arr_item = Arr_s[1]->getArray();
        int h = Arr_s[0]->getHeight();
        int w = Arr_s[0]->getWidth();
        for (short y = 0; y < h; y++)
        {
            for (short x = 0; x < w; x++)
            {
                for (auto it : obj)
                {
                    if (it->getID() == maze[y][x])
                        it->print({ x, y });
                }
            }
        }
        for (short y = 0; y < h; y++)
        {
            for (short x = 0; x < w; x++)
            {
                for (auto it : item)
                {
                    if (it->getID() == Arr_item[y][x])
                        it->print({ x, y });
                }
                for (auto it : enemys)
                {
                    if (it->getID() == maze[y][x])
                        it->print();
                }
                for (auto it : players)
                {
                    if (it->getID() == maze[y][x])
                        it->print();
                }
            }
        }
    }
    void drowGameInfo()
    {
        std::string _gold = "Gold";
        width = Arr_s[0]->getWidth() + 4;
        height = 2;
        COORD tmp = { width, height };
        if (item.empty() == false)
        {
            for (auto it : item)
                if (it->getID() == _gold)
                    goldLeft = it->GetCount();
        }
        else
            goldLeft = 0;
        for (int i = 0; i < players.size(); i++)
        {
            SetConsoleCursorPosition(h, tmp);
            SetConsoleTextAttribute(h, 10);
            std::cout << "Health: " << players[i]->getHealth() << "  ";
            tmp.Y++;
            SetConsoleCursorPosition(h, tmp);
            SetConsoleTextAttribute(h, 11);
            std::cout << "Energy: " << players[i]->getEnergy() << "  ";
        }
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        SetConsoleTextAttribute(h, 6);
        std::cout << "$_Left: " << goldLeft << "  ";
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        SetConsoleTextAttribute(h, 4);
        std::cout << "Corona: " << enemys.size() << "  ";
        tmp.Y += 2;
        SetConsoleCursorPosition(h, tmp);
        SetConsoleTextAttribute(h, 7);
        std::cout << "You must get all usd,";
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        SetConsoleTextAttribute(h, 12);
        std::cout << "stop COVID - 19";
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        std::cout << "and get to girl!";
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        SetConsoleTextAttribute(h, 7);
        std::cout << "Press ENTER to plant the";
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        std::cout << "bomb and Space to detonate";
        tmp.Y++;
        SetConsoleCursorPosition(h, tmp);
        std::cout << "Don't let corona get to you";
    }
    void insertItem(Item* I)
    {
        item.push_back(I);
        std::string** Arr_Item = Arr_s[1]->getArray();
        std::string** maze = Arr_s[0]->getArray();
        std::string hall = obj[0]->getID();
        int tmp = I->GetCount();
        while (tmp)
        {
        a:
            short x = rand() % (Arr_s[1]->getWidth() - 1) + 1;
            short y = rand() % (Arr_s[1]->getHeight() - 1) + 1;
            if (maze[y][x] == hall && Arr_Item[y][x] == hall)
            {
                Arr_Item[y][x] = I->getID();
                tmp--;
            }
            else
                goto a;
        }
    }
    void insertUnit(Unit* u)
    {
        std::string pers = "Pers";
        std::string enemy = "Enemy";
        if (u->getID() == pers)
            players.push_back(u);
        else if (u->getID() == enemy)
            enemys.push_back(u);
        bool tmp = true;
        std::string** maze = Arr_s[0]->getArray();
        std::string hall = obj[0]->getID();
        short x = u->GetCoord(1)->X;
        short y = u->GetCoord(1)->Y;
        while (tmp)
        {
        a:
            if (maze[y][x] == hall)
            {
                if (u->getID() == enemy && x < 10 && y < 10)
                {
                    x = rand() % (Arr_s[0]->getWidth() - 1) + 1;
                    y = rand() % (Arr_s[0]->getHeight() - 1) + 1;
                    goto a;
                }
                maze[y][x] = u->getID();
                tmp = false;
            }
            else if (maze[y][x] != hall)
            {
                x = rand() % (Arr_s[0]->getWidth() - 1) + 1;
                y = rand() % (Arr_s[0]->getHeight() - 1) + 1;
                u->setCoord(x, y);
                goto a;
            }
        }
    }
    void generateEnemy(int count = 0)
    {
        if (count > 0)
            for (int i = 0; i < count; i++)
                insertUnit(new _Enemy(Arr_s));
    }
    void renderPers(short direct, short index = 0)
    {
        if (item.empty() == false)
        {
            for (short i = 0; i < item.size(); i++)
                if (item[i]->getID() == "Bomb")
                {
                    *ifMove = players[index]->Action(direct, nullptr, item[i]);
                    break;
                }
            lastPosPlayer = { players[index]->GetCoord(2)->X, players[index]->GetCoord(2)->Y };
            PosPlayer = { players[index]->GetCoord(1)->X, players[index]->GetCoord(1)->Y };
            for (short i = 0; i < item.size(); i++)
            {
                if (Arr_s[1]->getArray()[lastPosPlayer.Y][lastPosPlayer.X] == item[i]->getID())
                    item[i]->print(lastPosPlayer);
                else if (Arr_s[1]->getArray()[lastPosPlayer.Y][lastPosPlayer.X] == obj[0]->getID())
                    obj[0]->print(lastPosPlayer);
            }
            if (direct == 13)
            {
                if (sound == true)
                {
                    mciSendString(L"close song2", NULL, 0, 0);
                    mciSendString(L"open 2.wav type mpegvideo alias song1", NULL, 0, 0);
                    mciSendString(L"play song1", NULL, 0, 0);
                }
            }
            else if (direct == 32)
            {
                if (sound == true)
                {
                    mciSendString(L"close song1", NULL, 0, 0);
                    mciSendString(L"open 3.mp3 type mpegvideo alias song2", NULL, 0, 0);
                    mciSendString(L"play song2 from 0 to 2000", NULL, 0, 0);
                }
                for (short y = 0; y < Arr_s[0]->getHeight(); y++)
                {
                    for (short x = 0; x < Arr_s[0]->getWidth(); x++)
                    {
                        if (Arr_s[0]->getArray()[y][x] == "Crush")
                        {
                            for (auto it : players)
                                if (it->GetCoord(1)->Y == y && it->GetCoord(1)->X == x)
                                    it->changeHealth(-100);
                            for (short i = 0; i < enemys.size(); i++)
                            {
                                if (enemys[i]->GetCoord(1)->Y == y && enemys[i]->GetCoord(1)->X == x)
                                {
                                    delete enemys[i];
                                    enemys.erase(enemys.begin() + i);
                                }
                            }
                            Arr_s[0]->getArray()[y][x] = obj[0]->getID();
                            if (Arr_s[1]->getArray()[y][x] == obj[0]->getID())
                                obj[0]->print({ x, y });
                        }
                    }
                }
            }
            if (Arr_s[1]->getArray()[PosPlayer.Y][PosPlayer.X] == "Gold")
            {
                if (sound == true)
                {
                    mciSendString(L"open 4.mp3 type mpegvideo alias song3", NULL, 0, 0);
                    mciSendString(L"play song3 from 0 to 400", NULL, 0, 0);
                }
                for (auto it : item)
                {
                    if (it->getID() == "Gold")
                        it->MinusCount(1);
                }
                Arr_s[1]->getArray()[PosPlayer.Y][PosPlayer.X] = "Hall";
            }
            if (Arr_s[1]->getArray()[PosPlayer.Y][PosPlayer.X] == "Energy")
                players[0]->changeEnergy(+1);
            else if (Arr_s[1]->getArray()[lastPosPlayer.Y][lastPosPlayer.X] == "Energy")
                if (sound == true)
                {
                    mciSendString(L"open 5.mp3 type mpegvideo alias song4", NULL, 0, 0);
                    mciSendString(L"play song4 from 500 to 1000", NULL, 0, 0);
                }
            if (Arr_s[1]->getArray()[PosPlayer.Y][PosPlayer.X] == "Heal")
                players[0]->changeHealth(+1);
            else if (Arr_s[1]->getArray()[lastPosPlayer.Y][lastPosPlayer.X] == "Heal")
                if (sound == true)
                {
                    mciSendString(L"open 6.mp3 type mpegvideo alias song5", NULL, 0, 0);
                    mciSendString(L"play song5 from 500 to 1000", NULL, 0, 0);
                }
        }
        else if (item.empty() == true)
        {
            *ifMove = players[index]->Action(direct);
            lastPosPlayer = { players[index]->GetCoord(2)->X, players[index]->GetCoord(2)->Y };
            PosPlayer = { players[index]->GetCoord(1)->X, players[index]->GetCoord(1)->Y };
            obj[0]->print(lastPosPlayer);
        }
        players[0]->print();
    }
    void renderEnemy()
    {
        std::string** ArrItem = Arr_s[1]->getArray();
        short x, y, n = 0;
        bool erase = false;
    enemyDie:
        Sleep(100);
        if (erase == true && enemys.empty() == false)
        {
            enemys.erase(enemys.begin() + n);
            erase = false;
        }
        if (enemys.empty() == false)
            for (short i = 0; i < enemys.size(); i++)
            {
                if (players.empty() == false)
                    enemys[i]->Action(0, ifMove, nullptr, players[0]->GetCoord(1));
                else
                    enemys[i]->Action(0);
                x = enemys[i]->GetCoord(2)->X;
                y = enemys[i]->GetCoord(2)->Y;
                obj[0]->print({ x, y });
                for (short i = 0; i < item.size(); i++)
                {
                    if (ArrItem[y][x] == item[i]->getID())
                    {
                        item[i]->print({ x, y });
                        break;
                    }
                }
                if (enemys[i]->GetCoord(1)->Y == players[0]->GetCoord(1)->Y && enemys[i]->GetCoord(1)->X == players[0]->GetCoord(1)->X)
                {
                    if (sound == true)
                    {
                        mciSendString(L"open 8.mp3 type mpegvideo alias song6", NULL, 0, 0);
                        mciSendString(L"play song6 from 0 to 400", NULL, 0, 0);
                    }
                    players[0]->changeHealth(-50);
                    delete enemys[i];
                    erase = true;
                    n = i;
                    goto enemyDie;
                }
                enemys[i]->print();
            }
    }
    void EventControl()
    {
        if (item.empty() == false)
        {
            for (auto it : item)
                if (it->getID() == "Gold")
                    goldLeft = it->GetCount();
        }
        if (goldLeft == 0 && enemys.empty() == true && control_gate == true)
        {
            D_Array* maze = Arr_s[0];
            if (sound == true)
            {
                mciSendString(L"open 7.mp3 type mpegvideo alias song5", NULL, 0, 0);
                mciSendString(L"play song5 from 0 to 1500", NULL, 0, 0);
            }
            for (short y = 0; y < maze->getHeight(); y++)
            {
                for (short x = 0; x < maze->getWidth(); x++)
                {
                    if (maze->getArray()[y][x] == obj[3]->getID())
                    {
                        maze->getArray()[y][x] = obj[0]->getID();
                        obj[0]->print({ x, y });
                    }
                }
            }
            control_gate = false;
        }
        if (control_gate == false && (_Exit.Y == PosPlayer.Y && _Exit.X == PosPlayer.X))
        {
            system("cls");
            COORD win = { 42,12 };
            SetConsoleCursorPosition(h, win);
            SetConsoleTextAttribute(h, 3);
            std::cout << "You win" << std::endl;
            SetConsoleTextAttribute(h, 0);
            exit(0);
        }
        if (players[0]->getHealth() <= 0)
        {
            system("cls");
            COORD die = { 40, 12 };
            SetConsoleCursorPosition(h, die);
            SetConsoleTextAttribute(h, 4);
            std::cout << "You die..." << std::endl;
            SetConsoleTextAttribute(h, 0);
            exit(0);
        }
        if (players[0]->getEnergy() <= 0)
        {
            system("cls");
            COORD over = { 40, 11 };
            SetConsoleCursorPosition(h, over);
            SetConsoleTextAttribute(h, 4);
            std::cout << "Game  Over" << std::endl;
            COORD out = { 37, 12 };
            SetConsoleCursorPosition(h, out);
            SetConsoleTextAttribute(h, 4);
            std::cout << "Out of energy =(" << std::endl;
            SetConsoleTextAttribute(h, 0);
            exit(0);
        }
    }
    ~Maze()
    {
        if (Arr_s.empty() != true)
            for (auto it : Arr_s)
                delete it;
        if (players.empty() != true)
            for (auto it : players)
                delete it;
        if (enemys.empty() != true)
            for (auto it : enemys)
                delete it;
        if (item.empty() != true)
            for (auto it : item)
                delete it;
        if (obj.empty() != true)
            for (auto it : obj)
                delete it;
        delete ifMove;
    }
};
