#include <stdio.h>

int main(void)
{
	char buffer[100];
	int variable = 0;
	printf("Hello world! \n");
	gets(buffer);
	variable = atoi(buffer);
	for (int i = 0; i < 5; i++)
	{
		printf("%i\n", i);
	}
	while (variable <= 5)
	{
		variable++;
		if (variable % 2 == 0)
		{
			printf("%s %3d", "Hi", variable);
		}
	}
	return 0;
}

void someFunction(int arg, char* arg2, float arg3)
{
	int x = 0;
	int y = 5;
}