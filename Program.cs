using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Abstract base class for all questions
public abstract class Question
{
    public string QuestionText { get; set; }
    public abstract bool CheckAnswer(string userAnswer);
    public abstract string GetCorrectAnswer();
    public abstract string GetQuestionType();
    public abstract void DisplayQuestion();
}

// Multiple choice question implementation
public class MultipleChoiceQuestion : Question
{
    private string correctAnswer;
    private List<string> options;

    public MultipleChoiceQuestion()
    {
        options = new List<string>();
    }

    public void SetOptions(List<string> newOptions, string correct)
    {
        options = newOptions;
        correctAnswer = correct;
    }

    public override void DisplayQuestion()
    {
        Console.WriteLine(QuestionText);
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
    }

    public override bool CheckAnswer(string userAnswer)
    {
        if (int.TryParse(userAnswer, out int choice) && choice >= 1 && choice <= options.Count)
        {
            return options[choice - 1].Equals(correctAnswer, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public override string GetQuestionType()
    {
        return "Multiple Choice";
    }
}

// Open-ended question implementation
public class OpenEndedQuestion : Question
{
    private List<string> acceptableAnswers;

    public OpenEndedQuestion()
    {
        acceptableAnswers = new List<string>();
    }

    public void SetAcceptableAnswers(List<string> answers)
    {
        acceptableAnswers = answers;
    }

    public override void DisplayQuestion()
    {
        Console.WriteLine(QuestionText);
    }

    public override bool CheckAnswer(string userAnswer)
    {
        return acceptableAnswers.Any(answer => 
            answer.Equals(userAnswer.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public override string GetCorrectAnswer()
    {
        return string.Join(" or ", acceptableAnswers);
    }

    public override string GetQuestionType()
    {
        return "Open Ended";
    }
}

// True/False question implementation
public class TrueFalseQuestion : Question
{
    private bool correctAnswer;

    public void SetCorrectAnswer(bool answer)
    {
        correctAnswer = answer;
    }

    public override void DisplayQuestion()
    {
        Console.WriteLine($"{QuestionText} (true/false)");
    }

    public override bool CheckAnswer(string userAnswer)
    {
        if (bool.TryParse(userAnswer.ToLower(), out bool answer))
        {
            return answer == correctAnswer;
        }
        return false;
    }

    public override string GetCorrectAnswer()
    {
        return correctAnswer.ToString().ToLower();
    }

    public override string GetQuestionType()
    {
        return "True/False";
    }
}

// Main game class
public class GeographyQuiz
{
    private List<Question> questions;

    public GeographyQuiz()
    {
        questions = new List<Question>();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Geography Quiz Game");
            Console.WriteLine("1. Create Game");
            Console.WriteLine("2. Play Game");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateGame();
                    break;
                case "2":
                    if (questions.Count > 0)
                        PlayGame();
                    else
                    {
                        Console.WriteLine("Please add questions first!");
                        Console.ReadKey();
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void CreateGame()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Create Game Mode");
            Console.WriteLine("1. Add Question");
            Console.WriteLine("2. Delete Question");
            Console.WriteLine("3. Edit Question");
            Console.WriteLine("4. Return to Main Menu");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddQuestion();
                    break;
                case "2":
                    DeleteQuestion();
                    break;
                case "3":
                    EditQuestion();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void AddQuestion()
    {
        Console.Clear();
        Console.WriteLine("Select question type:");
        Console.WriteLine("1. Multiple Choice");
        Console.WriteLine("2. Open Ended");
        Console.WriteLine("3. True/False");
        Console.Write("Choice: ");

        string choice = Console.ReadLine();
        Question newQuestion = null;

        switch (choice)
        {
            case "1":
                newQuestion = CreateMultipleChoiceQuestion();
                break;
            case "2":
                newQuestion = CreateOpenEndedQuestion();
                break;
            case "3":
                newQuestion = CreateTrueFalseQuestion();
                break;
            default:
                Console.WriteLine("Invalid choice!");
                Console.ReadKey();
                return;
        }

        if (newQuestion != null)
        {
            questions.Add(newQuestion);
            Console.WriteLine("Question added successfully!");
            Console.ReadKey();
        }
    }

    private MultipleChoiceQuestion CreateMultipleChoiceQuestion()
    {
        MultipleChoiceQuestion question = new MultipleChoiceQuestion();
        Console.Write("Enter question text: ");
        question.QuestionText = Console.ReadLine();

        List<string> options = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            Console.Write($"Enter option {i + 1}: ");
            options.Add(Console.ReadLine());
        }

        Console.Write("Enter the number of the correct answer (1-4): ");
        if (int.TryParse(Console.ReadLine(), out int correctIndex) && correctIndex >= 1 && correctIndex <= 4)
        {
            question.SetOptions(options, options[correctIndex - 1]);
            return question;
        }

        Console.WriteLine("Invalid correct answer selection!");
        return null;
    }

    private OpenEndedQuestion CreateOpenEndedQuestion()
    {
        OpenEndedQuestion question = new OpenEndedQuestion();
        Console.Write("Enter question text: ");
        question.QuestionText = Console.ReadLine();

        List<string> acceptableAnswers = new List<string>();
        Console.WriteLine("Enter acceptable answers (one per line, blank line to finish):");
        while (true)
        {
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                break;
            acceptableAnswers.Add(answer);
        }

        question.SetAcceptableAnswers(acceptableAnswers);
        return question;
    }

    private TrueFalseQuestion CreateTrueFalseQuestion()
    {
        TrueFalseQuestion question = new TrueFalseQuestion();
        Console.Write("Enter question text: ");
        question.QuestionText = Console.ReadLine();

        Console.Write("Enter correct answer (true/false): ");
        if (bool.TryParse(Console.ReadLine().ToLower(), out bool correctAnswer))
        {
            question.SetCorrectAnswer(correctAnswer);
            return question;
        }

        Console.WriteLine("Invalid answer format!");
        return null;
    }

    private void DeleteQuestion()
    {
        if (questions.Count == 0)
        {
            Console.WriteLine("No questions to delete!");
            Console.ReadKey();
            return;
        }

        ListQuestions();
        Console.Write("Enter question number to delete (0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= questions.Count)
        {
            questions.RemoveAt(index - 1);
            Console.WriteLine("Question deleted successfully!");
        }
        Console.ReadKey();
    }

    private void EditQuestion()
    {
        if (questions.Count == 0)
        {
            Console.WriteLine("No questions to edit!");
            Console.ReadKey();
            return;
        }

        ListQuestions();
        Console.Write("Enter question number to edit (0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= questions.Count)
        {
            Question oldQuestion = questions[index - 1];
            Question newQuestion = null;

            switch (oldQuestion.GetQuestionType())
            {
                case "Multiple Choice":
                    newQuestion = CreateMultipleChoiceQuestion();
                    break;
                case "Open Ended":
                    newQuestion = CreateOpenEndedQuestion();
                    break;
                case "True/False":
                    newQuestion = CreateTrueFalseQuestion();
                    break;
            }

            if (newQuestion != null)
            {
                questions[index - 1] = newQuestion;
                Console.WriteLine("Question edited successfully!");
            }
        }
        Console.ReadKey();
    }

    private void ListQuestions()
    {
        for (int i = 0; i < questions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. [{questions[i].GetQuestionType()}] {questions[i].QuestionText}");
        }
    }

    private void PlayGame()
    {
        Console.Clear();
        int score = 0;
        DateTime startTime = DateTime.Now;

        for (int i = 0; i < questions.Count; i++)
        {
            Console.Clear();
            Console.WriteLine($"Question {i + 1} of {questions.Count}");
            Console.WriteLine();

            questions[i].DisplayQuestion();
            Console.Write("\nYour answer: ");
            string answer = Console.ReadLine();

            if (questions[i].CheckAnswer(answer))
            {
                Console.WriteLine("Correct!");
                score++;
            }
            else
            {
                Console.WriteLine("Incorrect!");
            }
            Console.WriteLine("Press any key for next question...");
            Console.ReadKey();
        }

        TimeSpan timeSpent = DateTime.Now - startTime;
        DisplayResults(score, timeSpent);
    }

    private void DisplayResults(int score, TimeSpan timeSpent)
    {
        Console.Clear();
        Console.WriteLine("Game Over!");
        Console.WriteLine($"Score: {score} out of {questions.Count}");
        Console.WriteLine($"Time spent: {timeSpent.TotalMinutes:F1} minutes");
        
        Console.Write("\nWould you like to see the correct answers? (yes/no): ");
        if (Console.ReadLine().ToLower().StartsWith("y"))
        {
            Console.WriteLine("\nCorrect Answers:");
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"Q{i + 1}: {questions[i].QuestionText}");
                Console.WriteLine($"A: {questions[i].GetCorrectAnswer()}\n");
            }
        }

        Console.Write("\nWould you like to play again? (yes/no): ");
        if (Console.ReadLine().ToLower().StartsWith("y"))
        {
            PlayGame();
        }
    }
}

// Program entry point
class Program
{
    static void Main(string[] args)
    {
        GeographyQuiz quiz = new GeographyQuiz();
        quiz.Run();
    }
}