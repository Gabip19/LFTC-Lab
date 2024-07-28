#include <iostream>
#include <stack>
#include <sstream>

using namespace std;

bool isOperator(char c) {
    return c == '+' || c == '-' || c == '*' || c == '/';
}

void computeExpressionInOrder(std::stack<char>& operators, std::stack<std::string>& operands)
{
    char currentOp;
    string currentVal = operands.top();
    operands.pop();

    MOV eax, currentVal;

    while (!operators.empty())
    {
        currentOp = operators.top();
        operators.pop();

        if (currentOp == '*')
        {
            string op1 = operands.top();
            operands.pop();

            if (op1 == "^")
            {
                POP ebx;
            }
            else
            {
                MOV ebx, op1;
            }
            
            IMUL ebx;
        }
        else if (currentOp == '/')
        {
            string op1 = operands.top();
            operands.pop();

            if (op1 == "^")
            {
                POP ebx;
            }
            else
            {
                MOV ebx, op1;
            }

            IDIV ebx;
        }
        else if (!operators.empty() && (currentOp == '+' || currentOp == '-') && (operators.top() == '*' || operators.top() == '/'))
        {
            char nextOp = operators.top();
            operators.pop();
            
            MOV ecx, eax;

            string op1 = operands.top();
            operands.pop();
            string op2 = operands.top();
            operands.pop();

            if (op1 == "^")
            {
                POP eax;
            }
            else
            {
                MOV eax, op1;
            }

            MOV ebx, op2;

            if (nextOp == '*')
            {
                IMUL ebx;
            }
            else if (nextOp == '/')
            {
                IDIV ebx;
            }

            PUSH eax;
            MOV eax, ecx;

            operands.push("^");
            operators.push(currentOp);
        }
        else if (currentOp == '+')
        {
            string op1 = operands.top();
            operands.pop();

            MOV ebx, op1;
            ADD eax, ebx;
        }
        else if (currentOp == '-')
        {
            string op1 = operands.top();
            operands.pop();

            MOV ebx, op1;
            SUB eax, ebx;
        }
    }
}

int main() {
    std::stack<char> operators;
    std::stack<std::string> operands;

    // Fill in the stacks with your values
    operators.push('-');
    operators.push('/');
    operators.push('+');

    operands.push("b");
    operands.push("5");
    operands.push("3");
    operands.push("a");

    std::string result = computeExpressionInOrder(operators, operands);
    std::cout << "Computed expression: " << result << std::endl;

    return 0;
}
