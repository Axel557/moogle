using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Linq;
namespace MoogleEngine;

public class SearchItem
{
    public SearchItem(string title, string snippet, float score)
    {
        this.Title = title;
        this.Snippet = snippet;
        this.Score = score;
    }

    public string Title { get; private set; }

    public string Snippet { get; private set; }

    public float Score { get; private set; }

   public static float[]AllScores(float[]WeightQ,bool[]MustBe,bool[]MustNOTbe,string[] Query,bool[]Proximity)
    {  float[]valueProximity = Operators.ValuetoAdd(Proximity,Query);
       float[]Score = new float[Database.DocNum];
       for (int f1 = 0; f1 < Score.Length; f1++)
       {   float magnitudeQ = Scores.Magnitude(WeightQ);
           Score[f1] = (Scores.Magnitude(TF_IDF.DotProduct(WeightQ,f1))/Scores.Magnitudes[f1]*magnitudeQ)+valueProximity[f1];
           if (MustBe[f1]==true)
           {
               Score[f1]= -1;
           }
           else if (MustNOTbe[f1]==true)
           {
               Score[f1] = -1;
           }
       }
        return Score;
    }
    public static SearchItem[]CreateSearchItem(float[]Score,float[]Weight_Q)
    {  
        SearchItem[]Docs = new SearchItem[Score.Length];       
        string[] title = Database.Titles();
        for (int i = 0; i <Score.Length; i++)
    {
      Docs[i]= new SearchItem(Database.Alltitles[i]+" "+Score[i],CreateSnippet.Snippet(Weight_Q,i),Score[i]);
    }
    return Docs.OrderByDescending(ob => ob.Score).ToArray();
    }
   
}
