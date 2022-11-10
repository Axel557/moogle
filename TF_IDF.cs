using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
namespace MoogleEngine
{
    public class TF_IDF
    {   //public static float[]IDFs = IDF();
        public static float[,]Weight = weight();
        public static float[]Weight_Query(string[]query,int[]Importance)
        {   float[]WeightQ = new float[Database.NumofWords];
            
              for (int f = 0; f < Database.NumofWords; f++)
              {int counter = 0;
                 for (int i = 0; i < query.Length; i++)
                 { 
                    if (Database.Words[f]==query[i])
                    {
                        counter=Convert.ToInt32((counter+1)*Math.Pow(10,Importance[i]));
                    }
                }
               WeightQ[f] = counter*Weight[Database.DocNum,f];
              }
             return WeightQ;  
           }
        public static float[]DotProduct(float[]W_Query,int Docnum)
        {
         float[]DotProduct =new float[Database.NumofWords];
             
   
        for (int m1 = 0; (m1 < Database.NumofWords); m1++)
        {
          DotProduct[m1]=Weight[Docnum,m1]*W_Query[m1];
        }
    
    return DotProduct;
        }
        public static float[,]weight()
        {   int DocNum = Database.DocNum;
            float[,]Weight = new float[DocNum+1,Database.NumofWords];
            for (int word = 0; word < Database.NumofWords; word++)//gets word
            { int Present = 0;
                for (int doc = 0; doc < DocNum; doc++)//pics doc to search
                {  int counter = 0;
                   for (int k = 0; k < Database.Documents[doc].Length; k++)//searches in doc word by word for matches
                   {
                       if (Database.Documents[doc][k]==Database.Words[word])//compares
                       {
                           counter++;
                       }
                   }
                   Weight[doc,word] = counter;//TF
                   if (counter>0)//if present increases the counter(IDF calculations)
                   {
                       Present++;
                   }
                }
                Weight[DocNum,word] = Convert.ToSingle(Math.Log10(DocNum/Present));//IDF
            }
            for (int doc = 0; doc < DocNum; doc++)//weight calculations
            {
                for (int word = 0; word < Database.NumofWords; word++)
                {
                    Weight[doc,word] = Weight[doc,word]*Weight[DocNum,word];
                }
            }
            return Weight;
        }
    }
}