namespace CYKSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class CYKAlgorithm
    {
        protected List<List<string>> _terminal_rules = new List<List<string>>();
        protected List<List<string>> _non_terminal_rules = new List<List<string>>();
        protected string start_symbol = null;
        protected string allGrammars = null;

        protected Regex spaces = new Regex("\\s+");
        protected Regex newline = new Regex("\\r?\\n");

        protected int array_index_of(List<string> a, string e)
        {
            return a != null ? a.IndexOf(e) : -1;
        }

        protected List<string> merge_arrays(List<string> a, List<string> b)
        {
            if (a != null && b != null)
            {
                for (var i = 0; i < b.Count; ++i)
                {
                    if (array_index_of(a, b[i]) == -1)
                    {
                        a.Add(b[i]);
                    }
                }
            }
            return a;
        }


        protected List<string> left_hand_sides(string s)
        {
            List<string> res = new List<string>();
            for (var i = 0; i < this._terminal_rules.Count; ++i)
            {
                var r = this._terminal_rules[i];
                if (r[1] == s)
                    res.Add(r[0]);
            }
            return res;
        }

        protected List<string> left_hand_sides2(string s, string t)
        {
            List<string> res = new List<string>();
            for (var i = 0; i < this._non_terminal_rules.Count; ++i)
            {
                var r = this._non_terminal_rules[i];
                if (r[1] == s && r[2] == t)
                    res.Add(r[0]);
            }
            return res;
        }


        public CYKAlgorithm(string allGrammars)
        {
            this.allGrammars = allGrammars;

            string[] grammars = newline.Split(allGrammars);

            for (var i = 0; i < grammars.Length; ++i)
            {
                string g = grammars[i];

                List<string> a = new List<string>();

                string[] rule = g.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                if (rule.Length == 2)
                {
                    a.Add(rule[0].Trim());

                    string[] parts = rule[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length >= 1)
                    {
                        a.AddRange(parts);
                    }
                }

                if (a.Count == 3)
                {
                    var new_rule = new List<string>(new string[] { a[0], a[1], a[2] });

                    this._non_terminal_rules.Add(new_rule);

                    if (this.start_symbol == null)
                    {
                        this.start_symbol = a[0];
                    }
                }
                else if (a.Count == 2)
                {
                    var new_rule = new List<string>(new string[] { a[0], a[1] });

                    this._terminal_rules.Add(new_rule);
                }
            }
        }


        protected List<string> tokenize_sentence(string sentence)
        {

            return new List<string>(spaces.Split(sentence));
        }

        protected List<string>[][] allocate_chart(int N)
        {
            List<string>[][] c = new List<string>[N][];

            c[0] = new List<string>[N];

            for (var i = 1; i < N; ++i)
            {
                c[i] = new List<string>[(N - (i))];

            }

            return c;
        }

        public bool Accept(string sentence)
        {
            bool accepted = false;

            if (sentence != null)
            {
                List<string> sentenceStrings = tokenize_sentence(sentence);

                int N = sentenceStrings.Count;

                List<string>[][] C = allocate_chart(N);

                for (var j = 0; j < N; ++j)
                {
                    C[0][j] = this.left_hand_sides(sentenceStrings[j]);
                }

                for (var i = 1; i < N; ++i)
                {
                    for (var j = 0; j < N - i; ++j)
                    {
                        var nt = C[i][j];

                        for (var k = i - 1; k >= 0; --k)
                        {
                            List<string> nts1 = C[k][j];
                            List<string> nts2 = C[i - k - 1][j + k + 1];

                            if (nts1 != null && nts2 != null)
                            {
                                for (var ii = 0; ii < nts1.Count; ++ii)
                                {
                                    string nt1 = nts1[ii];

                                    for (var jj = 0; jj < nts2.Count; ++jj)
                                    {
                                        string nt2 = nts2[jj];
                                        List<string> rhss = this.left_hand_sides2(nt1, nt2);

                                        if (rhss == null || rhss.Count == 0)
                                            continue;


                                        if (nt == null)
                                        {
                                            nt = new List<string>();
                                            C[i][j] = nt;
                                        }

                                        merge_arrays(nt, rhss);
                                    }
                                }
                            }
                        }
                    }
                }

                accepted = array_index_of(C[N - 1][0], this.start_symbol) != -1;
            }
            return accepted;
        }

    }
}
