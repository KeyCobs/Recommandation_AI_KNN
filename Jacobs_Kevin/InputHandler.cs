using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacobs_Kevin
{
    internal class InputHandler
    {

        public Input GetInput(string input)
        {
            return FindGram(input);
        }
        private Input FindGram(string input)
        {
            Input i = new Input();
            //find veb, noun, Subj 
            //always Sub/verb/noun 
            //find Subj
            i.m_Subject = Gram(input.Split(' ')[0], "Assets/sub.txt");
            if (i.m_Subject != null)
            {
                input = input.Replace(input.Split(' ')[0] + " ", "");

            }
            //find verb
            i.m_Value = Gram(input, "Assets/verbsP.txt", 1);
            if (i.m_Value == 0)
            {
                i.m_Value = Gram(input, "Assets/verbsN.txt", -1);
            }
            //find Noun
            if (Gram(input.ToLower()) == null)
            {
                return null;
            }
            i.m_Noun = Gram(input.ToLower());
            

            return i;
        }

        private int Gram(string inp, string file, int score)
        {
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                string delim = sr.ReadLine().ToLower();
                for (int i = 0; i < inp.Split(' ').Length; i++)
                {
                    string s = inp.Split(' ')[i];
                    if(s == delim)
                    {
                        return score;
                    }
                }
                
            }
            return 0;

        }

        private string Gram(string inp, string file)
        {
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                string delim = sr.ReadLine().ToLower();

                if (inp.ToLower() == delim)
                {
                    return delim;
                }

            }
            return null;

        }
        private List<string> Gram(string inp)
        {
            string file = "Assets/Noun.txt";
            StreamReader sr = new StreamReader(file);
            List<string> l = new List<string>();
            while(!sr.EndOfStream)
            {
                    string delim = sr.ReadLine().ToLower();
                for (int i = 0; i < inp.Split(' ').Length; i++)
                {
                    string s = inp.Split(' ')[i];
                    if(s == delim)
                    {
                         l.Add(delim);
                    }
                }

            }

            return l;
        }


    }
}
