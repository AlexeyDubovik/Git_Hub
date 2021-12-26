#include "MazeG.h"
int main()
{
    system("mode con cols=90 lines=120");
    system("title Corona Maze!");
    cci.bVisible = false;
    cci.dwSize = 100;
    SetConsoleCursorInfo(h, &cci);
    SetConsoleMode(hin, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
    const int events_count = 256;
    INPUT_RECORD all_events[events_count];
    DWORD read_event;
    COORD c;
    Maze maze;
    maze.Generate_Maze(31, 81, 2);
    maze.Print_Maze();

    int direct = 0;
    COORD* START = new COORD {0, 2};
    COORD* FINISH = nullptr;
    FindPath* finder = new FindPath(maze.get_arr(), START);
    std::list <COORD*> path;
    SetConsoleCursorPosition(h, { START->X, START->Y });
    cout << "S";
    while (true)
    {
        ReadConsoleInput(hin, all_events, events_count, &read_event);
        for (int i = 0; i < read_event; i++)
        {
            c.X = all_events[i].Event.MouseEvent.dwMousePosition.X;
            c.Y = all_events[i].Event.MouseEvent.dwMousePosition.Y;
            if (all_events[i].Event.MouseEvent.dwEventFlags == RIGHTMOST_BUTTON_PRESSED)
            {
                SetConsoleCursorPosition(h, { 0, 0 });
                maze.Print_Maze();
                SetConsoleCursorPosition(h, { START->X, START->Y });
                cout << "S";
                SetConsoleCursorPosition(h, { c.X,c.Y });
                cout << "F";
                if (FINISH != nullptr)
                    delete FINISH;
                if (path.empty() == false)
                {
                    for (auto it : path)
                        delete it;
                    path.erase(path.begin(), path.end());
                }
                finder = new FindPath(maze.get_arr(), START);
                FINISH = new COORD{ c.X, c.Y };
                
                path = finder->getCOORD(FINISH);
            }
            if (path.empty() == false)
            {
                SetConsoleCursorPosition(h, { path.front()->X,path.front()->Y });
                cout << "*";
                delete path.front();
                path.pop_front();
            }
        }
        a:
        if (path.empty() == false)
        {
            SetConsoleCursorPosition(h, { path.front()->X,path.front()->Y });
            cout << "*";
            delete path.front();
            path.pop_front();
            goto a;
        }
        //direct = _getch();
    }
    return 0;
}
