%{
#include <cstdio>
#include <string>
#include <set>
#include <sstream>   
#include <cstring>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <ctype.h>
#include <stack>
using std::string;
using std::set;
using std::stack;

int yyerror(const char*);
int yylex();
void computeExpressionInOrder();
void moveToRegister(string, string);

extern int yylineno;
extern FILE* yyin;

set<string> identifiers;
std::stringstream instructions;
stack<char> operators;
stack<std::string> operands;
%}

%union {
  char string[2000];
}

%define parse.error verbose

%token INT
%token FLOAT
%token RIGHT_SHIFT
%token LEFT_SHIFT
%token INCLUDE
%token IOSTREAM
%token USING
%token NAMESPACE
%token RETURN
%token MAIN
%token CIN
%token COUT
%token STD

%token <string> ID
%token <string> NUMBER

%type <string> expr_aritmetica value

%%
program: antet_program functie

antet_program: '#' INCLUDE '<' IOSTREAM '>' USING NAMESPACE STD ';' ;

functie: antet_functie corp ;

antet_functie: INT ID '(' ')' ;

corp: '{' instructions RETURN NUMBER ';' '}' ;

instructions: | instruction instructions ;

instruction: declarare | atribuire | citire | afisare ;

declarare: INT ID ';'                               { identifiers.insert($2); }

atribuire: ID '=' expr_aritmetica ';'               { computeExpressionInOrder(); instructions << "MOV [" << $1 << "], eax\n"; }

value: ID                                           { strcpy($$, $1); }
value: NUMBER                                       { strcpy($$, $1); }

expr_aritmetica: value                              { operands.push($1); }

expr_aritmetica: value '+' expr_aritmetica          { operands.push($1); operators.push('+'); }

expr_aritmetica: value '-' expr_aritmetica          { operands.push($1); operators.push('-'); }

expr_aritmetica: value '*' expr_aritmetica          { operands.push($1); operators.push('*'); }

expr_aritmetica: value '/' expr_aritmetica          { operands.push($1); operators.push('/'); }

citire: CIN RIGHT_SHIFT ID ';'                      { instructions << "CALL io_readint\nMOV [" << $3 << "], eax\n\n"; }

afisare: COUT LEFT_SHIFT value ';'                  { moveToRegister("eax", $3); instructions << "CALL io_writeint\nCALL io_writeln\n\n"; }
%%

int yyerror(const char* error)
{
    printf("Syntax error at line %d\n", yylineno);
    exit(1);
    return 0;
}

void moveToRegister(string regist, string value) {
    if (isdigit(value[0])) {
        instructions << "MOV " << regist << ", " << value << "\n";
    } else {
        instructions << "MOV " << regist << ", [" << value << "]\n";
    }
}

void computeExpressionInOrder()
{
    char currentOp;
    string currentVal = operands.top();
    operands.pop();

    moveToRegister("eax", currentVal);

    while (!operators.empty())
    {
        currentOp = operators.top();
        operators.pop();

        if (currentOp == '*' || currentOp == '/') {
            string op1 = operands.top();
            operands.pop();

            if (op1 == "^") {
                instructions << "POP ebx\n";
            } else {
                moveToRegister("ebx", op1);
            }
            
            if (currentOp == '*') {
                instructions << "IMUL ebx\n\n";
            } else {
                instructions << "IDIV ebx\n\n";
            }
        } else if (!operators.empty() && (currentOp == '+' || currentOp == '-') && (operators.top() == '*' || operators.top() == '/')) {
            char nextOp = operators.top();
            operators.pop();
            
            instructions << "MOV ecx, eax\n";

            string op1 = operands.top();
            operands.pop();
            string op2 = operands.top();
            operands.pop();

            if (op1 == "^") {
                instructions << "POP eax\n";
            } else {
                moveToRegister("eax", op1);
            }

            moveToRegister("ebx", op2);

            if (nextOp == '*') {
                instructions << "IMUL ebx\n";
            }
            else if (nextOp == '/') {
                instructions << "IDIV ebx\n";
            }

            instructions << "PUSH eax\n";
            instructions << "MOV eax, ecx\n\n";

            operands.push("^");
            operators.push(currentOp);
        } else if (currentOp == '+' || currentOp == '-') {
            string op1 = operands.top();
            operands.pop();

            if (op1 == "^") {
                instructions << "POP ebx\n";
            } else {
                moveToRegister("ebx", op1);
            }

            if (currentOp == '+') {
                instructions << "ADD eax, ebx\n\n";
            } else {
                instructions << "SUB eax, ebx\n\n";
            }
        }
    }
}

int main(int argc, char* argv[])
{
    ++argv, --argc;
  
    if (argc > 0) {
        yyin = fopen(argv[0], "r"); 
    }
    else 
        yyin = stdin; 

    yyparse();
    std::ofstream fout("a.asm");

    fout <<
        "%include 'io.inc'\n"
        "global main\n\n"
        "section .text\n"
        "main:\n";

    fout << instructions.str() << "ret\n\n";
    fout << "section .data\n";
    for(auto x: identifiers) {
        fout << "\t" << x << " dd " << "0\n";
    }
    return 0;
}