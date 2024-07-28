/*** Definition Section ***/
%{
#include "scanner.tab.h"
%}

%option noyywrap
%option yylineno

%{
#include <string.h>
#include <string>
#include <vector>
#include <map>
#include <set>
#include <fstream>
#include <cassert>
using namespace std;
int errorFound = 0;
int lineNumber = 1;
%}

REAL_NUMBER  (0|[1-9][0-9]*)(\.[0-9]*)?(e[+-]?[0-9]+)?[fFlL]?
DEC_NUMBER  (0|[1-9][0-9]*)(\.[0-9]+)?(([u|U]?([l|L]|(ll)|LL)?)|([l|L]|(ll)|LL)?[u|U]?)
ID  [a-zA-Z_][a-zA-Z0-9_]*

/*** Rule Section ***/
%%

"int"                                   { return INT; }
">>"									{ return RIGHT_SHIFT; }
"<<"									{ return LEFT_SHIFT; }
\/{2}[^\n]*								{}
\<                                    	{ return '<'; }
\>                                    	{ return '>'; }
\;                                    	{ return ';'; }
\(                                    	{ return '('; }
\)                                    	{ return ')'; }
\,                                    	{ return ','; }
\+                                    	{ return '+'; }
\-                                    	{ return '-'; }
\*                                    	{ return '*'; }
\/                                    	{ return '/'; }
\=                                    	{ return '='; }
\#                                    	{ return '#'; }
\{                                    	{ return '{'; }
\}                                    	{ return '}'; }
include                                 { return INCLUDE; }
iostream								{ return IOSTREAM; }
using                                 	{ return USING; }
namespace                               { return NAMESPACE; }
return                               	{ return RETURN; }
std	                       				{ return STD; }
cin	                      				{ return CIN; }
cout                           			{ return COUT; }

{ID}									{ strcpy(yylval.string, yytext); return ID; }
{DEC_NUMBER}                           	{ strcpy(yylval.string, yytext); return NUMBER; }
{REAL_NUMBER}                           { strcpy(yylval.string, yytext); return NUMBER; }
[\n]									{ ++lineNumber; }
[ \t\r]+       							{ ; }
.         								{ errorFound = 1; printf("Illegal token %s at line %d!\n", yytext, lineNumber); return 0; }
%%
