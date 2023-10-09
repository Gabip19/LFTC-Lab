#include <iostream>
using namespace std;

int main() {
    int sum = 0;
    int n;
    int x;
    
    cin >> n;
    while (n > 0)
    {
        cin >> x;
        sum = sum + x;
        n = n -1;
    }

    cout << sum;
}