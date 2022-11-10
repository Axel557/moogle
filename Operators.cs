using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Collections.Generic;
namespace MoogleEngine
{
    public class Operators
    {   public static bool[]FindProximityOperator(string OriQuery,string[]query)
    {   char[]separators ={' ','!','*','^'};
        string[]WOperator = OriQuery.Split(separators,StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
        bool[]Where = new bool[query.Length];
        for (int h = 0; h < Where.Length; h++)
        {
            Where[h]=false;
        }
        for (int j = 0; j < (query.Length-1); j++)
        {
            int after = Array.IndexOf(WOperator,query[j])+1;
           if (WOperator[after]=="~")
           {  if((Database.Words.Contains(query[j]))&(Database.Words.Contains(query[j+1])))
              { Where[j]=true;
               Where[j+1]=true;}
           }
           //throw new Exception(query[0].ToString()+"here"+query[1].ToString());
        }
        return Where;
        }
        public static bool[]FindOperators(string[]query ,char Ope, string OriginQuery)
        {
            bool[]Where=new bool [query.Length];
           
            for (int i = 0; i <query.Length; i++)
            {
                int Pos = OriginQuery.IndexOf(query[i])-1;
             if (Pos>= 0)
             {if(OriginQuery[Pos] == Ope)
                { 
                     Where[i]=true;
                }else{Where[i]=false;}}else{Where[i]=false;}
             }
            return Where;
        }    
         public static int[] FindOperatorofImportance(string[]query, string OriginQuery)
        {
            int[]Importance = new int[query.Length];
            for (int i = 0; i < query.Length; i++)
            {   int counter = 0;
                int Pos = OriginQuery.IndexOf(query[i])-1;
                if (Pos>= 0)
                {if(OriginQuery[Pos] != ' ')
                {  if (OriginQuery[Pos] == '*')
                {
                    
                 counter = 1;
                    for (int j = Pos; j >= 0; j--)
                    { if(OriginQuery[j]!= '*')
                       { break;}
                      else{counter++;}
                    }
                }
                
                    Importance[i] = counter;
             
                }
            }
            }
            return Importance;
        }
        public static bool[] MustbePresent (string[]query,bool[]Where)
        {   bool[]Mustbe = new bool[Database.DocNum];
            for (int i = 0; i < Database.DocNum; i++)
            {
                Mustbe[i]= false;
            }
            for (int i = 0; i < Where.Length; i++)
            {
                for (int j = 0; j < Database.DocNum; j++)
              {
                 if (Where[i])
                { int Pos = Array.IndexOf(Database.Words,query[i]);
                    if(Pos>= 0)
                    {if (TF_IDF.Weight[j,Pos] == 0)
                    {
                        Mustbe[j] = true;
                    }}
                }
              }
            }
            return Mustbe;
        }
        public static bool[] MustbeNOTPresent (string[]query, bool[]Where)
        {   bool[]MustNOTbe = new bool[Database.DocNum];
            for (int i = 0; i < MustNOTbe.Length; i++)
            {
                MustNOTbe[i] = false;
            }
            for (int i = 0; i < Where.Length; i++)
            {
                for (int j = 0; j < Database.DocNum; j++)
                { if (Where[i])
                {   int Pos = Array.IndexOf(Database.Words,query[i]);
                   if(Pos >= 0)
                    {if (TF_IDF.Weight[j,Pos] != 0)
                    {
                        MustNOTbe[j] = true;
                    }}
                }
                }
            }
            return MustNOTbe;
        }
        public static float[] ValuetoAdd(bool[]Where,string[]Query)
        { float[]ValuetoAdd = new float[Database.DocNum];//creates float array size equal to the number of docs
         for (int i = 0; i < Database.DocNum; i++)// deafault value equals 0
         {
             ValuetoAdd[i] = 0;
         }
            for (int i = 0; i < (Where.Length-1); i++)//for to -1 the size of the bool[]where wich has true in the positions that are affected by the 
            {                                       // proximty operators , it's positions coincides with the query
                if ((Where[i])&(Where[i+1]))//only enters the second loop if the next is also true
                {
                    for (int j = 0; j < Database.DocNum; j++)//changes the doc to search
                    {
                        ValuetoAdd[j] += MinDistance(Query[i],Query[i+1],j); //MinDistance calculates the MIN distance between the 2 words
                    }                                                       //MinDistance returns int Max Value if one of the words is not present
                }
            }
            //throw new Exception(ValuetoAdd[158].ToString());
            for (int g = 0; g < ValuetoAdd.Length; g++)//
            {
                if (ValuetoAdd[g]!=0)//just in case
            {
                    ValuetoAdd[g] = 100/ValuetoAdd[g];//makes it an inverse proportion the less de distance the more it adds to the score
                }
            }
            return ValuetoAdd;

        }
        public static float MinDistance(string Word_A,string Word_B,int DocNum)
        { List<int>Pos_A = new List<int>();//list of  posintions of word A in Document# Docnum
           List<int>Pos_B = new List<int>();// same ^ but with B
          
        
            for (int i = 0; i < Database.Documents[DocNum].Length; i++)
            {
                if(Database.Documents[DocNum][i]==Word_A) //if it matches with A save the position
                {Pos_A.Add(i);}
                
                if(Database.Documents[DocNum][i]==Word_B)//if it matches with B save the position
                {Pos_B.Add(i);}
            }
            if(Pos_B.Count>0)
            {if (Pos_A.Count>0)//if a value for both is found enter loop, else returns int max value
            {   float MinDistance = int.MaxValue; // sets min as int Max value for comparison
                for (int a = 0; a < Pos_A.Count; a++)
                {
                    for (int b = 0; b < Pos_B.Count; b++)
                    {
                        float TempDist =Pos_A[a]-Pos_B[b];//find the diference btw the 2 positions//resets with every iteration
                        if (TempDist<0)
                        {
                            TempDist = TempDist*(-1);
                        }
                        if (TempDist<MinDistance)//compares with Min Distance
                        {
                            MinDistance = TempDist;//saves Temp as new Min
                        }
                    }
                } 
                return MinDistance;
            }else{return 0;} }else{return 0;}
        }
    }
}