namespace ST10442835_PROG6221_POE.Models
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }
        public string Explanation { get; set; }
    }
}
