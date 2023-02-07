using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace L4_2_Pairs_of_words__TEST
{
    class Word
    {
        private int number;
        private string word;
        private char firstL;
        private char lastL;
        public Word(string word, int number)
        {
            this.word = word;
            this.number = number;
            takeLetters();
        }
        public Word()
        {
            this.number = 0;
            this.word = null;
        }
        public void takeLetters()
        {
            if ((word != null) && (word.Length > 0))
            {
                firstL = word[0];
                lastL = word[word.Length - 1];
            }
        }
        public int getNumber() { return number; }
        public string getWord() { return word; }
        public char getFirstL() { return firstL; }
        public char getLastL() { return lastL; }
        public void setNumber(int number) { this.number = number; }
        public void setWord(string word)
        {
            this.word = word;
            takeLetters();
        }
    }
    class WordPair
    {
        private Word wordA;
        private Word wordB;
        private int length;
        public WordPair()
        {
            length = 0;
            wordA = new Word();
            wordB = new Word();
        }
        public Word getWordA() { return wordA; }
        public Word getWordB() { return wordB; }
        public int getLength() { return length; }
        public void setWordA(string wordA, int i)
        {
            this.wordA = new Word(wordA, i);
        }
        public void setWordB(string wordB, int i)
        {
            this.wordB = new Word(wordB, i);
        }
        public void calcLength()
        {
            if ((wordA.getWord() != null) && (wordB.getWord() != null))
            {
                length = wordA.getWord().Length + wordB.getWord().Length;
            }
        }
    }
    internal class Program
    {
        static bool IsWordPair(Word word1, Word word2)
        {
            bool check = false;
            if (word1.getLastL() == word2.getFirstL())
            {
                check = true;
            }
            return check;
        }
        static WordPair[] MaxPair(WordPair[] arr, out int n)
        {
            WordPair max = new WordPair();
            foreach (WordPair pair in arr)
            {
                pair.calcLength();
                if (max.getLength() < pair.getLength())
                {
                    max = pair;
                }
            }
            WordPair[] maxArr = new WordPair[arr.Length];
            n = 0;
            foreach (WordPair pair in arr)
            {
                maxArr[n] = new WordPair();
                pair.calcLength();
                if (pair.getLength() == max.getLength())
                {
                    maxArr[n++] = pair;
                }
            }
            return maxArr;
        }
        static void Pop(WordPair[] arr,int n,int index)
        {
            for(int i=index;i<n-1;i++)
            {
                arr[i]=arr[i+1];
            }
            n -= 1;
        }
        static void Process(string readFile, string writeFile, string analyseFile, char[] delimiters)
        {
            using (StreamReader reader = new StreamReader(readFile))
            {
                using (StreamWriter writerAnalyse = new StreamWriter(analyseFile, true))
                {
                    int a = 29;
                    writerAnalyse.WriteLine(new String('-', a));
                    writerAnalyse.WriteLine("|Line number|Number of pairs|");
                    writerAnalyse.WriteLine(new String('-', a));
                    using (StreamWriter writerResult = new StreamWriter(writeFile, true))
                    {
                        string line;
                        int k = 1;
                        while ((line = reader.ReadLine()) != null)
                        {
                            int counter = 0;
                            if (line.Length > 0)
                            {
                                string[] parts = line.Split(delimiters);
                                if (parts.Length == 1)
                                {
                                    writerResult.Write(line);
                                    writerResult.WriteLine("       - line {0} has only one word", k);
                                    writerAnalyse.WriteLine("|{0,13}|{1,13}|", k++, counter);
                                }
                                else
                                {
                                    WordPair[] pairWords = new WordPair[parts.Length];
                                    for (int i = 0; i < parts.Length; i++)
                                    {
                                        pairWords[i] = new WordPair();
                                        if (i == 0)
                                        {
                                            pairWords[counter].setWordA(parts[i], i);
                                            continue;
                                        }
                                        else
                                        {
                                            pairWords[counter++].setWordB(parts[i], i);
                                            pairWords[counter].setWordA(parts[i], i);
                                        }
                                    }
                                    for(int i=0;i<counter;i++)
                                    {
                                        if (!IsWordPair(pairWords[i].getWordA(), pairWords[i].getWordB()))
                                        {
                                            Pop(pairWords,counter,i);
                                        }
                                    }
                                    WordPair[] pairCheck = MaxPair(pairWords, out int n);
                                    for (int i = 0; i < n; i++)
                                    {
                                        if (pairCheck[i].getLength()!=0)
                                        {
                                            if (i == n - 1)
                                            {
                                                writerResult.Write(line.Substring(line.IndexOf(pairCheck[i].getWordB().getWord()),
                                                pairCheck[i].getWordB().getWord().Length));
                                                for (int j = line.IndexOf(pairCheck[i].getWordB().getWord()); j < line.Length; j++)
                                                {
                                                    writerResult.Write(line[j]);
                                                }
                                                line = line.Remove(line.IndexOf(pairCheck[i].getWordB().getWord()));
                                            }
                                            else
                                            {
                                                writerResult.Write(line.Substring(line.IndexOf(pairCheck[i].getWordB().getWord()),
                                                line.IndexOf(pairCheck[i + 1].getWordA().getWord()) - line.IndexOf(pairCheck[i].getWordB().getWord())));
                                                line = line.Remove(line.IndexOf(pairCheck[i].getWordB().getWord()),
                                                    line.IndexOf(pairCheck[i + 1].getWordA().getWord()) - line.IndexOf(pairCheck[i].getWordB().getWord()));
                                            }
                                        }

                                    }
                                    writerResult.WriteLine(line);
                                    writerAnalyse.WriteLine("|{0,13}|{1,13}|", k++, counter);
                                }
                            }
                            else
                            {
                                writerResult.WriteLine("line {0} has no characters", k);
                                writerAnalyse.WriteLine("|{0,13}|{1,13}|", k++, counter);
                                continue;
                            }
                        }
                        writerAnalyse.WriteLine(new String('-', a));
                    }
                }
            }
        }
        const string fileRead = "L4-2T.txt";
        const string fileWrite = "Results.txt";
        const string fileAnalyse = "Analyse.txt";
        static void Main(string[] args)
        {
            if (File.Exists(fileWrite))
            {
                File.Delete(fileWrite);
            }
            if (File.Exists(fileAnalyse))
            {
                File.Delete(fileAnalyse);
            }
            char[] delimiters = { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t' };
            Process(fileRead, fileWrite, fileAnalyse,delimiters);
        }й
    }
}
