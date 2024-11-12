Console.WriteLine("Welcome to the Number Guessing Game!\r\nI'm thinking of a number between 1 and 100.\r\nYou have 5 chances to guess the correct number.\r\n\r\nPlease select the difficulty level:\r\n1. Easy (10 chances)\r\n2. Medium (5 chances)\r\n3. Hard (3 chances)");


Console.WriteLine("Enter your choice: ");
string choice = Console.ReadLine();

int total_chance;

if (choice == "1")
{
    Console.WriteLine("Great! You have selected the Easy level.\r\nLet's start the game!\r\n");
    total_chance = 10;
}
else if (choice == "2")
{
    Console.WriteLine("Great! You have selected the Medium Hard level.\r\nLet's start the game!\r\n");
    total_chance = 5;
}
else
{
    Console.WriteLine("Great! You have selected the Hard level.\r\nLet's start the game!\r\n");
    total_chance = 3;
}

int guess, x, flag = 0;

Random random = new Random();
guess = random.Next(1, 101);
Console.WriteLine(guess);

for (int i = 0; i < total_chance; i++)
{
    Console.WriteLine("Enter your guess: ");
    x = Convert.ToInt16(Console.ReadLine());
    if (x == guess)
    {
        Console.WriteLine("Congratulations! You guessed the correct number in " + i + " attempts.");
        flag = 1;
        return;
    }
    else
    {
        if (x > guess)
        {
            Console.WriteLine("Incorrect! The number is less than " + x + ".");
            int chance_left = total_chance - (i + 1);
            Console.WriteLine("You have " + chance_left + " turns left!");
        }
        else
        {
            Console.WriteLine("Incorrect! The number is greater than " + x + ".");
            int chance_left = total_chance - (i + 1);
            Console.WriteLine("You have " + chance_left + " turns left!");
        }
    }
}

if (flag == 0)
{
    Console.WriteLine("The number is " + guess + ". Better luck next time!");
}