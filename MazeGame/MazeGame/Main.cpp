#include "Maze.h"

int main()
{
    system("mode con cols=90 lines=26");
    system("title Corona Maze!");
    cci.bVisible = false;
    cci.dwSize = 100;
    SetConsoleCursorInfo(h, &cci);
    SetConsoleMode(hin, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
    Maze* game = new Maze(23,51);
    game->insertUnit(new _Pers(game->getArray()));
    game->drowMaze();
    while (true)
    {
        game->EventControl();
        short direct = 5;
        game->drowGameInfo();
        game->renderEnemy();
        while (_kbhit())
        {
            direct = _getch();
        }
        game->renderPers(direct);
    }
}