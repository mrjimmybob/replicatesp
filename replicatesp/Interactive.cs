using System;
using System.Collections.Generic;
using System.Text;

namespace replicatesp
{
    internal class Interactive
    {
        public enum answer
        {
            no,
            yes,
            cancel,
            skip,
            abort,
            none
        };
        
        public static answer QuestionYesNo(string question)
        {
            return Question(question,
                "Yes or No [Y, N]",
                new string[] { "Y|YES", "N|NO" });
        }
        
        public static answer QuestionYesCancel(string question)
        {
             return Question(question,
                "Yes or Cancel [Y, C]",
                new string[] { "Y|YES", "C|CANCEL" });
        }

        public static answer QuestionYesNoCancel(string question)
        {
            return Question(question, 
                "Yes, No, or Cancel [Y, N, C]",
                new string[] { "Y|YES", "N|NO", "C|CANCEL" });
        }

        public static answer QuestionYesSkipCancel(string question)
        {
            return Question(question,
                "Yes, Skip, or Cancel [Y, S, C]",
                new string[] { "Y|YES", "S|SKIP", "C|CANCEL" });
        }


        private static answer Question(string question, string possibleAnswers, string[] validAnswers)
        {
            answer validAnswer;
            do
            {
                Print.Question(question, possibleAnswers);
                string userResponse = Console.ReadLine();
                validAnswer = GetAnswerIn(userResponse, validAnswers);
            } while (answer.none == validAnswer);
            return validAnswer;
        }

        public static answer GetAnswerIn(string response, string[] answers)
        {
            response = response.ToUpper();
            foreach (string ans in answers)
            {
                foreach (var sub in ans.Split('|'))
                {
                    if (sub.ToUpper().Equals(response.ToUpper()))
                    {
                        // Console.WriteLine($"Substring: {sub}" + " (" + ans + ")");
                        switch (ans[0])
                        {
                            case 'Y':
                                return answer.yes;
                            case 'N':
                                return answer.no;
                            case 'C':
                                return answer.cancel;
                            case 'S':
                                return answer.skip;
                            case 'A':
                                return answer.abort;
                        }
                    }
                }
            }
            return answer.none;
        }
    }
}
