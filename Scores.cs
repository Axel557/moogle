namespace MoogleEngine
{
    public class Scores
    {   public static float[]Magnitudes = Magnitude();
        public static float[]Magnitude()//single magnitude of each document
{
    float[]Magnitude = new float[Database.DocNum];
    for (int f1 = 0; f1 < Database.DocNum; f1++)
    {double Mag = 0;
        for (int f2 = 0; f2 < Database.NumofWords; f2++)
        {
            Mag+= TF_IDF.Weight[f1,f2]*TF_IDF.Weight[f1,f2];
        }
        Magnitude[f1]=Convert.ToSingle(Math.Sqrt(Mag));
    }
    return Magnitude;
}
        public static float Magnitude(float[]Q_Weight) //magnitud del query///////////////////////////////////////////////
{    double Magnitude = 0;
    for (int f1 = 0; f1 < Q_Weight.Length; f1++)
    {
        Magnitude+=Q_Weight[f1]*Q_Weight[f1];
    }
    return Convert.ToSingle(Math.Sqrt(Magnitude));
}
        public static string suggestion(string[]query)
               { 
                   string[]RawSuggestion = new string[query.Length];
                   for (int i = 0; i < query.Length; i++)
                   {   
                       if (Database.Words.Contains(query[i]))
                       {
                        RawSuggestion[i] = query[i];   
                       }
                       else{

                           int Min = int.MaxValue;
                           int Pos = 0;
                           for (int j = 0; j < Database.NumofWords; j++)
                           {
                               if (Scores.Levenshtein(query[i],Database.Words[j]) < Min)
                               {
                                   Min = Scores.Levenshtein(query[i],Database.Words[j]);
                                   Pos = j;
                               }
                           }
                           RawSuggestion[i] = Database.Words[Pos];
                        }
                    }
                     string suggestion=" ";
                           for (int c = 0; c < RawSuggestion.Length ; c++)
                           {
                               suggestion += " "+ RawSuggestion[c];
                           }
                           return suggestion;
                   }
        public static int Levenshtein(string s, string t)
               {
                   int n = s.Length; int m = t.Length; int[,] d = new int [n+1,m+1];
                   if (n==0)
                   {
                       return m;
                   }
                   if (m==0)
                   {
                       return n;
                   }
                   for (int i = 0; i <= n;d[i,0] = i++)
                   {
                   }
                   for (int j = 0; j <= m; d[0,j] =j++)
                   {
                   }
                   for (int i = 1; i <= n ; i++)                  
                   {
                    for (int j = 1; j <= m; j++)
                    {
                        int cost = (t[j-1]==s[i-1])?0:1;
                        d[i,j] = Math.Min(Math.Min(d[i-1,j]+1,d[i,j-1]+1),d[i-1,j-1]+cost);
                    }    
                   }
                   return d[n,m];
               }
    }
}