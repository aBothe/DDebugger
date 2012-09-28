import std.stdio;
import std.conv;

class A
{
	int a;
	string b;
	bool c;
}

void main(string[] args)
{	
	writeln("Hello World");	
	
	auto o = new A();
	
	int a = o.a;
	
	a++;
	
	a += 123;
	
	if(a == 124)
		writeln("a == 124");
	else
		writeln("a != 124");
	
	for(int i = 16; i!=0; i--)
	{
		writeln("##"~to!string(i) ~ " in for loop");	
	}
	
	readln();
}